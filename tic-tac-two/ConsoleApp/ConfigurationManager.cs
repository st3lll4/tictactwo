using DAL;
using GameLogic;
using UI;

namespace tic_tac_two
{
    public class ConfigurationManager
    {
        private readonly ConfigRepositoryJson _configRepository = new();
        public static GameConfiguration CurrentConfiguration { get; private set; } = default!;

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
                var configName = configNames[i];

                configMenuItems.Add(new MenuItem()
                {
                    Title = configName,
                    Shortcut = (i + 1).ToString(),
                    MenuItemAction = () =>
                    {
                        SetCurrentConfiguration(configName);
                        return configName;
                    }
                });
            }

            var selectMenu = new Menu(
                EMenuLevel.Deep,
                "Choose a configuration for future games:",
                configMenuItems,
                true
            );

            var selectedValue = selectMenu.Run();

            return selectedValue;
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

                Console.WriteLine(
                    "Enter the number of the configuration you want to view in detail, or press Enter to go back:");

                var input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    break;
                }

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


        private void SetCurrentConfiguration(string configName) // use in selecting
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
            const int max = 54; // got to stop somewhere, max really depends on the user's screen size

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
                    config.StartingPlayer = startingPlayerChoice == "1" ? EStartingPlayer.Player1 : EStartingPlayer.Player2;
                    break;
                }

                Console.WriteLine("Invalid input. Please try again.");
            }


            var boardCapacity = config.Width * config.Height;
            var maxWinningCondition = (int)Math.Floor(config.MovableGridSize * Math.Sqrt(2));    
            
            config.MovableGridSize = GetValidInput(
                "Enter the size of the movable grid (always square):", min, Math.Min(config.Height, config.Width));
            
            config.WinningCondition = GetValidInput(
                "Enter the winning condition of the game (more than 3):", 4, maxWinningCondition);
            
            config.InitialMoves = GetValidInput(
                "Enter the number of moves have to made before moving the pieces or the grid:", 0, boardCapacity);
            
            config.MaxPieces = GetValidInput(
                "Enter the number of pieces every player has:", config.WinningCondition, boardCapacity);

            Console.WriteLine($"Configuration '{name}' created successfully. Press Enter to return to menu.");
            Console.ReadLine();
            return config;
        }

        public string DeleteConfiguration()
        {
            var configNames = _configRepository.GetConfigurationNames();

            Console.WriteLine("Saved Configurations:");
            for (var i = 0; i < configNames.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {configNames[i]}");
            }

            Console.Write("Enter the number of the configuration to delete or Enter to go back: ");
            
            if (!int.TryParse(Console.ReadLine(), out var configIndex) || configIndex < 1 ||
                configIndex > configNames.Count)
            {
                Console.WriteLine();
                Console.WriteLine("Invalid selection. No configuration deleted."); // todo: test if you see the messages
                return "";
            }

            var selectedConfig = configNames[configIndex - 1];

            if (string.IsNullOrEmpty(selectedConfig)) return "";

            _configRepository.DeleteConfiguration(selectedConfig);

            return $"Configuration '{selectedConfig}' has been deleted.";
        }


        private static int GetValidInput(string prompt, int minValue, int maxValue)
        {
            int result;
            Console.WriteLine(prompt);

            while (!int.TryParse(Console.ReadLine(), out result) || result < minValue || result > maxValue)
            {
                Console.WriteLine($"Invalid input! Please enter a number between {minValue} and {maxValue}:");
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
                    return GetDefaultSymbol(player1Symbol);
                }

                if (char.TryParse(input, out var symbol) && IsValidSymbol(symbol))
                {
                    if (IsSymbolTaken(symbol, player1Symbol))
                    {
                        Console.WriteLine(
                            $"This symbol is already taken by Player 1 ('{player1Symbol}'). Please try again:");
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

        private static char GetDefaultSymbol(char player1Symbol)
        {
            if (player1Symbol == '\0')
            {
                return 'X';
            }

            if (player1Symbol != 'O')
            {
                return 'O';
            }

            Console.WriteLine("Default symbol is already taken by Player 1. Please enter a different symbol:");
            return '\0';
        }

        private static bool IsValidSymbol(char symbol)
        {
            return !char.IsWhiteSpace(symbol) && !char.IsControl(symbol);
        }

        private static bool IsSymbolTaken(char symbol, char player1Symbol)
        {
            return char.ToUpper(symbol) == char.ToUpper(player1Symbol);
        }
    }
}