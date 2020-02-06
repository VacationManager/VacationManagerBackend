using LoggerLibrary.Extension;
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
        private readonly IAccessTokenHelper _accessTokenHelper;
        private readonly IAccessTokenProvider _accessTokenProvider;

        public UserController(
            ILogger<UserController> logger,
            IUserRepository userRepository,
            IAccessTokenHelper accessTokenHelper,
            IAccessTokenProvider accessTokenProvider)
        {
            _logger = logger;
            _userRepository = userRepository;
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
    }
}
