using System.ComponentModel.DataAnnotations;
using GameLogic;

namespace Domain;

public class Game : BaseEntity
{
    [MaxLength(128)] public string GameName { get; set; } = default!;

    [MaxLength(10240)] public string GameState { get; set; } = default!; 
    
    // think about having attributes here what are in the game, so you can access by attribute
    // like displays game name and click on game name and then deserializes the game and displays board? or without deserializing
    //todo

    public int UserId { get; set; }
    public User User { get; set; } = default!;
    public int ConfigurationId { get; set; }
    public Configuration? Configuration { get; set; }
}