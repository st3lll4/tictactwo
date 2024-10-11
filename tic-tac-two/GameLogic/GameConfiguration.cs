namespace GameLogic
{
    public class GameConfiguration
    {
        public string GameName { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public char Player1Symbol { get; set; }
        public char Player2Symbol { get; set; }
        public string StartingPlayer { get; set; }
        
        // TODO: 
        // movable grid size
        // winning condition
        // move grid after n moves
        // max pieces for a player
    }
}