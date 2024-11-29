using tic_tac_two;

var user = GetUserName();


var menus = new Menus(user);
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