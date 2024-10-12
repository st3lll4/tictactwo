namespace GameLogic
{
    // the place where asked what you want to do cannot have a move grid option if board size is 3x3
    public class Game
    {
        private Board Board {get; set;}
        private Player Player1 { get; set; }
        private Player Player2 { get; set; }
        private Player CurrentPlayer { get; set; }
        private int Player1PiecesPlaced { get; set; } = 0;
        private int Player2PiecesPlaced { get; set; } = 0;

        //TODO: CHANGE ALL FIELDS TO PROPERTIES


        public Game(GameConfiguration config)
        {

            Board = new Board(config.Width, config.Height);

            Player1 = new Player(config.Player1Symbol);
            Player2 = new Player(config.Player2Symbol);

            CurrentPlayer = config.StartingPlayer == "Player 1" ? Player1 : Player2;

        }

        public void Start()
        { // todo: dont clear console all the time
            while (true)
            {
                Console.Clear();
                Board.Draw();

                if ((CurrentPlayer == Player1 && Player1PiecesPlaced < 2) ||
                    (CurrentPlayer == Player2 && Player2PiecesPlaced < 2))
                {
                    Console.WriteLine($"{CurrentPlayer.Symbol}'s turn: Place a piece within the grid.");
                    CurrentPlayer.PlacePiece(Board);
                    if (CurrentPlayer == Player1) Player1PiecesPlaced++;
                    else Player2PiecesPlaced++;
                }
                else
                {
                    Console.WriteLine($"{CurrentPlayer.Symbol}'s turn: Choose an action:");
                    Console.WriteLine("1. Place a piece within the grid.");
                    Console.WriteLine("2. Move the grid.");
                    Console.WriteLine("3. Move one of your pieces into the grid.");
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            CurrentPlayer.PlacePiece(Board);
                            break;
                        case "2":
                            MoveGrid();
                            break;
                        case "3":
                            CurrentPlayer.MoveOwnPiece(Board);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Press Enter to try again.");
                            Console.ReadLine();
                            continue;
                    }
                }

                // Check for win or tie after each action
                if (CheckWin())
                {
                    Console.Clear();
                    Board.Draw();
                    Console.WriteLine($"{CurrentPlayer.Symbol} wins!");
                    Console.ReadLine();
                    break;
                }


                if (CheckTie())
                {
                    Console.Clear();
                    Board.Draw();
                    Console.WriteLine("It's a tie!");
                    Console.ReadLine();
                    break;
                }

                SwitchPlayer();
            }
        }
        
        private void MoveGrid()
        {
            Console.WriteLine("Enter direction to move the grid (up, down, left, right, up-left, up-right, down-left, down-right):");
            var direction = Console.ReadLine();
            int dRow = 0, dCol = 0;
            switch (direction.ToLower())
            {
                case "up":
                    dRow = -1;
                    break;
                case "down":
                    dRow = 1;
                    break;
                case "left":
                    dCol = -1;
                    break;
                case "right":
                    dCol = 1;
                    break;
                case "up-left":
                    dRow = -1; dCol = -1;
                    break;
                case "up-right":
                    dRow = -1; dCol = 1;
                    break;
                case "down-left":
                    dRow = 1; dCol = -1;
                    break;
                case "down-right":
                    dRow = 1; dCol = 1;
                    break;
                default:
                    Console.WriteLine("Invalid direction. Press Enter to continue.");
                    Console.ReadLine();
                    return;
            }

            if (Board.MoveGrid(dRow, dCol))
            {
                Console.WriteLine("Grid moved successfully. Press Enter to continue.");
            }
            else
            {
                Console.WriteLine("Cannot move grid in that direction. Press Enter to continue.");
            }
            Console.ReadLine();
        }
        
        
        private bool CheckWin()
        {
            return Board.CheckWin(CurrentPlayer.Symbol);
        }

        private bool CheckTie()
        {
            bool player1Wins = Board.CheckWin(Player1.Symbol);
            bool player2Wins = Board.CheckWin(Player2.Symbol);
            return player1Wins && player2Wins;
        }


        private void SwitchPlayer()
        {
            CurrentPlayer = CurrentPlayer == Player1 ? Player2 : Player1;
        }

    }
}