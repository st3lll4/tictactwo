using System.ComponentModel.DataAnnotations;
using DAL;
using Domain;
using GameLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class Username : PageModel
{
    [Required(ErrorMessage = "you have to pick something")]
    [StringLength(30, ErrorMessage = "username too long! stay humble - 30 chars max")]
    [BindProperty]
    public required string UserName { get; set; }

    [BindProperty(SupportsGet = true)] public required string GameMode { get; set; }

    private readonly AppDbContext _context;

    public Username(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            var userExists = _context.Users.Any(u => u.UserName == UserName); // todo: what if i use json

            if (!userExists)
            {
                _context.Users.Add(new User { UserName = UserName, PlayerType = EPlayerType.Player });
                _context.SaveChanges();
            }

            return RedirectToPage("./StartGame", new { username = UserName, gamemode = GameMode });
        }

        return Page();
    }
}