using GameLogic;
using tic_tac_two;
using UI;

namespace tic_tac_two
{
    public static class GameController
    {
        private static readonly GameConfiguration CurrentConfig = ConfigurationManager.CurrentConfiguration;
        private static readonly GameBrain Brain = new(new GameState(CurrentConfig));

        public static string MainLoop()
        {
            var gameState = Brain.GameState;

            do
            {
                DrawBoard(gameState);
                Console.WriteLine($"Player {gameState.MovingPlayer}, it's your turn!");

                if (Brain.GetMovingPlayerPiecesPlaced() <= CurrentConfig.InitialMoves)
                {
                    SimpleMove(gameState);
                }
                else if (!gameState.IsStandardTicTacToe) // peaks sellele tegema ka oma ifi
                {
                    AdvancedMove(gameState);
                }

                if (Brain.CheckWin())
                {
                    Console.WriteLine($"Player {gameState.MovingPlayer} wins!");
                    break;
                }

                if (Brain.CheckTie())
                {
                    Console.WriteLine("A tie!");
                    break;
                }
            } while (true);

            return "";
        }

        private static void AdvancedMove(GameState gameState)
        {
            var validMoveMade = false;
            while (!validMoveMade)
            {
                Console.WriteLine("Enter the number of your next action:");
                Console.WriteLine("1. Move your existing piece");
                Console.WriteLine("2. Move the grid");
                if (Brain.GetMovingPlayerPiecesPlaced() < CurrentConfig.MaxPieces)
                {
                    Console.WriteLine("3. Place a new piece");
                }

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.WriteLine("Enter the coordinates of the piece you want to move: ");
                        var oldCoord = Console.ReadLine()!.Split(",");
                        var oldX = int.Parse(oldCoord[0]) - 1;
                        var oldY = int.Parse(oldCoord[1]) - 1;

                        Console.WriteLine("Enter new coordinates: ");
                        var newCoord = Console.ReadLine()!.Split(",");
                        var newX = int.Parse(newCoord[0]) - 1;
                        var newY = int.Parse(newCoord[1]) - 1;

                        if (Brain.MovePiece(oldX, oldY, newX, newY))
                        {
                            validMoveMade = true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid move! New place has to be free and inside the grid");
                        }
                        break;

                    case "2":
                        //Brain.MoveGrid();
                        validMoveMade = true; 
                        break;

                    case "3":
                        SimpleMove(gameState);
                        break;

                    default:
                        Console.WriteLine("Invalid input! PLS! enter a valid number.");
                        break;
                }
            }
        }

        private static void SimpleMove(GameState gameState)
        {
            var validMoveMade = false;
            while (!validMoveMade)
            {
                Console.Write("Enter row and column (x,y): ");
                var input = Console.ReadLine()!.Split(",");

                if (int.TryParse(input[0], out _) && int.TryParse(input[1], out _)) // looks clumsy but okay
                {
                    var x = int.Parse(input[0]) - 1;
                    var y = int.Parse(input[1]) - 1;
                    if (Brain.PlacePiece(x, y, gameState.MovingPlayer))
                    {
                        validMoveMade = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid move! Place your piece on a free spot inside the grid.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input! PLS! enter a valid number.");
                }
            }
        }


        private static void DrawBoard(GameState gameState)
        {
            Console.Write("  ");
            for (int i = 0; i < gameState.Config.Width; i++)
            {
                Console.Write(i < 10 ? $"  {i + 1} " : $" {i + 1} ");
            }

            Console.WriteLine();

            for (var i = 0; i < gameState.Config.Height; i++)
            {
                Console.Write(i < 9 ? $" {i + 1} " : $"{i + 1} ");

                for (var j = 0; j < gameState.Config.Width; j++)
                {
                    Console.BackgroundColor = Brain.IsInGrid(i, j) ? ConsoleColor.Black : ConsoleColor.Gray;

                    var toWrite = gameState.Board[i, j] == '\0' ? " " : $"{gameState.Board[i, j]}";
                    Console.Write($" {toWrite} ");
                    Console.ResetColor();
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
    }
}