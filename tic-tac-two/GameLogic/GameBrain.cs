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

            var gridEndRow = GameState.GridStartRow + Config.MovableGridSize;
            var gridEndCol = GameState.GridStartCol + Config.MovableGridSize;

            for (int i = GameState.GridStartRow; i < gridEndRow; i++)
            {
                for (int j = GameState.GridStartCol; j < gridEndCol; j++)
                {
                    if (CheckWinFromPosition(i, j, player))
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
            // if you're wondering why this looks ratchet then I didn't want to use chatgpt
        }

        private bool CheckSouth(int startRow, int startCol, char player)
        {
            int piecesInARow = 0;
            for (int i = 0; i < Config.WinningCondition; i++)
            {
                int row = startRow + i;
                if (row >= Config.Height || row >= GameState.GridStartRow + Config.MovableGridSize) break;

                if (GameState.Board[row, startCol] == player)
                    piecesInARow++;
                else
                    break;
            }

            return piecesInARow >= Config.WinningCondition;
        }

        private bool CheckNorth(int startRow, int startCol, char player)
        {
            int piecesInARow = 0;
            for (int i = 0; i < Config.WinningCondition; i++)
            {
                int row = startRow - i;
                if (row < 0 || row < GameState.GridStartRow) break;

                if (GameState.Board[row, startCol] == player)
                    piecesInARow++;
                else
                    break;
            }

            return piecesInARow >= Config.WinningCondition;
        }

        private bool CheckEast(int startRow, int startCol, char player)
        {
            int piecesInARow = 0;
            for (int i = 0; i < Config.WinningCondition; i++)
            {
                int col = startCol + i;
                if (col >= Config.Width || col >= GameState.GridStartCol + Config.MovableGridSize) break;

                if (GameState.Board[startRow, col] == player)
                    piecesInARow++;
                else
                    break;
            }

            return piecesInARow >= Config.WinningCondition;
        }

        private bool CheckWest(int startRow, int startCol, char player)
        {
            int piecesInARow = 0;
            for (int i = 0; i < Config.WinningCondition; i++)
            {
                int col = startCol - i;
                if (col < 0 || col < GameState.GridStartCol) break;

                if (GameState.Board[startRow, col] == player)
                    piecesInARow++;
                else
                    break;
            }

            return piecesInARow >= Config.WinningCondition;
        }

        private bool CheckSouthEast(int startRow, int startCol, char player)
        {
            int piecesInARow = 0;
            for (int i = 0; i < Config.WinningCondition; i++)
            {
                int row = startRow + i, col = startCol + i;
                if (row >= Config.Height || col >= Config.Width ||
                    row >= GameState.GridStartRow + Config.MovableGridSize ||
                    col >= GameState.GridStartCol + Config.MovableGridSize) break;

                if (GameState.Board[row, col] == player)
                    piecesInARow++;
                else
                    break;
            }

            return piecesInARow >= Config.WinningCondition;
        }

        private bool CheckNorthEast(int startRow, int startCol, char player)
        {
            int piecesInARow = 0;
            for (int i = 0; i < Config.WinningCondition; i++)
            {
                int row = startRow - i, col = startCol + i;
                if (row < 0 || col >= Config.Width ||
                    row < GameState.GridStartRow ||
                    col >= GameState.GridStartCol + Config.MovableGridSize) break;

                if (GameState.Board[row, col] == player)
                    piecesInARow++;
                else
                    break;
            }

            return piecesInARow >= Config.WinningCondition;
        }

        private bool CheckSouthWest(int startRow, int startCol, char player)
        {
            int piecesInARow = 0;
            for (int i = 0; i < Config.WinningCondition; i++)
            {
                int row = startRow + i, col = startCol - i;
                if (row >= Config.Height || col < 0 ||
                    row >= GameState.GridStartRow + Config.MovableGridSize ||
                    col < GameState.GridStartCol) break;

                if (GameState.Board[row, col] == player)
                    piecesInARow++;
                else
                    break;
            }

            return piecesInARow >= Config.WinningCondition;
        }

        private bool CheckNorthWest(int startRow, int startCol, char player)
        {
            int piecesInARow = 0;
            for (int i = 0; i < Config.WinningCondition; i++)
            {
                int row = startRow - i, col = startCol - i;
                if (row < 0 || col < 0 ||
                    row < GameState.GridStartRow ||
                    col < GameState.GridStartCol) break;

                if (GameState.Board[row, col] == player)
                    piecesInARow++;
                else
                    break;
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

        public void SetGameState(GameState gameState)
        {
            GameState = gameState;
        }
    }
}