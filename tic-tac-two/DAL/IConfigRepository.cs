using GameLogic;

namespace DAL;

public interface IConfigRepository
{
    List<string> GetConfigurationNames();
    GameConfiguration GetConfigurationByName(string name);
    void DeleteConfiguration(string selectedConfig);

    void SaveConfiguration(GameConfiguration newConfig);
    List<GameConfiguration> GetAllConfigurations();
    GameConfiguration DefaultConfiguration { get; set; }
}