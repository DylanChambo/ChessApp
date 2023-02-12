using BlazorState;
using ChessApp.Data;
using ChessApp.Data.Chess;

namespace ChessApp.Features.Chess;

public partial class ChessState : State<ChessState>
{
    public bool IsFlipped { get; private set; }
    public bool Mobile { get; set; }
    public Board Board { get; set; }
    

    public override void Initialize()
    { 
        IsFlipped = false;
        Mobile = false;
    }

    public void Init()
    {
        Board = new Board();
    }


}

