namespace GameLogic
{
    class Player(char symbol)
    {
        public char Symbol { get; } = symbol;

        public void MakeMove(Board board)
        {
            Console.WriteLine($"{Symbol}'s turn. Enter row and column (0-4) to place your piece:");
            var row = int.Parse(Console.ReadLine());
            var col = int.Parse(Console.ReadLine());

            if (board.PlacePiece(row, col, Symbol)) return;
            Console.WriteLine("Invalid move. Press Enter to try again.");
            Console.ReadLine();
        }
    }
}