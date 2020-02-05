using Dapper;
using LoggerLibrary.Extension;
using Microsoft.Extensions.Logging;
using VacationManagerBackend.Interfaces.Helper;
using VacationManagerBackend.Interfaces.Repositories;
using VacationManagerBackend.Models;

namespace VacationManagerBackend.Repositories
{
    public class VacationRepository : IVacationRepository
    {
        IDbHelper _dbHelper;
        ILogger _logger;

        public VacationRepository(
            IDbHelper dbHelper,
            ILogger<VacationRepository> logger)
        {
            _dbHelper = dbHelper;
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
    }
}
