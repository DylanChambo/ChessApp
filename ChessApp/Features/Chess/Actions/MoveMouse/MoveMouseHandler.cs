using BlazorState;
using ChessApp.Data;
using MediatR;

namespace ChessApp.Features.Chess;

public partial class ChessState
{
    public class MoveMouseHandler: ActionHandler<MoveMouseAction>
    {
        public MoveMouseHandler(IStore store): base(store) { }

        ChessState chessState => Store.GetState<ChessState>();

        public override Task<Unit> Handle(MoveMouseAction moveMouseAction, CancellationToken cancellationToken)
        {
            chessState.MousePos.X = moveMouseAction.MousePos.X;
            chessState.MousePos.Y = moveMouseAction.MousePos.Y;
            return Unit.Task;
        }
    }
}

