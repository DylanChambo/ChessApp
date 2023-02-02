using BlazorState;
using ChessApp.Data;

namespace ChessApp.Features.Chess;

public partial class ChessState
{
    public class MoveMouseAction: IAction
    {
        public Coord MousePos { get; set; }
    }
}

