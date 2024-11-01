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
                   col >= GameState.GridStartCol && col < GameState.GridStartCol + Config.MovableGridSize;
        }

        public bool MoveGrid(string direction)
        {
            switch (direction)
            {
                case "u":
                    if (GameState.GridStartRow == 0) return false; 
                    GameState.GridStartRow--;
                    return true; 

                case "d":
                    if ((GameState.GridStartRow + Config.MovableGridSize >= Config.Height)) return false; 
                    GameState.GridStartRow++;
                    return true; 

                case "r":
                    if ((GameState.GridStartCol + Config.MovableGridSize >= Config.Width)) return false;
                    Console.WriteLine(GameState.GridStartCol);
                    GameState.GridStartCol++;
                    return true; 

                case "l":
                    if (GameState.GridStartCol == 0) return false;
                    GameState.GridStartCol--;
                    return true; 

                case "ul":
                    if (GameState.GridStartRow == 0 || GameState.GridStartCol == 0) return false;
                    GameState.GridStartRow--;
                    GameState.GridStartCol--;
                    return true; 

                case "ur":
                    if (GameState.GridStartRow == 0 ||
                        (GameState.GridStartCol + Config.MovableGridSize >= Config.Width)) return false;
                    GameState.GridStartRow--;
                    GameState.GridStartCol++;
                    return true;

                case "dl":
                    if ((GameState.GridStartRow + Config.MovableGridSize >= Config.Height) ||
                        GameState.GridStartCol == 0) return false;
                    GameState.GridStartRow++;
                    GameState.GridStartCol--;
                    return true; 
                
                case "dr":
                    if ((GameState.GridStartRow + Config.MovableGridSize >= Config.Width) ||
                        (GameState.GridStartCol + Config.MovableGridSize >= Config.Height)) return false;
                    GameState.GridStartRow++; 
                    GameState.GridStartCol++;
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
            // SwitchPlayer();
            return true;
        }

        public void SwitchPlayer()
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