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
    public class GroupController : Controller
    {

        public AppDb Db { get; set; }
        // private readonly ILogger<UserController> _logger;
        private IGroupService _groupService;
        private User currentUser => (User)HttpContext.Items["User"];

        public GroupController(AppDb db, IGroupService groupService)
        {
            Db = db;
            _groupService = groupService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroup(Group group)
        {
            if (group == null)
                throw new AppException("Invalid group");
            Group result;
            if (group.Id != null)
            {
                result = await _groupService.GetGroupById(group.Id);
            }
            else
                result = await _groupService.GetGroupByName(group.Name);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupEvents(int id)
        {
            Group result;
            result = await _groupService.GetGroupWithMembers(id);
            return Ok(result);
        }
        /*        [HttpGet]
                public async Task<IActionResult> GetFullGroup(int id)
                {
                    Group result;
                    result = await _groupService.GetGroupWithMembers(id);
                    return Ok(result);
                }*/
        [HttpPost]
        public async Task<IActionResult> CreateGroup(Group group)
        {
            group.AdminId = currentUser.Id;
            int changed = await _groupService.CreateGroup(group);
            return Ok("Group Creation Successful");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateGroup(Group group)
        {
            if (currentUser.Id != group.AdminId)
                return new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            if (group.Id == 0) return BadRequest("No Id");
            bool result = await Db.Connection.UpdateAsync(group);
            if (result) return Ok("Update Successful");
            else return BadRequest("Update Failed");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            Group currentGroup = await _groupService.GetAdminId(id);
            if (currentUser.Id != currentGroup.AdminId)
                return new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            else
            {
                string query = "delete from movie_groups where id=@id";
                int changed = await Db.Connection.ExecuteAsync(query, new { @id = id });
                Console.WriteLine(changed);
                if (changed > 0)
                    return Ok("Deletion Successful");
                else
                    return new JsonResult(new { message = "Failed to delete group" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }

    }
}