using System.ComponentModel.DataAnnotations;
using DAL;
using Domain;
using GameLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApp.Pages;

public class CreateConfig : PageModel

{
    private IConfigRepository _configRepository;

    public CreateConfig(IConfigRepository configRepository)
    {
        _configRepository = configRepository;
    }

    [BindProperty(SupportsGet = true)] public string UserName { get; set; } = default!;

    [Required(ErrorMessage = "you have to pick something!")]
    [BindProperty(SupportsGet = true)]
    public string ConfigName { get; set; } = default!;

    [Required(ErrorMessage = "fill all fields!")]
    [Range(4, 14, ErrorMessage = "width must be between {0} and {1}!")]
    [BindProperty]
    public int Width { get; set; }

    [Required(ErrorMessage = "fill all fields!")]
    [Range(4, 100, ErrorMessage = "height must be between {1} and {2}!")] 
    [BindProperty]

    public int Height { get; set; }

    [Required(ErrorMessage = "fill all fields!")]
    [BindProperty]
    public char Player1Symbol { get; set; }

    [Required(ErrorMessage = "fill all fields!")]
    [BindProperty]
    public char Player2Symbol { get; set; }

    [Required(ErrorMessage = "fill all fields!")]
    [Range(1, 2, ErrorMessage = "starting player must be either 1 or 2!")]
    [BindProperty]
    public string StartingPlayer { get; set; }

    [Required(ErrorMessage = "fill all fields!")]
    [Range(3, 14, ErrorMessage = "movable grid size must be at least 3")]
    [BindProperty]
    public int MovableGridSize { get; set; }

    [Required(ErrorMessage = "fill all fields!")]
    [Range(3, 14, ErrorMessage = "winning condition must be at least 3")]
    [BindProperty]
    public int WinningCondition { get; set; }

    [Required(ErrorMessage = "fill all fields!")]
    [Range(0, 14, ErrorMessage = "initial moves must be between 0 and 15")]
    [BindProperty]
    public int InitialMoves { get; set; }

    [Required(ErrorMessage = "fill all fields!")]
    [Range(3, 100, ErrorMessage = "max pieces have to be between 3 and 100")]
    [BindProperty]
    public int MaxPieces { get; set; }

    [BindProperty] public string? Message { get; set; }
    
    [BindProperty(SupportsGet = true)] public bool ShowSweetAlert { get; set; }

    [BindProperty(SupportsGet = true)] public string? SweetAlertMessage { get; set; }

    [BindProperty] public bool ConfigCreated { get; set; } = false;

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

        if (Player1Symbol.ToString().Length != 1 || Player2Symbol.ToString().Length != 1)
        {
            ModelState.AddModelError("Player1Symbol",
                "exactly one character please!");
        }

        if (char.ToLower(Player1Symbol) == char.ToLower(Player2Symbol))
        {
            ModelState.AddModelError("Player2Symbol",
                "Player 1 and Player 2 symbols have to be different!");
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
                StartingPlayer = $"Player {StartingPlayer}",
                MovableGridSize = MovableGridSize,
                WinningCondition = WinningCondition,
                InitialMoves = InitialMoves,
                MaxPieces = MaxPieces
            };

            _configRepository.SaveConfiguration(config, UserName);
            Message = "Config saved successfully! Details of your config are:";
            ShowSweetAlert = true;
            ConfigCreated = true;
            return RedirectToPage("/CreateConfig", new
            {
                userName = UserName, 
                message = Message,
                showSweetAlert = ShowSweetAlert,
                configCreated = ConfigCreated
            } );
        }
        Message = "something went terribly, terribly wrong!";
        return Page();
    }
}