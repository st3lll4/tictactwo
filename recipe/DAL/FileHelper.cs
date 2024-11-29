namespace DAL;

public static class FileHelper 
{
    public static string BasePath = Environment
                                        .GetFolderPath(System.Environment.SpecialFolder.UserProfile)
                                    + Path.DirectorySeparatorChar + "recipeTest" + Path.DirectorySeparatorChar;
    
}