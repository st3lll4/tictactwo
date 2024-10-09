namespace GameLogic
{
    public class Game
    {
        private Board _board;
        private Player _player1;
        private Player _player2;
        private Player _currentPlayer;
        private int _moves;

        // Constructor accepts a GameConfiguration object
        public Game(GameConfiguration config)
        {
            // Use the configuration to initialize the board size
            _board = new Board(config.Width, config.Height); 
            
            // Use the configuration for player symbols
            _player1 = new Player(config.Player1Symbol);
            _player2 = new Player(config.Player2Symbol);

            // Set the starting player based on configuration
            _currentPlayer = config.StartingPlayer == "Player 1" ? _player1 : _player2;

            _moves = 0;
        }

        public void Start()
        {
            while (true)
            {
                Console.Clear();
                _board.Draw();
                _currentPlayer.MakeMove(_board);

                if (CheckWin())
                {
                    Console.Clear();
                    _board.Draw();
                    Console.WriteLine($"{_currentPlayer.Symbol} wins!");
                    Console.ReadLine();
                    break;
                }

                _moves++;
                SwitchPlayer();
            }
        }

        private void SwitchPlayer()
        {
            _currentPlayer = _currentPlayer == _player1 ? _player2 : _player1;
        }

        private bool CheckWin()
        {
            return _board.CheckWin(_currentPlayer.Symbol);
        }
    }
}
