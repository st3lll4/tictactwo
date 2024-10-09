using GameLogic;

namespace UI
{
    public class ConfigurationsMenu
    {
        private List<MenuItem> _configMenuItems = new();
        private ConfigurationManager? _configManager;

        // Constructor to inject dependencies
        public ConfigurationsMenu(ConfigurationManager? configManager)
        {
            _configManager = configManager;
            InitializeConfigMenu();
        }

        private void InitializeConfigMenu()
        {
            if (_configMenuItems.Count == 0)
            {
                _configMenuItems.Add(new MenuItem(1, "Select a configuration to play", SelectConfiguration));
                _configMenuItems.Add(new MenuItem(2, "Create a configuration", _configManager.CreateConfiguration));
                _configMenuItems.Add(new MenuItem(3, "Delete a configuration", _configManager.DeleteConfiguration));
                _configMenuItems.Add(new MenuItem(4, "View your configurations", SeeConfigurations));
                _configMenuItems.Add(new MenuItem(5, "Back to Main Menu", BackToMainMenu));
            }
        }

        

        public void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Your configurations");
                foreach (var item in _configMenuItems)
                {
                    Console.WriteLine($"{item.Number}. {item.Name}");
                }

                Console.Write("Enter an option: ");
                var choice = Console.ReadLine();

                if (int.TryParse(choice, out int choiceNumber))
                {
                    var selectedItem = _configMenuItems.Find(item => item.Number == choiceNumber);
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
            var savedConfigs = _configManager.GetSavedConfigurations();

            if (savedConfigs.Count == 0)
            {
                Console.WriteLine("No saved configurations found. Press 1 to add a configuration, 0 to exit:");
    
                if (int.TryParse(Console.ReadLine(), out int choice) && choice == 1)
                {
                   _configManager.CreateConfiguration();  
                }
    
                return;  // Exit the method if no valid selection or user chose to exit
            }


            Console.WriteLine("Enter the number of the configuration you want to play with:");
            for (int i = 0; i < savedConfigs.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {savedConfigs[i]}");
            }

            if (int.TryParse(Console.ReadLine(), out int configIndex) && configIndex >= 1 && configIndex <= savedConfigs.Count)
            {
                string selectedConfig = savedConfigs[configIndex - 1];
                _configManager.LoadConfiguration(selectedConfig);
                ShowMessage($"Configuration '{selectedConfig}' selected for future games.");
            }
            else
            {
                ShowMessage("Invalid input. Press Enter to return to the menu.");
            }
        }

        private void SeeConfigurations()
        {
            var savedConfigs = _configManager.GetSavedConfigurations();

            if (savedConfigs.Count == 0)
            {
                ShowMessage("No saved configurations found. Press enter to exit to configurations menu.");
                return;
            }

            Console.WriteLine("Your configurations:");
            for (int i = 0; i < savedConfigs.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {savedConfigs[i]}");
            }

            ShowMessage("Press Enter to return to the menu.");
        }

        private void BackToMainMenu()
        {
        }

        private void ShowMessage(string message)
        {
            Console.WriteLine(message);
            Console.ReadLine();
        }
    }
}