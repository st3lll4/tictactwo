using GameLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class StartGame : PageModel
{
    [BindProperty(SupportsGet = true)] public required string UserName { get; set; }

    [BindProperty(SupportsGet = true)] public required string GameMode { get; set; }
    
    
    public IActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPost(string action)
    {
        if (action == "new")
        {
            return RedirectToPage("./Config", new { username = UserName, gamemode = GameMode });
        }
        
        if (action == "saved") // todo: test for bot vs bot
        {
            return RedirectToPage("./SavedGames", new { username = UserName, gamemode = GameMode });
        }

        if (action == "join" || GameMode != "Bots")
        {
            return RedirectToPage("./JoinGame", new { username = UserName, gamemode = GameMode });
        }

        return Page();
    }
}