using System.Text.Json;
using GameLogic;

namespace DAL;

public class GameRepositoryJson : IGameRepository
{
    public void SaveGame(GameState gameState, string gameConfigName, string? saveName)
    {
        string fileName;
        if (saveName != "")
        {
            fileName = FileHelper.BasePath + saveName + FileHelper.GameExtension;
        }
        else
        {
            fileName = FileHelper.BasePath +
                           gameConfigName + "_" +
                           DateTime.Now.ToString("dd-MMMM-yyyy_HH:mm:ss") +
                           FileHelper.GameExtension;
        }

        var jsonStateString = JsonSerializer.Serialize(gameState, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        File.WriteAllText(fileName, jsonStateString);
    }

    public List<string> GetGameNames()
    {
        var files = Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.GameExtension);
        var result = new List<string>();
        foreach (var file in files)
        {
            var primaryName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(file));
            result.Add(primaryName);
        }

        return result;
    }

    public GameState GetGameByName(string name)
    {
        var filePath = FileHelper.BasePath + name + FileHelper.GameExtension;
        var gameJsonStr = File.ReadAllText(filePath);
        var gameState = JsonSerializer.Deserialize<GameState>(gameJsonStr);
        return gameState ?? throw new InvalidOperationException();
    }
}