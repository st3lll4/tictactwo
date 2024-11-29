using System.ComponentModel.DataAnnotations;
using GameLogic;

namespace Domain;

public class User : BaseEntity
{
    [MaxLength(128)] 
    public string UserName { get; set; } = default!;
    
    //public EPlayerType PlayerType { get; set; }
    
    public ICollection<Game>? Games { get; set; }
    public ICollection<Configuration>? Configurations { get; set; }
}