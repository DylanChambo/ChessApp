using BlazorState;
using ChessApp.Data;
using MediatR;

namespace ChessApp.Features.Mouse;

public partial class MouseState
{
    public class ChangeCursorHandler: ActionHandler<ChangeCursorAction>
    {
        public ChangeCursorHandler(IStore store): base(store) { }

        MouseState mouseState => Store.GetState<MouseState>();

        public override Task<Unit> Handle(ChangeCursorAction changeCursorAction, CancellationToken cancellationToken)
        {
            mouseState.Cursor = changeCursorAction.Cursor;
            return Unit.Task;
        }
    }
}

