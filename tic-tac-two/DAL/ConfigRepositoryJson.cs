using System.Text.Json;
using GameLogic;

namespace DAL;

public class ConfigRepositoryJson : IConfigRepository
{
    
    private const string ConfigFilePath = "configurations.json";
        private List<ConfigurationEntry> Configurations { get; set; }

        public ConfigRepositoryJson()
        {
            Configurations = LoadConfigurations();
        }
    

        public List<ConfigurationEntry> LoadConfigurations()
        {
            if (File.Exists(ConfigFilePath))
            {
                try
                {
                    var json = File.ReadAllText(ConfigFilePath);
                    var loadedConfigs = JsonSerializer.Deserialize<List<ConfigurationEntry>>(json);

                    if (loadedConfigs != null)
                    {
                        Configurations = loadedConfigs;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading configurations: {ex.Message}");
                }
            }
            else // mostly will never get here on my PC
            {
                Console.WriteLine("No previous configurations found.");
            }

            return Configurations; 
        }
        

        public void SaveConfigurations() // problem
        {
            try
            {
                var json = JsonSerializer.Serialize(Configurations);
                File.WriteAllText(ConfigFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving configurations: {ex.Message}");
            }
        }

        public GameConfiguration GetConfigurationByName(string configName)
        {
            var configEntry = Configurations.Find(c => c.ConfigName == configName);
            return configEntry!.Config; // only used when selecting config so it always exists
        }


        public List<string> GetSavedConfigurations()
        {
            return Configurations.Select(configEntry => configEntry.ConfigName).ToList();
        }
}