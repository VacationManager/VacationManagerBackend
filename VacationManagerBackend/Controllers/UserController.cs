using LoggerLibrary.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VacationManagerBackend.Interfaces.Helper;
using VacationManagerBackend.Interfaces.Repositories;
using VacationManagerBackend.Models;
using VacationManagerBackend.Models.Input;

namespace VacationManagerBackend.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;
        private readonly IVacationRepository _vacationRepository;
        private readonly IAccessTokenHelper _accessTokenHelper;
        private readonly IAccessTokenProvider _accessTokenProvider;

        public UserController(
            ILogger<UserController> logger,
            IUserRepository userRepository,
            IVacationRepository vacationRepository,
            IAccessTokenHelper accessTokenHelper,
            IAccessTokenProvider accessTokenProvider)
        {
            _logger = logger;
            _userRepository = userRepository;
            _vacationRepository = vacationRepository;
            _accessTokenHelper = accessTokenHelper;
            _accessTokenProvider = accessTokenProvider;
        }

        [HttpGet]
        public IActionResult GetUser()
        {
            var tokenPayload = _accessTokenProvider.GetTokenPayload();
            _logger.Info("Get User endpoint...", new { tokenPayload });

            if (tokenPayload != null)
            {
                var foundUser = _userRepository.GetUser(tokenPayload.UserId, null);

                if (foundUser != null)
                {
                    _logger.Info("Get User endpoint successful!", new { foundUser });
                    return Ok(foundUser);
                }
            }

            return Unauthorized();
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] InputUser user)
        {
            _logger.Info("Create User endpoint...", new { user });

            if (user != null)
            {
                var tokenPayload = _accessTokenProvider.GetTokenPayload();

                if (tokenPayload != null)
                {
                    if (tokenPayload.IsAdmin)
                    {
                        int createdUserId = _userRepository.CreateUser(user);

                        if (createdUserId > 0)
                        {
                            _logger.Info("Create User endpoint successful!", new { createdUserId });
                            return Ok(new { createdUserId });
                        }

                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }

                    return StatusCode(StatusCodes.Status403Forbidden);
                }

                return Unauthorized();
            }

            return BadRequest();
        }

        [HttpPatch]
        public IActionResult UpdateUser([FromBody] InputUser user)
        {
            _logger.Info("Update User...", new { user });

            if (user != null)
            {
                var tokenPayload = _accessTokenProvider.GetTokenPayload();

                if (tokenPayload != null)
                {
                    user.Id = tokenPayload.UserId;
                    _userRepository.SetUser(user, false);

                    _logger.Info("User successfully updated!", new { user, tokenPayload });
                    return NoContent();
                }

                return Unauthorized();
            }

            return BadRequest();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginInputData loginInputData)
        {
            _logger.Info("Login endpoint...", new { loginInputData });

            if (loginInputData != null && loginInputData.IsValid())
            {
                var foundUser = _userRepository.GetUser(null, loginInputData.EMailAddress);

                if (foundUser != null)
                {
                    if (foundUser.Password == loginInputData.Password)
                    {
                        var tokenPayload = new AccessTokenPayload(foundUser);
                        var at = _accessTokenHelper.GenerateAccessToken(tokenPayload);

                        var loginResult = new
                        {
                            tokenPayload.UserId,
                            DepartmendId = tokenPayload.DepartmentId,
                            tokenPayload.LastName,
                            tokenPayload.FirstName,
                            tokenPayload.ExpirationDate,
                            tokenPayload.IsManager,
                            tokenPayload.IsAdmin,
                            AccessToken = at
                        };

                        _logger.Info("Login endpoint successful!", new { tokenPayload });

                        return Ok(loginResult);
                    }

                    return Unauthorized();
                }

                return Unauthorized();
            }

            return BadRequest();
        }

        [HttpDelete]
        public IActionResult DeleteUser(int id)
        {
            _logger.Info("Delete User endpoint...", new { id });
            var tokenPayload = _accessTokenProvider.GetTokenPayload();

            if (tokenPayload != null)
            {
                if (tokenPayload.IsAdmin)
                {
                    _vacationRepository.DeleteVacationSlots(id);
                    _vacationRepository.DeleteVacationRequests(id);
                    _userRepository.DeleteUser(id);

                    _logger.Info("Delete User endpoint successful!", new { id });
                    return NoContent();
                }

                return StatusCode(StatusCodes.Status403Forbidden);
            }

            return Unauthorized();
        }
    }
}
