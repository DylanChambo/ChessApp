using BlazorState;
using ChessApp.Data;

namespace ChessApp.Features.Chess;

public partial class ChessState
{
    public class MovePieceAction: IAction
    {
        public Position OldPosition { get; set; }
        public Position NewPosition { get; set; }
    }
}

