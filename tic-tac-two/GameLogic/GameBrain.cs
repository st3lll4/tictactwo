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
            if (playerSymbol != GameState.MovingPlayer)
            {
                return false;
            }

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

        public void MakeBotMove()
        {
            var botSymbol = GameState.MovingPlayer;

            // Check for a winning move
            if (TryToWin()) return;

            // Take the center if available
            if (GameState.Board[GameState.GridCenterCol, GameState.GridCenterRow] == '\0') 
            {
                PlacePiece(GameState.GridCenterCol, GameState.GridCenterRow, botSymbol);
                return;
            }

            var rnd = new Random();
            var randomCorner = rnd.Next(0, 4);
            
            // Choose from the corners
            var leftUp = GameState.Board[GameState.GridStartRow, GameState.GridStartCol];
            var rightUp = GameState.Board[GameState.GridStartRow, GameState.GridStartCol + Config.MovableGridSize - 1];
            var leftDown = GameState.Board[GameState.GridStartRow + Config.MovableGridSize - 1, GameState.GridStartCol];
            var rightDown = GameState.Board[GameState.GridStartRow + Config.MovableGridSize - 1,
                GameState.GridStartCol + Config.MovableGridSize - 1];
            if (leftUp == '\0' && randomCorner == 0)
            {
                PlacePiece(GameState.GridStartCol, GameState.GridStartCol, botSymbol);
                return;
            }

            if (rightUp == '\0' && randomCorner == 1)
            {
                PlacePiece(GameState.GridStartCol, GameState.GridStartRow + Config.MovableGridSize - 1, botSymbol);
                return;
            }

            if (leftDown == '\0'  && randomCorner == 2)
            {
                PlacePiece(GameState.GridStartRow + Config.MovableGridSize - 1, GameState.GridStartCol, botSymbol);
                return;
            }

            if (rightDown == '\0'  && randomCorner == 3)
            {
                PlacePiece(GameState.GridStartRow + Config.MovableGridSize - 1,
                    GameState.GridStartCol + Config.MovableGridSize - 1, botSymbol);
                return;
            }

            var randomNumber = rnd.Next(0, 3);
            
            // Randomly move grid
            if (randomNumber == 2 && GetMovingPlayerPiecesPlaced() >= Config.InitialMoves)
            {
                RandomlyMoveGrid();
                return;
            }
            
            // Randomly move own piece
            if (randomNumber == 1 && GetMovingPlayerPiecesPlaced() >= Config.InitialMoves)
            {
                RandomlyMoveOwnPiece();
                return;
            }

            // Randomly place a piece
            var freeSpotsInGrid = GetSpaceInGrid();
            PlaceRandomPiece(freeSpotsInGrid);
        }

        private void PlaceRandomPiece(int freeSpotsInGrid)
        {
            var random = new Random();
            var randomSpot = random.Next(0, freeSpotsInGrid);
            var counter = 0;
            for (int i = GameState.GridStartRow; i < GameState.GridStartRow + Config.MovableGridSize; i++)
            {
                for (int j = GameState.GridStartCol; j < GameState.GridStartCol + Config.MovableGridSize; j++)
                {
                    if (GameState.Board[i, j] == '\0')
                    {
                        if (counter == randomSpot)
                        {
                            PlacePiece(i, j, GameState.MovingPlayer);
                            return;
                        }

                        counter++;
                    }
                }
            }
        }

        private bool TryToWin()
        {
            for (int y = GameState.GridStartRow; y < GameState.GridStartRow + Config.MovableGridSize; y++)
            {
                for (int x = GameState.GridStartCol; x < GameState.GridStartCol + Config.MovableGridSize; x++)
                {
                    if (GameState.Board[x, y] == '\0')
                    {
                        GameState.Board[x, y] = GameState.MovingPlayer;
                        bool isWinningMove = CheckWin(GameState.MovingPlayer);
                        GameState.Board[x, y] = '\0';
                        if (isWinningMove)
                        {
                            PlacePiece(x, y, GameState.MovingPlayer);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private void RandomlyMoveGrid()
        {
            List<string> directions = ["u", "d", "l", "r", "ul", "ur", "dl", "dr"];
            var random = new Random();
            var direction = directions[random.Next(0, directions.Count)];
            MoveGrid(direction);
        }

        private void RandomlyMoveOwnPiece()
        {
            int freeSpotsInGrid = GetSpaceInGrid();

            if (freeSpotsInGrid == 0)
            {
                RandomlyMoveGrid();
            }
            else
            {
                var random = new Random();
                var botPieces = new List<(int row, int col)>();

                for (int i = 0; i < GameState.Config.Width; i++)
                {
                    for (int j = 0; j < GameState.Config.Height; j++)
                    {
                        if (GameState.Board[i, j] == GameState.MovingPlayer)
                        {
                            botPieces.Add((i, j));
                        }
                    }
                }

                var pieceToMove = botPieces[random.Next(botPieces.Count)];

                var emptyPlaces = new List<(int row, int col)>();
                for (int i = GameState.GridStartRow; i < GameState.GridStartRow + Config.MovableGridSize; i++)
                {
                    for (int j = GameState.GridStartCol; j < GameState.GridStartCol + Config.MovableGridSize; j++)
                    {
                        if (GameState.Board[i, j] == '\0')
                        {
                            emptyPlaces.Add((i, j));
                        }
                    }
                }

                var newSpot = emptyPlaces[random.Next(emptyPlaces.Count)];
                MovePiece(pieceToMove.row, pieceToMove.col, newSpot.row, newSpot.col);
            }
        }


        private int GetSpaceInGrid()
        {
            var freeSpotsInGrid = 0;
            for (int i = GameState.GridStartRow; i < GameState.GridStartRow + Config.MovableGridSize; i++)
            {
                for (int j = GameState.GridStartCol; j < GameState.GridStartCol + Config.MovableGridSize; j++)
                {
                    if (GameState.Board[i, j] == '\0')
                    {
                        freeSpotsInGrid++;
                    }
                }
            }

            return freeSpotsInGrid + 1;
        }
    }
}