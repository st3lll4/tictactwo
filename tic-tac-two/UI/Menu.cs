namespace UI
{
    public class Menu
    {
        private string MenuHeader { get; set; }
        private static string _menuDivider = "~~~~~~~~~~~~~~~~~~~~";
        private List<MenuItem> MenuItems { get; set; }

        private MenuItem _menuItemExit = new MenuItem()
        {
            Shortcut = "E",
            Title = "Exit"
        };

        private MenuItem _menuItemReturn = new MenuItem()
        {
            Shortcut = "R",
            Title = "Return"
        };

        private MenuItem _menuItemReturnMain = new MenuItem()
        {
            Shortcut = "M",
            Title = "Return to Main menu"
        };

        private EMenuLevel _menuLevel { get; set; }

        private bool _isCustomMenu;

        public Menu(EMenuLevel menuLevel, string menuHeader, List<MenuItem> menuItems, bool isCustomMenu = false)
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
            _menuLevel = menuLevel;
            _isCustomMenu = isCustomMenu;

            if (_menuLevel != EMenuLevel.Main)
            {
                MenuItems.Add(_menuItemReturn);
            }

            if (_menuLevel == EMenuLevel.Deep)
            {
                MenuItems.Add(_menuItemReturnMain);
            }

            MenuItems.Add(_menuItemExit);
        }

        public void SetMenuItemAction(string shortCut, Func<string> action)
        {
            var menuItem = MenuItems.Single(m => m.Shortcut == shortCut);
            menuItem.MenuItemAction = action;
        }

        public string Run()
        {
            Console.Clear();
            do
            {
                var menuItem = DisplayMenuGetUserChoice();
                var menuReturnValue = "";

                if (menuItem.MenuItemAction != null)
                {
                    menuReturnValue = menuItem.MenuItemAction();

                    if (_isCustomMenu)
                    {
                        return menuReturnValue;
                    }
                }

                if (menuItem.Shortcut == _menuItemReturn.Shortcut)
                {
                    return menuItem.Shortcut;
                }

                if (menuItem.Shortcut == _menuItemExit.Shortcut || menuReturnValue == _menuItemExit.Shortcut)
                {
                    return _menuItemExit.Shortcut;
                }

                if ((menuItem.Shortcut == _menuItemReturnMain.Shortcut ||
                     menuReturnValue == _menuItemReturnMain.Shortcut) && _menuLevel != EMenuLevel.Main)
                {
                    return _menuItemReturnMain.Shortcut;
                }

            } while (true);
        }

        private MenuItem DisplayMenuGetUserChoice()
        {
            do
            {
                DrawMenu();

                var userInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine("It would be nice if you actually choose something! Try again...");
                    Console.WriteLine();
                }
                else
                {
                    userInput = userInput.ToUpper();

                    foreach (var menuItem in MenuItems)
                    {
                        if (menuItem.Shortcut.ToUpper() == userInput)
                        {
                            return menuItem;
                        }
                    }

                    Console.WriteLine("Please choose something from the existing options.");
                    Console.WriteLine();
                }
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
            Console.Write(">");
        }
    }
}
