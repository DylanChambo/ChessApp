using BlazorState;
using ChessApp.Data;
using MediatR;

namespace ChessApp.Features.Chess;

public partial class ChessState
{
    public class MovePieceHandler: ActionHandler<MovePieceAction>
    {
        public MovePieceHandler(IStore store): base(store) { }

        ChessState ChessState => Store.GetState<ChessState>();

        public override Task<Unit> Handle(MovePieceAction movePieceAction, CancellationToken cancellationToken)
        {
            Piece piece = ChessState.GetPiece(movePieceAction.OldPosition.File, movePieceAction.OldPosition.Rank);
            ChessState.SetPiece(movePieceAction.NewPosition.File, movePieceAction.NewPosition.Rank, piece);
            ChessState.SetPiece(movePieceAction.OldPosition.File, movePieceAction.OldPosition.Rank);

            return Unit.Task;
        }
    }
}

