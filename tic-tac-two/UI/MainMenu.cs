using GameLogic;

namespace UI
{
    public class MainMenu
    {
        private static List<MenuItem> _mainMenuItems = new List<MenuItem>();
        private GameConfiguration _gameConfiguration; // Reference to the current game configuration
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

        public static void Show()
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
                _gameConfiguration = _configManager.LoadConfiguration(configNames[0]);
            }
            else
            {
                _gameConfiguration = new GameConfiguration();
                Console.WriteLine("No configurations available.");
                // Optionally, you can prompt the user to create a new configuration or handle this scenario accordingly
            }
        }



        private void StartNewGame()
        {
            Game game = new Game(_gameConfiguration);
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

        private static void ShowMessage(string message)
        {
            Console.WriteLine(message);
            Console.ReadLine();
        }
    }
}
