using DAL;
using GameLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Game;

public class Index : PageModel
{
    public GameBrain Brain { get; set; } = default!;
    private static readonly IConfigRepository configrepo = new ConfigRepositoryDb(); // change here between json and db
    
    [BindProperty] 
    public List<String> configs { get; set; } = default!;
    public void OnGet()
    {
        var conf = DefaultConfigurations.DefaultConfiguration ;
        var state = new GameState(conf);
        Brain = new GameBrain(state);
        configs = configrepo.GetConfigurationNames();
    }
}