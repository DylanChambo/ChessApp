using BlazorState;
using ChessApp.Data;
using MediatR;
using static ChessApp.Features.Chess.ChessState;

namespace ChessApp.Features.Chess;

public partial class ChessState
{
    public class MovingPieceHandler : ActionHandler<MovingPieceAction>
    {
        public MovingPieceHandler(IStore store): base(store) { }

        ChessState chessState => Store.GetState<ChessState>();

        public override Task<Unit> Handle(MovingPieceAction movingPieceAction, CancellationToken cancellationToken)
        {
            chessState.MovingPositon = movingPieceAction.MovingPos;
            chessState.PiecePossibleMoves.Clear();
            chessState.SetPieceMoves();

            return Unit.Task;
        }
    }
}

