using LoggerLibrary.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VacationManagerBackend.Interfaces.Helper;
using VacationManagerBackend.Interfaces.Repositories;
using VacationManagerBackend.Models;

namespace VacationManagerBackend.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        ILogger _logger;
        IUserRepository _userRepository;
        IAccessTokenHelper _accessTokenHelper;
        IAccessTokenProvider _accessTokenProvider;

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
