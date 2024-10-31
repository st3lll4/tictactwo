using UI;

namespace tic_tac_two
{
    public class Menus
    {
        // Instance properties for menus
        public Menu ConfigurationsMenu { get; set; }
        public Menu MainMenu { get; set; }
        

        public Menus()
        {
            var configManager = new ConfigurationManager();

            ConfigurationsMenu = new Menu(
                EMenuLevel.Secondary,
                "Configurations",
                [
                    new MenuItem
                    {
                        Shortcut = "S",
                        MenuItemAction = configManager.SelectConfiguration,
                        Title = "Select a configuration to play"
                    },

                    new MenuItem
                    {
                        Shortcut = "C",
                        MenuItemAction = configManager.AddConfiguration,
                        Title = "Create a configuration"
                    },

                    new MenuItem
                    {
                        Shortcut = "D",
                        MenuItemAction = configManager.DeleteConfiguration,
                        Title = "Delete a configuration"
                    },

                    new MenuItem
                    {
                        Shortcut = "V",
                        MenuItemAction = configManager.SeeConfigurations,
                        Title = "View your configurations"
                    }
                ]
            );

            MainMenu = new Menu(
                EMenuLevel.Main,
                "TIC-TAC-TWOOOOO",
                [
                    new MenuItem
                    {
                        Shortcut = "N",
                        Title = "New game",
                        MenuItemAction = GameController.MainLoop,
                    },

                    new MenuItem
                    {
                        Shortcut = "L",
                        Title = "Load game"
                    },

                    new MenuItem
                    {
                        Shortcut = "C",
                        Title = "Configs",
                        MenuItemAction = ConfigurationsMenu.Run
                    }
                ]
            );
        }
    }
}
