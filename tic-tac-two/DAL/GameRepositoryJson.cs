using System.Text.Json;
using GameLogic;

namespace DAL;

public class GameRepositoryJson : IGameRepository
{
    public void SaveGame(GameState gameState, string gameConfigName, string saveName, string userName,
        string? user2Name)
    {
        string fileName;
        if (saveName != "")
        {
            fileName = FileHelper.BasePath + userName + "_" + user2Name + "_" + saveName + FileHelper.GameExtension;
        }
        else
        {
            fileName = FileHelper.BasePath + userName + "_" + user2Name + "_" +
                       gameConfigName + "_" +
                       saveName +
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

    public List<string> GetGamesByUser(string user)
    {
        var files = Directory.GetFiles(FileHelper.BasePath, "*" + user + "_*" + FileHelper.GameExtension);

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

    public bool CheckIfGameExists(string name)
    {
        var filePath = FileHelper.BasePath + name + FileHelper.GameExtension;
        if (File.Exists(filePath))
        {
            return true;
        }
        return false; 
    }

    public void UpdateGame(GameState gameState, string configName, string gameName, string userName, string? user2Name) //todo: check if works
    {
        var existingFile = FileHelper.BasePath + $"*{gameName}*{FileHelper.GameExtension}";

        File.Delete(existingFile);
        
        var fileName = FileHelper.BasePath + userName + "_" + user2Name + "_" +
                       configName + "_" +
                       gameName +
                       FileHelper.GameExtension;
        var jsonStateString = JsonSerializer.Serialize(gameState, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        File.WriteAllText(fileName, jsonStateString);
    }

    public bool IsGameJoinable(string name) // todo: test
    {
        var filePath = FileHelper.BasePath + $"*__{name}*{FileHelper.GameExtension}";
        return File.Exists(filePath);// returns true if game is joinable
    }

    public void JoinMultiplayerGame(string gameName, string player1Name, string player2Name)
    {
        var game = GetGameByName(gameName);
        
        if (string.IsNullOrEmpty(game.Player2Name))
        {
            game.Player2Name = player2Name;
            UpdateGame(game, game.Config.ConfigName, gameName, player1Name, player2Name);
        }
    }

    public void DeleteGame(string name) //todo: check if works 
    {
        var existingFile = FileHelper.BasePath + $"*{name}*{FileHelper.GameExtension}";

        File.Delete(existingFile);
    }
}