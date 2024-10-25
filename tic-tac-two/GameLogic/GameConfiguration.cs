namespace GameLogic
{
    public record GameConfiguration // todo: default values????
    {
        public string GameName { get; set; } = default!;
        
        public int Width;
        public int Height;
        public char Player1Symbol;
        public char Player2Symbol;
        public string StartingPlayer { get; set; } = default!;

        public int? MovableGridSize;

        public int WinningCondition;

        public int? InitialMoves;

        public int? MaxPieces;
    }
}