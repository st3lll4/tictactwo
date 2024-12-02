using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class JoinGame : PageModel
{
    [BindProperty(SupportsGet = true)] public required string UserName { get; set; }

    [BindProperty(SupportsGet = true)] public required string GameMode { get; set; }
    
    [BindProperty] public string GameId { get; set; }
    
    public IActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPost(string action)
    {
        if (action == "join")
        {
            return RedirectToPage("./PlayGame", new { username = UserName, gamemode = GameMode, gameid = GameId });
        }

        return Page();
    }
}