using Configs;

namespace UI

{
    public class Menus

    {
        public static Menu MainMenu = new Menu(
            EMenuLevel.Main,
            "TIC-TAC-TOE", [
                new MenuItem(1, "New game", null),
                new MenuItem(2, "Configs", ConfigurationsMenu.Run)
            ]);
        
        public static Menu ConfigurationsMenu =
                    new Menu(
                        EMenuLevel.Secondary,
                        "Configs", [
                            new MenuItem(1, "Select a configuration to play", ConfigurationManager.SelectConfiguration), // how to 
                            new MenuItem(2, "Create a configuration", ConfigurationManager.CreateConfiguration),
                            new MenuItem(3, "Delete a configuration", ConfigurationManager.DeleteConfiguration),
                            new MenuItem(4, "View your configurations", ConfigurationManager.SeeConfigurations)
                        ]);
        

    }
}



/*
namespace UI
{
public class MainMenu
{
   private List<MenuItem> MainMenuItems { get; } = [];

   private ConfigurationManager ConfigManager { get; }


   public MainMenu()
   {
       ConfigManager = new ConfigurationManager();
       InitializeTopLevelMenu();
   }

   private void InitializeTopLevelMenu()
   {
       if (MainMenuItems.Count == 0)
       {
           MainMenuItems.Add(new MenuItem(1, "Start New Game", StartNewGame));
           MainMenuItems.Add(new MenuItem(2, "Configurations", ShowConfigMenu));
           MainMenuItems.Add(new MenuItem(3, "Exit", ExitGame));
       }
   }

   public void Show()
   {
       while (true)
       {
           Console.Clear();
           Console.WriteLine("Main Menu");
           foreach (var item in MainMenuItems)
           {
               Console.WriteLine($"{item.Number}. {item.Name}");
           }

           Console.Write("Enter your choice: ");
           var choice = Console.ReadLine();

           if (int.TryParse(choice, out int choiceNumber))
           {
               var selectedItem = MainMenuItems.Find(item => item.Number == choiceNumber);
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

   private void StartNewGame()
   {
       var game = new Game(ConfigManager.CurrentConfiguration);
       game.Start();
   }

   private void ShowConfigMenu()
   {
       var configMenu = new ConfigurationsMenu(ConfigManager);
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
*/
