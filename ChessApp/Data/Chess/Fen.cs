using System.Drawing;

namespace ChessApp.Data.Chess;

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

        int pos = (int) Position.A8;
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

        //board.SideToMove = fenSplit[1] == "w" ? Side.White : Side.Black;

        //string castlingRights = (fenSplit.Length > 2) ? fenSplit[2] : "KQkq";
        //board.WhiteCastling.KingSide = castlingRights.Contains('K');
        //board.WhiteCastling.QueenSide = castlingRights.Contains('Q');
        //board.BlackCastling.KingSide = castlingRights.Contains('k');
        //board.BlackCastling.QueenSide = castlingRights.Contains('q');

        //board.epFile = '-';
        //if (fenSplit.Length > 3 && fenSplit[3] != "-")
        //{
        //    board.epFile = fenSplit[3][0];
        //}

        //if (fenSplit.Length > 4)
        //{
        //    board.HalfmoveClock = int.Parse(fenSplit[4]);
        //}
        //if (fenSplit.Length > 4)
        //{
        //    board.FullmoveCount = int.Parse(fenSplit[5]);
        //}
        
    }
}
