using DAL;
using Microsoft.EntityFrameworkCore;
using tic_tac_two;

var user = GetUserName();

var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

var connectionString = "Data Source=<%location%>app.db";
connectionString = connectionString.Replace("<%location%>", FileHelper.BasePath);
optionsBuilder.UseSqlite(connectionString);

using var db = new AppDbContext(optionsBuilder.Options);

var configRepository = new ConfigRepositoryDb(db);
var gameRepository = new GameRepositoryDb(db);
//var gameRepository = new GameRepositoryJson();
//var configRepository = new ConfigRepositoryJson();

var menus = new Menus(user, gameRepository, configRepository);
menus.MainMenu.Run();

static string GetUserName()
{
    var userName = "";
    while (string.IsNullOrWhiteSpace(userName))
    {
        Console.WriteLine("Hello. Who are you??");
        userName = Console.ReadLine();
    }

    return userName;
}