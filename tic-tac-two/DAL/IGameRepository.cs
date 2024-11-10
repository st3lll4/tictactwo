using GameLogic;

namespace DAL;

public interface IGameRepository
{
    public void SaveGame(GameState gameState, string gameConfigName, string saveName);
    public GameState GetGameByName(string name);
    public List<string> GetGameNames();


}