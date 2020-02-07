using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoggerLibrary.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VacationManagerBackend.Interfaces.Repositories;
using VacationManagerBackend.Models;

namespace VacationManagerBackend.Controllers
{
    public class SetupController : Controller
    {
        private readonly ILogger _logger;
        private readonly IConfigurationRepository _configurationRepository;

        public SetupController(ILogger<SetupController> logger, IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        [HttpGet("config")]
        public IActionResult Get()
        {
            var config = _configurationRepository.GetConfiguration();
            if (config == null)
            {
                _logger.Info("No config found");
                return NoContent();
            }
            return Ok(config);
        }


        [HttpPost]
        public IActionResult Post([FromBody] SetupData data)
        {
            if (data == null || !ModelState.IsValid)
            {
                _logger.Error("Invalid initialize data", new { data });
                return BadRequest();
            }
            if (_configurationRepository.SetupConfig(data))
            {
                _logger.Info("VacationManager initialized", new { setupData = data });
                return StatusCode(201);
            }
            return Conflict();
        }
    }
}