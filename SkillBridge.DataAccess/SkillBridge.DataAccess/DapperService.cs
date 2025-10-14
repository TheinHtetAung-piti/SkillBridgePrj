using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace SkillBridge.DataAccess
{
    public class DapperService 
    {
        private readonly SqlConnectionStringBuilder _sqlConnectionStringBuilder;

        public DapperService(IConfiguration configuration)
        {
            _sqlConnectionStringBuilder = new SqlConnectionStringBuilder(configuration.GetConnectionString("DbConnecton"));
        }

        public List<Collin> Query<Collin>(string query, object? parameters = null)
        {
            using IDbConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            var lst = connection.Query<Collin>(query, parameters).ToList();
            return lst;
        }

        public int Execute(string query, object? parameters = null)
        {
            using IDbConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            var result = connection.Execute(query, parameters);
            return result;
        }
    }
}
