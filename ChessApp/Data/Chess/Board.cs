using System.Diagnostics;

namespace ChessApp.Data.Chess;

public class Board
{
    const int VirtualBoardSize = 120;
    const int BoardSize = 64;
    const int MaxGameMoves = 2048;
    public int[] Squares { get; set; }
    public ulong[] Pawns { get; set; }
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

    public int[] Square120To64 { get; set; }
    public int[] Square64To120 { get; set; }
    public Board()
    {
        Squares = new int[VirtualBoardSize];
        Pawns = new ulong[3];
        KingSquare = new int[2];
        PieceNum = new int[13];
        BigPiece = new int[3];
        MajorPiece = new int[3];
        MinorPiece = new int[3];
        History = new Undo[MaxGameMoves];
        InitializeConversionArray();
}

    /// <summary>
    /// Generetes lookup tables to convert coordintates as fast as possible;
    /// </summary>
    public void InitializeConversionArray()
    {
        Square120To64 = new int[VirtualBoardSize];
        Square64To120 = new int[BoardSize];

        for (int i = 0; i < VirtualBoardSize; i++)
        {
            Square120To64[i] = -1;
        }

        for (int i = 0; i < BoardSize; i++)
        {
            Square120To64[i] = -1;
        }

        int square64 = 0;

        for (int rank = (int) Ranks.r1; rank <= (int) Ranks.r8; rank++)
        {
            for (int file = (int) Files.A; file <= (int)Files.H; file++)
            {
                int square = ConvertFRToIndex(file, rank);
                Square64To120[square64] = square;
                Square120To64[square] = square64;
                square64++;
            }
        }
    }

    public static int ConvertFRToIndex(int file, int rank)
    {
        return 21 + file + 10 * rank;
    }
}
