using GameLogic;

namespace DAL;

public interface IGameRepository
{
    public void SaveGame(GameState gameState, string gameConfigName, string saveName, string user1Name, string? user2Name);
    public GameState GetGameByName(string name);
    public List<string> GetGameNames();
    public List<string> GetGamesByUser(string user);
    public bool CheckIfGameExists(string name);
    void UpdateGame(GameState gameState, string configConfigName, string gameName, string userName, string? user2Name);

    public bool IsGameJoinable(string name);

    public void JoinMultiplayerGame(string gameName, string player1Name ,string player2Name);

}