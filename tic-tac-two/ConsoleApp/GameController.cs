using GameLogic;

namespace tic_tac_two
{
    public static class GameController
    {
        private static readonly GameConfiguration CurrentConfig = ConfigurationManager.CurrentConfiguration;
        private static readonly GameBrain Brain = new(new GameState(CurrentConfig));

        public static string MainLoop()
        {
            var gameState = Brain.GameState;

            GameIntro();

            do
            {
                DrawBoard(gameState);
                Console.WriteLine($"Player {gameState.MovingPlayer}, it's your turn!");

                if (Brain.GetMovingPlayerPiecesPlaced() < CurrentConfig.InitialMoves)
                {
                    SimpleMove(gameState);
                }
                else if (!gameState.IsStandardTicTacToe)
                {
                    AdvancedMove(gameState);
                }
                else if (gameState.IsStandardTicTacToe)
                {
                    TicTacToeMove(gameState);
                }
                

                if (Brain.CheckTie())
                {
                    DrawBoard(gameState);
                    Console.WriteLine("You somehow managed to tie in the game that's almost impossible to tie in... try harder next time.");
                    Console.WriteLine("Press enter to return to main menu, geniuses... "); 
                    Console.ReadLine();
                    break;
                }
                
                if (Brain.CheckWin(gameState.MovingPlayer))
                {
                    DrawBoard(gameState);
                    Console.WriteLine($"Player {gameState.MovingPlayer} wins! Winner winner, chicken dinner!");
                    Console.WriteLine("Press enter to return to main menu, genius... "); 
                    Console.ReadLine();
                    break;
                }
                Brain.SwitchPlayer();
                
            } while (true);

            return "";
        }


        private static void TicTacToeMove(GameState gameState) // todo: test
        {
            var validMoveMade = false;
            while (!validMoveMade)
            {
                Console.WriteLine("Enter the number of your next action:");
                Console.WriteLine("1. Move your existing piece");

                if (Brain.GetMovingPlayerPiecesPlaced() < CurrentConfig.MaxPieces)
                {
                    Console.WriteLine("2. Place a piece");
                }

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        validMoveMade = MovePiece();
                        break;

                    case "2":
                        if (Brain.GetMovingPlayerPiecesPlaced() < CurrentConfig.MaxPieces)
                        {
                            SimpleMove(gameState);
                            validMoveMade = true;
                        }
                        else
                        {
                            Console.WriteLine(
                                "You already used up all your pieces, move them or move the board or learn how to count or something...");
                        }

                        break;
                    default:
                        Console.WriteLine("Invalid input! PLS! enter valid numbers.");
                        break;
                }
            }
        }

        private static void AdvancedMove(GameState gameState)
        {
            var validMoveMade = false;
            while (!validMoveMade)
            {
                Console.WriteLine("Enter the number of your next action:");
                Console.WriteLine("1. Move your existing piece");
                Console.WriteLine(
                    "2. Move the griddy");
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
                            validMoveMade = true; // ?????
                        }
                        else
                        {
                            Console.WriteLine(
                                "You already used up all your pieces, move them or move the board or something....");
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
            int oldX, oldY;


            while (true)
            {
                Console.WriteLine("Enter the coordinates of the piece you want to move in the form of x,y: ");
                var oldCoord = Console.ReadLine()!.Split(",");

                if (oldCoord.Length < 2
                    || !int.TryParse(oldCoord[0], out oldX)
                    || !int.TryParse(oldCoord[1], out oldY))
                {
                    Console.WriteLine("Invalid input! Please enter the coordinates in the form of x,y: ");
                    continue;
                }

                if (Brain.GameState.Board[oldX - 1, oldY - 1] != Brain.GameState.MovingPlayer)
                    // checkib meetodi sees ka aga siin oluline sest siis kysib vanad koodrinaadid kohe uuesti
                {
                    Console.WriteLine("Invalid input! Pick a coordinate with your own piece, damn it.");
                    continue;
                }

                break;
            }

            while (true)
            {
                Console.WriteLine("Enter the new coordinates to move to: ");
                var newCoord = Console.ReadLine()!.Split(",");

                if (newCoord.Length < 2
                    || !int.TryParse(newCoord[0], out var newX)
                    || !int.TryParse(newCoord[1], out var newY))
                {
                    Console.WriteLine(
                        "Invalid input! Please enter the coordinates in the form of (x,y) (literally same as last time).");
                    continue;
                }

                //error check siin ka et ma saaks normaalset tagasisidet anda, aga gamebrain checkib ka
                if (Brain.GameState.Board[newX - 1, newY - 1] != '\0' || !Brain.IsInGrid(newX - 1, newY - 1))
                {
                    Console.WriteLine(
                        "Invalid input! Is it really that hard to pick a free coordinate inside the grid?");
                    continue;
                }

                if (Brain.MovePiece(oldX, oldY, newX, newY))
                {
                    return true;
                }

                Console.WriteLine("Something went wrong... dont know what though.");
            }
        }

        private static bool MoveGrid()
        {
            while (true)
            {
                Console.WriteLine("Enter the shortcut letter for direction you want to move the grid 1 step.");
                Console.WriteLine(
                    "Options are up [u], down [d], left [l], right [r], up-left [ul], up-right [ur], down-left [dl], down-right [dr]");
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
                    Console.WriteLine("Invalid input! PLS!! enter valid numbers.");
                }
            }
        }

        private static void GameIntro()
        {
            Console.WriteLine("Welcome to this ratchet ass console game!");
            Console.WriteLine($"You are playing with configuration {CurrentConfig.GameName} :)");
            Console.WriteLine(
                $"The board is {CurrentConfig.Width}x{CurrentConfig.Height}, you can place the total of {CurrentConfig.MaxPieces} pieces, ");
            if (CurrentConfig.GameName != "Tic-Tac-Toe")
            {
                Console.WriteLine($"you can move the grid after {CurrentConfig.InitialMoves} moves, ");
            }

            Console.WriteLine($"and you must win by aligning {CurrentConfig.WinningCondition} pieces.");
            Console.WriteLine("Good luck nerds!");
            Console.WriteLine();
            Console.WriteLine("Press enter to continue, genius... ");
            Console.WriteLine();

            Console.ReadLine();
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