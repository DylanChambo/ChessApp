using BlazorState;
using ChessApp.Data;

namespace ChessApp.Features.Chess;

public partial class ChessState: State<ChessState>
{
    public static readonly string DefaultFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
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

    private Piece[] Board { get; set; }
    public Boolean IsFlipped { get; private set; }

    public bool Mobile { get; set; }
    public Position MovingPositon { get; private set; }
    public Coord MousePos { get; private set; }
    public override void Initialize()
    {
        Board = new Piece[64];
        LoadFenPosition(DefaultFen);
        IsFlipped = false;
        MovingPositon = new Position('0', 0);
        MousePos = new Coord(0, 0);
    }

    public Piece GetPiece(int file, int rank)
    {  
        return Board[(file - 'a') + (8 * (rank - 1))];
    }

    public void SetPiece(int file, int rank, Piece piece = Piece.None)
    {
        Board[(file - 'a') + (8 * (rank - 1))] = piece;
    }

    public void LoadFenPosition(string fen)
    {
        Board = new Piece[64];
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

        string fenBoard = fen.Split(' ')[0];
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
                SetPiece(file, rank, pieceFromSymbol[symbol]);
                file++;
            }
        }
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
}
