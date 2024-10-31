namespace GameLogic
{
    public class GameState // hoiab koike mis on currently happening in the game, muutuvad asjad 
    {
        public char[,] Board { get; set; }
        public char NextMoveBy { get; set; }
        public GameConfiguration Config { get; private set; }
        public int Player1PiecesPlaced { get; set; }
        public int Player2PiecesPlaced { get; set; }
        public int GridStartRow { get; set; }
        public int GridStartCol { get; set; }
        

        public GameState(GameConfiguration config)
        {
            Config = config;
            Board = new char[config.Width, config.Height];

            NextMoveBy = config.StartingPlayer == "Player 1" ? config.Player1Symbol : config.Player2Symbol;
            GridStartRow = config.Height / 3;
            GridStartCol = config.Width / 3;
            
            Player1PiecesPlaced = 0;
            Player2PiecesPlaced = 0;    
        }
        
        

        public override string ToString()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }
    }
}