using System.Text.Json;
using Domain;
using GameLogic;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class GameRepositoryDb : IGameRepository
{
    private readonly AppDbContext _context = new (_contextOptions);
    private static string _connectionString = $"Data Source={FileHelper.BasePath}app.db";

    private static DbContextOptions<AppDbContext> _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
        .UseSqlite(_connectionString)
        .EnableDetailedErrors()
        .EnableSensitiveDataLogging()
        .Options;

    public void SaveGame(GameState gameState, string gameConfigName, string saveName, string userName)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserName == userName) ?? new User { UserName = userName };
        var config = _context.Configurations.FirstOrDefault(c => c.ConfigName == gameConfigName);

        var game = ConvertToGame(gameState, saveName, user, config); 
        
        _context.Games.Add(game);
        _context.SaveChanges();
    }
    
    private Game ConvertToGame(GameState gameState, string gameName, User user, Configuration? config)
    {
        // Serialize GameConfiguration to JSON string
        var configString = JsonSerializer.Serialize(gameState.Config);
        var boardDataJson = JsonSerializer.Serialize(gameState.BoardData);


        return new Game
        {
            GameName = gameName,
            User = user,
            UserId = user.Id,
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
            GridSize = gameState.GridSize,
            WinCondition = gameState.WinCondition,
            IsStandardTicTacToe = gameState.IsStandardTicTacToe
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
            GridSize = game.GridSize,
            WinCondition = game.WinCondition,
            IsStandardTicTacToe = game.IsStandardTicTacToe
        };
    }


    public GameState GetGameByName(string name)
    {
        return ConvertToGameState(_context.Games.FirstOrDefault(g => g.GameName == name)!);
    }
    

    public List<string> GetGameNames()
    {
        Console.WriteLine(_context.Games.Select(g => g.GameName).ToList());
        return _context.Games.Select(g => g.GameName).ToList();
    }

    public List<string> GetGamesByUser(string user)
    {
        return _context.Games.Where(g => g.User.UserName == user).Select(g => g.GameName).ToList();    }
}