namespace GameLogic;

public class Player
{
    public char Symbol { get; }

    public Player(char symbol)
    {
        Symbol = symbol;
    }

    public void PlacePiece(Board board)
    {
        while (true)
        {
            Console.WriteLine($"{Symbol}'s turn. Enter row and column to place your piece within the grid:");
            if (int.TryParse(Console.ReadLine(), out int row) && int.TryParse(Console.ReadLine(), out int col))
            {
                if (board.IsInGrid(row - 1, col - 1))
                {
                    if (board.PlacePiece(row - 1, col - 1, Symbol))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Cell is occupied. Try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Cell is not within the grid. Try again.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Try again.");
            }
        }
    }

    public void MoveOwnPiece(Board board)
    {
        while (true)
        {
            Console.WriteLine($"{Symbol}'s turn. Enter the row and column of your piece to move:");
            if (int.TryParse(Console.ReadLine(), out int fromRow) && int.TryParse(Console.ReadLine(), out int fromCol))
            {
                Console.WriteLine("Enter the row and column within the grid to move your piece to:");
                if (int.TryParse(Console.ReadLine(), out int toRow) && int.TryParse(Console.ReadLine(), out int toCol))
                {
                    if (board.GetCell(fromRow - 1, fromCol - 1) == Symbol &&
                        board.IsInGrid(toRow - 1, toCol - 1) &&
                        board.GetCell(toRow - 1, toCol - 1) == ' ')
                    {
                        if (board.MovePiece(fromRow - 1, fromCol - 1, toRow - 1, toCol - 1))
                        {
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid move. Try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Try again.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Try again.");
            }
        }
    }
}

