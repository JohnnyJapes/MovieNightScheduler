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
using MovieNightScheduler.Helpers;


namespace MovieNightScheduler.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ViewingController : Controller
    {
        public AppDb Db { get; set; }
        // private readonly ILogger<UserController> _logger;
        private User currentUser => (User)HttpContext.Items["User"];

        public ViewingController(AppDb db)
        {
            Db = db;
        }

        [HttpGet]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetViewing(int id)
        {
            if (id == null)
                throw new AppException("Invalid Viewing");
            var result = await Db.Connection.GetAsync<Viewing>(id);
            return Ok(result);
        }
        [HttpPost]
        public async void CreateViewing(Viewing newViewing)
        {
            var result = await Db.Connection.QueryAsync<Group>("select admin_id from movie_groups where id = @id", new { @id = newViewing.Group_id });
            if (currentUser.Id != result.First().AdminId)
                return;
            string query = "insert into viewings(title, description, date, group_id) values(@title, @description, @date, @group_id)";
            Db.Connection.ExecuteAsync(query, newViewing);
            return;
        }
    }
}
