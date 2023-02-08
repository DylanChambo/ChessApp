using BlazorState;
using ChessApp.Data;

namespace ChessApp.Features.Chess;

public partial class ChessState : State<ChessState>
{
    public bool IsFlipped { get; private set; }
    public bool Mobile { get; set; }
    public Position MovingPositon { get; private set; }

    public Chessboard Board { get; private set; }

    public override void Initialize()
    {
        Board = new Chessboard();
        IsFlipped = false;
        Mobile = false;
        MovingPositon = new Position('0', 0);
    }
}
