using DAL;
using GameLogic;

namespace tic_tac_two
{
    public class GameController
    {

        private static GameConfiguration _currentConfig = default!;
        private static GameBrain Brain = default!;
        private static IGameRepository GameRepository = default!;
        
        private static string _userName = default!;
        
        private static bool _quit = false;
        
        public GameController(string userName)
        {
            _userName = userName;
            _currentConfig = ConfigurationManager.CurrentConfiguration;
            Brain = new GameBrain(new GameState(_currentConfig));
            GameRepository = new GameRepositoryDb(); // change here between json and db
        }

        public string MainLoop()
        {
            var gameState = Brain.GameState; // miskiprst null
            
            GameIntro(); 

            do
            {
                DrawBoard(gameState);
                Console.WriteLine($"Player {gameState.MovingPlayer}, it's your turn!");

                if (Brain.GetMovingPlayerPiecesPlaced() < _currentConfig.InitialMoves)
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
                    Console.WriteLine(
                        "You somehow managed to tie in the game that's almost impossible to tie in... try harder next time.");
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
                if (_quit)
                {
                    ResetGame();
                    break;
                }
            } while (true);

            return "";
        }
        

        private void ResetGame()
        {
            Brain.SetGameState(new GameState(_currentConfig));
        }


        private void TicTacToeMove(GameState gameState) // todo: test
        {
            var validMoveMade = false;
            while (!validMoveMade)
            {
                Console.WriteLine("Enter the number for your next action:");
                
                if (Brain.GetMovingPlayerPiecesPlaced() != 0) {
                    Console.WriteLine("1. Move your existing piece");
                }

                if (Brain.GetMovingPlayerPiecesPlaced() < _currentConfig.MaxPieces)
                {
                    Console.WriteLine("2. Place a piece");
                }
                

                Console.WriteLine("X. Save and quit");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        validMoveMade = MovePiece();
                        break;

                    case "2":
                        if (Brain.GetMovingPlayerPiecesPlaced() < _currentConfig.MaxPieces)
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
                    case "X" or "x":
                        SaveAndQuit();
                        break;

                    default:
                        Console.WriteLine("PLS! enter valid numbers.");
                        break;
                }
            }
        }

        private void SaveAndQuit()
        {
            Console.WriteLine("Enter a name for the game to continue later, or if u don't care just press enter:");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                input = DateTime.Now.ToString("dd-MMMM-yyyy_HH-mm-ss");
            }
            GameRepository.SaveGame(Brain.GameState, _currentConfig.ConfigName, input, _userName);
            Console.WriteLine("Game saved. Press enter to run away to main menu..");
            Console.ReadLine();
            _quit = true;
        }


        private void AdvancedMove(GameState gameState)
        {
            var validMoveMade = false;
            while (!validMoveMade)
            {
                Console.WriteLine("Enter the number of your next action:");
                Console.WriteLine("1. Move your existing piece");
                Console.WriteLine("2. Move the griddy");
                if (Brain.GetMovingPlayerPiecesPlaced() < _currentConfig.MaxPieces)
                {
                    Console.WriteLine("3. Place a new piece");
                }

                Console.WriteLine("X. Save and quit");

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
                        if (Brain.GetMovingPlayerPiecesPlaced() < _currentConfig.MaxPieces)
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
                    case "X" or "x":
                        SaveAndQuit();
                        break;

                    default:
                        Console.WriteLine("PLS! enter a valid number.");
                        break;
                }
            }
        }

        private bool MovePiece()
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
                    Console.WriteLine("Not this... Please enter the coordinates in the form of x,y: ");
                    continue;
                }

                if (Brain.GameState.Board[oldX - 1, oldY - 1] != Brain.GameState.MovingPlayer)
                    // checkib meetodi sees ka aga siin oluline sest siis kysib vanad koodrinaadid kohe uuesti
                {
                    Console.WriteLine("Are you blind? Pick a coordinate with your own piece.");
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
                        "Please enter the coordinates in the form of (x,y) (literally same as every last time).");
                    continue;
                }

                //error check siin ka et ma saaks normaalset tagasisidet anda, aga gamebrain checkib ka
                if (Brain.GameState.Board[newX - 1, newY - 1] != '\0' || !Brain.IsInGrid(newX - 1, newY - 1))
                {
                    Console.WriteLine(
                        "Try again... Is it really that hard to pick a free coordinate inside the grid?");
                    continue;
                }

                // to array values
                oldX--;
                oldY--;
                newX--;
                newY--;

                if (Brain.MovePiece(oldX, oldY, newX, newY))
                {
                    return true;
                }

                Console.WriteLine("Something went wrong... dont know what though.");
            }
        }

        private bool MoveGrid()
        {
            while (true)
            {
                Console.WriteLine("Enter the shortcut letter for direction you want to move the grid 1 step.");
                Console.WriteLine(
                    "Options are up [u], down [d], left [l], right [r], up-left [ul], up-right [ur], down-left [dl], down-right [dr]");
                var direction = Console.ReadLine();

                if (direction == null)
                {
                    Console.WriteLine("PLS! enter a valid direction.");
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

        private void SimpleMove(GameState gameState)
        {
            var validMoveMade = false;
            while (!validMoveMade)
            {
                Console.Write("Enter row and column (x,y) or X to save and quit: ");

                var input = Console.ReadLine();

                if (input == "X" || input == "x")
                {
                    SaveAndQuit();
                    break;
                }

                var coords = input!.Split(",");

                if (int.TryParse(coords[0], out _) && int.TryParse(coords[1], out _)) // looks clumsy but okay
                {
                    var x = int.Parse(coords[0]) - 1;
                    var y = int.Parse(coords[1]) - 1;
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
                    Console.WriteLine("PLS!! enter valid numbers.");
                }
            }
        }

        private void GameIntro()
        {
            Console.WriteLine("Welcome to this ratchet ass console game!");
            Console.WriteLine($"You are playing with configuration {_currentConfig.ConfigName} :)");
            Console.WriteLine(
                $"The board is {_currentConfig.Width}x{_currentConfig.Height}, ");
            if (_currentConfig.ConfigName != "Tic-Tac-Toe")
            {
                Console.WriteLine($"you can place the total of {_currentConfig.MaxPieces} pieces, " +
                                  $"you can move the grid after {_currentConfig.InitialMoves} moves, ");
            }
            
            Console.WriteLine($"and you must win by aligning {_currentConfig.WinningCondition} pieces.");
            Console.WriteLine("Type X at any point to save the game and return to main menu.");
            Console.WriteLine("Good luck nerds!");
            Console.WriteLine();
            
        }

        private void DrawBoard(GameState gameState)
        {
            Console.Write("  ");
            for (int i = 0; i < gameState.Config.Width; i++)
            {
                Console.Write(i < 10 ? $"  {i + 1} " : $" {i + 1} ");
            }

            Console.WriteLine();

            for (int i = 0; i < gameState.Config.Height; i++)
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

        public string LoadGames()
        {

            var games = GameRepository.GetGamesByUser(_userName);

            if (games.Count == 0)
            {
                Console.WriteLine("Bro u have to play and save something first to see stuff here.");
                Console.ReadLine();
                return "";
            }

            Console.WriteLine(
                $"Pick a game to continue then, {_userName}, if a new game is not good enough for you...:");
            for (int i = 0; i < games.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {games[i]}");
            }

            var input = Console.ReadLine();
            
            if (int.TryParse(input, out var gameIndex))
            {
                var chosenGame = games[gameIndex - 1];
                if (gameIndex <= 0 || gameIndex > games.Count)
                {
                    Console.WriteLine("Nope! Try again.");
                    return "";
                }
                var gameState = GameRepository.GetGameByName(chosenGame);
                Brain.SetGameState(gameState);
                Console.WriteLine($"Enjoy playing your loaded game '{chosenGame}'!");
                MainLoop();
            }
            else
            {
                Console.WriteLine("NOPE! Try again.");
            }

            return "";
        }

     
    }
}