using static System.Collections.Specialized.BitVector32;

namespace ChessApp.Data;

public class FenUtils
{
    private static readonly string DefaultFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

    public static Chessboard GenDefaultBoard()
    {
        return GenBoardFromFen(DefaultFen);
    }

    public static Chessboard GenBoardFromFen(string fen)
    {
        Chessboard board = new Chessboard();
        Dictionary<char, Piece> pieceFromSymbol = new Dictionary<char, Piece>()
        {
            ['K'] = Piece.WhiteKing,
            ['P'] = Piece.WhitePawn,
            ['B'] = Piece.WhiteBishop,
            ['N'] = Piece.WhiteKnight,
            ['R'] = Piece.WhiteRook,
            ['Q'] = Piece.WhiteQueen,
            ['k'] = Piece.BlackKing,
            ['p'] = Piece.BlackPawn,
            ['b'] = Piece.BlackBishop,
            ['n'] = Piece.BlackKnight,
            ['r'] = Piece.BlackRook,
            ['q'] = Piece.BlackQueen
        };
        string[] fenSplit = fen.Split(' ');
        string fenBoard = fenSplit[0];

        board.SideToMove = fenSplit[1] == "w" ? Side.White : Side.Black;

        string castlingRights = (fenSplit.Length > 2) ? fenSplit[2] : "KQkq";
        board.WhiteCastling.KingSide = castlingRights.Contains('K');
        board.WhiteCastling.QueenSide = castlingRights.Contains('Q');
        board.BlackCastling.KingSide = castlingRights.Contains('k');
        board.BlackCastling.QueenSide = castlingRights.Contains('q');

        if (fenSplit.Length > 3 && fenSplit[3] != "-")
        {
            board.epFile = fenSplit[3][0];
        }

        if (fenSplit.Length > 4)
        {
            board.HalfmoveClock = int.Parse(fenSplit[4]);
        }
        if (fenSplit.Length > 4)
        {
            board.FullmoveCount = int.Parse(fenSplit[5]);
        }
        char file = 'a';
        int rank = 8;

        foreach (char symbol in fenBoard)
        {
            if (symbol == '/')
            {
                file = 'a';
                rank--;
            }
            else if (char.IsDigit(symbol))
            {
                file += (char)char.GetNumericValue(symbol);
            }
            else
            {
                board.SetPiece(file, rank, pieceFromSymbol[symbol]);
                file++;
            }
        }

        return board;
    }
}
