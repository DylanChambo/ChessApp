﻿using BlazorState;
using ChessApp.Data;
using MediatR;

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
            return Unit.Task;
        }
    }
}
