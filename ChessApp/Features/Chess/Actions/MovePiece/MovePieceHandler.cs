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
            Piece piece = chessState.Board.GetPiece(movePieceAction.Move.StartSquare.File, movePieceAction.Move.StartSquare.Rank);
            chessState.Board.SetPiece(movePieceAction.Move.StartSquare.File, movePieceAction.Move.StartSquare.Rank);
            chessState.Board.SetPiece(movePieceAction.Move.TargetSquare.File, movePieceAction.Move.TargetSquare.Rank, piece);
            chessState.MovingPositon = new Position('0', 0);
            chessState.Board.SideToMove = chessState.Board.SideToMove == Side.White ? Side.Black : Side.White;
            MoveGenerator.GenerateMoves(chessState.Board);
            chessState.Board.DisplayBoard();

            return Unit.Task;
        }
    }
}

