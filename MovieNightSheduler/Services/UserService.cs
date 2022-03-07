using MovieNightScheduler.Models;
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
        User GetById(int id);
      //  Task<IEnumerable<User>> GetAll();
    }

    public class UserService : IUserService
    {
        
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        public AppDb Db { get; set; }
        private IJwtUtils JwtUtils;

        public UserService(AppDb db,  IJwtUtils jwtUtils)
        {
            Db = db;
            JwtUtils = jwtUtils;
        }

        public AuthResponse Authenticate(AuthRequest req, string ipAddress)
        {
            // wrapped in "await Task.Run" fetching user from a db
            //User temp = new User{ Username= username, Password= password};
            var parameters = new DynamicParameters();
            parameters.Add("Username", req.Username, DbType.String);
            //parameters.Add("Password", password, DbType.String);
            var query = "select Id, Username, passwordHash from Users where username=@username";
         
            var results = Db.Connection.Query<User>(query, parameters);
          //  Console.WriteLine(results.First().Username);
           // Console.WriteLine(results.First().PasswordHash);
            if (results.First().Username == null || !BCrypt.Verify(req.Password, results.First().PasswordHash))
                throw new AppException("Username or password is incorrect.");
            //Task.Run(() => results.SingleOrDefault(x => x.Username == req.Username && BCrypt.Verify(req.Password, x.PasswordHash)));
            Console.WriteLine(BCrypt.Verify(req.Password, results.First().PasswordHash));
            var user =  Db.Connection.Get<User>(results.First().Id);

            //generate jwt and refresh token
            var jwtToken = JwtUtils.GenerateJwtToken(user);
            var refreshToken = JwtUtils.GenerateRefreshToken(ipAddress);
            //add to user object
            user.RefreshTokens.Add(refreshToken);

            //remove old tokens
            // to be implemented removeOldRefreshTokens(user);

            //save changes to DB
            //Db.Connection.Update(user); no idea if this would work. Dodged by writing up a sql query instead
            query = "update tokens set token=@token where userId=@id";
            Db.Connection.Execute(query, new { @token = refreshToken.Token, @id = user.Id });
            
          /*  catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }*/
            return new AuthResponse(user, jwtToken, refreshToken.Token);

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
