using MovieNightScheduler.Models;
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
        Task<User> Authenticate(string username, string password);
        User GetById(int id);
      //  Task<IEnumerable<User>> GetAll();
    }

    public class UserService : IUserService
    {
        
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        public AppDb Db { get; set; }

        public UserService(AppDb db)
        {
            Db = db;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            // wrapped in "await Task.Run" fetching user from a db
            //User temp = new User{ Username= username, Password= password};
            var parameters = new DynamicParameters();
            parameters.Add("Username", username, DbType.String);
            //parameters.Add("Password", password, DbType.String);
            var query = "select Id, Username, Password from Users where username=@username";
            try
            {
                var results = await Db.Connection.QueryAsync<User>(query, parameters);
                Console.WriteLine(results.First().Username);
                Console.WriteLine(results.First().PasswordHash);
                await Task.Run(() => results.SingleOrDefault(x => x.Username == username && BCrypt.Verify(password, x.PasswordHash)));
                Console.WriteLine(BCrypt.Verify(password, results.First().PasswordHash));
                var user = await Db.Connection.GetAsync<User>(results.First().Id);
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

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
