using BlazorState;
using ChessApp.Data;
using ChessApp.Scripts.Chess;
using System.Diagnostics;

namespace ChessApp.Features.Chess;

public partial class ChessState : State<ChessState>
{
    const string PawnMoveW = "rnbqkb1r/pp1p1pPp/8/2p1pP2/1P1P4/3P3P/P1P1P3/RNBQKB1R w KQkq e6 0 1";
    const string KnightsKings = "5k2/1n6/4n3/6N1/8/3N4/8/5K2 b - - 0 1";
    const string Rooks = "6k1/8/5r2/8/1nR5/5N2/8/6K1 w - - 0 1";
    const string Queens = "6k1/8/4nq2/8/1QR5/5N2/1N6/6K1 w - - 0 1";
    const string Bishops = "6k1/1b6/4N3/8/1n4B1/1B3N2/1N6/2b3K1 b - - 0 1";
    const string Castling1 = "r3k2r/8/8/8/8/8/8/R3K2R b KQkq - 0 1";
    const string Castling2 = "3rk2r/8/8/8/8/8/8/R3K2R w KQk - 0 1";
    public bool IsFlipped { get; private set; }
    public Board Board { get; private set; }
    public MoveList MoveList { get; private set; }



    public override void Initialize()
    { 
        IsFlipped = false;
    }

    public void Init()
    {
        Board = new Board();
        MoveList = new MoveList();
        MoveGenerator.GenerateAllMoves(Board, MoveList);
        for (int i = 0; i < MoveList.count; i++)
        {
            int move = MoveList.moves[i].move;
            if (!Board.MakeMove(move))
            {
                continue;
            }
            Debug.WriteLine($"Made: {(Position)Move.From(move)}{(Position)Move.To(move)}");
            Board.TakeMove();
        }
    }


}

