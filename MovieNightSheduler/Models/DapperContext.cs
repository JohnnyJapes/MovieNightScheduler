using MySqlConnector;
using Microsoft.Extensions.Configuration;
using System.Data;
using System;

namespace MovieNightSheduler.Models
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("ConnectionStrings:Default");
        }

        public IDbConnection CreateConnection()
            => new MySqlConnection(_connectionString);


    }
}
