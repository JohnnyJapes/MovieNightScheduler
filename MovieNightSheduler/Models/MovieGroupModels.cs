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
        public Group()
        {
            this.Users = new List<User>();
            this.Events = new List<Event>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public User AdminId { get; set; }
        public string? Description { get; set; }
        public List<User> Users { get; set; }
        public List<Viewing> Viewings { get; set; }

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

    public class Viewing
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int Group_id { get; set; }

    }


}
