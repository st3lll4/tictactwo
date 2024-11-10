using GameLogic;

namespace DAL;

public static class DefaultConfigurations
{
    public static GameConfiguration DefaultConfiguration => new()
    {
        GameName = "Tic-Tac-Two",
        Width = 5,
        Height = 5,
        Player1Symbol = 'X',
        Player2Symbol = 'O',
        StartingPlayer = "Player 1",
        MovableGridSize = 3,
        WinningCondition = 3,
        InitialMoves = 2,
        MaxPieces = 3
    };

    public static GameConfiguration DefaultConfiguration2 => new()
    {
        GameName = "Tic-Tac-Two with a 10x10 board",
        Width = 10,
        Height = 10,
        Player1Symbol = 'X',
        Player2Symbol = 'O',
        StartingPlayer = "Player 1",
        InitialMoves = 5,
        MaxPieces = 7,
        MovableGridSize = 5,
        WinningCondition = 5
    };

    public static GameConfiguration DefaultConfiguration3 => new()
    {
        GameName = "Tic-Tac-Toe",
        Width = 3,
        Height = 3,
        Player1Symbol = 'X',
        Player2Symbol = 'O',
        StartingPlayer = "Player 1",
        WinningCondition = 3,
        MovableGridSize = 3
    };
    
}