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
            chessState.Board.Move(movePieceAction.Move);
            chessState.MovingPositon = new Position('0', 0);
            MoveGenerator.GenerateMoves(chessState.Board);
            // If There are no moves the game is over. Check if in Check. If no moves and in check
            chessState.Board.DisplayBoard();

            return Unit.Task;
        }
    }
}

