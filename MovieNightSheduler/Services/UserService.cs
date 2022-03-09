﻿using MovieNightScheduler.Models;
using MovieNightScheduler.Helpers;
using MovieNightScheduler.Authorization;
using System.Data;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MovieNightScheduler.Services
{
    using Dapper;
    using Dapper.Contrib.Extensions;
    using BCrypt.Net;
    public interface IUserService
    {
        AuthResponse Authenticate(AuthRequest req, string ipAddress);
        AuthResponse RefreshToken(string token, string ipAddress);
        void RevokeToken(string token, string ipAddress);
        User GetById(int id);
      //  Task<IEnumerable<User>> GetAll();
    }

    public class UserService : IUserService
    {
        
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        public AppDb Db { get; set; }
        private IJwtUtils JwtUtils;
        private readonly AppSettings _appSettings;

        public UserService(AppDb db,  IJwtUtils jwtUtils, AppSettings appSettings)
        {
            Db = db;
            JwtUtils = jwtUtils;
            _appSettings = appSettings;
        }

        public AuthResponse Authenticate(AuthRequest req, string ipAddress)
        {
            // wrapped in "await Task.Run" fetching user from a db
            //User temp = new User{ Username= username, Password= password};
            var parameters = new DynamicParameters();
            parameters.Add("Username", req.Username, DbType.String);
            //parameters.Add("Password", password, DbType.String);
            var query = "select users.Id, Username, passwordHash, from Users where username=@username";
         
            var results = Db.Connection.Query<User, RefreshToken, User>(query,
                (user, refreshToken) => { user.RefreshTokens.Add(refreshToken); return user; }, parameters);
          //  Console.WriteLine(results.First().Username);
           // Console.WriteLine(results.First().PasswordHash);
            if (results.First().Username == null || !BCrypt.Verify(req.Password, results.First().PasswordHash))
                throw new AppException("Username or password is incorrect.");
            //Task.Run(() => results.SingleOrDefault(x => x.Username == req.Username && BCrypt.Verify(req.Password, x.PasswordHash)));
            Console.WriteLine(BCrypt.Verify(req.Password, results.First().PasswordHash));
            query = "select users.Id, Username, refreshTokens.Id, token, created, expires from Users left join refreshTokens on userId=users.Id where username=@username";
            User user = Db.Connection.Query<User, RefreshToken, User>(query,
                (user, refreshToken) => { user.RefreshTokens.Add(refreshToken); return user; }, parameters).First();

            //var user =  Db.Connection.Get<User>(results.First().Id);

            //generate jwt and refresh token
            var jwtToken = JwtUtils.GenerateJwtToken(user);
            var refreshToken = JwtUtils.GenerateRefreshToken(ipAddress);
            

            //remove old tokens
            removeOldRefreshTokens(user);
            //remove inactive refresh tokens from user

           /* List<RefreshToken> oldTokens = user.RefreshTokens.FindAll(x => !x.IsActive && x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
            user.RefreshTokens.RemoveAll(x => !x.IsActive && x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
            query = "delete from refreshTokens where id=@id";
            Db.Connection.Execute(query, oldTokens);*/

            //add to user object
            user.RefreshTokens.Add(refreshToken);
            //save changes to DB
            refreshToken.UserId = user.Id;
            query = "Insert into refreshTokens(token, created, expires, createdByIp) values(@token, @created, @expires, @createByIp) ";
            var rowsChanged = Db.Connection.Execute(query, refreshToken);
            if (rowsChanged != 1)
                throw new AppException("Database Insert Failed");
          /*  catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }*/
            return new AuthResponse(user, jwtToken, refreshToken.Token);

        }
        public AuthResponse RefreshToken(string token, string ipAddress)
        {
            var user = getUserByRefreshToken(token);
            var query = "select id,userId,  token set token=@token,  where userId=@UserId";
            getUserRefreshTokens(user);
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (refreshToken.IsRevoked)
            {
                            
                revokeDescendantRefreshTokens(refreshToken, user, ipAddress, $"Attempted reuse of revoked token: {token}");
                List<RefreshToken> revokedTokens = user.RefreshTokens;
                //query = "update refreshTokens set revoked=@revoked, revokedByIp=@revokedByIp, reasonRevoked=@reasonRevoked, replacedByToken=@replacedByToken where ";
                Db.Connection.Update(revokedTokens);
            }
            if (!refreshToken.IsActive)
                throw new AppException("Invalid Token");
            //replace old refresh token wtih new token (rotation)
            var newRefreshToken = rotateRefreshToken(refreshToken, ipAddress);
            Db.Connection.Update(refreshToken);
            Db.Connection.Insert(newRefreshToken);

            //remove old refresh tokens
            removeOldRefreshTokens(user);

            var jwtToken = JwtUtils.GenerateJwtToken(user);

            return new AuthResponse(user, jwtToken, newRefreshToken.Token);

        }

        public void RevokeToken(string token, string ipAddress)
        {
            var user = getUserByRefreshToken(token);
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);
            if (!refreshToken.IsActive)
                throw new AppException("Invalid Token");
            //revoke token
            revokeRefreshToken(refreshToken, ipAddress, "Revoked without replacement");
            Db.Connection.Update(refreshToken);
        }

        private  User getUserByRefreshToken(string token)
        {
            var query = "select " +
                "users.Id, " +
                "users.Username, " +
                "refreshTokens.Id, " +
                "token, " +
                "revoked, " +
                "replacedByToken, " +
                "created, " +
                "expires " +
                "from Users join refreshTokens on users.id=tokens.userId where token=@token";
            var results = Db.Connection.Query<User, RefreshToken, User>(query, (user, refreshToken) => { user.RefreshTokens.Add(refreshToken); return user; } ,new { @token = token, } );
            if (results.First() == null)
                throw new AppException("Invalid Token");
            //User user = results.First();
           /* query = "select * from refreshTokens where userId = @UserId";
            results =  Db.Connection.Query<RefreshToken>(query, new {@userId = user.Id});
            var listy = results.ToList();
            user.RefreshTokens = (RefreshToken)listy;*/

            return results.First();
        }

        private User getUserRefreshTokens(User user)
        {
            var query = "select " +
               "users.Id, " +
               "users.Username, " +
               "refreshTokens.Id, " +
               "token, " +
               "revoked, " +
               "replacedByToken, " +
               "created, " +
               "expires " +
               "from Users left join refreshTokens on users.id=tokens.userId where username=@username";
            var results = Db.Connection.Query<User, RefreshToken, User>(query, (user, refreshToken) => { user.RefreshTokens.Add(refreshToken); return user; }, new { @username = user.Username });
            if (results.First() == null)
                throw new AppException("Invalid User");
            return results.First();
        }
        private RefreshToken rotateRefreshToken(RefreshToken refreshToken, string ipAddress)
        {
            var newToken = JwtUtils.GenerateRefreshToken(ipAddress);
            revokeRefreshToken(refreshToken, ipAddress, "replaced by new token", newToken.Token);
            return newToken;
        }


        private void removeOldRefreshTokens(User user)
        {
            //remove inactive refresh tokes from user
            //user.RefreshTokens.RemoveAll(x => !x.IsActive && x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
            List<RefreshToken> oldTokens = user.RefreshTokens.FindAll(x => !x.IsActive && x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
            user.RefreshTokens.RemoveAll(x => !x.IsActive && x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
            string query = "delete from refreshTokens where id=@id";
            Db.Connection.Execute(query, oldTokens);
        }

        private void revokeDescendantRefreshTokens(RefreshToken refreshToken, User user, string ipAddress, string reason)
        {
            //recursive trace through refresh token chain and ensure all are revoked
            if(!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
            {
                var childToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
                if (childToken.IsActive) revokeRefreshToken(childToken, ipAddress, reason);
                else revokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
            }
        }
        
        private void revokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReasonRevoked = reason;
            token.ReplacedByToken = replacedByToken;
        }







        public User GetById(int id)
        {
            var user = Db.Connection.Get<User>(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }



        //  public async Task<IEnumerable<User>> GetAll()
        //   {
        // wrapped in "await Task.Run" to mimic fetching users from a db
        // return await Task.Run(() => _users);
        //      return User; 
        //  }
    }

}
