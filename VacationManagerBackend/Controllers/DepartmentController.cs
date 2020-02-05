using LoggerLibrary.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VacationManagerBackend.Interfaces.Helper;
using VacationManagerBackend.Interfaces.Repositories;

namespace VacationManagerBackend.Controllers
{
    [Route("[controller]")]
    public class DepartmentController : Controller
    {
        ILogger _logger;
        IDepartmentRepository _departmentRepository;
        IAccessTokenProvider _accessTokenProvider;

        public DepartmentController(
            ILogger<DepartmentController> logger,
            IDepartmentRepository departmentRepository,
            IAccessTokenProvider accessTokenProvider)
        {
            _logger = logger;
            _departmentRepository = departmentRepository;
            _accessTokenProvider = accessTokenProvider;
        }

        [HttpGet]
        public IActionResult GetDepartments()
        {
            _logger.Info("Get Departments endpoint...");
            var tokenPayload = _accessTokenProvider.GetTokenPayload();

            if (tokenPayload != null)
            {
                var foundDepartments = _departmentRepository.GetDepartments();

                if (foundDepartments != null && foundDepartments.Count > 0)
                {
                    _logger.Info("Get Departments endpoint successful!", new
                    {
                        Amount = foundDepartments.Count
                    });

                    return Ok(foundDepartments);
                }

                return NoContent();
            }

            return Unauthorized();
        }
    }
}
