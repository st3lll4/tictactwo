using DAL;
using GameLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class DeleteConfig : PageModel
{
    private readonly IConfigRepository _configRepository;
    
    [BindProperty(SupportsGet = true)] public string UserName { get; set; } = default!;

    public DeleteConfig(IConfigRepository configRepository)
    {
        _configRepository = configRepository;
    }
    
    public Dictionary<string, bool> CanDeleteConfiguration { get; set; }

    public List<string> ConfigurationNames { get; set; } = default!;

    public Dictionary<string, GameConfiguration> ConfigurationDetails { get; set; } = default!;

    public void OnGet()
    {
        ConfigurationNames = _configRepository.GetConfigurationNames();

        ConfigurationDetails = ConfigurationNames
            .ToDictionary(
                name => name, 
                name => _configRepository.GetConfigurationByName(name)
            );

        CanDeleteConfiguration = ConfigurationNames
            .ToDictionary(
                name => name, 
                name => !IsProtectedConfiguration(name)
            );
    }


    private bool IsProtectedConfiguration(string configName)
    {
        var protectedConfigs = new[]
        {
            "Tic-Tac-Two", 
            "Tic-Tac-Toe", 
            "Tic-Tac-Two with a 10x10 board"
        };

        return protectedConfigs.Contains(configName);
    }

    public IActionResult OnPostDelete(string configName)
    {
        if (!IsProtectedConfiguration(configName))
        {
            _configRepository.DeleteConfiguration(configName);
        }

        return RedirectToPage();
    }
}