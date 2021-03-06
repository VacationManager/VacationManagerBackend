﻿using LoggerLibrary.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
        private readonly IUserRepository _userRepository;
        private readonly IMailHelper _mailHelper;

        public VacationController(
            ILogger<VacationController> logger,
            IAccessTokenProvider accessTokenProvider,
            IVacationRepository vacationRepository,
            IUserRepository userRepository,
            IMailHelper mailHelper)
        {
            _logger = logger;
            _accessTokenProvider = accessTokenProvider;
            _vacationRepository = vacationRepository;
            _userRepository = userRepository;
            _mailHelper = mailHelper;
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

        [HttpGet("pending")]
        public IActionResult GetPendingRequests()
        {
            var user = _accessTokenProvider.GetTokenPayload();
            if (user == null) return Unauthorized();
            if (!user.IsAdmin && !user.IsManager)
            {
                _logger.Warning("User not allowed to get pending requests", new { user });
                return StatusCode(403);
            }
            var requests = _vacationRepository.GetPendingRequests(user.UserId);
            if (requests?.Count > 0)
                return Ok(requests);

            return NoContent();
        }

        [HttpPost]
        public IActionResult Post([FromBody] VacationRequest request)
        {
            var user = _accessTokenProvider.GetTokenPayload();
            if (user == null)
            {
                return Unauthorized();
            }
            if (request == null)
            {
                _logger.Warning("Missing vacation request body", new { user });
                return BadRequest();
            }

            request.UserId = user.UserId;
            _vacationRepository.CreateVacationRequest(request);
            return StatusCode(201);
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
                _logger.Warning("Invalid request update", new { user, request });
                return BadRequest();
            }
            if (!_vacationRepository.UpdateVacationRequest(request, user.UserId))
            {
                return StatusCode(403);
            }

            if (request.NewState != VacationRequestState.Pending)
            {
                var updatedRequest = _vacationRepository.GetVacationRequest(request.RequestId);
                var requestUser = _userRepository.GetUser(updatedRequest.UserId, null);
                _mailHelper.SendVacationRequestStateMail(requestUser, updatedRequest);
            }

            return Accepted();
        }
    }
}
