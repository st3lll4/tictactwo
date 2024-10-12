using GameLogic;

namespace UI
{
    public class ConfigurationsMenu
    {
        private List<MenuItem> _configMenuItems = [];
        private ConfigurationManager ConfigManager { get; set; }

        public ConfigurationsMenu(ConfigurationManager configManager)
        {
            ConfigManager = configManager;
            InitializeConfigMenu();
        }

        // TODO: easier UI in terms of going back
        
        private void InitializeConfigMenu()
        {
            if (_configMenuItems.Count == 0)
            {
                _configMenuItems.Add(new MenuItem(1, "Select a configuration to play", SelectConfiguration));
                _configMenuItems.Add(new MenuItem(2, "Create a configuration", CreateConfiguration));
                _configMenuItems.Add(new MenuItem(3, "Delete a configuration", ConfigManager.DeleteConfiguration));
                _configMenuItems.Add(new MenuItem(4, "View your configurations", SeeConfigurations));
                _configMenuItems.Add(new MenuItem(5, "Back to Main Menu", BackToMainMenu));
            }
        }

        private void CreateConfiguration()
        {
            ConfigManager.CreateConfiguration();
            Console.WriteLine("Configuration created");
            //TODO: DOESNT FUCKING WORK
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Configurations Menu");
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

      //      if (savedConfigs.Count == 0)
    //        {
  //              Console.WriteLine("No saved configurations found. Press 1 to add a configuration, any other key to return:");
//
        //        if (Console.ReadLine() == "1")
      //          {
    //                _configManager.CreateConfiguration();
  //              }
//
            //    return;
            //}

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

            if (savedConfigs.Count == 0)
            {
                ShowMessage("No saved configurations found. Press Enter to return.");
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
            //  intentionally left blank to allow breaking out of the menu loop.
        }

        private void ShowMessage(string message)
        {
            Console.WriteLine(message);
            Console.ReadLine();
        }
    }
}
