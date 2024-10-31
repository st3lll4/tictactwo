using GameLogic;
using tic_tac_two;

namespace ConsoleApp
{
    public static class GameController
    {
        private static readonly GameConfiguration CurrentConfig = ConfigurationManager.CurrentConfiguration;
        private static GameBrain brain { get; set; } // todo: kas nii reference?
        

        public static string MainLoop()
        {
            var gameState = new GameState(CurrentConfig);
            var gameBrain = new GameBrain(gameState);
            
            do
            {
                DrawBoard(gameState);
                Console.WriteLine($"Player {gameState.NextMoveBy}, it's your turn!");

                Console.Write("Enter row and column (x,y): ");
                var input = Console.ReadLine()!.Split(",");
                var x = int.Parse(input[0]) - 1;
                var y = int.Parse(input[1]) - 1;
                
                var playerWhoseTurn = gameState.NextMoveBy == CurrentConfig.Player1Symbol ? 
                    CurrentConfig.Player1Symbol : CurrentConfig.Player2Symbol;

                if (!gameBrain.PlacePiece(x, y, playerWhoseTurn))
                {
                    Console.WriteLine("Invalid move! Try again.");
                    continue;
                }

                if (gameBrain.CheckWin())
                {
                    Console.WriteLine($"Player {gameState.NextMoveBy} wins!");
                    break;
                }

                if (gameBrain.CheckTie())
                {
                    Console.WriteLine("a tie!");
                    break;
                }
            } while (true);

            return "";
        }
        
        

        private static void DrawBoard(GameState gameState)
        {
            Console.Clear();
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
                    Console.BackgroundColor = brain.IsInGrid(i,j) ? ConsoleColor.Green : ConsoleColor.DarkGray; // et saaks siin kasutada
                    Console.Write($" {gameState.Board[i, j]} ");
                    Console.ResetColor();
                    Console.Write(" ");
                }

                Console.WriteLine();
            }
        }
    }
}