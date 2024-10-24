using UI;

namespace tic_tac_two
{
    public class Menus
    {
        private readonly ConfigurationManager _configManager;

        // Instance properties for menus
        private Menu ConfigurationsMenu { get; set; }
        public Menu MainMenu { get; set; }

        public Menus()
        {
            _configManager = new ConfigurationManager();

            ConfigurationsMenu = new Menu(
                EMenuLevel.Secondary,
                "Configurations",
                [
                    new MenuItem
                    {
                        Shortcut = "S",
                        MenuItemAction = _configManager.SelectConfiguration,
                        Title = "Select a configuration to play"
                    },

                    new MenuItem
                    {
                        Shortcut = "C",
                        MenuItemAction = _configManager.AddConfiguration,
                        Title = "Create a configuration"
                    },

                    new MenuItem
                    {
                        Shortcut = "D",
                        MenuItemAction = _configManager.DeleteConfiguration,
                        Title = "Delete a configuration"
                    },

                    new MenuItem
                    {
                        Shortcut = "V",
                        MenuItemAction = _configManager.SeeConfigurations,
                        Title = "View your configurations"
                    }
                ]
            );

            // Initialize the MainMenu instance
            MainMenu = new Menu(
                EMenuLevel.Main,
                "TIC-TAC-TWOOOOO",
                [
                    new MenuItem
                    {
                        Shortcut = "N",
                        Title = "New game"
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
