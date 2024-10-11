using GameLogic;

namespace UI
{
    public class MainMenu
    {
        private List<MenuItem> _mainMenuItems = new List<MenuItem>();
        private ConfigurationManager _configManager;

        public MainMenu()
        {
            _configManager = new ConfigurationManager();
            InitializeTopLevelMenu();
            InitializeGameConfiguration();
        }

        private void InitializeTopLevelMenu()
        {
            if (_mainMenuItems.Count == 0)
            {
                _mainMenuItems.Add(new MenuItem(1, "Start New Game", StartNewGame));
                _mainMenuItems.Add(new MenuItem(2, "Configurations", ShowConfigMenu));
                _mainMenuItems.Add(new MenuItem(3, "Exit", ExitGame));
            }
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Main Menu");
                foreach (var item in _mainMenuItems)
                {
                    Console.WriteLine($"{item.Number}. {item.Name}");
                }

                Console.Write("Enter your choice: ");
                var choice = Console.ReadLine();

                if (int.TryParse(choice, out int choiceNumber))
                {
                    var selectedItem = _mainMenuItems.Find(item => item.Number == choiceNumber);
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

        private void InitializeGameConfiguration()
        {
            var configNames = _configManager.GetSavedConfigurations();

            if (configNames.Count > 0)
            {
                var defaultConfigName = configNames[0];
                _configManager.SetCurrentConfiguration(defaultConfigName);
            }
            else
            {
                Console.WriteLine("No configurations available. Please create a configuration first.");
                Console.ReadLine();
            }
        }

        private void StartNewGame()
        {
            if (_configManager.CurrentConfiguration == null)
            {
                Console.WriteLine("No game configuration selected. Please select a configuration first.");
                Console.ReadLine();
                return;
            }

            Game game = new Game(_configManager.CurrentConfiguration);
            game.Start();
        }

        private void ShowConfigMenu()
        {
            var configMenu = new ConfigurationsMenu(_configManager);
            configMenu.Show();
        }

        private void ExitGame()
        {
            Console.WriteLine("Bye-bye!");
            Environment.Exit(0);
        }

        private void ShowMessage(string message)
        {
            Console.WriteLine(message);
            Console.ReadLine();
        }
    }
}
