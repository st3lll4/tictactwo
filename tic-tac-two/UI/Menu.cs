using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace UI
{

    public class Menu
    {
        private string MenuHeader { get; set; }
        
        private static string _menuDivider = "-----------------";
        private List<MenuItem> MenuItems { get; set; }
        private EMenuLevel MenuLevel { get; set; }

        private MenuItem _menuItemReturn = new(100, "Go back", null);

        private MenuItem _menuItemReturnMain = new(200, "Return to main menu", null);

        private MenuItem _menuItemExit = new(300, "Exit the game", null);
        

        public void SetMenuItemAction(int number, Action action) // do i need
        {
            var menuItem = MenuItems.Single(m => m.Number == number);
            menuItem.MenuItemAction = action;
        }

        public Menu(EMenuLevel menuLevel, string menuHeader, List<MenuItem> menuItems)
        {
            if (string.IsNullOrWhiteSpace(menuHeader))
            {
                throw new ApplicationException("Menu header cannot be empty.");
            }

            MenuHeader = menuHeader;

            if (menuItems == null || menuItems.Count == 0)
            {
                throw new ApplicationException("Menu items cannot be empty.");
            }

            MenuItems = menuItems;
            MenuLevel = menuLevel;
            
            if (MenuLevel != EMenuLevel.Main)
            {
                MenuItems.Add(_menuItemReturn);
            }

            if (MenuLevel == EMenuLevel.Deep)
            {
                MenuItems.Add(_menuItemReturnMain);
            }

            MenuItems.Add(_menuItemExit);

        }

        public void Run()
        {
            Console.Clear();

            while (true)
            {
                DrawMenu();

                Console.Write("Enter your choice (number): ");
                var userInput = Console.ReadLine();

                if (!int.TryParse(userInput, out var choiceNumber))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    Console.WriteLine();
                }

                var selectedMenuItem = MenuItems.FirstOrDefault(m => m.Number == choiceNumber);

                if (selectedMenuItem == null)
                {
                    Console.WriteLine("Invalid option. Please select an available option.");
                    Console.WriteLine();
                    continue; 
                }

                selectedMenuItem.MenuItemAction?.Invoke();
                
                Console.WriteLine("No action associated with this menu item.");
                Console.WriteLine();
            }
        }



        private MenuItem DisplayMenuGetUserChoice()
        {
            do
            {
                DrawMenu();

                Console.Write("Enter your choice: ");
                var userInput = Console.ReadLine();

                if (int.TryParse(userInput, out var menuNumber))
                {
                    var selectedMenuItem = MenuItems.FirstOrDefault(m => m.Number == menuNumber);
            
                    if (selectedMenuItem != null)
                    {
                        return selectedMenuItem;
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice. Please select an available menu option.");
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a valid number.");
                }
                Console.WriteLine();
            } while (true);
        }


        private void DrawMenu()
        {
            Console.WriteLine(MenuHeader);
            Console.WriteLine(_menuDivider);

            foreach (var t in MenuItems)
            {
                Console.WriteLine(t);
            }

            Console.WriteLine();

            Console.Write("> ");
        }
    }
}
