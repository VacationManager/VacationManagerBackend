using Dapper;
using LoggerLibrary.Extension;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using VacationManagerBackend.Extension;
using VacationManagerBackend.Interfaces.Helper;
using VacationManagerBackend.Interfaces.Repositories;
using VacationManagerBackend.Models;
using VacationManagerBackend.Models.Dto;

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

        public List<VacationSlot> GetConfirmedVacationSlotsFromUser(int userId)
        {
            _logger.Info("Get confirmed VacationSlots from User...", new { userId });

            using (var conn = _dbHelper.GetConnection())
            {
                var dParams = new DynamicParameters();
                dParams.Add("@UserId", userId);

                const string query = @" SELECT	vs.[Date],
		                                        vs.[DayTimeType] AS IsAfternoon,
		                                        vs.[VacationRequestId]
                                        FROM viVacationSlot AS vs
                                        INNER JOIN viVacationRequest AS vr
                                        ON vr.Id = vs.VacationRequestId
                                        INNER JOIN viUser AS u
                                        ON u.Id = vr.UserId
                                        WHERE u.Id = @UserId
                                        AND vr.RequestState = 1 -- Is request confirmed?";

                var confirmedSlots = conn.Query<VacationSlot>(query, dParams).AsList();

                _logger.Info("Get confirmed VacationSlots from User...", new
                {
                    Amount = confirmedSlots != null ? confirmedSlots.Count : 0
                });

                return confirmedSlots;
            }
        }

        public List<VacationRequest> GetUserVacationRequests(int userId)
        {
            _logger.Info("Get UserVacationRequest...", new { userId });

            using (var conn = _dbHelper.GetConnection())
            {
                var dParams = new DynamicParameters();
                dParams.Add("@UserId", userId);

                const string query = @" SELECT vr.*
                                        FROM viVacationRequest AS vr
                                        WHERE vr.UserId = @UserId";

                var foundRequests = conn.Query<VacationRequest>(query, dParams).AsList();

                _logger.Info("Get UserVacationRequest result", new
                {
                    Amount = foundRequests != null ? foundRequests.Count : 0
                });

                return foundRequests;
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

        public List<UserRequest> GetPendingRequests(int managerId)
        {
            const string query = @"SELECT r.[Id]
                                        ,[StartTime]
                                        ,[EndTime]
                                        ,[RequestState]
                                        ,[Annotation]
                                        ,[UserId]
                                        ,[FirstName]
                                        ,[LastName]
                                        ,[MailAddress]
                                    FROM [viVacationRequest] r
                                    LEFT JOIN [viUser] u
                                    ON u.[Id] = r.[UserId]
                                    WHERE [RequestState] = 0
                                    AND [DepartmentId] = (SELECT [DepartmentId]
                                                            FROM [viUser]
                                                            WHERE [Id] = @managerId
                                                            AND [IsManager] = 1)";
            var param = new DynamicParameters(new { managerId });

            using (var con = _dbHelper.GetConnection())
            {
                return con.Query<UserRequest>(query, param).ToList();
            }
        }

        public bool UpdateVacationRequest(VacationRequestDto request, int userId)
        {
            const string cmd = "[spUpdateVacationRequest]";
            var param = new DynamicParameters(new
            {
                requestId = request.RequestId,
                newState = request.NewState,
                userId
            });
            param.Add("@isAllowed", direction: ParameterDirection.ReturnValue);
            using (var con = _dbHelper.GetConnection())
            {
                con.Execute(cmd, param, commandType: CommandType.StoredProcedure);
                return param.Get<int>("@isAllowed") == 1;
            }
        }

        public void DeleteVacationRequests(int userId)
        {
            _logger.Info("Delete VacationRequests...", new { userId });

            using (var conn = _dbHelper.GetConnection())
            {
                var dParams = new DynamicParameters();
                dParams.Add("@UserId", userId);

                conn.Execute("spDeleteVacationRequest", dParams, commandType: CommandType.StoredProcedure);
                _logger.Info("VacationRequests successfully deleted!");
            }
        }

        public void DeleteVacationSlots(int userId)
        {
            _logger.Info("Delete VacationSlots...", new { userId });

            using (var conn = _dbHelper.GetConnection())
            {
                var dParams = new DynamicParameters();
                dParams.Add("@UserId", userId);

                conn.Execute("spDeleteVacationSlot", dParams, commandType: CommandType.StoredProcedure);
                _logger.Info("VacationSlots successfully deleted!", new { userId });
            }
        }
    }
}
