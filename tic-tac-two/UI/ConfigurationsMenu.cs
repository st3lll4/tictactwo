/*
using DAL;
using GameLogic;

namespace UI
{
    public class ConfigurationsMenu
    {
        private List<MenuItem> ConfigMenuItems { get; } = [];
        private ConfigurationManager ConfigManager { get; }

        public ConfigurationsMenu(ConfigurationManager configManager)
        {
            ConfigManager = configManager;
            InitializeConfigMenu();
        }

        private void InitializeConfigMenu()
        {
            if (ConfigMenuItems.Count != 0) return;
            ConfigMenuItems.Add(new MenuItem(1, "Select a configuration to play", SelectConfiguration));
            ConfigMenuItems.Add(new MenuItem(2, "Create a configuration", ConfigManager.CreateConfiguration));
            ConfigMenuItems.Add(new MenuItem(3, "Delete a configuration", ConfigManager.DeleteConfiguration));
            ConfigMenuItems.Add(new MenuItem(4, "View your configurations", SeeConfigurations));
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Configurations Menu");
                foreach (var item in ConfigMenuItems)
                {
                    Console.WriteLine($"{item.Number}. {item.Name}");
                }

                Console.Write("Enter an option (Press Enter to go back): ");
                var choice = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(choice))
                {
                    break; 
                }

                if (int.TryParse(choice, out int choiceNumber))
                {
                    var selectedItem = ConfigMenuItems.Find(item => item.Number == choiceNumber);
                    if (selectedItem != null)
                    {
                        selectedItem.Action.Invoke();
                    }
                    else
                    {
                        ShowMessage("Invalid option. Press Enter to try again.");
                    }
                }
                else
                {
                    ShowMessage("Invalid input. Press Enter to try again.");
                }
            }
        }

        private void SelectConfiguration()
        {
            var savedConfigs = ConfigManager.GetSavedConfigurations();

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
                ConfigManager.SetCurrentConfiguration(selectedConfig);
                ShowMessage($"Configuration '{selectedConfig}' selected for future games.");
            }
            else
            {
                ShowMessage("Invalid input. Press Enter to return to the menu.");
            }
        }

        private void SeeConfigurations()
        {
            var savedConfigs = ConfigManager.GetSavedConfigurations();

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

                // If Enter is pressed without input, go back
                if (string.IsNullOrWhiteSpace(input))
                {
                    return;
                }

                if (int.TryParse(input, out int choice) && choice >= 1 && choice <= savedConfigs.Count)
                {
                    var selectedConfigName = savedConfigs[choice - 1];
                    var selectedConfig = ConfigManager.GetConfigurationByName(selectedConfigName);

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
        

        private static void ShowMessage(string message)
        {
            Console.WriteLine(message);
            Console.ReadLine();
        }
    }
}
*/
