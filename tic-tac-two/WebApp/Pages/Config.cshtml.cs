using System.Text.Json;
using DAL;
using Domain;
using GameLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages;

public class Config : PageModel
{
    [BindProperty(SupportsGet = true)] public string UserName { get; set; } = default!;

    [BindProperty(SupportsGet = true)] public string GameMode { get; set; } = default!;

    private readonly IConfigRepository _configRepository;
    private readonly IGameRepository _gameRepository;

    [BindProperty] public string ConfigurationName { get; set; } = default!;

    [BindProperty] public string SaveName { get; set; } = default!;

    public SelectList ConfigSelectList { get; set; } = default!;

    public Config(IConfigRepository configRepository, IGameRepository gameRepository)
    {
        _configRepository = configRepository;
        _gameRepository = gameRepository;
    }

    public IActionResult OnGet()
    {
        var selectListData = _configRepository.GetConfigsByUser(UserName)
            .Select(name => name)
            .ToList();

        ConfigSelectList = new SelectList(selectListData);

        return Page();
    }

    public IActionResult OnPost()
    {
        var config = _configRepository.GetConfigurationByName(ConfigurationName);
        var gameState = new GameState(config);
        
        if (string.IsNullOrWhiteSpace(SaveName))
        {
            SaveName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        }
        
        if (_gameRepository.CheckIfGameExists(SaveName))
        {
            ModelState.AddModelError("SaveName", "Game with this name already exists");
            var selectListData = _configRepository.GetConfigsByUser(UserName)
                .Select(name => name)
                .ToList();

            ConfigSelectList = new SelectList(selectListData);
            return Page();
        }

        _gameRepository.SaveGame(gameState, config.ConfigName, SaveName, UserName, null);

        return RedirectToPage("./PlayGame", new { userName = UserName, gameMode = GameMode, gameName = SaveName });
    }
}