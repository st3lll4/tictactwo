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
                    if (CantMoveUp()) return false;
                    GameState.GridStartRow--;
                    return true;

                case "d":
                    if (CantMoveDown()) return false;
                    GameState.GridStartRow++;
                    return true;

                case "r":
                    if (CantMoveRight()) return false;
                    GameState.GridStartCol++;
                    return true;

                case "l":
                    if (CantMoveLeft()) return false;
                    GameState.GridStartCol--;
                    return true;

                case "ul":
                    if (CantMoveUp() || CantMoveLeft()) return false;
                    GameState.GridStartRow--;
                    GameState.GridStartCol--;
                    return true;

                case "ur":
                    if (CantMoveUp() || CantMoveRight()) return false;
                    GameState.GridStartRow--;
                    GameState.GridStartCol++;
                    return true;

                case "dl":
                    if (CantMoveDown() || CantMoveLeft()) return false;
                    GameState.GridStartRow++;
                    GameState.GridStartCol--;
                    return true;

                case "dr":
                    if (CantMoveDown() || CantMoveRight()) return false;
                    GameState.GridStartRow++;
                    GameState.GridStartCol++;
                    return true;

                default:
                    return false;
            }
        }

        private bool CantMoveDown()
        {
            return GameState.GridStartRow + Config.MovableGridSize >= Config.Height;
        }

        private bool CantMoveUp()
        {
            return GameState.GridStartRow == 0;
        }

        private bool CantMoveRight()
        {
            return GameState.GridStartCol + Config.MovableGridSize >= Config.Width;
        }

        private bool CantMoveLeft()
        {
            return GameState.GridStartCol == 0;
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

        public bool CheckWin(char player)
        {
            if (GetMovingPlayerPiecesPlaced() < Config.WinningCondition) return false;
            
            var startRow = GameState.GridStartRow;
            var startCol = GameState.GridStartCol;


            for (var i = startRow; i < startRow + Config.MovableGridSize; i++)
            {
                for (var j = startCol; j < startCol + Config.MovableGridSize; j++) // errors
                {

                    if (CheckWinFromPosition(startRow + i, startCol + j, player))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        

        private bool CheckWinFromPosition(int startRow, int startCol, char player)
        {
            if (startRow < 0 || startRow >= Config.Height || startCol < 0 || startCol >= Config.Width)
            {
                return false; 
            }
            return (CheckSouth(startRow, startCol, player))
                   || CheckEast(startRow, startCol, player)
                   || CheckSouthEast(startRow, startCol, player)
                   || CheckNorth(startRow, startCol, player)
                   || CheckWest(startRow, startCol, player)
                   || CheckNorthEast(startRow, startCol, player)
                   || CheckNorthWest(startRow, startCol, player)
                   || CheckSouthWest(startRow, startCol, player);
        }

        private bool CheckSouthWest(int startRow, int startCol, char player)
        {
            var piecesInARow = 0;
            for (var i = startRow; i < Config.MovableGridSize; i++)
            {
                if (startRow + i >= Config.Height || startCol - i < 0) break;
                if (GameState.Board[startRow + i, startCol - i] == player)
                {
                    piecesInARow++;
                }
                else
                {
                    break;
                }
            } return piecesInARow >= Config.WinningCondition;
        }

        private bool CheckNorthWest(int startRow, int startCol, char player)
        {
            var piecesInARow = 0;
            for (var i = 0 ; i < Config.MovableGridSize; i++)
            {
                if (startRow - i < 0 || startCol - i < 0) break; 
                if (GameState.Board[startRow - i, startCol - i] == player)
                {
                    piecesInARow++;
                }
                else
                {
                    break;
                }
            } return piecesInARow >= Config.WinningCondition;
        }

        private bool CheckNorthEast(int startRow, int startCol, char player)
        {
            var piecesInARow = 0;
            for (var i = 0 ; i < Config.MovableGridSize; i++)
            {
                if (startRow - i < 0 || startCol + i >= Config.Width) break; 
                if (GameState.Board[startRow - i, startCol + i] == player)
                {
                    piecesInARow++;
                }
                else
                {
                    break;
                }
            } return piecesInARow >= Config.WinningCondition;
        }

        private bool CheckWest(int startRow, int startCol, char player)
        {
            var piecesInARow = 0;
            for (var i = startRow; i < Config.MovableGridSize; i--)
            {
                if (startRow - i < 0) break; 
                if (GameState.Board[startRow, startCol - i] == player)
                {
                    piecesInARow++;
                }
                else
                {
                    break;
                }
            } return piecesInARow >= Config.WinningCondition;
        }

        private bool CheckNorth(int startRow, int startCol, char player)
        {
            var piecesInARow = 0;
            for (var i = startRow; i >= 0; i--)
            {
                if (startRow - i < 0) break; 
                if (GameState.Board[startRow - i, startCol] == player)
                {
                    piecesInARow++;
                }
                else
                {
                    break;
                }
            }
            return piecesInARow >= Config.WinningCondition;
        }

        private bool CheckSouthEast(int startRow, int startCol, char player)
        {
            var piecesInARow = 0;
            for (var i = 0; i < Config.MovableGridSize; i++)
            {
                if (startRow + i >= Config.Height - 1 || startCol + i >= Config.Width) break;
                if (GameState.Board[startRow + i, startCol + i] == player)
                {
                    piecesInARow++;
                }
                else
                {
                    break;
                }
            } return piecesInARow >= Config.WinningCondition;
        }

        private bool CheckEast(int startRow, int startCol, char player)
        {
            var piecesInARow = 0;
            for (var i = 0; i < Config.MovableGridSize; i++)
            {
                if (startCol + i >= Config.Width) break; 
                if (GameState.Board[startRow, startCol + i] == player)
                {
                    piecesInARow++;
                }
                else
                {
                    break;
                }
            }
            return piecesInARow >= Config.WinningCondition;
        }

        private bool CheckSouth(int startRow, int startCol, char player)
        {
            var piecesInARow = 0;
            for (var i = startRow; i < Config.MovableGridSize; i++)
            {
                if (startRow + i >= Config.Height) break; 
                if (GameState.Board[startRow + i, startCol] == player)
                {
                    piecesInARow++;
                }
                else
                {
                    break;
                }
            }

            return piecesInARow >= Config.WinningCondition;
        }


        public bool CheckTie()
        {
            if (GameState.Player1PiecesPlaced < GameState.WinCondition
                || GameState.Player2PiecesPlaced < GameState.WinCondition) return false;
            return CheckWin(Config.Player1Symbol) && CheckWin(Config.Player2Symbol);
        }

        public int GetMovingPlayerPiecesPlaced()
        {
            return GameState.MovingPlayer == Config.Player1Symbol
                ? GameState.Player1PiecesPlaced
                : GameState.Player2PiecesPlaced;
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