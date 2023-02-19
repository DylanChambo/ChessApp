using BlazorState;
using MediatR;

namespace ChessApp.Features.MovingPiece;

public partial class MovingPieceState
{
    public class MovingPieceHandler : ActionHandler<MovingPieceAction>
    {
        public MovingPieceHandler(IStore store): base(store) { }

        MovingPieceState movingPieceState => Store.GetState<MovingPieceState>();

        public override Task<Unit> Handle(MovingPieceAction movingPieceAction, CancellationToken cancellationToken)
        {
            movingPieceState.Position = movingPieceAction.Position;
            return Unit.Task;
        }
    }
}

