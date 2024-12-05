using System.Text.Json;
using GameLogic;

namespace DAL
{
    public class ConfigRepositoryJson : IConfigRepository
    {
        public GameConfiguration DefaultConfiguration { get; set; }
        public GameConfiguration DefaultConfiguration2 { get; set; }
        public GameConfiguration DefaultConfiguration3 { get; set; }

        public ConfigRepositoryJson()
        {
            DefaultConfiguration = DefaultConfigurations.DefaultConfiguration;
            DefaultConfiguration2 = DefaultConfigurations.DefaultConfiguration2;
            DefaultConfiguration3 = DefaultConfigurations.DefaultConfiguration3;
            CheckAndCreateInitialDirectory();
        }


        private void CheckAndCreateInitialDirectory()
        {
            if (!Directory.Exists(FileHelper.BasePath))
            {
                Directory.CreateDirectory(FileHelper.BasePath);
            }

            var data = Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.ConfigExtension).ToList();
            if (data.Count == 0)
            {
                InitializeDefaultConfigurations();
            }
        }


        private void InitializeDefaultConfigurations()
        {
            SaveConfiguration(DefaultConfigurations.DefaultConfiguration);
            SaveConfiguration(DefaultConfigurations.DefaultConfiguration2);
            SaveConfiguration(DefaultConfigurations.DefaultConfiguration3);
        }

        public void SaveConfiguration(GameConfiguration config)
        {
            var configJsonStr = JsonSerializer.Serialize(config, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            var safeFileName = string.Join("_", config.ConfigName.Split(Path.GetInvalidFileNameChars()));
            File.WriteAllText(FileHelper.BasePath + safeFileName + FileHelper.ConfigExtension, configJsonStr);
        }


        public List<string> GetConfigurationNames()
        {
            var result = new List<string>();
            foreach (var fullFileName in Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.ConfigExtension))
            {
                var primaryName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(fullFileName));
                result.Add(primaryName);
            }

            return result;
        }
        

        public List<string> GetConfigsByUser(string userName)
        {
            var result = new List<string>
            {
                DefaultConfiguration.ConfigName,
                DefaultConfiguration2.ConfigName,
                DefaultConfiguration3.ConfigName
            };

            var files = Directory.GetFiles(FileHelper.BasePath, userName + "*" + FileHelper.ConfigExtension);
            
            foreach (var file in files)
            {
                var primaryName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(file));
                result.Add(primaryName);
            }
            return result;
        }
        

        public GameConfiguration GetConfigurationByName(string name)
        {
            var configJsonStr = File.ReadAllText(FileHelper.BasePath + name + FileHelper.ConfigExtension);
            var config = JsonSerializer.Deserialize<GameConfiguration>(configJsonStr);
            return config!;
        }


        public void SaveConfiguration(GameConfiguration config, string userName)
        {
            var configJsonStr = JsonSerializer.Serialize(config, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            var safeFileName = string.Join("_", config.ConfigName.Split(Path.GetInvalidFileNameChars()));
            File.WriteAllText(FileHelper.BasePath + userName + "_" + safeFileName + FileHelper.ConfigExtension, configJsonStr);
        }


        public bool DeleteConfiguration(string name)
        {
            var filePath = FileHelper.BasePath + name + FileHelper.ConfigExtension;
            if (!File.Exists(filePath)) return false;
            File.Delete(filePath);
            return true;
        }
    }
}