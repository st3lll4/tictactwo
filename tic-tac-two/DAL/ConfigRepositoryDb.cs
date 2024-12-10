using System.ComponentModel.Design;
using Domain;
using GameLogic;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class ConfigRepositoryDb : IConfigRepository
{
    private readonly AppDbContext _context;

    public ConfigRepositoryDb(AppDbContext context)
    {
        _context = context;
        DefaultConfiguration = DefaultConfigurations.DefaultConfiguration;
        DefaultConfiguration2 = DefaultConfigurations.DefaultConfiguration2;
        DefaultConfiguration3 = DefaultConfigurations.DefaultConfiguration3;

        CheckAndCreateInitialDirectory();
    }

    public GameConfiguration DefaultConfiguration { get; set; }
    public GameConfiguration DefaultConfiguration2 { get; set; }
    public GameConfiguration DefaultConfiguration3 { get; set; }


    private void InitializeDefaultConfigurations()
    {
        SaveConfiguration(DefaultConfigurations.DefaultConfiguration);
        SaveConfiguration(DefaultConfigurations.DefaultConfiguration2);
        SaveConfiguration(DefaultConfigurations.DefaultConfiguration3);
    }

    public void SaveConfiguration(GameConfiguration config)
    {
        var domainConfig = ConvertToDomainConfiguration(config);
        _context.Configurations.Add(domainConfig);
        _context.SaveChanges();
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

    public void SaveConfiguration(GameConfiguration config, string userName)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserName == userName) ?? new User { UserName = userName };

        var domainConfig = ConvertToDomainConfiguration(config, user);
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

    private Configuration ConvertToDomainConfiguration(GameConfiguration gameConfig)
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
            MaxPieces = gameConfig.MaxPieces
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

    public List<string> GetConfigsByUser(string userName)
    {
        var userConfigs = _context.Configurations
            .Where(c => c.User != null && c.User.UserName == userName)
            .Select(c => c.ConfigName)
            .ToList();

        var defaultConfigs = new List<string>
        {
            DefaultConfiguration.ConfigName,
            DefaultConfiguration2.ConfigName,
            DefaultConfiguration3.ConfigName
        };

        var result = new List<string>(userConfigs);

        foreach (var defaultConfig in defaultConfigs)
        {
            if (!result.Contains(defaultConfig))
            {
                result.Add(defaultConfig);
            }
        }

        return result;
    }
}