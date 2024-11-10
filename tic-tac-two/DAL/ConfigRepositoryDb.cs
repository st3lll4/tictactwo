using DAL.Migrations;
using Domain;
using GameLogic;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class ConfigRepositoryDb : IConfigRepository
{
    private readonly AppDbContext _context;

    public GameConfiguration DefaultConfiguration { get; set; }
    public GameConfiguration DefaultConfiguration2 { get; set; }
    public GameConfiguration DefaultConfiguration3 { get; set; }

    public ConfigRepositoryDb(AppDbContext context)
    {
        _context = context;

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
        return _context.Configurations.ToList();
    }
    
    public GameConfiguration GetConfigurationByName(string name)
    {
        return _context.Configurations.FirstOrDefault(c => c.GameName == name)!;
    }

    public void SaveConfiguration(GameConfiguration config)
    {
        _context.Configurations.Add(config);
        _context.SaveChanges();
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