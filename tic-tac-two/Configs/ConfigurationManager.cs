namespace Configs
{
    public class ConfigurationManager
    {
        private List<ConfigurationEntry> Configurations { get; set; } // not in sync with the actual configs
        public GameConfiguration CurrentConfiguration { get; private set; }
        
        private readonly GameConfiguration _defaultConfiguration = new() {
            GameName = "Tic-Tac-Two",
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
            Configurations = ConfigRepositoryJson.LoadConfigurationsFromFile(); // problem
            InitializeDefaultConfigurations();
        }
        

        private void InitializeDefaultConfigurations()
        {
            if (Configurations.Count != 0) return;

            Configurations.Add(new ConfigurationEntry(_defaultConfiguration.GameName, _defaultConfiguration));

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
            Configurations.Add(new ConfigurationEntry(defaultConfig2.GameName, defaultConfig2));
                
            
            var defaultConfig3 = new GameConfiguration()
            {
                GameName = "Tic-Tac-Toe",
                Width = 3,
                Height = 3,
                Player1Symbol = 'X',
                Player2Symbol = 'O',
                StartingPlayer = "Player 1",
                WinningCondition = 3
            };
            Configurations.Add(new ConfigurationEntry(defaultConfig3.GameName,defaultConfig3));
            
            SaveAndUpdate();
        }

        private void SaveAndUpdate()
        {
            ConfigRepositoryJson.SaveConfigurationsToFile();
            Configurations = ConfigRepositoryJson.LoadConfigurationsFromFile();
        }

        private void SaveConfiguration(GameConfiguration config, string configName) // adds to the list and to the file
        {
            Configurations.RemoveAll(c => c.ConfigName == configName);
            Configurations.Add(new ConfigurationEntry(configName, config));
            SaveAndUpdate();
        }
        

        public GameConfiguration GetConfigurationByName(string configName)
        {
            var configEntry = Configurations.Find(c => c.ConfigName == configName);
            return configEntry!.Config; // only used when selecting config so it always exists
        }

        public void SetCurrentConfiguration(string configName)
        {
            var config = GetConfigurationByName(configName);
            CurrentConfiguration = config; 
        }

        public List<string> GetSavedConfigurations()
        {
            return Configurations.Select(configEntry => configEntry.ConfigName).ToList();
        }

        public void CreateConfiguration()
        {
            Console.Clear();
            Console.WriteLine("Game configuration creation:");

            var config = new GameConfiguration();

            Console.WriteLine("Name your configuration:");
            var name = Console.ReadLine() ?? $"Config nr {Configurations.Count + 1}";
            config.GameName = name;

            const int min = 2;
            const int max = 54; // max that console to display normally
            
            config.Width = GetValidInput("Enter the width of the board (at least 3):", min, max);
            config.Height = GetValidInput("Enter the height of the board (at least 3):", min, max);

            Console.WriteLine("Enter the symbol for Player 1 (default is X):");
            config.Player1Symbol = GetValidSymbol();
            
            Console.WriteLine("Enter the symbol for Player 2 (default is O):");
            
            config.Player2Symbol = GetValidSymbol(config.Player1Symbol);

            Console.WriteLine("Who should start? Enter '1' for Player 1 or '2' for Player 2:");
            while (true)
            {
                var startingPlayerChoice = Console.ReadLine();

                if (startingPlayerChoice is "1" or "2")
                {
                    config.StartingPlayer = startingPlayerChoice == "1" ? "Player 1" : "Player 2";
                    break;
                }

                Console.WriteLine("Invalid input. Please try again.");
            }
            if (config.Height != 3 && config.Width != 3) 
            {
                var boardCapacity = config.Width * config.Height;
                config.MovableGridSize = GetValidInput("Enter the size of the movable grid (always square):", min, Math.Min(config.Height, config.Width));
                config.WinningCondition = GetValidInput("Enter the winning condition of the game (more than 3):", 3, boardCapacity);
                config.InitialMoves =
                    GetValidInput("Enter the number of moves have to made before moving the pieces or the grid:", 0, boardCapacity);
                config.MaxPieces = GetValidInput("Enter the number of pieces every player has:", 0, boardCapacity);
            }
            else
            {
                config.MovableGridSize = _defaultConfiguration.MovableGridSize;
                config.WinningCondition = _defaultConfiguration.WinningCondition;
                config.InitialMoves = _defaultConfiguration.InitialMoves;
                config.MaxPieces = _defaultConfiguration.MaxPieces;
            }
            SaveConfiguration(config, name);
            Console.WriteLine($"Configuration '{name}' created successfully.");
            Console.ReadLine(); 
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
                Configurations.RemoveAt(configIndex - 1);
                Console.WriteLine($"Configuration '{selectedConfig}' deleted successfully.");

                SaveAndUpdate();
            }
            else
            {
                Console.WriteLine("Invalid input. Press Enter to return to the menu.");
            }

            Console.ReadLine();
        }

        private static int GetValidInput(string prompt, int minValue, int maxValue)
        {
            int result;
            Console.WriteLine(prompt);

            while (!int.TryParse(Console.ReadLine(), out result) || result <= minValue || result > maxValue)
            {
                Console.WriteLine($"Invalid input! Please enter a number between {minValue + 1} and {maxValue}:");
            }

            return result;
        }


        private static char GetValidSymbol(char player1Symbol = '\0')
        {
            while (true)
            {
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    return player1Symbol == '\0' ? 'X' : 'O';
                }

                if (char.TryParse(input, out var symbol) && !char.IsWhiteSpace(symbol))
                {
                    if (symbol == player1Symbol)
                    {
                        Console.WriteLine($"Player 2's symbol cannot be the same as Player 1's symbol ('{player1Symbol}'). Please try again:");
                    }
                    else
                    {
                        return symbol; 
                    }
                }
                else
                {
                    Console.WriteLine("Invalid symbol! Please enter a valid non-space character:");
                }
            }
        }
    }
}
