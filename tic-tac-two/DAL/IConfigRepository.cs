using GameLogic;

namespace DAL;

public interface IConfigRepository
{
    List<string> GetSavedConfigurations();
    GameConfiguration GetConfigurationByName(string name);

    void SaveConfigurations();

    List<ConfigurationEntry> LoadConfigurations();

}