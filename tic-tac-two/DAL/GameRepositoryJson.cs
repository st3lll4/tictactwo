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
        var searchPattern = $"*{user}*{FileHelper.GameExtension}";
        var matchingFiles = Directory.GetFiles(FileHelper.BasePath, searchPattern);
        
        var result = new List<string>();
        foreach (var file in matchingFiles)
        {
            var primaryName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(file));
            result.Add(primaryName);
        }

        return result;
    }

    public GameState GetGameByName(string name)
    {
        var searchPattern = $"*{name}*{FileHelper.GameExtension}";
        var matchingFiles = Directory.GetFiles(FileHelper.BasePath, searchPattern);

        var filePath = matchingFiles[0];
        var gameJsonStr = File.ReadAllText(filePath);
        var gameState = JsonSerializer.Deserialize<GameState>(gameJsonStr);
        return gameState ?? throw new InvalidOperationException("Failed to deserialize game state");
    }

    public bool CheckIfGameExists(string name)
    {
        var searchPattern = $"*{name}*{FileHelper.GameExtension}";
        var matchingFiles = Directory.GetFiles(FileHelper.BasePath, searchPattern);        
        if (matchingFiles.Length > 0)
        {
            return true;
        }

        return false;
    }

    public void UpdateGame(GameState gameState, string configName, string gameName, string userName, string? user2Name)
    {
        var searchPattern = $"*{gameName}*{FileHelper.GameExtension}";
        var matchingFiles = Directory.GetFiles(FileHelper.BasePath, searchPattern);

        var existingFilePath = matchingFiles[0];

        var jsonStateString = JsonSerializer.Serialize(gameState, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(existingFilePath, jsonStateString);
    }

    public bool IsGameJoinable(string name)
    {
        var searchPattern = $"*{name}*{FileHelper.GameExtension}";
        var matchingFiles = Directory.GetFiles(FileHelper.BasePath, searchPattern);

        foreach (var file in matchingFiles)
        {
            var gameJsonStr = File.ReadAllText(file);
            var gameState = JsonSerializer.Deserialize<GameState>(gameJsonStr);

            if (gameState != null && string.IsNullOrEmpty(gameState.Player2Name) && !gameState.IsGameOver)
            {
                return true;
            }
        }

        return false;
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

    public void DeleteGame(string name)
    {
        var searchPattern = $"*{name}*{FileHelper.GameExtension}";
        var matchingFiles = Directory.GetFiles(FileHelper.BasePath, searchPattern);
        File.Delete(matchingFiles[0]);
    }
}