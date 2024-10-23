using Configs;

namespace UI

{
    public class Menus
    {
        public static readonly Menu ConfigurationsMenu = new Menu(
            EMenuLevel.Secondary, "DAL", [
                new MenuItem(1, "Select a configuration to play", ConfigurationManager.SelectConfiguration), // how to
                new MenuItem(2, "Create a configuration", ConfigurationManager.CreateConfiguration),
                new MenuItem(3, "Delete a configuration", ConfigurationManager.DeleteConfiguration),
                new MenuItem(4, "View your configurations", ConfigurationManager.SeeConfigurations)
                        ]);
        
        public static readonly Menu MainMenu = new Menu(
            EMenuLevel.Main,
            "TIC-TAC-TOE", [
                new MenuItem(1, "New game", null),
                new MenuItem(2, "DAL", ConfigurationsMenu.Run)
            ]);
    }
}
