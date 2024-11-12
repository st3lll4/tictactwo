using Domain;
using GameLogic;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class ConfigRepositoryDb : IConfigRepository
{
    private readonly AppDbContext _context = new AppDbContext(_contextOptions);
    private static string _connectionString = $"Data Source={FileHelper.BasePath}app.db";

    private static DbContextOptions<AppDbContext> _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
        .UseSqlite(_connectionString)
        .EnableDetailedErrors()
        .EnableSensitiveDataLogging()
        .Options;

    public GameConfiguration DefaultConfiguration { get; set; }
    public GameConfiguration DefaultConfiguration2 { get; set; }
    public GameConfiguration DefaultConfiguration3 { get; set; }

    public ConfigRepositoryDb()
    {
        DefaultConfiguration = DefaultConfigurations.DefaultConfiguration;
        DefaultConfiguration2 = DefaultConfigurations.DefaultConfiguration2;
        DefaultConfiguration3 = DefaultConfigurations.DefaultConfiguration3;

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
        return _context.Configurations.Select(c => c.GameName).ToList();
    }

    public List<GameConfiguration> GetAllConfigurations()
    {
        return _context.Configurations.ToList().Select(ConvertToGameConfiguration).ToList();
    }

    public GameConfiguration GetConfigurationByName(string name)
    {
        return ConvertToGameConfiguration(_context.Configurations.FirstOrDefault(c => c.GameName == name)!);
    }

    public void SaveConfiguration(GameConfiguration config)
    {
        var domainConfig = ConvertToDomainConfiguration(config);
        _context.Configurations.Add(domainConfig);
        _context.SaveChanges();
    }

    private Configuration ConvertToDomainConfiguration(GameConfiguration gameConfig)
    {
        return new Configuration
        {
            GameName = gameConfig.GameName,
            Width = gameConfig.Width,
            Height = gameConfig.Height,
            Player1Symbol = gameConfig.Player1Symbol,
            Player2Symbol = gameConfig.Player2Symbol,
            StartingPlayer = gameConfig.StartingPlayer,
            MovableGridSize = gameConfig.MovableGridSize,
            WinningCondition = gameConfig.WinningCondition,
            InitialMoves = gameConfig.InitialMoves,
            MaxPieces = gameConfig.MaxPieces
        };
    }
    
    private GameConfiguration ConvertToGameConfiguration(Configuration config)
    {
        return new GameConfiguration
        {
            GameName = config.GameName,
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
        var config = _context.Configurations.FirstOrDefault(c => c.GameName == name);
        if (config == null) return false;

        _context.Configurations.Remove(config);
        _context.SaveChanges();
        return true;
    }
}