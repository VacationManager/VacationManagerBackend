using System.Data;

namespace VacationManagerBackend.Interfaces.Helper
{
    public interface IDbHelper
    {
        IDbConnection GetConnection();
    }
}
