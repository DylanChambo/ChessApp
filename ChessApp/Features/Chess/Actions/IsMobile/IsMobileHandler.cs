using BlazorState;
using ChessApp.Data;
using MediatR;

namespace ChessApp.Features.Chess;

public partial class ChessState
{
    public class IsMobileHandler : ActionHandler<IsMobileAction>
    {
        public IsMobileHandler(IStore store): base(store) { }

        ChessState chessState => Store.GetState<ChessState>();

        public override Task<Unit> Handle(IsMobileAction isMobileAction, CancellationToken cancellationToken)
        {
            chessState.Mobile = isMobileAction.Mobile;
            return Unit.Task;
        }
    }
}

