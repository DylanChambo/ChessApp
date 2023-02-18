using BlazorState;
using ChessApp.Data;
using ChessApp.Scripts.Chess;

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
        Board = new Board("rnbqkb1r/pp1p1pPp/8/2p1pP2/1P1P4/3P3P/P1P1P3/RNBQKB1R w KQkq e6 0 1");
        MoveList moveList = new MoveList();
        MoveGenerator.GenerateAllMoves(Board, moveList);
        moveList.Display();
    }


}

