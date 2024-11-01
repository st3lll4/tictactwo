using System.Text.Json;
using GameLogic;

namespace DAL
{

    public class ConfigRepositoryJson : IConfigRepository
    {
        public GameConfiguration DefaultConfiguration { get; set; }


        public ConfigRepositoryJson()
        {
            DefaultConfiguration = new GameConfiguration
            {
                GameName = "Tic-Tac-Two",
                Width = 5,
                Height = 5,
                Player1Symbol = 'X',
                Player2Symbol = 'O',
                StartingPlayer = "Player 1",
                MovableGridSize = 3,
                WinningCondition = 3,
                InitialMoves = 1, // change back to 2
                MaxPieces = 3
            };
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
            SaveConfiguration(DefaultConfiguration);

            var defaultConfig2 = new GameConfiguration()
            {
                GameName = "Tic-Tac-Two with a 10x10 board",
                Width = 10,
                Height = 10,
                Player1Symbol = 'X',
                Player2Symbol = 'O',
                StartingPlayer = "Player 1",
                InitialMoves = 5,
                MaxPieces = 7, // idk
                MovableGridSize = 5,
                WinningCondition = 5
            };
            SaveConfiguration(defaultConfig2);
            
            var defaultConfig3 = new GameConfiguration()
            {
                GameName = "Tic-Tac-Toe",
                Width = 3,
                Height = 3,
                Player1Symbol = 'X',
                Player2Symbol = 'O',
                StartingPlayer = "Player 1",
                WinningCondition = 3,
                MovableGridSize = 3
            };
            SaveConfiguration(defaultConfig3);
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
        
        
        public List<GameConfiguration> GetAllConfigurations()
        {
            var configurations = new List<GameConfiguration>();

            foreach (var fullFileName in Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.ConfigExtension))
            {
                try
                {
                    var configJsonStr = File.ReadAllText(fullFileName);
                    var config = JsonSerializer.Deserialize<GameConfiguration>(configJsonStr);
                    if (config != null)
                    {
                        configurations.Add(config);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading configuration from file {fullFileName}: {ex.Message}");
                }
            }
            return configurations;
        }
        

        public GameConfiguration GetConfigurationByName(string name)
        {
            var configJsonStr = File.ReadAllText(FileHelper.BasePath + name + FileHelper.ConfigExtension);
            var config = JsonSerializer.Deserialize<GameConfiguration>(configJsonStr);
            return config!;
        }
        

        public void SaveConfiguration(GameConfiguration config)
        {
            var configJsonStr = JsonSerializer.Serialize(config);
            var safeFileName = string.Join("_", config.GameName.Split(Path.GetInvalidFileNameChars()));
            File.WriteAllText(FileHelper.BasePath + safeFileName + FileHelper.ConfigExtension, configJsonStr);
        }

        
        public void DeleteConfiguration(string name)
        {
            var filePath = FileHelper.BasePath + name + FileHelper.ConfigExtension;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Console.WriteLine($"Configuration '{name}' has been deleted.");
            }
            else
            {
                Console.WriteLine($"Configuration '{name}' not found.");
            }
        }
    }
}