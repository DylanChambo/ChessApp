using BlazorState;
using ChessApp.Data;
using MediatR;

namespace ChessApp.Features.Mouse;

public partial class MouseState
{
    public class MoveMouseHandler: ActionHandler<MoveMouseAction>
    {
        public MoveMouseHandler(IStore store): base(store) { }

        MouseState mouseState => Store.GetState<MouseState>();

        public override Task<Unit> Handle(MoveMouseAction moveMouseAction, CancellationToken cancellationToken)
        {
            Console.WriteLine("MoveMouse");
            mouseState.MousePos.X = moveMouseAction.MousePos.X;
            mouseState.MousePos.Y = moveMouseAction.MousePos.Y;
            return Unit.Task;
        }
    }
}

