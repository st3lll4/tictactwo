namespace Configs;

public interface IGameRepository
{
    public void SaveGame(string jsonStateString, string gameConfigName);
}