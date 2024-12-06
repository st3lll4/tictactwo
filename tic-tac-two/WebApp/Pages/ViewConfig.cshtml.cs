using System.ComponentModel.DataAnnotations;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class ViewConfig : PageModel
{
    private IConfigRepository _configRepository;
    private readonly AppDbContext _context;

    public ViewConfig(IConfigRepository configRepository, AppDbContext context)
    {
        _configRepository = configRepository;
        _context = context;
    }
    
    [BindProperty] public string UserName { get; set; } = default!;
    
    [BindProperty(SupportsGet = true)] public string ConfigurationName { get; set; } = default!;
    
    [BindProperty] public int Width { get; set; }
    
    [BindProperty] public int Height { get; set; }
    
    [BindProperty] public char Player1Symbol { get; set; }
    
    [BindProperty] public char Player2Symbol { get; set; }

    [BindProperty] public string StartingPlayer { get; set; } = default!;
    
    [BindProperty] public int MovableGridSize { get; set; } 
    [BindProperty] public int WinningCondition { get; set; }

    [BindProperty] public int? InitialMoves { get; set; }

    [BindProperty] public int MaxPieces { get; set; } 
    
    
    public IActionResult OnGet()
    {
        var config = _configRepository.GetConfigurationByName(ConfigurationName);

        ConfigurationName = config.ConfigName;
        Width = config.Width;
        Height = config.Height;
        Player1Symbol = config.Player1Symbol;
        Player2Symbol = config.Player2Symbol;
        StartingPlayer = config.StartingPlayer;
        MovableGridSize = config.MovableGridSize;
        WinningCondition = config.WinningCondition;
        InitialMoves = config.InitialMoves;
        MaxPieces = config.MaxPieces;
        return Page();
    }
}