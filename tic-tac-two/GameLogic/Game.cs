namespace GameLogic
{
    public class Game
    {
        private Board _board;
        private Player _player1;
        private Player _player2;
        private Player _currentPlayer;
        private int _player1PiecesPlaced = 0;
        private int _player2PiecesPlaced = 0;
        
        //TODO: CHANGE ALL FIELDS TO PROPERTIES


        public Game(GameConfiguration config)
        {

            _board = new Board(config.Width, config.Height);

            _player1 = new Player(config.Player1Symbol);
            _player2 = new Player(config.Player2Symbol);

            _currentPlayer = config.StartingPlayer == "Player 1" ? _player1 : _player2;

        }

        public void Start()
        { // todo: dont clear console all the time
            while (true)
            {
                Console.Clear();
                _board.Draw();

                if ((_currentPlayer == _player1 && _player1PiecesPlaced < 2) ||
                    (_currentPlayer == _player2 && _player2PiecesPlaced < 2))
                {
                    Console.WriteLine($"{_currentPlayer.Symbol}'s turn: Place a piece within the grid.");
                    _currentPlayer.PlacePiece(_board);
                    if (_currentPlayer == _player1) _player1PiecesPlaced++;
                    else _player2PiecesPlaced++;
                }
                else
                {
                    Console.WriteLine($"{_currentPlayer.Symbol}'s turn: Choose an action:");
                    Console.WriteLine("1. Place a piece within the grid.");
                    Console.WriteLine("2. Move the grid.");
                    Console.WriteLine("3. Move one of your pieces into the grid.");
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            _currentPlayer.PlacePiece(_board);
                            break;
                        case "2":
                            MoveGrid();
                            break;
                        case "3":
                            _currentPlayer.MoveOwnPiece(_board);
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
                    _board.Draw();
                    Console.WriteLine($"{_currentPlayer.Symbol} wins!");
                    Console.ReadLine();
                    break;
                }


                if (CheckTie())
                {
                    Console.Clear();
                    _board.Draw();
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

            if (_board.MoveGrid(dRow, dCol))
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
            return _board.CheckWin(_currentPlayer.Symbol);
        }

        private bool CheckTie()
        {
            bool player1Wins = _board.CheckWin(_player1.Symbol);
            bool player2Wins = _board.CheckWin(_player2.Symbol);
            return player1Wins && player2Wins;
        }


        private void SwitchPlayer()
        {
            _currentPlayer = _currentPlayer == _player1 ? _player2 : _player1;
        }

    }
}