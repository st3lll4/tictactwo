namespace Configs;

public interface IConfigRepository
{
    List<string> GetSavedConfigurations();
    GameConfiguration GetConfigurationByName(string name);

    void SaveConfigurations();

    List<ConfigurationEntry>? LoadConfigurations();

}