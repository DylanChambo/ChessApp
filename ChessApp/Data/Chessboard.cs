using AnyClone;
using ChessApp.Features.Chess;
using static ChessApp.Features.Chess.ChessState;

namespace ChessApp.Data;

public class Chessboard
{
    public Piece[] Board { get; set; }
    public Side SideToMove { get; set; }
    public Castling WhiteCastling { get; set; }
    public Castling BlackCastling { get; set; }
    public char epFile { get; set; } 
    public int HalfmoveClock { get; set; }
    public int FullmoveCount { get; set; }
    public List<Move> Moves { get; set; }

    public bool Check { get; set; }

    public Chessboard() 
    {
        Board = new Piece[64];
        WhiteCastling = new Castling();
        BlackCastling = new Castling();
    }

    public Chessboard(Chessboard board)
    {
        board.CloneTo(this);
    }

    public Piece GetPiece(int file, int rank)
    {
        if ('a' <= file && file <= 'h')
        {
            if (1 <= rank && rank <= 8)
            {
                return Board[(file - 'a') + (8 * (rank - 1))];
            }
        }
        return Piece.Null;
    }

    public void SetPiece(int file, int rank, Piece piece = Piece.None)
    {
        if ('a' <= file && file <= 'h')
        {
            if (1 <= rank && rank <= 8)
            {
                Board[(file - 'a') + (8 * (rank - 1))] = piece;
            }
        }
    }

    public void Move(Move move)
    {
        Piece piece = GetPiece(move.StartSquare.File, move.StartSquare.Rank);
        SetPiece(move.StartSquare.File, move.StartSquare.Rank);
        SetPiece(move.TargetSquare.File, move.TargetSquare.Rank, piece);
        SideToMove = SideToMove == Side.White ? Side.Black : Side.White;
    }

    public void DisplayBoard()
    {
        Dictionary<Piece, char> symbolFromPiece = new Dictionary<Piece, char>()
        {
            [Piece.None] = ' ',
            [Piece.WhiteKing] = 'K',
            [Piece.WhitePawn] = 'P',
            [Piece.WhiteBishop] = 'B',
            [Piece.WhiteKnight] = 'N',
            [Piece.WhiteRook] = 'R',
            [Piece.WhiteQueen] = 'Q',
            [Piece.BlackKing] = 'k',
            [Piece.BlackPawn] = 'p',
            [Piece.BlackBishop] = 'b',
            [Piece.BlackKnight] = 'n',
            [Piece.BlackRook] = 'r',
            [Piece.BlackQueen] = 'q'
        };

        for (int rank = 8; rank >= 1; rank--)
        {
            for (char file = 'a'; file <= 'h'; file++)
            {
                Console.Write(symbolFromPiece[GetPiece(file, rank)]);
            }
            Console.WriteLine("");
        }
        Console.WriteLine("--------");

    }
    public static string GetSquareColour(Position pos)
    {
        if ((pos.Rank + pos.File) % 2 == 0)
        {
            return "dark";
        }
        else
        {
            return "light";
        }
    }
}

public class Castling
{
    public bool KingSide { get; set; }
    public bool QueenSide { get; set; }
}