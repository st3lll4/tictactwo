using DAL;
using GameLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class PlayGame : PageModel
{
    private readonly IGameRepository _gameRepository;
    private readonly IConfigRepository _configRepository;

    [BindProperty(SupportsGet = true)] public string GameName { get; set; }

    [BindProperty] public GameState GameState { get; set; } = default!;

    [BindProperty(SupportsGet = true)] public string UserName { get; set; } = default!;
    [BindProperty(SupportsGet = true)] public string User2Name { get; set; } = default!;

    [BindProperty(SupportsGet = true)] public string GameMode { get; set; } = default!;

    [BindProperty] public GameBrain Brain { get; set; } = default!;

    [BindProperty(SupportsGet = true)] public string? Message { get; set; }

    public char[,] Board { get; set; } = default!;

    [BindProperty(SupportsGet = true)] public bool ShowSweetAlert { get; set; }

    [BindProperty(SupportsGet = true)] public string SweetAlertMessage { get; set; }

    public bool IsCurrentPlayerTurn { get; private set; } //todo
    
    [BindProperty(SupportsGet = true)] 
    public bool IsGameReady { get; set; } = false;


    public PlayGame(IGameRepository gameRepository, AppDbContext context, IConfigRepository configRepository)
    {
        _gameRepository = gameRepository;
        _configRepository = configRepository;
    }

    public IActionResult OnGet(int? x, int? y, string? direction)
    {
        var game = _gameRepository.GetGameByName(GameName);
        
        if (GameMode == "Multiplayer" && 
            (string.IsNullOrEmpty(game.Player1Name) || string.IsNullOrEmpty(game.Player2Name)))
        {
            IsGameReady = false;
            Message = "Waiting for the second player to join................";
            return Page();
        }

        IsGameReady = true;
        
        GameState = new GameState(game.Config)
        {
            BoardData = game.BoardData,
            GridStartCol = game.GridStartCol,
            GridStartRow = game.GridStartRow,
            GridCenterCol = game.GridCenterCol,
            GridCenterRow = game.GridCenterRow,
            IsGameOver = game.IsGameOver,
            Player1PiecesPlaced = game.Player1PiecesPlaced,
            Player2PiecesPlaced = game.Player2PiecesPlaced,
            MovingPlayer = game.MovingPlayer,
        };
        Board = GameState.Board;
        Brain = new GameBrain(GameState);


        if (direction != null && !GameState.IsGameOver)
        {
            if (!Brain.MoveGrid(direction))
            {
                Message = "Grid out of bounds man!";
            }
            else
            {
                CheckGameOver();
                Brain.SwitchPlayer();
            }

            _gameRepository.UpdateGame(GameState, game.Config.ConfigName, GameName, UserName, User2Name);
        }

        if (x.HasValue && y.HasValue)
        {
            if (!Brain.IsInGrid(x.Value, y.Value))
            {
                Message = "move inside the grid!";
                goto SkipToEnd;
            }

            if (Board[x.Value, y.Value] != GameState.MovingPlayer && Board[x.Value, y.Value] != '\0')
            {
                Message = "this spot is taken already!";
                goto SkipToEnd;
            }

            if (Board[x.Value, y.Value] == GameState.MovingPlayer)
            {
                Board[x.Value, y.Value] = '\0';
                goto SkipToEnd;
            }

            if (Board[x.Value, y.Value] == '\0')
            {
                Brain.PlacePiece(x.Value, y.Value, GameState.MovingPlayer);
                CheckGameOver();
                if (!GameState.IsGameOver)
                {
                    Brain.SwitchPlayer();
                }
            }

            SkipToEnd:
            _gameRepository.UpdateGame(GameState, game.Config.ConfigName, GameName, UserName, null);
            return RedirectToPage(new
            {
                username = UserName,
                user2name = User2Name,
                gamemode = GameMode,
                gameName = GameName,
                message = Message, 
                showSweetAlert = ShowSweetAlert,
                sweetAlertMessage = SweetAlertMessage 
            });
        }

        return Page();
    }


    private void CheckGameOver()
    {
        if (Brain.CheckTie())
        {
            ShowSweetAlert = true;
            SweetAlertMessage = "damn, a tie!";
            GameState.IsGameOver = true;
            Message = "Game over bros!";
        }

        if (Brain.CheckWin(GameState.MovingPlayer))
        {
            ShowSweetAlert = true;
            SweetAlertMessage = $"{GameState.MovingPlayer} wins!";
            GameState.IsGameOver = true;
            Message = "Game over bros!";
        }
    }
}