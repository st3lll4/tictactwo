namespace GameLogic
{
    public class Board
    {
        private int _width;
        private int _height;
        private char[,] _cells; // Board cells dynamically sized
        private int _smallBoardStartX;
        private int _smallBoardStartY;

        // Constructor takes width and height of the board
        public Board(int width, int height)
        {
            _width = width;
            _height = height;
            _smallBoardStartX = 1; // Start small board at row 1
            _smallBoardStartY = 1; // Start small board at column 1
            _cells = new char[_width + 1, _height + 1]; // Adjust array size for 1-based index display
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int i = 1; i <= _width; i++) // Start loops from 1 to match 1-based indexing
            {
                for (int j = 1; j <= _height; j++)
                {
                    _cells[i, j] = ' '; // Empty cells initialized with a space
                }
            }
        }

        public void Draw()
        {
            // Print column headers dynamically
            Console.Write("   "); // Space for row numbers
            for (int i = 1; i <= _width; i++) // Columns start from 1
            {
                Console.Write($"  {i} "); // Print column numbers with padding
            }
            Console.WriteLine();

            // Print rows with row numbers and grid separators
            for (int i = 1; i <= _height; i++) // Rows start from 1
            {
                if (i >= 10)
                {
                    Console.Write($" {i}|");
                }
                else
                {
                    Console.Write($" {i} |");
                }

                for (int j = 1; j <= _width; j++)
                {
                    // Apply background color: default for all cells, special color for 3x3 grid
                    if (IsInGrid(i, j))
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray; // Special color for 3x3 grid
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue; // Default board color
                    }

                    // Print each cell with space padding
                    Console.Write($" {_cells[i, j]} ");
                    Console.ResetColor(); // Reset the color after printing each cell
                    Console.Write(" "); // Right border for each cell
                }
                Console.WriteLine();
            }
        }


        public bool IsInGrid(int row, int col)
        {
            // Check if within the 3x3 grid centered on _smallBoardStartX and _smallBoardStartY
            return Math.Abs(row - _smallBoardStartX) <= 1 && Math.Abs(col - _smallBoardStartY) <= 1;
        }

        public bool PlacePiece(int row, int col, char piece)
        {
            if (IsInGrid(row, col) && _cells[row, col] == ' ')
            {
                _cells[row, col] = piece;
                return true;
            }
            return false;
        }

        public bool CheckWin(char symbol)
        {
            // Check for win in 3x3 grid
            for (int i = _smallBoardStartX - 1; i <= _smallBoardStartX + 1; i++)
            {
                if (_cells[i, _smallBoardStartY - 1] == symbol && _cells[i, _smallBoardStartY] == symbol && _cells[i, _smallBoardStartY + 1] == symbol)
                    return true;
            }
            for (int j = _smallBoardStartY - 1; j <= _smallBoardStartY + 1; j++)
            {
                if (_cells[_smallBoardStartX - 1, j] == symbol && _cells[_smallBoardStartX, j] == symbol && _cells[_smallBoardStartX + 1, j] == symbol)
                    return true;
            }
            if (_cells[_smallBoardStartX - 1, _smallBoardStartY - 1] == symbol && _cells[_smallBoardStartX, _smallBoardStartY] == symbol && _cells[_smallBoardStartX + 1, _smallBoardStartY + 1] == symbol)
                return true;
            if (_cells[_smallBoardStartX + 1, _smallBoardStartY - 1] == symbol && _cells[_smallBoardStartX, _smallBoardStartY] == symbol && _cells[_smallBoardStartX - 1, _smallBoardStartY + 1] == symbol)
                return true;

            return false;
        }
    }
}
