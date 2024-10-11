namespace GameLogic
{
    public class Board
    {
        private int _width;
        private int _height;
        private char[,] _cells;
        private int _gridCenterRow;
        private int _gridCenterCol;

        public Board(int width, int height)
        {
            _width = width;
            _height = height;
            _cells = new char[_height, _width];
            InitializeBoard();

            // Initialize the grid at the center of the board
            _gridCenterRow = _height / 2;
            _gridCenterCol = _width / 2;
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    _cells[i, j] = ' ';
                }
            }
        }
        
        public bool MoveGrid(int dRow, int dCol)
        {
            int newCenterRow = _gridCenterRow + dRow;
            int newCenterCol = _gridCenterCol + dCol;

            // Ensure the new grid center is within bounds
            if (newCenterRow >= 1 && newCenterRow < _height - 1 &&
                newCenterCol >= 1 && newCenterCol < _width - 1)
            {
                _gridCenterRow = newCenterRow;
                _gridCenterCol = newCenterCol;
                return true;
            }
            return false;
        }
        
        public bool IsInGrid(int row, int col)
        {
            return Math.Abs(row - _gridCenterRow) <= 1 && Math.Abs(col - _gridCenterCol) <= 1;
        }

        public char GetCell(int row, int col)
        {
            if (row >= 0 && row < _height && col >= 0 && col < _width)
            {
                return _cells[row, col];
            }
            return '\0'; // Return null character for invalid positions
        }

        public bool MovePiece(int fromRow, int fromCol, int toRow, int toCol)
        {
            if (_cells[fromRow, fromCol] != ' ' && _cells[toRow, toCol] == ' ')
            {
                _cells[toRow, toCol] = _cells[fromRow, fromCol];
                _cells[fromRow, fromCol] = ' ';
                return true;
            }
            return false;
        }


        public void Draw()
        {
            // Print column headers
            Console.Write("   ");
            for (int i = 0; i < _width; i++)
            {
                Console.Write($"{i + 1,3}");
            }
            Console.WriteLine();

            // Print rows with grid highlighting
            for (int i = 0; i < _height; i++)
            {
                Console.Write($"{i + 1,3}");
                for (int j = 0; j < _width; j++)
                {
                    if (IsInGrid(i, j))
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }

                    Console.Write($" {_cells[i, j]} ");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }
        
        //TODO: indexes are wrong, if i say 2,2 it puts into 1,1
        public bool PlacePiece(int row, int col, char piece)
        {
            row -= 1;
            col -= 1;

            if (row >= 0 && row < _height && col >= 0 && col < _width && _cells[row, col] == ' ')
            {
                _cells[row, col] = piece;
                return true;
            }
            return false;
        }
        
        public bool CheckWin(char symbol)
        {
            // Check rows within the grid
            for (int i = _gridCenterRow - 1; i <= _gridCenterRow + 1; i++)
            {
                if (i < 0 || i >= _height) continue;
                int count = 0;
                for (int j = _gridCenterCol - 1; j <= _gridCenterCol + 1; j++)
                {
                    if (j < 0 || j >= _width) continue;
                    if (_cells[i, j] == symbol)
                    {
                        count++;
                    }
                }
                if (count == 3) return true; // Found a winning row
            }

            // Check columns within the grid
            for (int j = _gridCenterCol - 1; j <= _gridCenterCol + 1; j++)
            {
                if (j < 0 || j >= _width) continue;
                int count = 0;
                for (int i = _gridCenterRow - 1; i <= _gridCenterRow + 1; i++)
                {
                    if (i < 0 || i >= _height) continue;
                    if (_cells[i, j] == symbol)
                    {
                        count++;
                    }
                }
                if (count == 3) return true; // Found a winning column
            }

            // Check diagonal from top-left to bottom-right within the grid
            int diagCount1 = 0;
            for (int i = -1; i <= 1; i++)
            {
                int row = _gridCenterRow + i;
                int col = _gridCenterCol + i;
                if (row < 0 || row >= _height || col < 0 || col >= _width) continue;
                if (_cells[row, col] == symbol)
                {
                    diagCount1++;
                }
            }
            if (diagCount1 == 3) return true; // Found a winning diagonal (top-left to bottom-right)

            // Check diagonal from top-right to bottom-left within the grid
            int diagCount2 = 0;
            for (int i = -1; i <= 1; i++)
            {
                int row = _gridCenterRow + i;
                int col = _gridCenterCol - i;
                if (row < 0 || row >= _height || col < 0 || col >= _width) continue;
                if (_cells[row, col] == symbol)
                {
                    diagCount2++;
                }
            }
            if (diagCount2 == 3) return true; // Found a winning diagonal (top-right to bottom-left)

            return false; // No win found within the grid
        }

    }
}
