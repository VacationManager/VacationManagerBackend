using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using VacationManagerBackend.Interfaces.Helper;
using VacationManagerBackend.Models.Config;

namespace VacationManagerBackend.Helper
{
    public class DbHelper : IDbHelper
    {
        private readonly DbConfig _dbConfig;

        public DbHelper(IOptions<DbConfig> dbOpitons)
        {
            _dbConfig = dbOpitons.Value;
        }

        public IDbConnection GetConnection()
        {
            var con = new SqlConnection();
            con.Open();

            return con;
        }
    }
}
