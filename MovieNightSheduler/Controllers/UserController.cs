
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using MovieNightScheduler.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using MovieNightScheduler.Authorization;
using MovieNightScheduler.Services;


namespace MovieNightScheduler.Controllers
{
    using BCrypt.Net;
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {

         public AppDb Db { get; set; }
       // private readonly ILogger<UserController> _logger;
         private IUserService _userService;
        private User currentUser => (User)HttpContext.Items["User"];

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
        public async Task<IActionResult> Authenticate([FromBody] AuthRequest model)
        {
            var response = await _userService.Authenticate(model, ipAddress());
            setTokenCookie(response.RefreshToken);
            /*            if (response == null)
                            return BadRequest(new { message = "Username or password is incorrect" });
            */
            return Ok(response);
        }
        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await _userService.RefreshToken(refreshToken, ipAddress());
            setTokenCookie(response.RefreshToken);
            return Ok(response);

        }
        [Authorize]
        [HttpPost("revoke-token")]
        public IActionResult RevokeToken(RevokeTokenRequest model)
        {
            // accept refresh token in request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            _userService.RevokeToken(token, ipAddress());
            return Ok(new { message = "Token revoked" });
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            //var user = await Db.Connection.GetAsync<User>(id);
            var user = await _userService.GetById(id);
            return Ok(user);
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> CreateUser(AuthRequest newUser)
        {
            var query = "Insert into Users(username, passwordHash) values(@Username, @PasswordHash)";
            var parameters = new DynamicParameters();
            string passwordHash = BCrypt.HashPassword(newUser.Password);
            parameters.Add("Username", newUser.Username, DbType.String);
            parameters.Add("PasswordHash", passwordHash, DbType.String);
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
        public async Task<IActionResult> UpdateUser(User user)
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
            if (currentUser.Id != user.Id)
                return new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized }; 
            if (user.Id == 0) return BadRequest("No Id");
            bool result = await Db.Connection.UpdateAsync(user);
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
        private void setTokenCookie(string token)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(5)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
        private string ipAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }



    }
}
