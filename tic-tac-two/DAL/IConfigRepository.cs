using Domain;
using GameLogic;

namespace DAL;

public interface IConfigRepository
{
    List<string> GetConfigurationNames();
    GameConfiguration GetConfigurationByName(string name);
    bool DeleteConfiguration(string selectedConfig);

    void SaveConfiguration(GameConfiguration newConfig);
    List<GameConfiguration> GetAllConfigurations();
    GameConfiguration DefaultConfiguration { get; set; }
    GameConfiguration DefaultConfiguration2 { get; set; } 
    GameConfiguration DefaultConfiguration3 { get; set; }

}