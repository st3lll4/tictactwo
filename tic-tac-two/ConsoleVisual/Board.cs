// namespace GameLogic
// {
//     public class Board // move to consoleapp 
//     {
//         private int Width { get; }
//         private int Height { get; }
//         protected char[,] Cells { get; private set; }
//         private int GridCenterRow { get; set; }
//         private int GridCenterCol { get; set; }
//
//
//         public Board(int width, int height)
//         {
//             Width = width;
//             Height = height;
//             Cells = new char[Height, Width];
//             InitializeBoard();
//
//             GridCenterRow = Height / 2;
//             GridCenterCol = Width / 2;
//         }
//
//         private void InitializeBoard()
//         {
//             for (var i = 0; i < Height; i++)
//             {
//                 for (var j = 0; j < Width; j++)
//                 {
//                     Cells[i, j] = ' '; 
//                 }
//             }
//         }
//
//         // public bool MoveGrid(int dRow, int dCol)
//         // {
//         //     var newCenterRow = GridCenterRow + dRow;
//         //     var newCenterCol = GridCenterCol + dCol;
//         //
//         //     if (newCenterRow < 1 || newCenterRow >= Height - 1 ||
//         //         newCenterCol < 1 || newCenterCol >= Width - 1) return false; 
//         //     GridCenterRow = newCenterRow;
//         //     GridCenterCol = newCenterCol;
//         //     return true; 
//         // }
//         
//         private bool IsInGrid(int row, int col)
//         {
//             return Math.Abs(row - GridCenterRow) <= 1 && Math.Abs(col - GridCenterCol) <= 1;
//         }
//
//         // public char GetCell(int row, int col)
//         // {
//         //     if (row >= 0 && row < Height && col >= 0 && col < Width)
//         //     {
//         //         return Cells[row, col];
//         //     }
//         //     return ' '; 
//         // }
//         //
//         // public bool MovePiece(int fromRow, int fromCol, int toRow, int toCol)
//         // {
//         //     if (Cells[fromRow, fromCol] != ' ' && Cells[toRow, toCol] == ' ')
//         //     {
//         //         Cells[toRow, toCol] = Cells[fromRow, fromCol];
//         //         Cells[fromRow, fromCol] = ' ';
//         //         return true;
//         //     }
//         //     return false;
//         // }
//
//         public void Draw()
//         {
//             // Print the column headers (numbers at the top)
//             PrintRowHeader(Width);
//
//             // Print the grid with row numbers on the left
//             for (var i = 0; i < Height; i++)
//             {
//                 PrintGridRow(i);  // Print each grid row with row numbers
//             }
//         }
//
//         private static void PrintRowHeader(int width)
//         {
//             Console.Write("  "); 
//             for (var i = 1; i <= width; i++)
//             {
//                 Console.Write(i < 10 ? $"  {i} " : $" 1{i} "); // not sure if works
//                 
//             }
//             Console.WriteLine();
//         }
//
//         private void PrintGridRow(int rowIndex)
//         {
//             Console.Write($"{rowIndex + 1,2} "); 
//
//             for (var j = 0; j < Width; j++)
//             {
//                 Console.BackgroundColor = IsInGrid(rowIndex, j) ? ConsoleColor.Green : ConsoleColor.DarkGray;
//                 Console.Write($" {Cells[rowIndex, j]} "); 
//                 Console.ResetColor();
//
//                 if (j < Width - 1) Console.Write(" "); 
//             }
//
//             Console.WriteLine();
//         }
//         
//     }
// }
