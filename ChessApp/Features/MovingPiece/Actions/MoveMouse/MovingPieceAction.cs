using BlazorState;
using ChessApp.Scripts.Chess;

namespace ChessApp.Features.MovingPiece;

public partial class MovingPieceState
{
    public class MovingPieceAction : IAction
    {
        public Files File { get; set; }
        public Ranks Rank { get; set; }
    }
}

