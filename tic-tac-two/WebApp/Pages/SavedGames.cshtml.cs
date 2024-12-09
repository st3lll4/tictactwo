using System.Text.Json;
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

    public SelectList GameSelectList { get; set; } = default!;

    [BindProperty] public string SelectedGame { get; set; }


    public SavedGames(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public IActionResult OnGet()
    {
        var selectListData = _gameRepository.GetGamesByUser(UserName)
            .Where(game => _gameRepository.GetGameByName(game).IsGameOver == false)
            .Select(name => name)
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
        return RedirectToPage("./PlayGame", new { username = UserName, gamemode = GameMode, gameName = SelectedGame });
    }
    
    public IActionResult OnPostBack()
    {
        return RedirectToPage("./Username", new { gamemode = GameMode });
    }
}