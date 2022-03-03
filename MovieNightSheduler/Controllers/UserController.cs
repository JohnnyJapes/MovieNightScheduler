
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using MovieNightSheduler.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using MovieNightSheduler.Authorization;
using MovieNightSheduler.Services;


namespace MovieNightSheduler.Controllers
{
    using BCrypt.Net;
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {

         public AppDb Db { get; set; }
       // private readonly ILogger<UserController> _logger;
         private IUserService _userService;

        public UserController(AppDb db, IUserService userService)
        {
            Db = db;
            _userService = userService;
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
           //string username = User.Username;
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


        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserAuth model)
        {
            var user = await _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await Db.Connection.GetAsync<User>(id);
            return Ok(user);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(User newUser)
        {
            var query = "Insert into Users(username, password) values(@Username, @Password)";
            var parameters = new DynamicParameters();
            string passwordHash = BCrypt.HashPassword(newUser.Password);
            parameters.Add("Username", newUser.Username, DbType.String);
            parameters.Add("Password", passwordHash, DbType.String);
            try
            {
                int rows = await Db.Connection.ExecuteAsync(query, parameters);
                if (rows == 0) return BadRequest("Please use a unique name");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
           

            return Ok();

        }
        [HttpPut]
        //assumes Id is provided in user object
        public async Task<IActionResult> UpdateUser(User User)
        {
            //   var query = "update Users set password = @password where username=@username";
            /*     var parameters = new DynamicParameters();
                 parameters.Add("Username", User.Username, DbType.String);
                 //   parameters.Add("Password", User.Password, DbType.String);
                 var query = "select Id from Users where username='asd'";
                 var results = await Db.Connection.QueryAsync<User>(query);
                 Console.WriteLine(results.ToList());*/


            //   await Db.Connection.ExecuteAsync(query, parameters);
            // await Db.Connection.Update
            if (User.Id == 0) return BadRequest("No Id");
            bool result = await Db.Connection.UpdateAsync(User);
            if (result) return Ok("Update Successful");
            else return BadRequest("Update Failed");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            bool result = await Db.Connection.DeleteAsync(new User() { Id = id });
            if (result) return Ok("Deletion Successful");
            else return BadRequest("Deletion Failed");
        }



    }
}
