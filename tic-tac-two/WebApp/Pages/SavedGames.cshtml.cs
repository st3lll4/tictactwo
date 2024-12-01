using DAL;
using Domain;
using GameLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages;

public class SavedGames : PageModel
{
    [BindProperty(SupportsGet = true)] public string UserName { get; set; } = default!;

    [BindProperty(SupportsGet = true)] public string GameMode { get; set; } = default!;

    [BindProperty] public string Error { get; set; } = default!;

    private readonly IGameRepository _gameRepository;
    private readonly IConfigRepository _configRepository;

    public SelectList GameSelectList { get; set; } = default!;

    [BindProperty] public Game SelectedGame { get; set; }


    public SavedGames(IGameRepository gameRepository, IConfigRepository configRepository)
    {
        _gameRepository = gameRepository;
        _configRepository = configRepository;
    }

    public IActionResult OnGet()
    {
        ViewData["UserName"] = UserName;

        var selectListData = _gameRepository.GetGamesByUser(UserName)
            .Select(name => new { value = name })
            .ToList();

        if (selectListData.Count == 0)
        {
            Error = "No games to show!";
        }

        GameSelectList = new SelectList(selectListData);

        return Page();
    }

    public IActionResult OnPost()
    {
        var config = _configRepository.GetConfigurationByName(SelectedGame.Config);
        var gameState = new GameState(config);
        return RedirectToPage("./PlayGame", new { username = UserName, gamemode = GameMode, gamestate = gameState });
    }
    
    public IActionResult OnPostBack()
    {
        return RedirectToPage("./Username", new { gamemode = GameMode });
    }
}