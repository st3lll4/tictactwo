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
            User1 = user1, // todo: wtf i do w the users
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
        };
    }

    private GameState ConvertToGameState(Game game)
    {
        var configObject = JsonSerializer.Deserialize<GameConfiguration>(game.Config);
        var boardData = JsonSerializer.Deserialize<List<List<char>>>(game.BoardData);
        
        return new GameState(configObject)
        {
            BoardData = boardData,
            MovingPlayer = game.MovingPlayer,
            Config = configObject,
            Player1PiecesPlaced = game.Player1PiecesPlaced,
            Player2PiecesPlaced = game.Player2PiecesPlaced,
            GridStartRow = game.GridStartRow,
            GridStartCol = game.GridStartCol,
            GridCenterRow = game.GridCenterRow,
            GridCenterCol = game.GridCenterCol,
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
    

    public List<string> GetGameNames()
    {
        Console.WriteLine(_context.Games.Select(g => g.GameName).ToList());
        return _context.Games.Select(g => g.GameName).ToList();
    }

    public List<string> GetGamesByUser(string user)
    {
        return _context.Games.Where(
                g => g.User1.UserName == user || 
                     (g.User2 != null && g.User2.UserName == user))
            .Select(g => g.GameName).ToList();    
    }
}