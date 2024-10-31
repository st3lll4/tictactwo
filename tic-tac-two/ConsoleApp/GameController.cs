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

                if (Brain.GetMovingPlayerPiecesPlaced() < CurrentConfig.InitialMoves)
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
                Console.WriteLine("2. Move the griddy"); // something weird happens if runs twice in a row ? idk if still, check todo
                if (Brain.GetMovingPlayerPiecesPlaced() < CurrentConfig.MaxPieces)
                {
                    Console.WriteLine("3. Place a new piece");
                }

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        validMoveMade = MovePiece();
                        break;

                    case "2":
                        validMoveMade = MoveGrid();
                        break;

                    case "3":
                        if (Brain.GetMovingPlayerPiecesPlaced() < CurrentConfig.MaxPieces)
                        {
                            SimpleMove(gameState);
                        }
                        else
                        {
                            Console.WriteLine("You already used up all your pieces, move them or move the board or something....");
                        }
                        break;

                    default:
                        Console.WriteLine("Invalid input! PLS! enter a valid number.");
                        break;
                }
            }
        }

        private static bool MovePiece()
        {
            while (true)
            {
                Console.WriteLine("Enter the coordinates of the piece you want to move: ");
                var oldCoord = Console.ReadLine()!.Split(",");
                if (oldCoord.Length < 2 || !int.TryParse(oldCoord[0], out var oldX) || !int.TryParse(oldCoord[1], out var oldY))
                {
                    Console.WriteLine("Invalid input! Please enter valid coordinates (x,y).");
                    continue; 
                }
                

                Console.WriteLine("Enter new coordinates: ");
                var newCoord = Console.ReadLine()!.Split(",");
                if (newCoord.Length < 2 || !int.TryParse(newCoord[0], out var newX) || !int.TryParse(newCoord[1], out var newY))
                {
                    Console.WriteLine("Invalid input! Please enter valid coordinates (x,y).");
                    continue; 
                }

                if (Brain.MovePiece(oldX, oldY, newX, newY))
                {
                    return true; 
                }
                Console.WriteLine("Invalid move! The new position must be free and inside the grid.");
            }
        }


        private static bool MoveGrid()
        {
            while (true)
            {
                Console.WriteLine("Enter the shortcut letter for direction you want to move the grid 1 step.");
                Console.WriteLine("Options are up [u], down [d], left [l], right [r], up-left [ul], up-right [ur], down-left [dl], down-right [dr]");
                var direction = Console.ReadLine();

                if (direction == null)
                {
                    Console.WriteLine("Invalid input! PLS! enter a valid direction.");
                    continue; 
                }
                
                direction = direction.ToLower();

                string[] availableDirections = ["u", "d", "l", "r", "ul", "ur", "dl", "dr"];
                if (!availableDirections.Contains(direction))
                {
                    Console.WriteLine("Please for the love of god, pick a valid direction!!!");
                    continue; 
                }

                if (Brain.MoveGrid(direction))
                {
                    return true; 
                }
                Console.WriteLine("Invalid move! You can't move the grid like that dude.");
                
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