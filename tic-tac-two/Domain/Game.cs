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
    public int GridSize { get; set; } // todo: miks mul pole see confist voetav????:
    public int WinCondition { get; set; } //todo: miks mul pole see confist voetav????
    public bool IsStandardTicTacToe { get; set; } //todo: miks mul pole see confist voetav????
    

    public int UserId { get; set; }
    public User User { get; set; } = default!;
    public int ConfigurationId { get; set; }
    public Configuration? Configuration { get; set; }
}