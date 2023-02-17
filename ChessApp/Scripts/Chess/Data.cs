namespace ChessApp.Scripts.Chess;

public static class Data
{
    public static bool[] PieceBig = { false, false, true, true, true, true, true, false, true, true, true, true, true };
    public static bool[] PieceMajor = { false, false, false, false, true, true, true, false, false, false, true, true, true };
    public static bool[] PieceMinor = { false, false, true, true, false, false, false, false, true, true, false, false, false };
    public static int[] PieceValue = { 0, 100, 300, 325, 500, 900, 99999, 100, 300, 325, 500, 900, 99999 };
    public static Sides[] PieceColour = { Sides.Both, Sides.White, Sides.White, Sides.White, Sides.White, Sides.White, Sides.White, Sides.Black, Sides.Black, Sides.Black, Sides.Black, Sides.Black, Sides.Black };


    public static Files[] Files { get; private set; }
    public static Ranks[] Ranks { get; private set; }

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
