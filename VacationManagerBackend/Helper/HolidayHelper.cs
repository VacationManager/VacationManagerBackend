using LoggerLibrary.Extension;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VacationManagerBackend.Interfaces.Helper;
using VacationManagerBackend.Models.Holidays;

namespace VacationManagerBackend.Helper
{
    public class HolidayHelper : IHolidayHelper
    {
        private readonly ILogger _logger;

        public HolidayHelper(ILogger<HolidayHelper> logger)
        {
            _logger = logger;
        }

        public async Task<HolidayResult> GetHolidays(int year)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("X-DFA-Token", "dfa");
                try
                {
                    var response = await client.PostAsync($"https://deutsche-feiertage-api.de/api/v1/{year}", null);
                    var result = JsonConvert.DeserializeObject<HolidayResult>(await response.Content.ReadAsStringAsync());
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.Error("Failed to get holidays", new { year, stateus = response.StatusCode, result });
                        return new HolidayResult();
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    _logger.Error("Error while fetching holidays", new { year, ex });
                    return new HolidayResult();
                }
            }
        }
    }
}
