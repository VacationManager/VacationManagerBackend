using LoggerLibrary.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VacationManagerBackend.Interfaces.Helper;
using VacationManagerBackend.Interfaces.Repositories;
using VacationManagerBackend.Models.Input;

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

        [HttpPost]
        public IActionResult CreateDepartment([FromBody] InputDepartment department)
        {
            _logger.Info("Create Department endpoint...", new { department });
            var tokenPayload = _accessTokenProvider.GetTokenPayload();

            if (tokenPayload != null)
            {
                int createdDepartmentId = _departmentRepository.CreateDepartment(department.DepartmentName);

                if (createdDepartmentId > 0)
                {
                    _logger.Info("Create Department endpoint successful!", new { createdDepartmentId });
                    return Ok(new { createdDepartmentId });
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Unauthorized();
        }
    }
}
