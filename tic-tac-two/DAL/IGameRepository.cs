using GameLogic;

namespace DAL;

public interface IGameRepository
{
    public void SaveGame(GameState gameState, string gameConfigName, string? saveName, string userName);
    public GameState GetGameByName(string name);
    public List<string> GetGameNames();
    public List<string> GetGameByUser(string user);


}