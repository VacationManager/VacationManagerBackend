using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationManagerBackend.Interfaces.Helper;
using VacationManagerBackend.Interfaces.Repositories;
using VacationManagerBackend.Models;

namespace VacationManagerBackend.Controllers
{
    [Route("[controller]")]
    public class VacationController : Controller
    {
        private readonly IAccessTokenProvider _accessTokenProvider;
        private readonly IVacationRepository _vacationRepository;

        public VacationController(IAccessTokenProvider accessTokenProvider, IVacationRepository vacationRepository)
        {
            _accessTokenProvider = accessTokenProvider;
            _vacationRepository = vacationRepository;
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
    }
}
