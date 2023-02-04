using BlazorState;
using ChessApp.Data;

namespace ChessApp.Features.Mouse;

public partial class MouseState
{
    public class MoveMouseAction: IAction
    {
        public Coord MousePos { get; set; }
    }
}

