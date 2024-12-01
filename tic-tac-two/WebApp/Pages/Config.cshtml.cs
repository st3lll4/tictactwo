using DAL;
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

    [BindProperty] public GameConfiguration Configuration { get; set; } = default!;

    public SelectList ConfigSelectList { get; set; } = default!;


    public Config(IConfigRepository configRepository)
    {
        _configRepository = configRepository;
    }

    public IActionResult OnGet()
    {
        ViewData["UserName"] = UserName;

        var selectListData = _configRepository.GetConfigsByUser(UserName)
            .Select(name => new { value = name })
            .ToList();

        ConfigSelectList = new SelectList(selectListData);

        return Page();
    }

    public IActionResult OnPost()
    {
        var config = _configRepository.GetConfigurationByName(Configuration.ConfigName);
        var gameState = new GameState(config);
        return RedirectToPage("./PlayGame", new { username = UserName, gamemode = GameMode, gamestate = gameState });
    }
}