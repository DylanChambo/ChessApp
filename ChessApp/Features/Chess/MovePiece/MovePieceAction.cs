using BlazorState;
using ChessApp.Data;

namespace ChessApp.Features.Chess;

public partial class ChessState
{
    public class MovePieceAction : IAction
    {
        public MovePieceAction(int move)
        {
            Move = move;
        }

        public int Move { get; set; }
    }
}

