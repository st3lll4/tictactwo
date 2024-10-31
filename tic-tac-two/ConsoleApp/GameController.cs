using GameLogic;
using tic_tac_two;

namespace tic_tac_two
{
    public static class GameController
    {
        private static readonly GameConfiguration CurrentConfig = ConfigurationManager.CurrentConfiguration;
        private static readonly GameBrain Brain = new(new GameState(CurrentConfig));

        public static string MainLoop()
        {
            var gameState = Brain.GameState; 
    
            do {
                DrawBoard(gameState);
                Console.WriteLine($"Player {gameState.MovingPlayer}, it's your turn!");

                Console.Write("Enter row and column (x,y): ");
                var input = Console.ReadLine()!.Split(",");
                var x = int.Parse(input[0]) - 1;
                var y = int.Parse(input[1]) - 1;
                

                if (!Brain.PlacePiece(x, y, gameState.MovingPlayer))
                {
                    Console.WriteLine("Invalid move! Place your piece on a free spot inside the grid.");
                    continue;
                }

                if (Brain.CheckWin())
                {
                    Console.WriteLine($"Player {gameState.MovingPlayer} wins!");
                    break;
                }

                if (Brain.CheckTie())
                {
                    Console.WriteLine("a tie!");
                    break;
                }
            } while (true);

            return "";
        }
        
        

        private static void DrawBoard(GameState gameState)
        {
            Console.Write("  ");
            for (int i = 0; i < gameState.Config.Width; i++)
            {
                Console.Write(i < 10 ? $"  {i + 1} " : $" {i + 1} ");
            }

            Console.WriteLine();
            
            for (var i = 0; i < gameState.Config.Height; i++)
            {
                Console.Write(i < 9 ? $" {i + 1} " : $"{i + 1} ");
                
                for (var j = 0; j < gameState.Config.Width; j++)
                {
                    Console.BackgroundColor = Brain.IsInGrid(i,j) ? ConsoleColor.Black : ConsoleColor.Gray; 
                    
                    var toWrite = gameState.Board[i, j] == '\0' ? " " : $"{gameState.Board[i, j]}";
                    Console.Write($" {toWrite} ");
                    Console.ResetColor();
                    Console.Write(" ");
                }

                Console.WriteLine();
            }
        }
    }
}