using BlazorState;
using ChessApp.Scripts.Chess;

namespace ChessApp.Features.MovingPiece;

public partial class MovingPieceState : State<MovingPieceState>
{
    public Files File { get; set; }
    public Ranks Rank { get; set; }

    public override void Initialize()
    {
        File = Files.None;
        Rank = Ranks.None;
    }
}