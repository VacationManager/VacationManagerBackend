using Dapper;
using LoggerLibrary.Extension;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using VacationManagerBackend.Extension;
using VacationManagerBackend.Interfaces.Helper;
using VacationManagerBackend.Interfaces.Repositories;
using VacationManagerBackend.Models;

namespace VacationManagerBackend.Repositories
{
    public class VacationRepository : IVacationRepository
    {
        private readonly IDbHelper _dbHelper;
        private readonly IHolidayHelper _holidayHelper;
        private readonly ILogger _logger;

        public VacationRepository(
            IDbHelper dbHelper,
            IHolidayHelper holidayHelper,
            ILogger<VacationRepository> logger)
        {
            _dbHelper = dbHelper;
            _holidayHelper = holidayHelper;
            _logger = logger;
        }

        public VacationRequest GetVacationRequest(int id)
        {
            _logger.Info("Get VacationRequest...", new { id });

            using (var conn = _dbHelper.GetConnection())
            {
                var dParams = new DynamicParameters();
                dParams.Add("@Id", id);

                const string query = @" SELECT vr.*
                                        FROM viVacationRequest AS vr
                                        WHERE vr.Id = @Id";

                var foundVacationReqest = conn.QueryFirstOrDefault<VacationRequest>(query, dParams);
                _logger.Info("Get VacationRequest result", new { foundVacationReqest });

                return foundVacationReqest;
            }
        }

        public async Task CreateVacationRequest(VacationRequest vacationRequest)
        {
            var holidayDates = new List<DateTime>();
            for (int year = vacationRequest.StartTime.Year; year <= vacationRequest.EndTime.Year; year++)
            {
                holidayDates.AddRange((await _holidayHelper.GetHolidays(year))?.Dates);
            }
            const string cmd = "[spCreateVacationRequest]";
            var param = new DynamicParameters(new
            {
                userId = vacationRequest.UserId,
                startTime = vacationRequest.StartTime,
                endTime = vacationRequest.EndTime,
                annotation = vacationRequest.Annotation,
                holidays = holidayDates.ToDayDataTable().AsTableValuedParameter()
            });
            param.Add("@result", direction: ParameterDirection.ReturnValue);

            using (var con = _dbHelper.GetConnection())
            {
                con.Execute(cmd, param, commandType: CommandType.StoredProcedure);
                var result = param.Get<int>("result");
            }
        }
    }
}
