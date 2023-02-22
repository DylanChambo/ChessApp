namespace ChessApp.Scripts.Chess.AI;

public class Evaluate
{

    public static readonly int[] PawnTable = {
    0   ,   0   ,   0   ,   0   ,   0   ,   0   ,   0   ,   0   ,
    10  ,   10  ,   0   ,   -10 ,   -10 ,   0   ,   10  ,   10  ,
    5   ,   0   ,   0   ,   5   ,   5   ,   0   ,   0   ,   5   ,
    0   ,   0   ,   10  ,   20  ,   21  ,   10  ,   0   ,   0   ,
    5   ,   5   ,   5   ,   10  ,   10  ,   5   ,   5   ,   5   ,
    10  ,   10  ,   10  ,   20  ,   20  ,   10  ,   10  ,   10  ,
    20  ,   20  ,   20  ,   30  ,   30  ,   20  ,   20  ,   20  ,
    0   ,   0   ,   0   ,   0   ,   0   ,   0   ,   0   ,   0
};

    public static readonly int[] KnightTable = {
    0   ,   -10 ,   0   ,   0   ,   0   ,   0   ,   -10 ,   0   ,
    0   ,   0   ,   0   ,   5   ,   5   ,   0   ,   0   ,   0   ,
    0   ,   0   ,   10  ,   10  ,   10  ,   10  ,   0   ,   0   ,
    0   ,   0   ,   10  ,   20  ,   20  ,   10  ,   5   ,   0   ,
    5   ,   10  ,   15  ,   20  ,   20  ,   15  ,   10  ,   5   ,
    5   ,   10  ,   10  ,   20  ,   20  ,   10  ,   10  ,   5   ,
    0   ,   0   ,   5   ,   10  ,   10  ,   5   ,   0   ,   0   ,
    0   ,   0   ,   0   ,   0   ,   0   ,   0   ,   0   ,   0
};

    public static readonly int[] BishopTable = {
    0   ,   0   ,   -10 ,   0   ,   0   ,   -10 ,   0   ,   0   ,
    0   ,   0   ,   0   ,   10  ,   10  ,   0   ,   0   ,   0   ,
    0   ,   0   ,   10  ,   15  ,   15  ,   10  ,   0   ,   0   ,
    0   ,   10  ,   15  ,   20  ,   20  ,   15  ,   10  ,   0   ,
    0   ,   10  ,   15  ,   20  ,   20  ,   15  ,   10  ,   0   ,
    0   ,   0   ,   10  ,   15  ,   15  ,   10  ,   0   ,   0   ,
    0   ,   0   ,   0   ,   10  ,   10  ,   0   ,   0   ,   0   ,
    0   ,   0   ,   0   ,   0   ,   0   ,   0   ,   0   ,   0
};

    public static readonly int[] RookTable = {
    0   ,   0   ,   5   ,   10  ,   10  ,   5   ,   0   ,   0   ,
    0   ,   0   ,   5   ,   10  ,   10  ,   5   ,   0   ,   0   ,
    0   ,   0   ,   5   ,   10  ,   10  ,   5   ,   0   ,   0   ,
    0   ,   0   ,   5   ,   10  ,   10  ,   5   ,   0   ,   0   ,
    0   ,   0   ,   5   ,   10  ,   10  ,   5   ,   0   ,   0   ,
    0   ,   0   ,   5   ,   10  ,   10  ,   5   ,   0   ,   0   ,
    25  ,   25  ,   25  ,   25  ,   25  ,   25  ,   25  ,   25  ,
    0   ,   0   ,   5   ,   10  ,   10  ,   5   ,   0   ,   0
};

//    public static readonly int[] KingEndgame = {
//    -50 ,   -10 ,   0   ,   0   ,   0   ,   0   ,   -10 ,   -50 ,
//    -10,    0   ,   10  ,   10  ,   10  ,   10  ,   0   ,   -10 ,
//    0   ,   10  ,   20  ,   20  ,   20  ,   20  ,   10  ,   0   ,
//    0   ,   10  ,   20  ,   40  ,   40  ,   20  ,   10  ,   0   ,
//    0   ,   10  ,   20  ,   40  ,   40  ,   20  ,   10  ,   0   ,
//    0   ,   10  ,   20  ,   20  ,   20  ,   20  ,   10  ,   0   ,
//    -10,    0   ,   10  ,   10  ,   10  ,   10  ,   0   ,   -10 ,
//    -50 ,   -10 ,   0   ,   0   ,   0   ,   0   ,   -10 ,   -50
//};

//    public static readonly int[] KingOpening = {
//    0   ,   5   ,   5   ,   -10 ,   -10 ,   0   ,   10  ,   5   ,
//    -30 ,   -30 ,   -30 ,   -30 ,   -30 ,   -30 ,   -30 ,   -30 ,
//    -50 ,   -50 ,   -50 ,   -50 ,   -50 ,   -50 ,   -50 ,   -50 ,
//    -70 ,   -70 ,   -70 ,   -70 ,   -70 ,   -70 ,   -70 ,   -70 ,
//    -70 ,   -70 ,   -70 ,   -70 ,   -70 ,   -70 ,   -70 ,   -70 ,
//    -70 ,   -70 ,   -70 ,   -70 ,   -70 ,   -70 ,   -70 ,   -70 ,
//    -70 ,   -70 ,   -70 ,   -70 ,   -70 ,   -70 ,   -70 ,   -70 ,
//    -70 ,   -70 ,   -70 ,   -70 ,   -70 ,   -70 ,   -70 ,   -70
//};

    public static int EvaluatePosition(Board board)
    {
        int score = board.Material[(int)Sides.White] - board.Material[(int)Sides.Black];

        Pieces piece = Pieces.WhitePawn;
        for (int i = 0; i < board.PieceNum[(int)piece]; i++)
        {
            Position pos = board.PieceList[(int)piece, i];
            score += PawnTable[Conversion.Square120To64[(int)pos]];
        }

        piece = Pieces.BlackPawn;
        for (int i = 0; i < board.PieceNum[(int)piece]; i++)
        {
            Position pos = board.PieceList[(int)piece, i];
            score -= PawnTable[Data.Mirror64[Conversion.Square120To64[(int)pos]]];
        }


        piece = Pieces.WhiteKnight;
        for (int i = 0; i < board.PieceNum[(int)piece]; i++)
        {
            Position pos = board.PieceList[(int)piece, i];
            score += KnightTable[Conversion.Square120To64[(int)pos]];
        }

        piece = Pieces.BlackKnight;
        for (int i = 0; i < board.PieceNum[(int)piece]; i++)
        {
            Position pos = board.PieceList[(int)piece, i];
            score -= KnightTable[Data.Mirror64[Conversion.Square120To64[(int)pos]]];
        }


        piece = Pieces.WhiteBishop;
        for (int i = 0; i < board.PieceNum[(int)piece]; i++)
        {
            Position pos = board.PieceList[(int)piece, i];
            score += BishopTable[Conversion.Square120To64[(int)pos]];
        }

        piece = Pieces.BlackBishop;
        for (int i = 0; i < board.PieceNum[(int)piece]; i++)
        {
            Position pos = board.PieceList[(int)piece, i];
            score -= BishopTable[Data.Mirror64[Conversion.Square120To64[(int)pos]]];
        }


        piece = Pieces.WhiteRook;
        for (int i = 0; i < board.PieceNum[(int)piece]; i++)
        {
            Position pos = board.PieceList[(int)piece, i];
            score += RookTable[Conversion.Square120To64[(int)pos]];
        }

        piece = Pieces.BlackRook;
        for (int i = 0; i < board.PieceNum[(int)piece]; i++)
        {
            Position pos = board.PieceList[(int)piece, i];
            score -= RookTable[Data.Mirror64[Conversion.Square120To64[(int)pos]]];
        }

        if (board.Side == Sides.White)
        {
            return score;
        } else
        {
            return -score;
        }
    }
}
