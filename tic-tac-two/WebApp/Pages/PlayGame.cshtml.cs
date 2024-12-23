using DAL;
using GameLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class PlayGame : PageModel
{
    private readonly IGameRepository _gameRepository;

    [BindProperty(SupportsGet = true)] public string GameName { get; set; } = default!;

    [BindProperty] public GameState GameState { get; set; } = default!;
    
    [BindProperty(SupportsGet = true)] public string? UserName { get; set; } = default!;

    [BindProperty(SupportsGet = true)] public string User1 { get; set; } = default!;
    [BindProperty(SupportsGet = true)] public string User2 { get; set; } = default!;

    [BindProperty(SupportsGet = true)] public string GameMode { get; set; } = default!;

    [BindProperty] public GameBrain Brain { get; set; } = default!;

    [BindProperty(SupportsGet = true)] public string? Message { get; set; }

    public char[,] Board { get; set; } = default!;
    public string MovingPlayer { get; set; } = default!;

    [BindProperty(SupportsGet = true)] public bool ShowSweetAlert { get; set; }

    [BindProperty(SupportsGet = true)] public string SweetAlertMessage { get; set; } = default!;

    [BindProperty(SupportsGet = true)] public bool IsGameReady { get; set; } = false;
    
    public PlayGame(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public IActionResult OnGet(int? x, int? y, string? direction)
    {
        var game = _gameRepository.GetGameByName(GameName);

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
            Player1Name = game.Player1Name,
            Player2Name = game.Player2Name
        };

        Board = GameState.Board;
        Brain = new GameBrain(GameState);

        if (GameState.IsGameOver)
        {
            ShowSweetAlert = true;
            SweetAlertMessage = $"{GameState.MovingPlayer} wins!";
            Message = "Game over bros!";
            return Page();
        }

        if (GameMode == "Multiplayer" &&
            (string.IsNullOrEmpty(GameState.Player1Name) || string.IsNullOrEmpty(GameState.Player2Name)))
        {
            IsGameReady = false;
            Message = "waiting for player 2 to join.........";
            if (GameState.Player2Name == null)
            {
                return Page();
            }

            return RedirectToPage(new
            {
                user1 = GameState.Player1Name,
                user2 = GameState.Player2Name,
                gamemode = GameMode,
                gameName = GameName,
                message = Message,
                showSweetAlert = ShowSweetAlert,
                sweetAlertMessage = SweetAlertMessage,
                isgameover = GameState.IsGameOver
            });
        }

        if (GameMode == "Single player" && 
            GameState is { Player1PiecesPlaced: 0, Player2PiecesPlaced: 0 })
        {
            GameState.Player1Name = UserName ?? GameState.Player1Name;
            GameState.Player2Name = "Bot1";
            _gameRepository.UpdateGame(GameState, game.Config.ConfigName, GameName, GameState.Player1Name,
                GameState.Player2Name);
        }

        if (GameMode == "Bots")
        {
            GameState.Player1Name = "Bot1";
            GameState.Player2Name = "Bot2";
        }
        
        IsGameReady = true;

        if ((GameState.MovingPlayer == GameState.Config.Player2Symbol && GameState.Player2Name == "Bot2") ||
            GameState.MovingPlayer == GameState.Config.Player1Symbol && GameState.Player1Name == "Bot1" ||
            GameState.MovingPlayer == GameState.Config.Player2Symbol && GameState.Player2Name == "Bot1")
        {
            if (GameState is { Player1PiecesPlaced: 0, Player2PiecesPlaced: 0 } && GameMode == "Bots")
            {
                Brain.MakeBotMove();
                Brain.SwitchPlayer();
                _gameRepository.UpdateGame(GameState, game.Config.ConfigName, GameName, GameState.Player1Name,
                    GameState.Player2Name);
                return Page();
            }
            
            Brain.MakeBotMove();
            CheckGameOver();
            if (!GameState.IsGameOver)
            {
                Brain.SwitchPlayer();
            }

            _gameRepository.UpdateGame(GameState, game.Config.ConfigName, GameName, GameState.Player1Name,
                GameState.Player2Name);

            return Page();
        }

        //moving grid part
        if (direction != null && !GameState.IsGameOver)
        {
            if (!Brain.MoveGrid(direction))
            {
                Message = "Grid out of bounds man!";
            }
            else
            {
                CheckGameOver();
                if (!GameState.IsGameOver)
                {
                    Brain.SwitchPlayer();
                }
            }

            _gameRepository.UpdateGame(GameState, game.Config.ConfigName, GameName, GameState.Player1Name,
                GameState.Player2Name);

            return RedirectToPage(new
            {
                user1 = GameState.Player1Name,
                user2 = GameState.Player2Name,
                gamemode = GameMode,
                gameName = GameName,
                message = Message,
                showSweetAlert = ShowSweetAlert,
                sweetAlertMessage = SweetAlertMessage,
                isgameover = GameState.IsGameOver
            });
        }

        //placing piece part
        if (x.HasValue && y.HasValue && MovingPlayer != "Bot1" && MovingPlayer != "Bot2")
        {
            if (!Brain.IsInGrid(x.Value, y.Value) && Board[x.Value, y.Value] != GameState.MovingPlayer)
            {
                Message = "move inside the grid!";
                goto SkipToEnd;
            }

            if (Board[x.Value, y.Value] != GameState.MovingPlayer && Board[x.Value, y.Value] != '\0')
            {
                Message = "this spot is taken already!";
                goto SkipToEnd;
            }

            if (Brain.GetMovingPlayerPiecesPlaced() >= GameState.Config.MaxPieces && 
                Board[x.Value, y.Value] == '\0')
            {
                Message = "you have already used all your pieces!";
                goto SkipToEnd;
            }

            if (Board[x.Value, y.Value] == GameState.MovingPlayer)
            {
                Board[x.Value, y.Value] = '\0';
                if (GameState.MovingPlayer == GameState.Config.Player1Symbol)
                {
                    GameState.Player1PiecesPlaced--;
                } else if (GameState.MovingPlayer == GameState.Config.Player2Symbol)
                {
                    GameState.Player2PiecesPlaced--;
                }
                goto SkipToEnd; // pigem halb
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
            _gameRepository.UpdateGame(GameState, game.Config.ConfigName, GameName, GameState.Player1Name,
                GameState.Player2Name); // update ui sees weird, parem repo-mootor-ui
            return RedirectToPage(new
            {
                user1 = GameState.Player1Name,
                user2 = GameState.Player2Name,
                gamemode = GameMode,
                gameName = GameName,
                message = Message,
                showSweetAlert = ShowSweetAlert,
                sweetAlertMessage = SweetAlertMessage,
                isgameover = GameState.IsGameOver
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

    public IActionResult OnPost()
    {
        _gameRepository.UpdateGame(GameState, GameState.Config.ConfigName, GameName, GameState.Player1Name,
            GameState.Player2Name);
        return RedirectToPage("/StartGame", new { userName = User1, gameMode = GameMode });
    }
}