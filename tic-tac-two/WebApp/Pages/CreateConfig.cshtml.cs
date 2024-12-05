using System.ComponentModel.DataAnnotations;
using DAL;
using Domain;
using GameLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class CreateConfig : PageModel

{
    private IConfigRepository _configRepository;
    private AppDbContext _context;

    public CreateConfig(IConfigRepository configRepository, AppDbContext context)
    {
        _configRepository = configRepository;
        _context = context;
    }

    [Required(ErrorMessage = "you have to pick something!")]
    [StringLength(30, ErrorMessage = "username too long! stay humble - 30 chars max")]
    public string UserName { get; set; } = default!;

    [Required(ErrorMessage = "you have to pick something!")]
    public string ConfigName { get; set; } = default!;

    [Required(ErrorMessage = "fill all fields!")]
    [Range(3, 15, ErrorMessage = "width must be between {0} and {1}!")]
    public int Width { get; set; }

    [Required(ErrorMessage = "fill all fields!")]
    [Range(3, 15, ErrorMessage = "height must be between {0} and {1}!")]
    public int Height { get; set; }

    [Required(ErrorMessage = "fill all fields!")]
    [StringLength(1, ErrorMessage = "enter 1 character!")]
    public char Player1Symbol { get; set; }

    [Required(ErrorMessage = "fill all fields!")]
    [StringLength(1, ErrorMessage = "enter 1 character!")]
    public char Player2Symbol { get; set; }

    [Required(ErrorMessage = "fill all fields!")]
    [Range(1, 2, ErrorMessage = "starting player must be either 1 or 2.")]
    public string StartingPlayer { get; set; }
    
    [Required(ErrorMessage = "fill all fields!")]
    [Range(3, 15, ErrorMessage = "movable grid size must be at least 3")]
    public int MovableGridSize { get; set; }

    [Required(ErrorMessage = "fill all fields!")]
    [Range(3, 15, ErrorMessage = "winning condition must be at least 3")]
    public int WinningCondition { get; set; }

    [Required(ErrorMessage = "fill all fields!")]
    [Range(0, 15, ErrorMessage = "initial moves must be between 0 and 15")]
    public int InitialMoves { get; set; }

    [Required(ErrorMessage = "fill all fields!")]
    [Range(3, 100, ErrorMessage = "max pieces have to be between 3 and 100")]
    public int MaxPieces { get; set; }
    [BindProperty(SupportsGet = true)] public string? Message { get; set; }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            var config = new GameConfiguration
            {
                ConfigName = ConfigName,
                Width = Width,
                Height = Height,
                Player1Symbol = Player1Symbol,
                Player2Symbol = Player2Symbol,
                StartingPlayer = StartingPlayer,
                MovableGridSize = MovableGridSize,
                WinningCondition = WinningCondition,
                InitialMoves = InitialMoves,
                MaxPieces = MaxPieces
            };

            var userExists = _context.Users.Any(u => u.UserName == UserName);

            if (!userExists)
            {
                _context.Users.Add(new User { UserName = UserName });
                _context.SaveChanges();
            }

            _configRepository.SaveConfiguration(config, UserName);
            Message = "Config saved successfully!";
        }
        else
        {
            Message = "something went terribly, terribly wrong!";
        }

        return Page();
    }
}

