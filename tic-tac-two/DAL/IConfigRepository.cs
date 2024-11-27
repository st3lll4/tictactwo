using Domain;
using GameLogic;

namespace DAL;

public interface IConfigRepository
{
    List<string> GetConfigurationNames();
    bool DeleteConfiguration(string selectedConfig);

    void SaveConfiguration(GameConfiguration newConfig);
    public GameConfiguration GetConfigurationByName(string name);

    GameConfiguration DefaultConfiguration { get; set; }
    GameConfiguration DefaultConfiguration2 { get; set; } 
    GameConfiguration DefaultConfiguration3 { get; set; }

}