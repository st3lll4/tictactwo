using System.Text.Json;
using System.Text.Json.Serialization;

namespace GameLogic
{
    public class GameState // hoiab koike mis on currently happening in the game, muutuvad asjad 
    {
        [JsonIgnore]
        public char[,] Board { get; set; }

        [JsonPropertyName("Board")]
        public List<List<char>> BoardData
        {
            get => ConvertBoardToList();
            set => Board = ConvertListToBoard(value);
        }
        
        public char MovingPlayer { get; set; }
        public GameConfiguration Config { get; set; }
        public int Player1PiecesPlaced { get; set; }
        public int Player2PiecesPlaced { get; set; }
        public int GridStartRow { get; set; }
        public int GridStartCol { get; set; }

        private int GridCenterRow { get; set; }
        private int GridCenterCol { get; set; }
        private int GridSize { get; set; }
        
        public int WinCondition { get; set; }
        
        public bool IsStandardTicTacToe { get; set; }
        
        private JsonSerializerOptions _options = new()
        {
            WriteIndented = true
        };

        public GameState(GameConfiguration config)
        {
            Config = config;
            Board = new char[config.Height, config.Width]; 

            MovingPlayer = config.StartingPlayer == EStartingPlayer.Player1 ? config.Player1Symbol : config.Player2Symbol;
            
            GridCenterRow = config.Height / 2; // 5/2 = 2.5
            GridCenterCol = config.Width / 2; 
            
            GridSize = config.MovableGridSize; 
            
            GridStartRow = GridCenterRow - (GridSize / 2);
            GridStartCol = GridCenterCol - (GridSize / 2);

            
            IsStandardTicTacToe = GridSize == 3 && 
                                  config.InitialMoves == null && 
                                  config.MaxPieces == null;

            Player1PiecesPlaced = 0;
            Player2PiecesPlaced = 0;

            WinCondition = config.WinningCondition;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, _options);
        }
        
        
        private List<List<char>> ConvertBoardToList()
        {
            var data = new List<List<char>>(Board.GetLength(0));
            for (int y = 0; y < Board.GetLength(0); y++)
            {
                var row = new List<char>(Board.GetLength(1));
                for (int x = 0; x < Board.GetLength(1); x++)
                {
                    row.Add(Board[y, x]);
                }
                data.Add(row);
            }
            return data;
        }

        private char[,] ConvertListToBoard(List<List<char>> listData)
        {
            int rows = listData.Count;
            int cols = listData[0].Count;
            var board = new char[rows, cols];
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    board[y, x] = listData[y][x];
                }
            }
            return board;
        }
    }
}