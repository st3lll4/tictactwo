using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages;

public class JoinGame : PageModel
{
    private readonly IGameRepository _gameRepository;
    [BindProperty(SupportsGet = true)] public required string UserName { get; set; }

    [BindProperty(SupportsGet = true)] public required string GameMode { get; set; }

    [BindProperty] public required string GameName { get; set; }
    [BindProperty(SupportsGet = true)] public string? Error { get; set; }
    public SelectList GameSelectList { get; set; }
    
    public JoinGame(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public IActionResult OnGet()
    {
        var selectListData = _gameRepository.GetGameNames()
            .Where(n =>
            {
                var game = _gameRepository.GetGameByName(n);
                return !game.IsGameOver && _gameRepository.IsGameJoinable(n) && _gameRepository.GetGameByName(n).Player1Name != UserName;
            })
            .Select(name => name)
            .ToList();

        GameSelectList = new SelectList(selectListData);
        return Page();
    }


    public IActionResult OnPost(string action)
    {
        if (_gameRepository.CheckIfGameExists(GameName)
            && _gameRepository.IsGameJoinable(GameName))
        {
            var game = _gameRepository.GetGameByName(GameName);
            _gameRepository.JoinMultiplayerGame(GameName, game.Player1Name, UserName);
            return RedirectToPage("./PlayGame", new
            {
                userName = game.Player1Name,
                user2Name = UserName, 
                gameMode = GameMode,
                gameName = GameName
            });
        }
        Error = "cant join this game";
        var selectListData = _gameRepository.GetGameNames()
            .Where(n =>
            {
                var game = _gameRepository.GetGameByName(n);
                return !game.IsGameOver && _gameRepository.IsGameJoinable(n) && _gameRepository.GetGameByName(n).Player1Name != UserName;
            })
            .Select(name => name)
            .ToList();

        GameSelectList = new SelectList(selectListData);
        return Page();
    }
}