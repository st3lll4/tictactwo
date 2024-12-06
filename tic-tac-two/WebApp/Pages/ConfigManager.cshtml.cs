using GameLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class ConfigManager : PageModel
{
    [BindProperty(SupportsGet = true)] public string UserName { get; set; } = "";

    
    public IActionResult OnGet()
    {
        if (string.IsNullOrEmpty(UserName.Trim()))
        {
            return RedirectToPage("./UserName");
        }
        return Page();
    }

    public IActionResult OnPost(string action)
    {
        if (action == "new")
        {
            return RedirectToPage("./CreateConfig", new { username = UserName });
        }
        
        if (action == "view") 
        {
            return RedirectToPage("./ViewConfig", new { username = UserName });
        }

        if (action == "delete")
        {
            return RedirectToPage("./DeleteConfig", new { username = UserName });
        }

        return Page();
    }
}