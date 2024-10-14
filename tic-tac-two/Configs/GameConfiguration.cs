namespace Configs
{
    public record GameConfiguration
    {
        public string GameName { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public char Player1Symbol { get; set; }
        public char Player2Symbol { get; set; }
        public string StartingPlayer { get; set; }
        
        public int? MovableGridSize { get; set; } // always square
        
        public int WinningCondition { get; set; }
        
        public int? InitialMoves { get; set; } 
            
        public int? MaxPieces { get; set; }
    }
}