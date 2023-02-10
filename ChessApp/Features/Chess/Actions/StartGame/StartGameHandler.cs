using BlazorState;
using ChessApp.Data;
using MediatR;

namespace ChessApp.Features.Chess;

public partial class ChessState
{
    public class StartGameHandler : ActionHandler<StartGameAction>
    {
        public StartGameHandler(IStore store): base(store) { }

        ChessState chessState => Store.GetState<ChessState>();

        public override Task<Unit> Handle(StartGameAction startGameAction, CancellationToken cancellationToken)
        {
            chessState.Board.StartGame(startGameAction.White, startGameAction.Black);
            return Unit.Task;
        }
    }
}

