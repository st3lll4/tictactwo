using System.Text.Json;

namespace GameLogic
{
    public class ConfigurationManager
    {
        private const string ConfigFilePath = "configurations.json";
        private List<ConfigurationEntry> _configurations = [];

        public GameConfiguration CurrentConfiguration { get; private set; }
        
        private readonly GameConfiguration _defaultConfiguration = new() {
            GameName = "Default",
            Width = 5,
            Height = 5,
            Player1Symbol = 'X',
            Player2Symbol = 'O',
            StartingPlayer = "Player 1",
            MovableGridSize = 3,
            WinningCondition = 3,
            InitialMoves = 2,
            MaxPieces = 3
        };


        public ConfigurationManager()
        {
            CurrentConfiguration = _defaultConfiguration;
            LoadConfigurationsFromFile();
            InitializeDefaultConfigurations();
        }

        private void LoadConfigurationsFromFile()
        {
            if (File.Exists(ConfigFilePath))
            {
                try
                {
                    var json = File.ReadAllText(ConfigFilePath);
                    var loadedConfigs = JsonSerializer.Deserialize<List<ConfigurationEntry>>(json);

                    if (loadedConfigs != null)
                    {
                        _configurations = loadedConfigs;
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
        }

        private void InitializeDefaultConfigurations()
        {
            if (_configurations.Count != 0) return;

            _configurations.Add(new ConfigurationEntry { ConfigName = _defaultConfiguration.GameName, Config = _defaultConfiguration });

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
            _configurations.Add(new ConfigurationEntry { ConfigName = defaultConfig2.GameName, Config = defaultConfig2 });
                
            
            // TODO: if this conf is chosen, dont offer options, only offer moves
            
            var defaultConfig3 = new GameConfiguration()
            {
                GameName = "Tic-Tac-Toe",
                Width = 3,
                Height = 3,
                Player1Symbol = 'X',
                Player2Symbol = 'O',
                StartingPlayer = "Player 1",
                InitialMoves = 5, // normal tic-tac-toe so no grid moving is allowed
                MaxPieces = 5,
                MovableGridSize = 3,
                WinningCondition = 3
            };
            _configurations.Add(new ConfigurationEntry { ConfigName = defaultConfig3.GameName, Config = defaultConfig3 });
            
            SaveConfigurationsToFile();
        }

        private void SaveConfiguration(GameConfiguration config, string configName)
        {
            _configurations.RemoveAll(c => c.ConfigName == configName);
            _configurations.Add(new ConfigurationEntry { ConfigName = configName, Config = config });
            Console.WriteLine($"Configuration '{configName}' saved successfully.");
            SaveConfigurationsToFile();
        }

        private void SaveConfigurationsToFile()
        {
            try
            {
                var json = JsonSerializer.Serialize(_configurations);
                File.WriteAllText(ConfigFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving configurations: {ex.Message}");
            }
        }

        private GameConfiguration GetConfigurationByName(string configName)
        {
            var configEntry = _configurations.Find(c => c.ConfigName == configName);
            if (configEntry == null)
            {
                Console.WriteLine($"Configuration '{configName}' not found.");
                return null;
            }

            return configEntry.Config;
        }

        public void SetCurrentConfiguration(string configName)
        {
            var config = GetConfigurationByName(configName);
            CurrentConfiguration = config; 
        }

        public List<string> GetSavedConfigurations()
        {
            List<string> configNames = new List<string>();
            foreach (var configEntry in _configurations)
            {
                configNames.Add(configEntry.ConfigName);
            }
            return configNames;
        }

        public void CreateConfiguration()
        {
            Console.Clear();
            Console.WriteLine("Game configuration creation:");

            var config = new GameConfiguration();

            Console.WriteLine("Name your configuration:");
            var name = Console.ReadLine() ?? $"Config nr {_configurations.Count + 1}";
            config.GameName = name;

            config.Width = GetValidInput("Enter the width of the board (at least 3):", 3, 100);
            config.Height = GetValidInput("Enter the height of the board (at least 3):", 3, 100);

            Console.WriteLine("Enter the symbol for Player 1 (default is X):");
            config.Player1Symbol = GetValidSymbol();

            Console.WriteLine("Enter the symbol for Player 2 (default is O):");
            config.Player2Symbol = GetValidSymbol();

            Console.WriteLine("Who should start? Enter '1' for Player 1 or '2' for Player 2:");
            while (true)
            {
                Console.WriteLine("Enter '1' for Player 1 or '2' for Player 2:");
                var startingPlayerChoice = Console.ReadLine();

                if (startingPlayerChoice is "1" or "2")
                {
                    config.StartingPlayer = startingPlayerChoice == "1" ? "Player 1" : "Player 2";
                    break;
                }

                Console.WriteLine("Invalid input. Please try again.");
            }
            if (config is { Height: > 3, Width: > 3 })
            {
                var boardCapacity = config.Width * config.Height;
                config.MovableGridSize = GetValidInput("Enter the size of the movable grid (always square)", 3, boardCapacity);
                config.WinningCondition = GetValidInput("Enter the winning condition of the game (more than 3)", 3, boardCapacity);
                config.InitialMoves =
                    GetValidInput("Enter the number of moves have to made before moving the pieces or the grid", 0, boardCapacity);
                config.MaxPieces = GetValidInput("Enter the number of pieces every player has", 0, boardCapacity);
            }
            else
            {
                config.MovableGridSize = _defaultConfiguration.MovableGridSize;
                config.WinningCondition = _defaultConfiguration.WinningCondition;
                config.InitialMoves = _defaultConfiguration.InitialMoves;
                config.MaxPieces = _defaultConfiguration.MaxPieces;
            }
            SaveConfiguration(config, name);
        }

        public void DeleteConfiguration()
        {
            var configNames = GetSavedConfigurations();

            if (configNames.Count == 0)
            {
                Console.WriteLine("No configurations to delete. Press Enter to return.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Saved Configurations:");
            for (int i = 0; i < configNames.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {configNames[i]}");
            }

            Console.Write("Enter the number of the configuration to delete: ");
            if (int.TryParse(Console.ReadLine(), out var configIndex) && configIndex >= 1 && configIndex <= configNames.Count)
            {
                var selectedConfig = configNames[configIndex - 1];
                _configurations.RemoveAt(configIndex - 1);
                Console.WriteLine($"Configuration '{selectedConfig}' deleted successfully.");

                SaveConfigurationsToFile();
            }
            else
            {
                Console.WriteLine("Invalid input. Press Enter to return to the menu.");
            }

            Console.ReadLine();
        }

        private int GetValidInput(string prompt, int minValue, int maxValue)
        {
            int result;
            Console.WriteLine(prompt);

            // Keep asking until a valid integer between minValue and maxValue is entered
            while (!int.TryParse(Console.ReadLine(), out result) || result <= minValue || result > maxValue)
            {
                Console.WriteLine($"Invalid input! Please enter a number between {minValue + 1} and {maxValue}:");
            }

            return result;
        }


        private static char GetValidSymbol()
        {
            char symbol;
            while (!char.TryParse(Console.ReadLine(), out symbol) || char.IsWhiteSpace(symbol))
            {
                Console.WriteLine("Invalid symbol! Please enter a valid non-space character:");
            }
            return symbol;
        }
    }
}
