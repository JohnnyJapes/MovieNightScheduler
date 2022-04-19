using MovieNightScheduler.Models;

using MovieNightScheduler.Authorization;
using System.Data;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MovieNightScheduler.Services
{
    using Dapper;
    using MovieNightScheduler.Helpers;
    using Dapper.Contrib.Extensions;
    public interface IGroupService
    {
        Task<Group> GetGroupById(int id);
        Task<Group> GetAdminId(int id);
        Task<Group> GetGroupWithMembers(int id);
        Task<Group> GetGroupByName(string name);
        Task<int> CreateGroup(Group group);
        Task<Group> GetGroupWithViewings(int id);
    }

    public class GroupService : IGroupService
    {

        //
        public AppDb Db { get; set; }
        private readonly AppSettings _appSettings;

        public GroupService(AppDb db, IOptions<AppSettings> appSettings)
        {
            Db = db;
            _appSettings = appSettings.Value;
        }
        public async Task<Group> GetGroupById(int id)
        {
            string query = "select id, name, description from movie_groups where id=@id";

            var results = await Db.Connection.QueryAsync<Group>(query, new { @id = id });

            return results.First();
        }
        public async Task<Group> GetAdminId(int id)
        {
            string query = "select id, admin_id from movie_groups where id=@id";

            var results = await Db.Connection.QueryAsync<Group>(query, new { @id = id });

            return results.First();
        }
        public async Task<int> CreateGroup(Group group)
        {
            string query = "Insert Into movie_groups(name, description, admin_id) values(@name, @description, @adminId)";

            var changed = await Db.Connection.ExecuteAsync(query, group);
            if (changed != 1)
                throw new AppException("Group Creation Failed on Database insertion");
            return changed;
        }

        public async Task<Group> GetGroupByName(string name)
        {
            string query = "select id, name, description from movie_groups where name=@name";

            var results = await Db.Connection.QueryAsync<Group>(query, new { @name = name });

            return results.First();
        }

        public async Task<Group> GetGroupWithMembers(int id)
        {
            string query = "select mg.id, admin_id, name, description, users.id, username from movie_groups as mg " +
                "left join group_members on group_id=mg.id left join users on users.id=user_id where mg.id=@id";
            var results = await Db.Connection.QueryAsync<Group, User,/* Viewing,*/ Group>(query,
                (group, user/*, viewing*/) => { group.Users.Add(user); /*group.Viewings.Add(viewing);*/ return group; }, new { @id = id });

            var result = results.GroupBy(r => r.Id).Select(g =>
            {
                var groupedUser = g.First();
                groupedUser.Users = g.Select(r => r.Users.Single()).ToList();
                //groupedUser.Viewings = g.Select(r => r.Viewings.Single()).ToList();
                return groupedUser;
            });

            return result.First();
        }
        public async Task<Group> GetGroupWithViewings(int id)
        {
            string query = "select mg.id, admin_id, name, description, viewings.id, title, description, date from movie_groups as mg " +
                "left join group_members on group_id=mg.id left join users on users.id=user_id where mg.id=@id";
            var results = await Db.Connection.QueryAsync<Group, User,/* Viewing,*/ Group>(query,
                (group, user/*, viewing*/) => { group.Users.Add(user); /*group.Viewings.Add(viewing);*/ return group; }, new { @id = id });

            var result = results.GroupBy(r => r.Id).Select(g =>
            {
                var groupedUser = g.First();
                groupedUser.Users = g.Select(r => r.Users.Single()).ToList();
                //groupedUser.Viewings = g.Select(r => r.Viewings.Single()).ToList();
                return groupedUser;
            });

            return result.First();

        }
    }
}