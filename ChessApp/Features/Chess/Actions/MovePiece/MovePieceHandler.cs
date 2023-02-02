using BlazorState;
using ChessApp.Data;
using MediatR;

namespace ChessApp.Features.Chess;

public partial class ChessState
{
    public class MovePieceHandler: ActionHandler<MovePieceAction>
    {
        public MovePieceHandler(IStore store): base(store) { }

        ChessState chessState => Store.GetState<ChessState>();

        public override Task<Unit> Handle(MovePieceAction movePieceAction, CancellationToken cancellationToken)
        {
            Piece piece = chessState.GetPiece(movePieceAction.OldPosition.File, movePieceAction.OldPosition.Rank);
            chessState.SetPiece(movePieceAction.NewPosition.File, movePieceAction.NewPosition.Rank, piece);
            chessState.SetPiece(movePieceAction.OldPosition.File, movePieceAction.OldPosition.Rank);

            return Unit.Task;
        }
    }
}

