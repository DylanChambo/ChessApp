using BlazorState;
using ChessApp.Data;

namespace ChessApp.Features.Chess;

public partial class ChessState
{
    public class MovePieceAction: IAction
    {
        public Move Move { get; set; }
    }
}

