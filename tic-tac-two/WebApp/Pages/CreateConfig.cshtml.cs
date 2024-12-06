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
    private readonly AppDbContext _context;

    public CreateConfig(IConfigRepository configRepository, AppDbContext context)
    {
        _configRepository = configRepository;
        _context = context;
    }
    
    [BindProperty(SupportsGet = true)]
    public string UserName { get; set; } = default!;

    [Required(ErrorMessage = "you have to pick something!")]
    [BindProperty(SupportsGet = true)]
    public string ConfigName { get; set; } = default!;

    [Required(ErrorMessage = "fill all fields!")]
    [Range(3, 15, ErrorMessage = "width must be between {0} and {1}!")]
    [BindProperty(SupportsGet = true)]
    public int Width { get; set; }

    [Required(ErrorMessage = "fill all fields!")]    
    [Range(3, 15, ErrorMessage = "height must be between {0} and {1}!")]
    [BindProperty(SupportsGet = true)]

    public int Height { get; set; }

    [Required(ErrorMessage = "fill all fields!")]
    [StringLength(1, ErrorMessage = "enter 1 character!")]
    [BindProperty(SupportsGet = true)]
    public char Player1Symbol { get; set; }

    [Required(ErrorMessage = "fill all fields!")]
    [StringLength(1, ErrorMessage = "enter 1 character!")]
    [BindProperty(SupportsGet = true)]
    public char Player2Symbol { get; set; }

    [Required(ErrorMessage = "fill all fields!")]
    [Range(1, 2, ErrorMessage = "starting player must be either 1 or 2!")]
    [BindProperty(SupportsGet = true)]
    public string StartingPlayer { get; set; }
    
    [Required(ErrorMessage = "fill all fields!")]
    [Range(3, 15, ErrorMessage = "movable grid size must be at least 3")]
    [BindProperty(SupportsGet = true)]
    public int MovableGridSize { get; set; }

    [Required(ErrorMessage = "fill all fields!")]
    [Range(3, 15, ErrorMessage = "winning condition must be at least 3")]
    [BindProperty(SupportsGet = true)]
    public int WinningCondition { get; set; }

    [Required(ErrorMessage = "fill all fields!")]
    [Range(0, 15, ErrorMessage = "initial moves must be between 0 and 15")]
    [BindProperty(SupportsGet = true)]
    public int InitialMoves { get; set; }

    [Required(ErrorMessage = "fill all fields!")]
    [Range(3, 100, ErrorMessage = "max pieces have to be between 3 and 100")]
    [BindProperty(SupportsGet = true)]
    public int MaxPieces { get; set; }
    [BindProperty(SupportsGet = true)] public string? Message { get; set; }
    
    public IActionResult OnGet()
    {
        ModelState.Clear();
        return Page();
    }

    public IActionResult OnPost()
    {
        if (MovableGridSize >= Width || MovableGridSize >= Height)
        {
            ModelState.AddModelError("MovableGridSize",
                "movable grid size has to be smaller than width and height!");
        }

        else if (WinningCondition > MovableGridSize)
        {
            ModelState.AddModelError("WinningCondition",
                "winning condition has to be less than or equal to movable grid size!");
        }

        else if (MaxPieces < WinningCondition)
        {
            ModelState.AddModelError("MaxPieces",
                "the amount of pieces each player has must be greater than or equal to the winning condition!");
        }
        
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

