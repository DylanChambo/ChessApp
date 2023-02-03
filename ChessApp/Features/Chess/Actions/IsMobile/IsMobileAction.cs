using BlazorState;
using ChessApp.Data;

namespace ChessApp.Features.Chess;

public partial class ChessState
{
    public class IsMobileAction : IAction
    {
        public bool Mobile { get; set; }
    }
}

