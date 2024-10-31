using System.Runtime.InteropServices;

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
                   col >= GameState.gridStartCol && col < GameState.gridStartCol + Config.MovableGridSize;
        }

        public bool MoveGrid(string direction)
        {
            var gridStartRow = GameState.GridStartRow + 1; // vb saaks paremini
            var gridStartCol = GameState.gridStartCol + 1;
            
            switch (direction)
            {
                case "u":
                    if (gridStartRow <= 0) return false;
                    GameState.GridStartRow--;
                    return true;

                case "d":
                    if (!(gridStartCol + Config.MovableGridSize <= Config.Height)) return false;
                    GameState.GridStartRow++;
                    return true;

                case "r":
                    if (!(gridStartCol + Config.MovableGridSize <= Config.Width)) return false;
                    GameState.gridStartCol++;
                    return true;

                case "l":
                    if (gridStartCol <= 0) return false;
                    GameState.gridStartCol--;
                    return true;

                case "ul":
                    if (gridStartRow <= 0 || gridStartCol <= 0) return false;
                    GameState.GridStartRow--;
                    GameState.gridStartCol--;
                    return true;

                case "ur":
                    if (gridStartRow <= 0 ||
                        !(gridStartCol + Config.MovableGridSize <= Config.Width)) return false;
                    GameState.GridStartRow--;
                    GameState.gridStartCol++;
                    return true;

                case "dl":
                    if (!(GameState.gridStartCol + Config.MovableGridSize <= Config.Height) ||
                        GameState.gridStartCol <= 0) return false;
                    GameState.GridStartRow++;
                    GameState.gridStartCol--;
                    return true;
                
                case "dr":
                    if (!(gridStartRow + Config.MovableGridSize <= Config.Width) ||
                        !(gridStartCol + Config.MovableGridSize <= Config.Height)) return false;
                    GameState.GridStartRow++;
                    GameState.gridStartCol++;
                    return true;
            }
            return false;
        }

        public bool PlacePiece(int x, int y, char playerSymbol)
        {
            if (!IsInGrid(x, y) || GameState.Board[x, y] != '\0' || playerSymbol != GameState.MovingPlayer)
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

        public bool MovePiece(int oldX, int oldY, int newX, int newY)
        {
            oldX--;
            oldY--;
            newX--;
            newY--;
            if (GameState.Board[oldX, oldY] != GameState.MovingPlayer 
                || !IsInGrid(newY, newY) 
                || GameState.Board[newX, newY] != '\0')
            {
                return false;
            }
            GameState.Board[oldX, oldY] = '\0';
            GameState.Board[newX, newY] = GameState.MovingPlayer;
            return true;
        }
    }
}