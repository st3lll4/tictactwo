using System.ComponentModel.Design;
using Domain;
using GameLogic;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class ConfigRepositoryDb : IConfigRepository
{
    private readonly AppDbContext _context = new AppDbContext(_contextOptions);
    private readonly string _userName;
    private static string _connectionString = $"Data Source={FileHelper.BasePath}app.db";

    private static DbContextOptions<AppDbContext> _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
        .UseSqlite(_connectionString)
        .EnableDetailedErrors()
        .EnableSensitiveDataLogging()
        .Options; // todo: this is WRONG, have to open and close connection ?

    public GameConfiguration DefaultConfiguration { get; set; }
    public GameConfiguration DefaultConfiguration2 { get; set; }
    public GameConfiguration DefaultConfiguration3 { get; set; }
    

    public ConfigRepositoryDb(string username)
    {
        DefaultConfiguration = DefaultConfigurations.DefaultConfiguration;
        DefaultConfiguration2 = DefaultConfigurations.DefaultConfiguration2;
        DefaultConfiguration3 = DefaultConfigurations.DefaultConfiguration3;
        _userName = username;
       
        CheckAndCreateInitialDirectory();
        
    }

    private void InitializeDefaultConfigurations()
    {
        SaveConfiguration(DefaultConfigurations.DefaultConfiguration);
        SaveConfiguration(DefaultConfigurations.DefaultConfiguration2);
        SaveConfiguration(DefaultConfigurations.DefaultConfiguration3);
    }

    private void CheckAndCreateInitialDirectory()
    {
        if (!Directory.Exists(FileHelper.BasePath))
        {
            Directory.CreateDirectory(FileHelper.BasePath);
        }

        var data = _context.Configurations.ToList();
        if (data.Count == 0)
        {
            InitializeDefaultConfigurations();
        }
    }

    public List<string> GetConfigurationNames()
    {
        return _context.Configurations.Select(c => c.ConfigName).ToList();
    }

    public List<GameConfiguration> GetAllConfigurations()
    {
        return _context.Configurations.ToList().Select(ConvertToGameConfiguration).ToList();
    }

    public GameConfiguration GetConfigurationByName(string name)
    {
        return ConvertToGameConfiguration(_context.Configurations.FirstOrDefault(c => c.ConfigName == name)!);
    }

    public void SaveConfiguration(GameConfiguration config)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserName == _userName) ?? new User { UserName = _userName };
        
        var domainConfig = ConvertToDomainConfiguration(config, user);
        Console.WriteLine($"UserId for {_userName}: {user.Id}");
        _context.Configurations.Add(domainConfig);
        _context.SaveChanges();
    }

    private Configuration ConvertToDomainConfiguration(GameConfiguration gameConfig, User user)
    {
        return new Configuration
        {
            ConfigName = gameConfig.ConfigName,
            Width = gameConfig.Width,
            Height = gameConfig.Height,
            Player1Symbol = gameConfig.Player1Symbol,
            Player2Symbol = gameConfig.Player2Symbol,
            StartingPlayer = gameConfig.StartingPlayer,
            MovableGridSize = gameConfig.MovableGridSize,
            WinningCondition = gameConfig.WinningCondition,
            InitialMoves = gameConfig.InitialMoves,
            MaxPieces = gameConfig.MaxPieces,
            UserId = user.Id,
            User = user
        };
    }
    
    private GameConfiguration ConvertToGameConfiguration(Configuration config)
    {
        return new GameConfiguration
        {
            ConfigName = config.ConfigName,
            Width = config.Width,
            Height = config.Height,
            Player1Symbol = config.Player1Symbol,
            Player2Symbol = config.Player2Symbol,
            StartingPlayer = config.StartingPlayer,
            MovableGridSize = config.MovableGridSize,
            WinningCondition = config.WinningCondition,
            InitialMoves = config.InitialMoves,
            MaxPieces = config.MaxPieces
        };
    }
    
    
    public bool DeleteConfiguration(string name)
    {
        var config = _context.Configurations.FirstOrDefault(c => c.ConfigName == name);
        if (config == null) return false;

        _context.Configurations.Remove(config);
        _context.SaveChanges();
        return true;
    }

    public List<string> GetConfigsByUser()
    {
        var result = _context.Configurations.Where(c => c.User.UserName == _userName).Select(c => c.ConfigName)
            .ToList();
        if (result.Contains(DefaultConfiguration.ConfigName) || result.Contains(DefaultConfiguration2.ConfigName) ||
            result.Contains(DefaultConfiguration3.ConfigName)) return result;
        result.Add(DefaultConfiguration.ConfigName);
        result.Add(DefaultConfiguration2.ConfigName);
        result.Add(DefaultConfiguration3.ConfigName);

        return result;
    }
}