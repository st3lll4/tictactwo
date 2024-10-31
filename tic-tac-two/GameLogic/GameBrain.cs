namespace GameLogic
{
    public class GameBrain
    {
        public GameState GameState { get; set; }
        private GameConfiguration Config => GameState.Config;

        public GameBrain(GameState gameState)
        {
            GameState = gameState;
        }

        public bool IsInGrid(int row, int col) 
        {
            return row >= GameState.GridStartRow && row < GameState.GridStartRow + Config.MovableGridSize &&
                   col >= GameState.GridStartCol && col < GameState.GridStartCol + Config.MovableGridSize;
        }

        public void MoveGrid(int dRow, int dCol) 
        {
            //todo
        }

        public bool PlacePiece(int x, int y, char playerSymbol)
        {
            if (!IsInGrid(x, y) || GameState.Board[x, y] != '\0') // ???
            {
                return false;
            }
            GameState.Board[x, y] = playerSymbol;
            if (GameState.MovingPlayer == Config.Player1Symbol) 
            {
                GameState.Player1PiecesPlaced++;
            }
            else
            {
                GameState.Player2PiecesPlaced++;
            }
            SwitchPlayer();
            return true;
        }

        private void SwitchPlayer()
        {
            GameState.MovingPlayer = GameState.MovingPlayer == GameState.Config.Player1Symbol
                ? GameState.Config.Player2Symbol
                : GameState.Config.Player1Symbol;
        }

        public bool CheckWin()
        {
            // Win condition check logic using _gameState.Board and WinningCondition
            return false;
        }

        public bool CheckTie()
        {
            // Tie condition check logic
            return false;
        }
        
        public int GetMovingPlayerPiecesPlaced()
        {
            return GameState.MovingPlayer == Config.Player1Symbol ? GameState.Player1PiecesPlaced : GameState.Player2PiecesPlaced;
        }

        public void MovePiece() // variables
        {
            throw new NotImplementedException();
        }
    }
}