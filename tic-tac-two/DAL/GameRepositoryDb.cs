using System.Text.Json;
using Domain;
using GameLogic;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class GameRepositoryDb : IGameRepository
{
    private readonly AppDbContext _context = new (_contextOptions);
    private static string _connectionString = $"Data Source={FileHelper.BasePath}app.db"; //todo: change

    private static DbContextOptions<AppDbContext> _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
        .UseSqlite(_connectionString)
        .EnableDetailedErrors()
        .EnableSensitiveDataLogging()
        .Options;

    public void SaveGame(GameState gameState, string gameConfigName, string saveName, string user1Name, string? user2Name)
    {
        var user1 = _context.Users.FirstOrDefault(u => u.UserName == user1Name) ?? new User { UserName = user1Name };
        User? user2 = null;
        if (user2Name != null) {
            user2 = _context.Users.FirstOrDefault(u => u.UserName == user2Name) ??
                        new User { UserName = user2Name };
        }
        
        var config = _context.Configurations.FirstOrDefault(c => c.ConfigName == gameConfigName);

        var game = ConvertToGame(gameState, saveName, user1, config, user2); 
        
        _context.Games.Add(game);
        _context.SaveChanges();
    }
    
    private Game ConvertToGame(GameState gameState, string gameName, User user1, Configuration? config, User? user2)
    {
        var configString = JsonSerializer.Serialize(gameState.Config);
        var boardDataJson = JsonSerializer.Serialize(gameState.BoardData);
        
        return new Game
        {
            GameName = gameName,
            User1 = user1,
            User1Id = user1.Id,
            User2 = user2 ?? null,
            User2Id = user2?.Id,
            Configuration = config,
            ConfigurationId = config?.Id ?? 0,
            BoardData = boardDataJson,
            MovingPlayer = gameState.MovingPlayer,
            Config = configString, 
            Player1PiecesPlaced = gameState.Player1PiecesPlaced,
            Player2PiecesPlaced = gameState.Player2PiecesPlaced,
            GridStartRow = gameState.GridStartRow,
            GridStartCol = gameState.GridStartCol,
            GridCenterRow = gameState.GridCenterRow,
            GridCenterCol = gameState.GridCenterCol,
            IsGameOver = gameState.IsGameOver
        };
    }

    private GameState ConvertToGameState(Game game)
    {
        GameConfiguration? configObject = null;
        List<List<char>>? boardData = null;

        try 
        {
            configObject = JsonSerializer.Deserialize<GameConfiguration>(game.Config);
            boardData = JsonSerializer.Deserialize<List<List<char>>>(game.BoardData);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Failed to deserialize game data", ex);
        }
        
        User? user1 = _context.Users.FirstOrDefault(u => u.Id == game.User1Id);
        User? user2 = game.User2Id == null? null : _context.Users.FirstOrDefault(u => u.Id == game.User2Id);
        
        return new GameState(configObject!)
        {
            BoardData = boardData!,
            MovingPlayer = game.MovingPlayer,
            Config = configObject!,
            Player1PiecesPlaced = game.Player1PiecesPlaced,
            Player2PiecesPlaced = game.Player2PiecesPlaced,
            GridStartRow = game.GridStartRow,
            GridStartCol = game.GridStartCol,
            GridCenterRow = game.GridCenterRow,
            GridCenterCol = game.GridCenterCol,
            IsGameOver = game.IsGameOver,
            Player1Name = user1?.UserName!,
            Player2Name = user2?.UserName!
        };
    }


    public GameState GetGameByName(string name)
    {
        return ConvertToGameState(_context.Games.FirstOrDefault(g => g.GameName == name)!);
    }

    public bool CheckIfGameExists(string name)
    {
        return _context.Games.Any(g => g.GameName == name);
    }

    public void UpdateGame(GameState gameState, string configName, string gameName, string userName, string? user2Name)
    {
        var game = _context.Games
            .Include(g => g.User1)
            .Include(g => g.User2)
            .FirstOrDefault(g => g.GameName == gameName);

        game!.BoardData = JsonSerializer.Serialize(gameState.BoardData);
        game.MovingPlayer = gameState.MovingPlayer;
        game.Config = JsonSerializer.Serialize(gameState.Config);
        game.Player1PiecesPlaced = gameState.Player1PiecesPlaced;
        game.Player2PiecesPlaced = gameState.Player2PiecesPlaced;
        game.GridStartRow = gameState.GridStartRow;
        game.GridStartCol = gameState.GridStartCol;
        game.GridCenterRow = gameState.GridCenterRow;
        game.GridCenterCol = gameState.GridCenterCol;
        game.IsGameOver = gameState.IsGameOver;
        

        if (game.User1!.UserName != userName)
        {
            var user1 = _context.Users.FirstOrDefault(u => u.UserName == userName) ?? new User { UserName = userName };
            game.User1 = user1;
            game.User1Id = user1.Id;
        }

        if (user2Name != null && (game.User2 == null || game.User2.UserName != user2Name))
        {
            var user2 = _context.Users.FirstOrDefault(u => u.UserName == user2Name) ?? new User { UserName = user2Name };
            game.User2 = user2;
            game.User2Id = user2.Id;
        }
        else if (user2Name == null && game.User2 != null)
        {
            game.User2 = null;
            game.User2Id = null;
        }

        _context.Games.Update(game);
        _context.SaveChanges();
    }

    public bool IsGameJoinable(string name)
    {
        var game = _context.Games.FirstOrDefault(g => g.GameName == name);
        return game!.User2Id == null; // returns false if game is not joinable
    }

    public void JoinMultiplayerGame(string gameName, string player1Name ,string player2Name)
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
        _context.Remove(_context.Games.Where(g => g.GameName == name));
        _context.SaveChanges();
    }

    public List<string> GetGameNames()
    {
        Console.WriteLine(_context.Games.Select(g => g.GameName).ToList());
        return _context.Games.Select(g => g.GameName).ToList();
    }

    public List<string> GetGamesByUser(string user)
    {
        return _context.Games.Where(
                g => g.User1 != null && (g.User1.UserName == user || 
                                         (g.User2 != null && g.User2.UserName == user)))
            .Select(g => g.GameName).ToList();    
    }
}