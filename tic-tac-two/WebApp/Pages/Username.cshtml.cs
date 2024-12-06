using System.ComponentModel.DataAnnotations;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class Username : PageModel
{
    [Required(ErrorMessage = "you have to pick something")]
    [StringLength(30, ErrorMessage = "username too long! stay humble - 30 chars max")]
    [BindProperty(SupportsGet = true)]
    public required string UserName { get; set; }

    [BindProperty(SupportsGet = true)] public string? GameMode { get; set; } = default!;

    private readonly AppDbContext _context;

    public Username(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        ModelState.Clear(); 
        return Page();
    }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid) //todo: add error handling everywhere in inputs
        {
            var userExists = _context.Users.Any(u => u.UserName == UserName); 

            if (!userExists)
            {
                _context.Users.Add(new User { UserName = UserName });
                _context.SaveChanges();
            }

            if (GameMode != null)
            {
                return RedirectToPage("./StartGame", new { userName = UserName, gamemode = GameMode });
            }
            else
            {
                return RedirectToPage("./ConfigManager", new { userName = UserName });
            }
        }

        return Page();
    }
}