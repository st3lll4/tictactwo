namespace GameLogic
{
    public class GameBrain
    {
        private GameState GameState { get; set; }
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
            if (!IsInGrid(x, y) || GameState.Board[x, y] != '\0') // kas vordleb?? 
            {
                return false;
            }
            GameState.Board[x, y] = playerSymbol;
            SwitchPlayer();
            return true;
        }

        private void SwitchPlayer()
        {
            GameState.NextMoveBy = GameState.NextMoveBy == GameState.Config.Player1Symbol
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
    }
}