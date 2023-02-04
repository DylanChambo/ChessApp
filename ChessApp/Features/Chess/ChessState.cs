using BlazorState;
using ChessApp.Data;
using static ChessApp.Features.Chess.ChessState;

namespace ChessApp.Features.Chess;

public partial class ChessState : State<ChessState>
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

    public Boolean IsFlipped { get; private set; }
    public bool Mobile { get; set; }
    public Coord MousePos { get; private set; }

    private Piece[] Board { get; set; }
    public Position MovingPositon { get; private set; }
    public List<Position>? PiecePossibleMoves { get; set; }

    public Side SideToMove { get; private set; }
    public Castling WhiteCastling;
    public Castling BlackCastling;
    public Position? EnPassantTarget;
    public int HalfmoveClock;
    public int FullmoveCount;

    public override void Initialize()
    {
        Board = new Piece[64];
        LoadFenPosition(DefaultFen);
        IsFlipped = false;
        MovingPositon = new Position('0', 0);
        MousePos = new Coord(0, 0);
        // TODO: Calculate all posible moves into a Dictionary<Position, Position[]>
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
        if ('a' <= file && file <='h')
        {
            if (1 <= rank && rank <= 8)
            {
                Board[(file - 'a') + (8 * (rank - 1))] = piece;
            }
        }
        
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
        string[] fenSplit = fen.Split(' ');
        string fenBoard = fenSplit[0];

        SideToMove = fenSplit[1] == "w" ? Side.White : Side.Black;

        WhiteCastling.KingSide = fenSplit[2].Contains('K');
        WhiteCastling.QueenSide = fenSplit[2].Contains('Q');
        BlackCastling.KingSide = fenSplit[2].Contains('k');
        BlackCastling.QueenSide = fenSplit[2].Contains('q');

        if (fenSplit[3] != "-")
        {
            EnPassantTarget = new Position(fenSplit[3][0], fenSplit[3][0] - '0');
        }

        HalfmoveClock = int.Parse(fenSplit[4]);
        FullmoveCount = int.Parse(fenSplit[5]);

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
    public void SetPieceMoves()
    {
        Piece piece = GetPiece(MovingPositon.File, MovingPositon.Rank);
        if (piece == Piece.WhitePawn || piece == Piece.BlackPawn)
        {
            PiecePossibleMoves = PawnMove(MovingPositon.File, MovingPositon.Rank, piece);
            foreach (Position pos in PiecePossibleMoves)
            {
                Console.WriteLine($"{pos.File}{pos.Rank}");
            }
        }
    }

    public bool IsOpposition(int file, int rank, Side side)
    {
        Piece piece = GetPiece(file, rank);
        if (side == Side.White)
        {
            return (Piece.BlackKing <= piece && piece <= Piece.BlackQueen);
        } else
        {
            return (Piece.WhiteKing <= piece && piece <= Piece.WhiteQueen);
        }
    }

    public List<Position> PawnMove(char file, int rank, Piece piece)
    {
        // TODO add en passant
        List<Position> positions = new List<Position> {};
        int direction;
        Side side;
        if (piece == Piece.WhitePawn)
        {
            direction = 1;
            side = Side.White;

        } else
        {
            direction = -1;
            side = Side.Black;
        }
        
        // Check if they can move forward
        if (GetPiece(file, rank + direction) == Piece.None && GetPiece(file, rank + direction) != Piece.Null)
        {
            positions.Add(new Position(file, rank + direction));
            // Double moves on first turn
            if (side == Side.White && GetPiece(file, rank + direction * 2) == Piece.None && rank == 2)
            {
                positions.Add(new Position(file, rank + direction * 2));
            }
            else if (side == Side.Black && GetPiece(file, rank + direction * 2) == Piece.None && rank == 7)
            {
                positions.Add(new Position(file, rank + direction * 2));
            }
        }

        // Check if they can move forward attacking kingside
        if (IsOpposition(file + 1, rank + direction, side))
        {
            positions.Add(new Position((char)(file + 1), rank + direction));
        }

        // Check if they can move forward attacking queenside
        if (IsOpposition(file - 1, rank + direction, side))
        {
            positions.Add(new Position((char)(file - 1), rank + direction));
        }

        return positions;
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
public struct Castling
{
    public bool KingSide { get; set; }
    public bool QueenSide { get; set; }
}