namespace Configs
{
    public static class ConfigurationManager
    {
        private static List<ConfigurationEntry>? Configurations { get; set; } 
        public static GameConfiguration CurrentConfiguration { get; private set; }
        
        private static readonly GameConfiguration DefaultConfiguration = new() {
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


        static ConfigurationManager()
        {
            CurrentConfiguration = DefaultConfiguration;
            InitializeDefaultConfigurations();
        }
        
        private void SaveAndUpdate()
        {
            ConfigRepositoryJson.SaveConfigurations(); 
            Configurations = ConfigRepositoryJson.LoadConfigurations();
        }

        private static void InitializeDefaultConfigurations()
        {
            if (Configurations.Count != 0) return;

            Configurations.Add(new ConfigurationEntry(DefaultConfiguration.GameName, DefaultConfiguration));

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

        public void SelectConfiguration() // moved from menu
        {
            var savedConfigs = GetSavedConfigurations(); 

            Console.WriteLine("Enter the number of the configuration you want to play with (Press Enter to go back):");
            for (int i = 0; i < savedConfigs.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {savedConfigs[i]}");
            }
            
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return;
            }

            if (int.TryParse(input, out var configIndex) && configIndex >= 1 && configIndex <= savedConfigs.Count)
            {
                var selectedConfig = savedConfigs[configIndex - 1];
                SetCurrentConfiguration(selectedConfig);
                ShowMessage($"Configuration '{selectedConfig}' selected for future games.");
            }
            else
            {
                ShowMessage("Invalid input. Press Enter to return to the menu.");
            }
        }
        
        public void SeeConfigurations()
        {
            var savedConfigs = GetSavedConfigurations();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Your saved configurations:");
                for (var i = 0; i < savedConfigs.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {savedConfigs[i]}");
                }

                Console.WriteLine("Enter the number of the configuration you want to view in detail, or press Enter to go back:");

                var input = Console.ReadLine();
                

                if (int.TryParse(input, out int choice) && choice >= 1 && choice <= savedConfigs.Count)
                {
                    var selectedConfigName = savedConfigs[choice - 1];
                    var selectedConfig = GetConfigurationByName(selectedConfigName);

                    DisplayConfigurationDetails(selectedConfig);
                    Console.ReadLine();
                }
                else
                {
                    ShowMessage("Invalid input. Please try again.");
                }
            }
        }
        
        private static void DisplayConfigurationDetails(GameConfiguration config)
        {
            Console.Clear();
            Console.WriteLine($"Configuration: {config.GameName}");
            Console.WriteLine($"Board width: {config.Width}");
            Console.WriteLine($"Board height: {config.Height}");
            Console.WriteLine($"Player 1 symbol: {config.Player1Symbol}");
            Console.WriteLine($"Player 2 symbol: {config.Player2Symbol}");
            Console.WriteLine($"Starting player: {config.StartingPlayer}");
            Console.WriteLine($"Movable grid size: {config.MovableGridSize}");
            Console.WriteLine($"Winning condition: {config.WinningCondition}");
            Console.WriteLine($"Initial moves before getting more options: {config.InitialMoves}");
            Console.WriteLine($"Max pieces per Player: {config.MaxPieces}");
            Console.WriteLine("\nPress Enter to return.");
        }


        private void SaveConfiguration(GameConfiguration config, string configName) // adds to the list and to the file
        {
            Configurations.RemoveAll(c => c.ConfigName == configName);
            Configurations.Add(new ConfigurationEntry(configName, config));
            SaveAndUpdate();
        }


        private GameConfiguration GetConfigurationByName(string configName)
        {
            var configEntry = Configurations.Find(c => c.ConfigName == configName);
            return configEntry!.Config; // only used when selecting config so it always exists
        }

        private void SetCurrentConfiguration(string configName)
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
                config.MovableGridSize = DefaultConfiguration.MovableGridSize;
                config.WinningCondition = DefaultConfiguration.WinningCondition;
                config.InitialMoves = DefaultConfiguration.InitialMoves;
                config.MaxPieces = DefaultConfiguration.MaxPieces;
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
        
        private static void ShowMessage(string message)
        {
            Console.WriteLine(message);
            Console.ReadLine();
        }
    }
}
