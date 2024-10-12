using GameLogic;

namespace UI
{
    public class ConfigurationsMenu
    {
        private List<MenuItem> ConfigMenuItems { get; set; } = [];
        private ConfigurationManager ConfigManager { get; set; }

        public ConfigurationsMenu(ConfigurationManager configManager)
        {
            ConfigManager = configManager;
            InitializeConfigMenu();
        }

        // TODO: easier UI in terms of going back
        
        private void InitializeConfigMenu()
        {
            if (ConfigMenuItems.Count != 0) return;
            ConfigMenuItems.Add(new MenuItem(1, "Select a configuration to play", SelectConfiguration));
            ConfigMenuItems.Add(new MenuItem(2, "Create a configuration", ConfigManager.CreateConfiguration));
            ConfigMenuItems.Add(new MenuItem(3, "Delete a configuration", ConfigManager.DeleteConfiguration));
            ConfigMenuItems.Add(new MenuItem(4, "View your configurations", SeeConfigurations));
            ConfigMenuItems.Add(new MenuItem(5, "Back to Main Menu", BackToMainMenu));
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

                Console.Write("Enter an option: ");
                var choice = Console.ReadLine();

                if (int.TryParse(choice, out int choiceNumber))
                {
                    var selectedItem = ConfigMenuItems.Find(item => item.Number == choiceNumber);
                    if (selectedItem != null)
                    {
                        if (selectedItem.Name == "Back to Main Menu")
                        {
                            break;
                        }
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


            Console.WriteLine("Enter the number of the configuration you want to play with:");
            for (int i = 0; i < savedConfigs.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {savedConfigs[i]}");
            }

            if (int.TryParse(Console.ReadLine(), out int configIndex) && configIndex >= 1 && configIndex <= savedConfigs.Count)
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

                Console.WriteLine("Enter the number of the configuration you want to view in detail, or '0' to return to the menu:");

                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 0 && choice <= savedConfigs.Count)
                {
                    if (choice == 0) break; 

                    var selectedConfigName = savedConfigs[choice - 1];
                    var selectedConfig = ConfigManager.GetConfigurationByName(selectedConfigName);

                    {
                        DisplayConfigurationDetails(selectedConfig);
                        if (Console.ReadLine() == "0") break; 
                    }
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


        private static void BackToMainMenu()
        {
            //  intentionally left blank to allow breaking out of the menu loop.
        }

        private static void ShowMessage(string message)
        {
            Console.WriteLine(message);
            Console.ReadLine();
        }
    }
}
