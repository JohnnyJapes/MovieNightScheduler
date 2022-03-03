using MovieNightSheduler.Models;
using Dapper;
using Dapper.Contrib.Extensions;

namespace MovieNightSheduler.Services
{
    using BCrypt.Net;
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
      //  Task<IEnumerable<User>> GetAll();
    }

    public class UserService : IUserService
    {
        
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        public AppDb Db { get; set; }
        // private readonly ILogger<UserController> _logger;

        public UserService(AppDb db)
        {
            Db = db;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            // wrapped in "await Task.Run" fetching user from a db
            User temp = new User{ Username= username, Password= password};
            var query = "select Id, Username, Password from Users where username=@username";
            try
            {
                var results = await Db.Connection.QueryAsync<User>(query, temp);
                Console.WriteLine(results.First().Username);
                Console.WriteLine(results.First().Password);
                await Task.Run(() => results.SingleOrDefault(x => x.Username == username && BCrypt.Verify(password, x.Password)));
                Console.WriteLine(BCrypt.Verify(temp.Password, results.First().Password));
                var user = await Db.Connection.GetAsync<User>(results.First().Id);
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

      //  public async Task<IEnumerable<User>> GetAll()
     //   {
            // wrapped in "await Task.Run" to mimic fetching users from a db
            // return await Task.Run(() => _users);
      //      return User; 
      //  }
    }

}
