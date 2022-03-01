using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using MovieNightSheduler.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib;

namespace MovieNightSheduler.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {

         public AppDb Db { get; set; }
       // private readonly ILogger<UserController> _logger;

        public UserController(AppDb db)
        {
            Db = db;
        }
 

        /*public UserController(ILogger<UserController> logger)
        {
            _logger = logger;

        }*/

        [HttpGet]
        public async Task<IActionResult> GetUser(User User)
        {
            //await Db.Connection.OpenAsync();
           // var query = "SELECT * FROM Users";
           string username = User.Username;
            var parameters = new DynamicParameters();
            parameters.Add("Username", User.Username, DbType.String);
            //   parameters.Add("Password", User.Password, DbType.String);
            var query = "select Id from Users where username=@username";
            try
            {
                var results = await Db.Connection.QueryAsync<User>(query, User);
                Console.WriteLine(results.First().Id);
                return Ok(results.ToList());
            }
            catch (Exception ex)
            {
               return BadRequest(ex.Message); 
            }

            /*    using (var connection = _context.CreateConnection())
                {
                    var Groups = await connection.QueryAsync<Group>(query);
                    return Ok(Groups.ToList());
                }*/

            //var results = await Db.Connection.QueryAsync<User>(query);


        }
        [HttpPost]
        public async Task<IActionResult> Post(User newUser)
        {
            var query = "Insert into Users(username, password) values(@Username, @Password)";
            var parameters = new DynamicParameters();
            parameters.Add("Username", newUser.Username, DbType.String);
            parameters.Add("Password", newUser.Password, DbType.String);
            try
            {
                await Db.Connection.ExecuteAsync(query, parameters);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
           

            return Ok();

        }
        [HttpPut]
        public async Task<IActionResult> Put(User User)
        {
         //   var query = "update Users set password = @password where username=@username";
            var parameters = new DynamicParameters();
            parameters.Add("Username", User.Username, DbType.String);
         //   parameters.Add("Password", User.Password, DbType.String);
            var query = "select Id from Users where username='asd'";
            var results = await Db.Connection.QueryAsync<User>(query);
            Console.WriteLine(results.ToList());


            //   await Db.Connection.ExecuteAsync(query, parameters);
           // await Db.Connection.Update

            return Ok(results.ToList());

        }



    }
}
