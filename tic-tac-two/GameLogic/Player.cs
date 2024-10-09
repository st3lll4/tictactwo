namespace GameLogic
{
    class Player
    {
        public char Symbol { get; private set; }

        public Player(char symbol)
        {
            Symbol = symbol;
        }

        public void MakeMove(Board board)
        {
            Console.WriteLine($"{Symbol}'s turn. Enter row and column (0-4) to place your piece:");
            int row = int.Parse(Console.ReadLine());
            int col = int.Parse(Console.ReadLine());

            if (!board.PlacePiece(row, col, Symbol))
            {
                Console.WriteLine("Invalid move. Press Enter to try again.");
                Console.ReadLine();
            }
        }
    }
}