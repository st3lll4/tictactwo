using UI;

namespace tic_tac_two

{
    public class Menus
    {

        public static readonly Menu ConfigurationsMenu = new Menu(
            EMenuLevel.Secondary, "Configurations", [
                new MenuItem
                {
                    Shortcut = "S",
                    MenuItemAction = ConfigurationManager.SelectConfiguration,
                    Title = "Select a configuration to play"
                },
                new MenuItem
                {
                    Shortcut = "C",
                    MenuItemAction = ConfigurationManager.AddConfiguration,
                    Title = "Create a configuration"
                },
                new MenuItem
                {
                    Shortcut = "D",
                    MenuItemAction = ConfigurationManager.DeleteConfiguration,
                    Title = "Delete a configuration"
                },
                new MenuItem
                {
                    Shortcut = "V",
                    MenuItemAction = ConfigurationManager.SeeConfigurations,
                    Title = "View your configurations"
                }
            ]);


        public static readonly Menu MainMenu = new Menu(
            EMenuLevel.Main,
            "TIC-TAC-TWOOOOO", [
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
            ]);
    }
}
    
    