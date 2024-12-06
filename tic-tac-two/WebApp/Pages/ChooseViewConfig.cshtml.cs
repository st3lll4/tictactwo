using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages;

public class ChooseViewConfig : PageModel
{
    [BindProperty(SupportsGet = true)] public string UserName { get; set; } = default!;

    private readonly IConfigRepository _configRepository;

    [BindProperty] public string ConfigurationName { get; set; } = default!;

    public SelectList ConfigSelectList { get; set; } = default!;

    public ChooseViewConfig(IConfigRepository configRepository)
    {
        _configRepository = configRepository;
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
        return RedirectToPage("./ViewConfig", new { userName = UserName, configurationName = ConfigurationName });
    }
}