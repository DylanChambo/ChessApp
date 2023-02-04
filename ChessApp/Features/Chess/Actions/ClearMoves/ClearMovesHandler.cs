using BlazorState;
using ChessApp.Data;
using MediatR;

namespace ChessApp.Features.Chess;

public partial class ChessState
{
    public class ClearMovesHandler : ActionHandler<ClearMovesAction>
    {
        public ClearMovesHandler(IStore store): base(store) { }

        ChessState chessState => Store.GetState<ChessState>();

        public override Task<Unit> Handle(ClearMovesAction isMobileAction, CancellationToken cancellationToken)
        {
            chessState.PiecePossibleMoves.Clear();
            return Unit.Task;
        }
    }
}

