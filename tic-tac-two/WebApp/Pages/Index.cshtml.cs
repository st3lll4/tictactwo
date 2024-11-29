using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    [BindProperty] public string UserName { get; set; }

    [BindProperty(SupportsGet = true)] public string Error { get; set; }

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        if (!String.IsNullOrWhiteSpace(UserName))
        {
            return RedirectToPage("./Home", new { userName = UserName });
        }

        Error = "please enter an username or something!";
        
        return Page();
    }
}