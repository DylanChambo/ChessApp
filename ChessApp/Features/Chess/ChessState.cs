using BlazorState;
using ChessApp.Data;
using System.IO.Pipelines;
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
    public List<Position> PiecePossibleMoves { get; set; }

    public Side SideToMove { get; private set; }
    public Castling WhiteCastling;
    public Castling BlackCastling;
    public Position? EnPassantTarget;
    public int HalfmoveClock;
    public int FullmoveCount;

    public override void Initialize()
    {
        Board = new Piece[64];
        PiecePossibleMoves = new List<Position> { };
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
    public bool IsOpposition(Piece piece, Side side)
    {
        return side == Side.White ? IsBlack(piece) : IsWhite(piece);
    }

    public bool IsBlack(Piece piece)
    {
        return (Piece.BlackKing <= piece && piece <= Piece.BlackQueen);
    }

    public bool IsWhite(Piece piece)
    {
        return (Piece.WhiteKing <= piece && piece <= Piece.WhiteQueen);
    }
    public void SetPieceMoves()
    {
        Piece piece = GetPiece(MovingPositon.File, MovingPositon.Rank);
        Side side;

        if (IsWhite(piece) && SideToMove == Side.White)
        {
            side = Side.White;
        }
        else if (IsBlack(piece) && SideToMove == Side.Black)
        {
            side = Side.Black;
        } else
        {
            return;
        }

        if (piece == Piece.WhitePawn || piece == Piece.BlackPawn)
        {
            PawnMove(MovingPositon.File, MovingPositon.Rank, side);
        }
        else if (piece == Piece.WhiteKnight || piece == Piece.BlackKnight)
        {
            KnightMove(MovingPositon.File, MovingPositon.Rank, side);
        }
        else if (piece == Piece.WhiteBishop || piece == Piece.BlackBishop)
        {
            BishopMove(MovingPositon.File, MovingPositon.Rank, side);
        }
        else if (piece == Piece.WhiteRook || piece == Piece.BlackRook)
        {
            RookMove(MovingPositon.File, MovingPositon.Rank, side);
        }
        else if (piece == Piece.WhiteQueen || piece == Piece.BlackQueen)
        {
            BishopMove(MovingPositon.File, MovingPositon.Rank, side);
            RookMove(MovingPositon.File, MovingPositon.Rank, side);
        }
        else if (piece == Piece.WhiteKing || piece == Piece.BlackKing)
        {
            KingMove(MovingPositon.File, MovingPositon.Rank, side);
        }
    } 

    public void PawnMove(char file, int rank, Side side)
    {
        // TODO: add en passant
        int direction = side == Side.White ? 1 : -1;
        
        // Check if they can move forward
        if (GetPiece(file, rank + direction) == Piece.None)
        {
            PiecePossibleMoves.Add(new Position(file, rank + direction));
            // Double moves on first turn
            if (GetPiece(file, rank + direction * 2) == Piece.None && ((side == Side.White && rank == 2) || (side == Side.Black && rank == 7))) 
            {
                PiecePossibleMoves.Add(new Position(file, rank + direction * 2));
            }
        }

        // Check if they can move forward attacking kingside
        if (IsOpposition(GetPiece(file + 1, rank + direction), side))
        {
            PiecePossibleMoves.Add(new Position((char)(file + 1), rank + direction));
        }

        // Check if they can move forward attacking queenside
        if (IsOpposition(GetPiece(file - 1, rank + direction), side))
        {
            PiecePossibleMoves.Add(new Position((char)(file - 1), rank + direction));
        }
    }

    public void KnightMove (char file, int rank, Side side)
    {
        for (int k = 1; k <=2; k++)
        {
            for (int i = -1; i <= 1; i += 2)
            {
                for (int j = -1; j <= 1; j += 2)
                {
                    int r = k == 1 ? 2 * i : 1 * i;
                    int f = k * j;
                    Piece piece = GetPiece(file + f, rank + r);
                    if (piece == Piece.None || IsOpposition(piece, side))
                    {
                        PiecePossibleMoves.Add(new Position((char)(file + f), rank + r));
                    }
                }
            }
        }
    }

    public void BishopMove (char file, int rank, Side side)
    {
        for (int i = -1; i <= 1; i += 2)
        {
            for (int j = -1; j <= 1; j += 2)
            {
                int k = 1;
                while (true) {
                    Piece piece = GetPiece(file + k * i, rank + k * j);
                    if (piece == Piece.None)
                    {
                        PiecePossibleMoves.Add(new Position((char)(file + k * i), rank + k * j));

                    } else if (IsOpposition(piece, side)) {
                        PiecePossibleMoves.Add(new Position((char)(file + k * i), rank + k * j));
                        break;
                    } 
                    else { 
                        break;
                    }
                    k++;
                }
               
            }
        }
    }

    public void RookMove(char file, int rank, Side side)
    {

        for (int i = 0; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j += 2)
            {
                int k = 1;
                while (true)
                {
                    int l = 1 - i;
                    Piece piece = GetPiece(file + (k * j * i), rank + (k * j * l));
                    if (piece == Piece.None)
                    {
                        PiecePossibleMoves.Add(new Position((char)(file + (k * j * i)), rank + (k * j * l)));
                    }
                    else if (IsOpposition(piece, side))
                    {
                        PiecePossibleMoves.Add(new Position((char)(file + (k * j * i)), rank + (k * j * l)));
                        break;
                    }
                    else
                    {
                        break;
                    }
                    k++;
                }
            }
        }

    }

    public void KingMove(char file, int rank, Side side)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) { continue; }
                Piece piece = GetPiece(file + i, rank + j);
                if (piece == Piece.None || IsOpposition(piece, side))
                {
                    PiecePossibleMoves.Add(new Position((char)(file + i), rank + j));
                }
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
public struct Castling
{
    public bool KingSide { get; set; }
    public bool QueenSide { get; set; }
}