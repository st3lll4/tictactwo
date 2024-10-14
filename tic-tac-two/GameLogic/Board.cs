using Configs;

namespace GameLogic
{
    public class Board
    {
        public int Width { get; }
        public int Height { get; }
        public char[,] Cells { get; private set; }
        public int GridCenterRow { get; private set; }
        public int GridCenterCol { get; private set; }


        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            Cells = new char[Height, Width];
            InitializeBoard();

            GridCenterRow = Height / 2;
            GridCenterCol = Width / 2;
        }

        private void InitializeBoard()
        {
            for (var i = 0; i < Height; i++)
            {
                for (var j = 0; j < Width; j++)
                {
                    Cells[i, j] = ' '; 
                }
            }
        }

        public bool MoveGrid(int dRow, int dCol)
        {
            var newCenterRow = GridCenterRow + dRow;
            var newCenterCol = GridCenterCol + dCol;

            if (newCenterRow < 1 || newCenterRow >= Height - 1 ||
                newCenterCol < 1 || newCenterCol >= Width - 1) return false; 
            GridCenterRow = newCenterRow;
            GridCenterCol = newCenterCol;
            return true; 
        }
        
        public bool IsInGrid(int row, int col)
        {
            return Math.Abs(row - GridCenterRow) <= 1 && Math.Abs(col - GridCenterCol) <= 1;
        }

        public char GetCell(int row, int col)
        {
            if (row >= 0 && row < Height && col >= 0 && col < Width)
            {
                return Cells[row, col];
            }
            return ' '; 
        }

        public bool PlacePiece(int row, int col, char piece)
        {
            if (row >= 0 && row < Height && col >= 0 && col < Width && Cells[row, col] == ' ')
            {
                Cells[row, col] = piece;  
                return true;
            }
            return false;
        }

        public bool MovePiece(int fromRow, int fromCol, int toRow, int toCol)
        {
            if (Cells[fromRow, fromCol] != ' ' && Cells[toRow, toCol] == ' ')
            {
                Cells[toRow, toCol] = Cells[fromRow, fromCol];
                Cells[fromRow, fromCol] = ' ';
                return true;
            }
            return false;
        }

        public void Draw()
        {
            Console.Write("   ");
            for (int i = 0; i < Width; i++)
            {
                Console.Write($"{i + 1,3}");
            }
            Console.WriteLine();

            for (int i = 0; i < Height; i++)
            {
                Console.Write($"{i + 1,3}");
                for (int j = 0; j < Width; j++)
                {
                    if (IsInGrid(i, j))
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }

                    Console.Write($" {Cells[i, j]} ");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }

        public bool CheckWin(char symbol)
        {
            for (var i = GridCenterRow - 1; i <= GridCenterRow + 1; i++)
            {
                var rowCount = 0;
                var colCount = 0;
                var diag1Count = 0;
                var diag2Count = 0;

                for (var j = GridCenterCol - 1; j <= GridCenterCol + 1; j++)
                {
                    if (Cells[i, j] == symbol) rowCount++;
                    if (Cells[j, i] == symbol) colCount++;

                    if (Cells[GridCenterRow - 1 + j, GridCenterCol - 1 + j] == symbol) diag1Count++;
                    if (Cells[GridCenterRow - 1 + j, GridCenterCol + 1 - j] == symbol) diag2Count++;

                    if (rowCount == 3 || colCount == 3 || diag1Count == 3 || diag2Count == 3)
                        return true; 
                }
            }
            return false;
        }
    }
}
