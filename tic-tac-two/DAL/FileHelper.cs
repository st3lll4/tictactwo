namespace DAL;

public static class FileHelper // not in use rn
{
    public static string BasePath = Environment
                                        .GetFolderPath(System.Environment.SpecialFolder.UserProfile)
                                    + Path.DirectorySeparatorChar + "tic-tac-two" + Path.DirectorySeparatorChar;


    public static string ConfigExtension = ".config.json";
    public static string GameExtension = ".game.json";
}