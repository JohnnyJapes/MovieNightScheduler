using System.Data;
using Dapper;
using Dapper.Contrib;
using System.Threading.Tasks;
using MySqlConnector;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace MovieNightScheduler.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AdminId { get; set; }
        public List<User> Users { get; set; }

        // internal AppDb Db { get; set; }

        /*internal Group(AppDb db)
        {
            Db = db;
        }
        private readonly DapperContext _context;
        public Group(DapperContext context) 
        {
            _context = context;
        }
        public async Task InsertAsync()
        {
            //await Db.Connection.OpenAsync();
            var query = "Select * from Users";

            var companies = Db.Connection.QueryAsync<Group>(query);

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `users` (`username`, `password`) VALUES (@title, @content);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            Id = (int)cmd.LastInsertedId;
        }
        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `BlogPost` SET `Title` = @title, `Content` = @content WHERE `Id` = @id;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `BlogPost` WHERE `Id` = @id;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = Id,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@title",
                DbType = DbType.String,
                Value = Name,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@content",
                DbType = DbType.String,
                Value = AdminId,
            });
        }*/
    }
    public class User
    {
        public User()
        {
            this.Groups = new List<Group>();
            this.RefreshTokens = new List<RefreshToken>();
        }
        public int Id { get; set; }
        public string Username { get; set; }
        public List<Group> Groups { get; set; }
        [JsonIgnore]
        public string PasswordHash { get; set; }
        [JsonIgnore]
        public List<RefreshToken> RefreshTokens { get; set; }
        // public string Last_name { get; set; } 
    }

    public class Events
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int Group_id { get; set; }

    }


}
