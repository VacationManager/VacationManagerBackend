using LoggerLibrary.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationManagerBackend.Enums;
using VacationManagerBackend.Interfaces.Helper;
using VacationManagerBackend.Interfaces.Repositories;
using VacationManagerBackend.Models;
using VacationManagerBackend.Models.Dto;

namespace VacationManagerBackend.Controllers
{
    [Route("[controller]")]
    public class VacationController : Controller
    {
        private readonly ILogger _logger;
        private readonly IAccessTokenProvider _accessTokenProvider;
        private readonly IVacationRepository _vacationRepository;

        public VacationController(
            ILogger<VacationController> logger,
            IAccessTokenProvider accessTokenProvider,
            IVacationRepository vacationRepository)
        {
            _logger = logger;
            _accessTokenProvider = accessTokenProvider;
            _vacationRepository = vacationRepository;
        }

        [HttpGet]
        public IActionResult GetUserVacationRequests()
        {
            var tokenPayload = _accessTokenProvider.GetTokenPayload();
            _logger.Info("Get UserVacationRequests endpoint...", new { tokenPayload });

            if (tokenPayload != null)
            {
                var foundVacationRequests = _vacationRepository.GetUserVacationRequests(tokenPayload.UserId);

                if (foundVacationRequests != null && foundVacationRequests.Count > 0)
                {
                    _logger.Info("Get UserVacationRequests endpoint successful!", new { foundVacationRequests.Count });
                    return Ok(foundVacationRequests);
                }

                return NoContent();
            }

            return Unauthorized();
        }

        [HttpPost]
        public IActionResult Post([FromBody] VacationRequest request)
        {
            if (request == null)
            {
                // TODO: log error
                return BadRequest();
            }
            var user = _accessTokenProvider.GetTokenPayload();
            if (user == null)
            {
                return Unauthorized();
            }

            request.UserId = user.UserId;
            _vacationRepository.CreateVacationRequest(request);
            return Ok();
        }

        [HttpPatch]
        public IActionResult Patch([FromBody] VacationRequestDto request)
        {
            var user = _accessTokenProvider.GetTokenPayload();
            if (user == null)
            {
                return Unauthorized();
            }
            if (!user.IsManager)
            {
                return StatusCode(403);
            }
            if (request?.NewState == null || !Enum.IsDefined(typeof(VacationRequestState), request.NewState))
            {
                return BadRequest();
            }
            if (!_vacationRepository.UpdateVacationRequest(request, user.UserId))
            {
                return StatusCode(403);
            }
            return Ok();
        }
    }
}
