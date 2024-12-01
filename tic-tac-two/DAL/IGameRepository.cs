using GameLogic;

namespace DAL;

public interface IGameRepository
{
    public void SaveGame(GameState gameState, string gameConfigName, string saveName, string user1Name, string? user2Name);
    public GameState GetGameByName(string name);
    public List<string> GetGameNames();
    public List<string> GetGamesByUser(string user);


}