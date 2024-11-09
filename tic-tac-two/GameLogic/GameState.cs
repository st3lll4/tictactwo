namespace GameLogic
{
    public class GameState // hoiab koike mis on currently happening in the game, muutuvad asjad 
    {
        public char[,] Board { get; set; }
        public char MovingPlayer { get; set; }
        public GameConfiguration Config { get; private set; }
        public int Player1PiecesPlaced { get; set; }
        public int Player2PiecesPlaced { get; set; }
        public int GridStartRow { get; set; }
        public int GridStartCol { get; set; }

        private int GridCenterRow { get; set; }
        private int GridCenterCol { get; set; }
        private int GridSize { get; set; }
        
        public int WinCondition { get; set; }
        
        public bool IsStandardTicTacToe { get; set; }

        public GameState(GameConfiguration config)
        {
            Config = config;
            Board = new char[config.Height, config.Width]; 

            MovingPlayer = config.StartingPlayer == EStartingPlayer.Player1 ? config.Player1Symbol : config.Player2Symbol;
            
            GridCenterRow = config.Height / 2; // 5/2 = 2.5
            GridCenterCol = config.Width / 2; 
            
            GridSize = config.MovableGridSize; 
            
            GridStartRow = GridCenterRow - (GridSize / 2);
            GridStartCol = GridCenterCol - (GridSize / 2);

            
            IsStandardTicTacToe = GridSize == 3 && 
                                  config.InitialMoves == null && 
                                  config.MaxPieces == null;

            Player1PiecesPlaced = 0;
            Player2PiecesPlaced = 0;

            WinCondition = config.WinningCondition;
        }
        

        public override string ToString()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }
    }
}