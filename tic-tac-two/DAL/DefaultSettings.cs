namespace DAL;

public class DefaultSettings
{
    // think about moving here: todo
    /*
     * public GameConfiguration DefaultConfiguration { get; set; }


       public ConfigRepositoryJson()
       {
           DefaultConfiguration = new GameConfiguration
           {
               GameName = "Tic-Tac-Two",
               Width = 5,
               Height = 5,
               Player1Symbol = 'X',
               Player2Symbol = 'O',
               StartingPlayer = EStartingPlayer.Player1,
               MovableGridSize = 3,
               WinningCondition = 3,
               InitialMoves = 2,
               MaxPieces = 3
           };
           CheckAndCreateInitialDirectory();
       }


       private void CheckAndCreateInitialDirectory()
       {
           if (!Directory.Exists(FileHelper.BasePath))
           {
               Directory.CreateDirectory(FileHelper.BasePath);
           }
           var data = Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.ConfigExtension).ToList();
           if (data.Count == 0)
           {
               InitializeDefaultConfigurations(); ---------  then this part needs to change in json
           }
       }
       
       
       private void InitializeDefaultConfigurations()
       {
           SaveConfiguration(DefaultConfiguration);

           var defaultConfig2 = new GameConfiguration()
           {
               GameName = "Tic-Tac-Two with a 10x10 board",
               Width = 10,
               Height = 10,
               Player1Symbol = 'X',
               Player2Symbol = 'O',
               StartingPlayer = EStartingPlayer.Player1,
               InitialMoves = 5,
               MaxPieces = 7, // idk
               MovableGridSize = 5,
               WinningCondition = 5
           };
           SaveConfiguration(defaultConfig2);
           
           var defaultConfig3 = new GameConfiguration()
           {
               GameName = "Tic-Tac-Toe",
               Width = 3,
               Height = 3,
               Player1Symbol = 'X',
               Player2Symbol = 'O',
               StartingPlayer = EStartingPlayer.Player1,
               WinningCondition = 3,
               MovableGridSize = 3
           };
           SaveConfiguration(defaultConfig3);
       }
     */
}