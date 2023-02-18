namespace ChessApp.Scripts.Chess;

public enum Pieces
{
    None,
    WhitePawn,
    WhiteKnight,
    WhiteBishop,
    WhiteRook,
    WhiteQueen,
    WhiteKing,
    BlackPawn,
    BlackKnight,
    BlackBishop,
    BlackRook,
    BlackQueen,
    BlackKing,
    Offboard
}

public enum Files
{
    A,
    B,
    C,
    D,
    E,
    F,
    G,
    H,
    None,
}

public enum Ranks
{
    r1,
    r2,
    r3,
    r4,
    r5,
    r6,
    r7,
    r8,
    None
}

public enum Sides
{
    White,
    Black,
    Both
}

public enum Position
{
    No_Square,
    a1 = 21, b1, c1, d1, e1, f1, g1, h1,
    a2 = 31, b2, c2, d2, e2, f2, g2, h2,
    a3 = 41, b3, c3, d3, e3, f3, g3, h3,
    a4 = 51, b4, c4, d4, e4, f4, g4, h4,
    a5 = 61, b5, c5, d5, e5, f5, g5, h5,
    a6 = 71, b6, c6, d6, e6, f6, g6, h6,
    a7 = 81, b7, c7, d7, e7, f7, g7, h7,
    a8 = 91, b8, c8, d8, e8, f8, g8, h8,
}

public static class Castling
{
    public const int WhiteKingSide = 1;
    public const int WhiteQueenSide = 2;
    public const int BlackKingSide = 4;
    public const int BlackQueenSide = 8;
}