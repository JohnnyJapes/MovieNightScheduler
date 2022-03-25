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

    }