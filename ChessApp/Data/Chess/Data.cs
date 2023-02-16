namespace ChessApp.Data.Chess;

public static class Data
{
    public static bool[] PieceBig = { false, false, true, true, true, true, true, false, true, true, true, true, true };
    public static bool[] PieceMajor = { false, false, false, false, true, true, true, false, false, false, true, true, true };
    public static bool[] PieceMinor = { false, false, true, true, false, false, false, false, true, true, false, false, false };
    public static int[] PieceValue = { 0, 100, 300, 325, 500, 900, 99999, 100, 300, 325, 500, 900, 99999 };
    public static Sides[] PieceColour = { Sides.Both,
        Sides.White, Sides.White, Sides.White, Sides.White, Sides.White,
        Sides.Black, Sides.Black, Sides.Black, Sides.Black, Sides.Black };
}
