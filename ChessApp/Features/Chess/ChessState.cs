using BlazorState;
using ChessApp.Data;

namespace ChessApp.Features.Chess;

public partial class ChessState : State<ChessState>
{
    public bool IsFlipped { get; private set; }
    public bool Mobile { get; set; }
    public Position MovingPositon { get; private set; }

    public Chessboard Board { get; private set; }

    public Player WhitePlayer { get; set; }

    public Player BlackPlayer { get; set; }

    public override void Initialize()
    {
        Board = new Chessboard();
        IsFlipped = false;
        Mobile = false;
        MovingPositon = new Position('0', 0);
    }

    public void StartGame(Player White = Player.This, Player Black = Player.This)
    {
        WhitePlayer = White;
        BlackPlayer = Black;
        Board.GameState = GameState.Playing;
    }
}

public enum Player
{
    This,
    Bot,
    OnlinePlayer
}