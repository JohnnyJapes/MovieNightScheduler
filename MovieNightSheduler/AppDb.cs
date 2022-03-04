using System;
using MySqlConnector;
using System.Data;

namespace MovieNightScheduler
{
    public class AppDb : IDisposable
    {
        public MySqlConnection Connection { get; }
       // public IDbConnection Connection;

        public AppDb(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
        }

        public void Dispose() => Connection.Dispose();
    }
}
