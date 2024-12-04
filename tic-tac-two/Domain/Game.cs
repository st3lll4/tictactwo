using System.ComponentModel.DataAnnotations;
using GameLogic;

namespace Domain;

public class Game : BaseEntity
{
    [MaxLength(128)] public string GameName { get; set; } = default!;

    public string BoardData { get; set; } = default!;
    public char MovingPlayer { get; set; }
    public string Config { get; set; } = default!; 
    public int Player1PiecesPlaced { get; set; }
    public int Player2PiecesPlaced { get; set; }
    public int GridStartRow { get; set; }
    public int GridStartCol { get; set; }
    public int GridCenterRow { get; set; }
    public int GridCenterCol { get; set; }
    public bool IsGameOver { get; set; }

    // [foreignkey]
    public int User1Id { get; set; }
    public User? User1 { get; set; } // navigation property

    public int? User2Id { get; set; }
    public User? User2 { get; set; } // navigation property
    
    public int ConfigurationId { get; set; }
    public Configuration? Configuration { get; set; }
}