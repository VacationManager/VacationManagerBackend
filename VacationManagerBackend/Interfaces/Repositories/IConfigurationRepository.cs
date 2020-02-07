using VacationManagerBackend.Models;

namespace VacationManagerBackend.Interfaces.Repositories
{
    public interface IConfigurationRepository
    {
        Configuration GetConfiguration();
        LoginResult SetupConfig(SetupData data);
    }
}
