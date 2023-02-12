using System.Diagnostics;

namespace ChessApp.Data.Chess;

public class Board
{
    public const int VirtualBoardSize = 120;
    public const int BoardSize = 64;
    public const int MaxGameMoves = 2048;
    public int[] Squares { get; set; }
    public BitBoard[] Pawns { get; set; }
    public int[] KingSquare { get; set; }
    public int Side { get; set; }
    public int EnPassantSquare { get; set; }
    public int FiftyMoveCount { get; set; }
    public int Ply { get; set; }
    public int HisPly { get; set; }
    public int CastlePerm { get; set; }
    public ulong PosKey { get; set; }

    public int[] PieceNum { get; set; }
    public int[] BigPiece { get; set; }
    public int[] MajorPiece { get; set; }

    public int[] MinorPiece { get; set; }

    public Undo[] History { get; set; }

    int[,] PieceList { get; set; }

    public Board()
    {
        Squares = new int[VirtualBoardSize];
        Pawns = new BitBoard[3];
        KingSquare = new int[2];
        PieceNum = new int[13];
        BigPiece = new int[3];
        MajorPiece = new int[3];
        MinorPiece = new int[3];
        History = new Undo[MaxGameMoves];
        PieceList = new int[13,10];

        Pawns[0].PrintBitBoard();
        Pawns[0].SetBit(8);
        Pawns[0].SetBit(9);
        Pawns[0].SetBit(10);
        Pawns[0].PrintBitBoard();
        Debug.WriteLine(Pawns.Count());
    }


    
}
