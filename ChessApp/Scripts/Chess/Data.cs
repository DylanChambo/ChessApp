namespace ChessApp.Scripts.Chess;

public static class Data
{
    public readonly static bool[] PieceBig = { false, false, true, true, true, true, true, false, true, true, true, true, true, false };
    public readonly static bool[] PieceMajor = { false, false, false, false, true, true, true, false, false, false, true, true, true, false };
    public readonly static bool[] PieceMinor = { false, false, true, true, false, false, false, false, true, true, false, false, false, false };
    public readonly static int[] PieceValue = { 0, 100, 305, 333, 563, 950, 99999, 100, 305, 333, 563, 950, 99999 };
    public readonly static Sides[] PieceColour = { Sides.Both, Sides.White, Sides.White, Sides.White, Sides.White, Sides.White, Sides.White, Sides.Black, Sides.Black, Sides.Black, Sides.Black, Sides.Black, Sides.Black, Sides.Both };

    public readonly static bool[] IsPiecePawn = { false, true, false, false, false, false, false, true, false, false, false, false, false, false };
    public readonly static bool[] IsPieceKnight = { false, false, true, false, false, false, false, false, true, false, false, false, false, false };
    public readonly static bool[] IsPieceKing = { false, false, false, false, false, false, true, false, false, false, false, false, true, false };
    public readonly static bool[] IsPieceRookQueen = { false, false, false, false, true, true, false, false, false, false, true, true, false, false };
    public readonly static bool[] IsPieceBishopQueen = { false, false, false, true, false, true, false, false, false, true, false, true, false, false };
    public readonly static bool[] IsPieceSliding = { false, false, false, true, true, true, false, false, false, true, true, true, false, false };
    public readonly static int[] KnightDirection = { -8, -19, -21, -12, 8, 19, 21, 12 };
    public readonly static int[] RookDirection = { -1, -10, 1, 10 };
    public readonly static int[] BishopDirection = { -9, -11, 9, 11 };
    public readonly static int[] KingDirection = { -1, -10, 1, 10, -9, -11, 9, 11 };

    public static Files[] Files { get; private set; }
    public static Ranks[] Ranks { get; private set; }

    public readonly static int[] CastlePerm =
    {
        15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
        15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
        15, 13, 15, 15, 15, 12, 15, 15, 14, 15,
        15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
        15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
        15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
        15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
        15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
        15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
        15,  7, 15, 15, 15,  3, 15, 15, 11, 15,
        15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
        15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
    };

    static Data()
    {
        InitFileRankBoard();
    }

    private static void InitFileRankBoard()
    {
        Files = new Files[Board.VirtualBoardSize];
        Ranks = new Ranks[Board.VirtualBoardSize];
        for (int i = 0; i < Board.VirtualBoardSize; i++)
        {
            Files[i] = Chess.Files.None;
            Ranks[i] = Chess.Ranks.None;
        }

        for (Ranks rank = Chess.Ranks.r1; rank <= Chess.Ranks.r8; rank++)
        {
            for (Files file = Chess.Files.A; file <= Chess.Files.H; file++)
            {
                int square = Conversion.ConvertFRTo120((int)file, (int)rank);
                Files[square] = file;
                Ranks[square] = rank;
            }
        }
    }
}
