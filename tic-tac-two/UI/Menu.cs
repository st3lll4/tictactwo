﻿using GameLogic;

namespace UI
{
    public static class Menu
    {
        public static void Display()
        {
            var mainMenu = new MainMenu();
            MainMenu.Show();
        }
    }
}