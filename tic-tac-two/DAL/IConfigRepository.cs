using Domain;
using GameLogic;

namespace DAL;

public interface IConfigRepository
{
    List<string> GetConfigurationNames();
    bool DeleteConfiguration(string selectedConfig);

    void SaveConfiguration(GameConfiguration newConfig);
    public void SaveConfiguration(GameConfiguration config, string userName);
    public GameConfiguration GetConfigurationByName(string name);
    public List<string> GetConfigsByUser(string userName);
 
    GameConfiguration DefaultConfiguration { get; set; }
    GameConfiguration DefaultConfiguration2 { get; set; } 
    GameConfiguration DefaultConfiguration3 { get; set; }

}