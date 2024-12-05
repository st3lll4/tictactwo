using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    [Required(ErrorMessage = "Select a gamemode!")]
    [BindProperty]
    public string GameMode { get; set; } = default!;


    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            if (GameMode != "Bots")
            {
                return RedirectToPage("./Username", new { gamemode = GameMode });
            }
            return RedirectToPage("./StartGame", new { username = "Bot1", gamemode = GameMode });
        }
        
        return Page();
    }
}