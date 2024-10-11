namespace GameLogic
{
    public record GameConfiguration
    {
        public string GameName { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public char Player1Symbol { get; set; }
        public char Player2Symbol { get; set; }
        public string StartingPlayer { get; set; }
        
        // TODO: 
        // movable grid size
        public int MovableGridSize { get; set; } // always square
        
        // winning condition
        public int WinningCondition { get; set; }
        
        // move grid after n moves
        public int InitialMoves { get; set; }
            
        // max pieces for a player
        public int MaxPieces { get; set; }
    }
}