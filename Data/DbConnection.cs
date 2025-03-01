using System;
using System.Data;
using System.Data.SqlClient;

namespace CasaLannister.Data
{
    public class DbConnection
    {
        private readonly string _connectionString;

        public DbConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}