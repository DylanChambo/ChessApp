namespace ChessApp.Data;

public enum Piece
{
    None,
    WhiteKing,
    WhitePawn,
    WhiteKnight,
    WhiteBishop,
    WhiteRook,
    WhiteQueen,
    BlackKing,
    BlackPawn,
    BlackKnight,
    BlackBishop,
    BlackRook,
    BlackQueen,
    Null
}

public class PieceUtils
{
    public static int Pawn = 1;
    public static int Knight = 3;
    public static int Bishop = 3;
    public static int Rook = 5;
    public static int Queen = 9;

    public static bool IsOpposition(Piece piece, Side side)
    {
        return side == Side.White ? IsBlack(piece) : IsWhite(piece);
    }

    public static bool IsBlack(Piece piece)
    {
        return (Piece.BlackKing <= piece && piece <= Piece.BlackQueen);
    }

    public static bool IsWhite(Piece piece)
    {
        return (Piece.WhiteKing <= piece && piece <= Piece.WhiteQueen);
    }

    public static bool IsKing(Piece piece)
    {
        return (Piece.WhiteKing == piece || piece == Piece.BlackKing);
    }

    public static bool IsPawn(Piece piece)
    {
        return (Piece.WhitePawn == piece || piece == Piece.BlackPawn);
    }

    public static bool IsKnight(Piece piece)
    {
        return (Piece.WhiteKnight == piece || piece == Piece.BlackKnight);
    }

    public static bool IsBishop(Piece piece)
    {
        return (Piece.WhiteBishop == piece || piece == Piece.BlackBishop);
    }

    public static bool IsRook(Piece piece)
    {
        return (Piece.WhiteRook == piece || piece == Piece.BlackRook);
    }

    public static bool IsQueen(Piece piece)
    {
        return (Piece.WhiteQueen == piece || piece == Piece.BlackQueen);
    }

    public static void CalcMaterial(Chessboard board)
    {
        Material material = new Material();
        for (char file = 'a'; file <= 'h'; file++)
        {
            for (int rank = 1; rank <= 8; rank++)
            {
                Piece piece = board.GetPiece(file, rank);
                int value = GetPieceValue(piece);
                if (IsWhite(piece)) {
                    material.White += value;
                } else
                {
                    material.Black += value;
                } 
            }
        }
        board.Material = material;
    }

    public static int GetPieceValue(Piece piece)
    {
        int value;
        if (piece == Piece.None)
        {
            value = 0;
        }
        else if (IsPawn(piece))
        {
            value = Pawn;
        }
        else if (IsKnight(piece))
        {
            value = Knight;
        }
        else if (IsBishop(piece))
        {
            value = Bishop;
        }
        else if (IsRook(piece))
        {
            value = Rook;
        }
        else if (IsQueen(piece))
        {
            value = Queen;
        }
        else
        {
            value = 0;
        }
        return value;
    }
}

public struct Material
{
    public int White { get; set; }
    public int Black { get; set; }

    public Material()
    {
        White = 0;
        Black = 0;
    }
}
