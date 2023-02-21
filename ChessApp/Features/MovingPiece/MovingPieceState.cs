using BlazorState;
using ChessApp.Scripts.Chess;

namespace ChessApp.Features.MovingPiece;

public partial class MovingPieceState : State<MovingPieceState>
{
    public Position Position { get; set; }

    public override void Initialize()
    {
        Position = 0;
    }
}