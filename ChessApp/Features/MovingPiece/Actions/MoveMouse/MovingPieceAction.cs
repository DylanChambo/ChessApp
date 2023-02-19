using BlazorState;
using ChessApp.Scripts.Chess;

namespace ChessApp.Features.MovingPiece;

public partial class MovingPieceState
{
    public class MovingPieceAction : IAction
    {
        public Position Position { get; set; }

        public MovingPieceAction(Position position)
        {
            Position = position;
        }
    }
}

