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
        Board = new Board("rnbqkbnr/pp1ppppp/8/2p5/4P3/5N2/PPPP1PPP/RNBQKB1R b KQkq - 1 2");
        Board.Pawns[0].PrintBitBoard();
        Board.Pawns[1].PrintBitBoard();
        Board.Pawns[2].PrintBitBoard();
    }


}

