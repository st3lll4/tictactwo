using System.ComponentModel.DataAnnotations;
using GameLogic;

namespace Domain;

public class Configuration : BaseEntity
{
    [MaxLength(128)]
    public string GameName { get; set; } = default!;
    
    public int Width { get; set; }
    
    public int Height { get; set; }
    
    public char Player1Symbol { get; set; }
    
    public char Player2Symbol { get; set; }
    
    public string StartingPlayer { get; set; } = default!;
        
    public int MovableGridSize { get; set; } 
        
    public int WinningCondition { get; set; }
        
    public int? InitialMoves { get; set; } 
            
    public int MaxPieces { get; set; }

    public ICollection<Game>? Games { get; set; }
}