using System.ComponentModel.DataAnnotations;

namespace Domain;

public class User : BaseEntity
{
    [MaxLength(128)] 
    public string UserName { get; set; } = default!;
    
    public ICollection<Game>? Games { get; set; }
}