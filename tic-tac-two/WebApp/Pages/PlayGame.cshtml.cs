using System.Text.Json;
using DAL;
using GameLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class PlayGame : PageModel
{
    private readonly IGameRepository _gameRepository;
    
    [BindProperty(SupportsGet = true)] public string GameName { get; set; }

    [BindProperty] public GameState GameState { get; set; } = default!;
    
    [BindProperty(SupportsGet = true)] public string UserName { get; set; } = default!;
    
    [BindProperty(SupportsGet = true)] public string GameMode { get; set; } = default!;

    [BindProperty] public GameBrain Brain { get; set; } = default!;

    public char[,] Board { get; set; } = default!;
    
    private readonly AppDbContext _context;
    
    public PlayGame(IGameRepository gameRepository, AppDbContext context)
    {
        _gameRepository = gameRepository;
        _context = context;
    }
    
    public IActionResult OnGet()
    {
        var game = _context.Games.FirstOrDefault(g => g.GameName == GameName);
        
        var boardData = JsonSerializer.Deserialize<List<List<char>>>(game!.BoardData); 
        
        GameState = new GameState
        {
            BoardData = boardData!
        };

        Board = GameState.Board;
        
        return Page();
    }
}