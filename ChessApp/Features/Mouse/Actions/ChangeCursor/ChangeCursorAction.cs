using BlazorState;
using ChessApp.Data;

namespace ChessApp.Features.Mouse;

public partial class MouseState
{
    public class ChangeCursorAction: IAction
    {
        public string Cursor { get; set; }
    }
}

