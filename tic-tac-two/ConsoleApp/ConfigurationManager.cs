using DAL;
using GameLogic;
using UI;

namespace tic_tac_two
{
    public class ConfigurationManager
    {
        private readonly ConfigRepositoryJson _configRepository = new();
        public GameConfiguration CurrentConfiguration { get; private set; } // use in game

        public ConfigurationManager()
        {
            CurrentConfiguration = _configRepository.DefaultConfiguration;
        }
        

        public string SelectConfiguration()
        {
            var configMenuItems = new List<MenuItem>();
            var configNames = _configRepository.GetConfigurationNames();

            for (var i = 0; i < configNames.Count; i++)
            {
                var index = i;
                configMenuItems.Add(new MenuItem()
                {
                    Title = configNames[i],
                    Shortcut = (i + 1).ToString(),
                    MenuItemAction = () => index.ToString()
                });
            }

            var configMenu = new Menu(
                EMenuLevel.Secondary,
                "Choose a configuration for future games:",
                configMenuItems,
                true
            );

            var selectedIndexStr = configMenu.Run();
            if (!int.TryParse(selectedIndexStr, out var selectedIndex)) return "No configuration selected.";
            var selectedConfigName = configNames[selectedIndex];
            var configManager = new ConfigurationManager();
            configManager.SetCurrentConfiguration(selectedConfigName);
            return $"Configuration '{selectedConfigName}' selected.";
        }


        public string SeeConfigurations()
        {
            var savedConfigs = _configRepository.GetAllConfigurations();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Your saved configurations:");
                for (var i = 0; i < savedConfigs.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {savedConfigs[i].GameName}");
                }
                Console.WriteLine("Enter the number of the configuration you want to view in detail, or press Enter to go back:");

                var input = Console.ReadLine();

                if (string.IsNullOrEmpty(input)) break;

                if (int.TryParse(input, out var choice) && choice >= 1 && choice <= savedConfigs.Count)
                {
                    var selectedConfig = savedConfigs[choice - 1];
                    DisplayConfigurationDetails(selectedConfig);
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Invalid input. Please try again.");
                    Console.ReadLine();
                }
            }

            return "Configuration viewing completed.";
        }
        

        private void DisplayConfigurationDetails(GameConfiguration config)
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
        

        public void SetCurrentConfiguration(string configName) // use in selecting
        {
            var config = _configRepository.GetConfigurationByName(configName);
            CurrentConfiguration = config;
        }

        public string AddConfiguration()
        {
            var newConfig = CreateConfiguration();

            _configRepository.SaveConfiguration(newConfig);

            Console.WriteLine($"Configuration '{newConfig.GameName}' has been created and saved.");

            return $"Configuration '{newConfig.GameName}' created successfully.";
        }

        private GameConfiguration CreateConfiguration()
        {
            Console.Clear();
            Console.WriteLine("Game configuration creation:");

            var config = new GameConfiguration();

            Console.WriteLine("Name your configuration:");
            var name = Console.ReadLine() ?? $"Config nr {_configRepository.GetAllConfigurations().Count + 1}";
            config.GameName = name;

            const int min = 3;
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

            if (config.Height != 3 || config.Width != 3)
            {
                var boardCapacity = config.Width * config.Height;
                config.MovableGridSize = GetValidInput("Enter the size of the movable grid (always square):", min,
                    Math.Min(config.Height, config.Width));
                config.WinningCondition = GetValidInput("Enter the winning condition of the game (more than 3):", 4,
                    boardCapacity);
                config.InitialMoves =
                    GetValidInput("Enter the number of moves have to made before moving the pieces or the grid:", 0,
                        boardCapacity);
                config.MaxPieces = GetValidInput("Enter the number of pieces every player has:", config.WinningCondition, boardCapacity);
            }
            else
            {
                config.MovableGridSize = _configRepository.DefaultConfiguration.MovableGridSize;
                config.WinningCondition = _configRepository.DefaultConfiguration.WinningCondition;
                config.InitialMoves = _configRepository.DefaultConfiguration.InitialMoves;
                config.MaxPieces = _configRepository.DefaultConfiguration.MaxPieces;
            }

            Console.WriteLine($"Configuration '{name}' created successfully. Press Enter to return to menu.");
            Console.ReadLine();
            return config;

        }

        public string DeleteConfiguration()
        {
            var configNames = _configRepository.GetConfigurationNames();

            if (configNames.Count == 0)
            {
                Console.WriteLine("No configurations to delete."); // TODO: idk how to exit this
                Console.ReadLine();
            }

            Console.WriteLine("Saved Configurations:");
            for (var i = 0; i < configNames.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {configNames[i]}");
            }

            Console.Write("Enter the number of the configuration to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int configIndex) || configIndex < 1 ||
                configIndex > configNames.Count)
            {
                Console.WriteLine();
                Console.WriteLine("Invalid selection. No configuration deleted.");
                return "No configuration deleted.";
            }
            
            var selectedConfig = configNames[configIndex - 1];
            _configRepository.DeleteConfiguration(selectedConfig);

            return $"Configuration '{selectedConfig}' has been deleted.";
        }
    

    private static int GetValidInput(string prompt, int minValue, int maxValue)
        {
            int result;
            Console.WriteLine(prompt);

            while (!int.TryParse(Console.ReadLine(), out result) || result < minValue || result > maxValue)
            {
                Console.WriteLine($"Invalid input! Please enter a number between {minValue + 1} and {maxValue}:");
            }

            return result;
        }


        private static char GetValidSymbol(char player1Symbol = '\0') // TODO:CHECK IF WORKS
        {
            while (true)
            {
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    if (player1Symbol == '\0')
                    {
                        return 'X';
                    }
                    else
                    {
                        char defaultSymbol;

                        if (player1Symbol != 'O')
                        {
                            defaultSymbol = 'O';
                        }
                        else if (player1Symbol != 'X')
                        {
                            defaultSymbol = 'X';
                        }
                        else
                        {
                            Console.WriteLine("Default symbol is already taken by Player 1. Please enter a different symbol:");
                            continue;
                        }
                        return defaultSymbol;
                    }
                }

                if (char.TryParse(input, out var symbol) && !char.IsWhiteSpace(symbol) && !char.IsControl(symbol))
                {
                    if (char.ToUpper(symbol) == char.ToUpper(player1Symbol))
                    {
                        Console.WriteLine($"This symbol is already taken by Player 1 ('{player1Symbol}'). Please try again:");
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
