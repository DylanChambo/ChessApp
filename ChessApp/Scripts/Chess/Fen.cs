using System.Drawing;

namespace ChessApp.Scripts.Chess;

public class Fen
{
    const string DefaultFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    public static Dictionary<char, Pieces> pieceFromSymbol = new Dictionary<char, Pieces>()
    {
        ['K'] = Pieces.WhiteKing,
        ['P'] = Pieces.WhitePawn,
        ['B'] = Pieces.WhiteBishop,
        ['N'] = Pieces.WhiteKnight,
        ['R'] = Pieces.WhiteRook,
        ['Q'] = Pieces.WhiteQueen,
        ['k'] = Pieces.BlackKing,
        ['p'] = Pieces.BlackPawn,
        ['b'] = Pieces.BlackBishop,
        ['n'] = Pieces.BlackKnight,
        ['r'] = Pieces.BlackRook,
        ['q'] = Pieces.BlackQueen
    };
    public static void PopulateBoardFromFen(Board board, string fen = DefaultFen)
    {

        string[] fenSplit = fen.Split(' ');

        board.ResetBoard();

        string fenBoard = fenSplit[0];

        int pos = (int)Position.a8;
        foreach (char symbol in fenBoard)
        {
            if (symbol == '/')
            {
                pos -= 18;
            }
            else if (char.IsDigit(symbol))
            {
                pos += (char)char.GetNumericValue(symbol);
            }
            else
            {
                board.Squares[pos] = pieceFromSymbol[symbol];
                pos++;
            }
        }

        board.Side = fenSplit[1] == "w" ? Sides.White : Sides.Black;

        string castlingRights = fenSplit.Length > 2 ? fenSplit[2] : "KQkq";
        board.CastlePerm |= castlingRights.Contains('K') ? Castling.WhiteKingSide : 0;
        board.CastlePerm |= castlingRights.Contains('Q') ? Castling.WhiteQueenSide : 0;
        board.CastlePerm |= castlingRights.Contains('k') ? Castling.BlackKingSide : 0;
        board.CastlePerm |= castlingRights.Contains('q') ? Castling.BlackQueenSide : 0;

        board.EnPassantSquare = Position.No_Square;

        if (fenSplit.Length > 3 && fenSplit[3] != "-")
        {
            int file = fenSplit[3][0] - 'a';
            int rank = fenSplit[3][1] - '1';
            board.EnPassantSquare = (Position)Conversion.ConvertFRTo120(file, rank);
        }

        //if (fenSplit.Length > 4)
        //{
        //    board.FiftyMoveCount = int.Parse(fenSplit[4]);
        //}
        //if (fenSplit.Length > 5)
        //{
        //    board.FullmoveCount = int.Parse(fenSplit[5]);
        //}

        board.HashKey = HashKey.GenerateHashKey(board);
    }
}
