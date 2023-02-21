using BlazorState;
using ChessApp.Scripts.Chess;
using MediatR;

namespace ChessApp.Features.Chess;

public partial class ChessState
{
    public class MovePieceHandler : ActionHandler<MovePieceAction>
    {
        public MovePieceHandler(IStore store): base(store) { }

        ChessState chessState => Store.GetState<ChessState>();

        public override Task<Unit> Handle(MovePieceAction movePieceAction, CancellationToken cancellationToken)
        {
            chessState.Board.MakeMove(movePieceAction.Move);
            chessState.CheckGameState();
            chessState.UpdateMoveList();
            chessState.lastMoveFrom = (Position) Move.From(movePieceAction.Move);
            chessState.lastMoveTo = (Position) Move.To(movePieceAction.Move);
    
            return Unit.Task;
        }
    }
}

