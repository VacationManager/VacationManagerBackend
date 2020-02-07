using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using VacationManagerBackend.Interfaces.Helper;

namespace VacationManagerBackend.Controllers
{
    [Route("[controller]")]
    public class TestController : Controller
    {
        private readonly IDbHelper _dbHelper;

        public TestController(IDbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        [HttpGet("data")]
        public IActionResult Get()
        {
            using (var con = _dbHelper.GetConnection())
            {
                con.Execute("[spInsertTestData]", commandType: CommandType.StoredProcedure);
            }
            return Ok();
        }
    }
}