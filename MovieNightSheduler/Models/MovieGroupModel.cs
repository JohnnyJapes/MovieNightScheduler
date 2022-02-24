namespace MovieNightSheduler.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }   
        public int AdminId { get; set; }
    }
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }   
        public string First_name { get; set; } 
        public string Last_name { get; set; } 
    }
}
