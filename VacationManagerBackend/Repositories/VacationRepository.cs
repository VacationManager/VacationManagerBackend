using Microsoft.Extensions.Logging;
using VacationManagerBackend.Interfaces.Helper;
using VacationManagerBackend.Interfaces.Repositories;

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
    }
}
