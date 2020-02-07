using VacationManagerBackend.Models;

namespace VacationManagerBackend.Interfaces.Repositories
{
    public interface IConfigurationRepository
    {
        Configuration GetConfiguration();
        bool SetupConfig(SetupData data);
    }
}
