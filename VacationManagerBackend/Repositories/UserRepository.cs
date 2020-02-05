using VacationManagerBackend.Interfaces.Helper;

namespace VacationManagerBackend.Repositories
{
    public class UserRepository
    {
        IDbHelper _dbHelper;

        public UserRepository(IDbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
    }
}
