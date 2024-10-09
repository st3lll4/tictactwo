namespace GameLogic
{
    public class GameConfiguration
    {
        public string GameName { get; set; } = default!;

        public int Width { get; set; }
        public int Height { get; set; }
        public char Player1Symbol { get; set; } = 'X';
        public char Player2Symbol { get; set; } = 'O';
        public string StartingPlayer { get; set; } = "Player 1";

    }
}
