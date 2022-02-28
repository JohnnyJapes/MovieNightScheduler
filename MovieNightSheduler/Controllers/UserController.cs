using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using MovieNightSheduler.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace MovieNightSheduler.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {

         public AppDb Db { get; set; }
        private readonly ILogger<UserController> _logger;

        public UserController(AppDb db)
        {
            Db = db;
        }
 

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;

        }

        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            //await Db.Connection.OpenAsync();
            var query = "SELECT * FROM Users";

        /*    using (var connection = _context.CreateConnection())
            {
                var Groups = await connection.QueryAsync<Group>(query);
                return Ok(Groups.ToList());
            }*/

            var results = await Db.Connection.QueryAsync<User>(query);

            return Ok(results.ToList());
        }
        [HttpPost]
        public async Task<IActionResult> Post(User newUser)
        {
            var query = "Insert into Users(username, password) values(@Username, @Password)";
            var parameters = new DynamicParameters();
            parameters.Add("Username", newUser.Username, DbType.String);
            parameters.Add("Password", newUser.Password, DbType.String);

            await Db.Connection.ExecuteAsync(query, parameters);

            return Ok();

        }
      
            
        
    }
}
