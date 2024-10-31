using System.Diagnostics.CodeAnalysis;

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
        
        public int GridCenterRow { get; set; }
        public int GridCenterCol { get; set; }
        public int GridSize { get; set; }
        
        public bool IsStandardTicTacToe { get; set; }

        [SuppressMessage("ReSharper", "PossibleLossOfFraction")]
        public GameState(GameConfiguration config)
        {
            Config = config;
            Board = new char[config.Height, config.Width]; 

            MovingPlayer = config.StartingPlayer == "Player 1" ? config.Player1Symbol : config.Player2Symbol;
            
            GridCenterRow = config.Height / 2; 
            GridCenterCol = config.Width / 2; 
            
            GridSize = config.MovableGridSize ?? 3; 
            
            GridStartRow = GridCenterRow - (GridSize - 1) / 2; 
            GridStartCol = GridCenterCol - (GridSize - 1) / 2;
            
            IsStandardTicTacToe = GridSize == 0 && 
                                  config.InitialMoves == null && 
                                  config.MaxPieces == null;

            Player1PiecesPlaced = 0;
            Player2PiecesPlaced = 0;    
        }
        
        

        public override string ToString()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }
    }
}