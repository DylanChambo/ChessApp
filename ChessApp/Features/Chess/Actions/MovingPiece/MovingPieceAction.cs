using BlazorState;
using ChessApp.Data;

namespace ChessApp.Features.Chess;

public partial class ChessState
{
    public class MovingPieceAction : IAction
    {
        public Position MovingPos { get; set; }
    }
}

