using System.Diagnostics;

namespace ChessApp.Scripts.Chess;

public class Board
{
    public const int VirtualBoardSize = 120;
    public const int BoardSize = 64;
    public const int MaxGameMoves = 2048;
    public Pieces[] Squares { get; set; }
    public BitBoard[] Pawns { get; set; }
    public Position[] KingSquare { get; set; }
    public Sides Side { get; set; }
    public Position EnPassantSquare { get; set; }
    public int FiftyMoveCount { get; set; }
    public int Ply { get; set; }
    public int HisPly { get; set; }
    public int CastlePerm { get; set; }
    public ulong HashKey { get; set; }
    public int[] PieceNum { get; set; }
    public int[] BigPiece { get; set; }
    public int[] MajorPiece { get; set; }
    public int[] MinorPiece { get; set; }
    public int[] Material { get; set; }
    public Undo[] History { get; set; }
    public Position[,] PieceList { get; set; }

    public Board()
    {
        Squares = new Pieces[VirtualBoardSize];
        Pawns = new BitBoard[3];
        KingSquare = new Position[2];
        PieceNum = new int[13];
        BigPiece = new int[2];
        MajorPiece = new int[2];
        MinorPiece = new int[2];
        Material = new int[2];
        History = new Undo[MaxGameMoves];
        PieceList = new Position[13, 10];
        Fen.PopulateBoardFromFen(this);
        UpdateMaterialLists();
    }

    public Board(string FenString) : this()
    {
        Fen.PopulateBoardFromFen(this, FenString);
        UpdateMaterialLists();
    }

    public void ResetBoard()
    {
        int i;
        for (i = 0; i < VirtualBoardSize; i++)
        {
            Squares[i] = Pieces.Offboard;
        }

        for (i = 0; i < BoardSize; i++)
        {
            Squares[Conversion.Square64To120[i]] = Pieces.None;
        }

        for (i = 0; i < 2; i++)
        {
            BigPiece[i] = 0;
            MajorPiece[i] = 0;
            MinorPiece[i] = 0;
        }
        for (i = 0; i < 3; i++)
        {
            Pawns[i].Reset();
        }
        for (i = 0; i < 13; i++)
        {
            PieceNum[i] = 0;
        }

        KingSquare[(int)Sides.White] = KingSquare[(int)Sides.Black] = Position.No_Square;
        Side = Sides.Both;
        EnPassantSquare = Position.No_Square;
        FiftyMoveCount = 0;
        Ply = 0;
        HisPly = 0;
        CastlePerm = 0;
        HashKey = 0UL;
    }

    public void UpdateMaterialLists()
    {
        for (int i = 0; i < VirtualBoardSize; i++)
        {
            Pieces piece = Squares[i];
            if (piece != Pieces.Offboard && piece != Pieces.None)
            {
                Sides side = Data.PieceColour[(int)piece];
                if (Data.PieceBig[(int)piece])
                {
                    BigPiece[(int)side]++;
                }
                if (Data.PieceMajor[(int)piece])
                {
                    MajorPiece[(int)side]++;
                }
                if (Data.PieceMinor[(int)piece])
                {
                    MinorPiece[(int)side]++;
                }

                Material[(int)side] += Data.PieceValue[(int)piece];

                // Set Piece List and increment
                PieceList[(int)piece, PieceNum[(int)piece]] = (Position)i;
                PieceNum[(int)piece]++;

                if (piece == Pieces.WhiteKing)
                {
                    KingSquare[(int)Sides.White] = (Position)i;
                }
                else if (piece == Pieces.BlackKing)
                {
                    KingSquare[(int)Sides.Black] = (Position)i;
                }

                // Set Pawns Bitboard
                if (piece == Pieces.WhitePawn)
                {
                    Pawns[(int)Sides.White].SetBit(Conversion.Square120To64[i]);
                    Pawns[(int)Sides.Both].SetBit(Conversion.Square120To64[i]);
                }
                else if (piece == Pieces.BlackPawn)
                {
                    Pawns[(int)Sides.Black].SetBit(Conversion.Square120To64[i]);
                    Pawns[(int)Sides.Both].SetBit(Conversion.Square120To64[i]);
                }
            }
        }
    }

    public void CheckBoard()
    {
        int[] pieceNum = new int[13];
        int[] bigPiece = new int[2];
        int[] majorPiece = new int[2];
        int[] minorPiece = new int[2];
        int[] material = new int[2];

        BitBoard[] pawns = new BitBoard[3];

        pawns[(int)Sides.White] = Pawns[(int)Sides.White];
        pawns[(int)Sides.Black] = Pawns[(int)Sides.Black];
        pawns[(int)Sides.Both] = Pawns[(int)Sides.Both];
    }

    public bool IsSquareAttacked(Position pos, Sides side)
    {
        // Check Pawns
        if (side == Sides.White)
        {
            if (Squares[(int)pos - 11] == Pieces.WhitePawn || Squares[(int)pos - 9] == Pieces.WhitePawn)
            {
                return true;
            }
        } else
        {
            if (Squares[(int)pos + 11] == Pieces.BlackPawn || Squares[(int)pos + 9] == Pieces.BlackPawn)
            {
                return true;
            }
        }

        // Check Knights
        for(int i = 0; i <= Data.KnightDirection.Length; i++)
        {
            Pieces piece = Squares[(int)pos + Data.KnightDirection[i]];
            if (Data.IsPieceKnight[(int)piece] && Data.PieceColour[(int)piece] == side)
            {
                return true;
            }
        }

        // Check Rooks and Queens
        for (int i = 0; i <= Data.RookDirection.Length; i++)
        {
            int direction = Data.RookDirection[i];
            int square = (int)pos + direction;
            Pieces piece = Squares[square];
            while (piece != Pieces.Offboard)
            {
                if (piece != Pieces.None)
                {
                    if (Data.IsPieceRookQueen[(int)piece] && Data.PieceColour[(int)piece] == side)
                    {
                        return true;
                    }
                    break;
                }
                square += direction;
                piece = Squares[square];
            } 
        }

        // Check Bishop and Queens
        for (int i = 0; i <= Data.BishopDirection.Length; i++)
        {
            int direction = Data.BishopDirection[i];
            int square = (int)pos + direction;
            Pieces piece = Squares[square];
            while (piece != Pieces.Offboard)
            {
                if (piece != Pieces.None)
                {
                    if (Data.IsPieceBishopQueen[(int)piece] && Data.PieceColour[(int)piece] == side)
                    {
                        return true;
                    }
                    break;
                }
                square += direction;
                piece = Squares[square];
            }
        }

        // Check Kings
        for (int i = 0; i <= Data.KingDirection.Length; i++)
        {
            Pieces piece = Squares[(int)pos + Data.KingDirection[i]];
            if (Data.IsPieceKing[(int)piece] && Data.PieceColour[(int)piece] == side)
            {
                return true;
            }
        }

        return false;
    }
}
