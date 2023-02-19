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
    public ulong PositionKey { get; set; }
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
            Material[i] = 0;
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
        PositionKey = 0UL;
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
        for(int i = 0; i < Data.KnightDirection.Length; i++)
        {
            Pieces piece = Squares[(int)pos + Data.KnightDirection[i]];
            if (Data.IsPieceKnight[(int)piece] && Data.PieceColour[(int)piece] == side)
            {
                return true;
            }
        }

        // Check Rooks and Queens
        for (int i = 0; i < Data.RookDirection.Length; i++)
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
        for (int i = 0; i < Data.BishopDirection.Length; i++)
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
        for (int i = 0; i < Data.KingDirection.Length; i++)
        {
            Pieces piece = Squares[(int)pos + Data.KingDirection[i]];
            if (Data.IsPieceKing[(int)piece] && Data.PieceColour[(int)piece] == side)
            {
                return true;
            }
        }

        return false;
    }

    public void ClearPiece(Position pos) 
    {
        Pieces piece = Squares[(int)pos];
        PositionKey ^= HashKey.PieceKeys[(int)piece, (int)pos];
        Squares[(int)pos] = Pieces.None;

        Sides side = Data.PieceColour[(int)piece];
        Material[(int)side] -= Data.PieceValue[(int)piece];

        if (Data.PieceBig[(int)piece])
        {
            BigPiece[(int)side]--;
            if (Data.PieceMajor[(int)piece])
            {
                MajorPiece[(int)side]--;
            } else
            {
                MinorPiece[(int)side]--;
            }
        } else
        {
            Pawns[(int)side].ClearBit(Conversion.Square120To64[(int)pos]);
            Pawns[(int)Sides.Both].ClearBit(Conversion.Square120To64[(int)pos]);
        }
        // Update PieceList
        int i;
        for (i = 0; PieceList[(int)piece, i] != pos; i++);
        PieceNum[(int)piece]--;
        // Replace the removed item with the last one in the array
        PieceList[(int)piece, i] = PieceList[(int)piece, PieceNum[(int)piece]];
    }

    public void AddPiece(Position pos, Pieces piece)
    {
        PositionKey ^= HashKey.PieceKeys[(int)piece, (int)pos];
        Squares[(int)pos] = piece;

        Sides side = Data.PieceColour[(int)piece];

        if (Data.PieceBig[(int)piece])
        {
            BigPiece[(int)side]++;
            if (Data.PieceMajor[(int)piece])
            {
                MajorPiece[(int)side]++;
            }
            else
            {
                MinorPiece[(int)side]++;
            }
        }
        else
        {
            Pawns[(int)side].SetBit(Conversion.Square120To64[(int)pos]);
            Pawns[(int)Sides.Both].SetBit(Conversion.Square120To64[(int)pos]);
        }
        Material[(int)side] += Data.PieceValue[(int)piece];
        PieceList[(int)piece, PieceNum[(int)piece]++] = pos;
    }

    public void MovePiece(Position from, Position to)
    {
        Pieces piece = Squares[(int)from];
        Sides side = Data.PieceColour[(int)piece];

        PositionKey ^= HashKey.PieceKeys[(int)piece, (int)from];
        Squares[(int)from] = Pieces.None;

        PositionKey ^= HashKey.PieceKeys[(int)piece, (int)to];
        Squares[(int)to] = piece;

        if (!Data.PieceBig[(int)piece])
        { 
            Pawns[(int)side].SetBit(Conversion.Square120To64[(int)from]);
            Pawns[(int)Sides.Both].SetBit(Conversion.Square120To64[(int)from]);
            Pawns[(int)side].SetBit(Conversion.Square120To64[(int)to]);
            Pawns[(int)Sides.Both].SetBit(Conversion.Square120To64[(int)to]);
        }

        for (int i = 0; i < PieceNum[(int)piece]; i++)
        {
            if (PieceList[(int)piece, i] == from)
            {
                PieceList[(int)piece, i] = to;
                break;
            }
        }
    }

    public bool MakeMove(int move)
    {
        Position from = (Position) Move.From(move);
        Position to = (Position) Move.To(move);

        History[HisPly].PositionKey = PositionKey;

        // EnPasssant
        if (Move.IsEnPassant(move))
        {
            if (Side == Sides.White)
            {
                ClearPiece(to - 10);
            } else
            {
                ClearPiece(to + 10);
            }
        }
        // Castling
        else if (Move.IsCastle(move))
        {
            switch(to)
            {
                case Position.c1:
                    MovePiece(Position.a1, Position.d1);
                    break;
                case Position.c8:
                    MovePiece(Position.a8, Position.d8);
                    break;
                case Position.g1:
                    MovePiece(Position.h1, Position.f1);
                    break;
                case Position.g8:
                    MovePiece(Position.h8, Position.f8);
                    break;
                default:
                    break;
            }
        }
        if (EnPassantSquare != Position.No_Square)
        {
            PositionKey ^= HashKey.PieceKeys[(int)Pieces.None, (int)EnPassantSquare];
        }
        PositionKey ^= HashKey.CastleKey[CastlePerm];

        History[HisPly].Move = move;
        History[HisPly].FiftyMove = FiftyMoveCount;
        History[HisPly].EnPassant = EnPassantSquare;
        History[HisPly].CastlePerm = CastlePerm;

        CastlePerm &= Data.CastlePerm[(int)from];
        CastlePerm &= Data.CastlePerm[(int)to];
        EnPassantSquare = Position.No_Square;

        PositionKey ^= HashKey.CastleKey[CastlePerm];

        Pieces captured = (Pieces)Move.Captured(move);
        FiftyMoveCount++;

        if (captured != Pieces.None)
        {
            ClearPiece(to);
            FiftyMoveCount = 0;
        }

        HisPly++;
        Ply++;

        if (Data.IsPiecePawn[(int)Squares[(int)from]])
        {
            FiftyMoveCount = 0;
            if (Move.IsPawnStart(move))
            {
                if (Side == Sides.White)
                { 
                    EnPassantSquare = from + 10;

                } else
                {
                    EnPassantSquare = from - 10;
                }
                PositionKey ^= HashKey.PieceKeys[(int)Pieces.None, (int)EnPassantSquare];
            }
        }

        MovePiece(from, to);
        Pieces promotedPiece = (Pieces)Move.Promoted(move);
        if (promotedPiece != Pieces.None)
        {
            ClearPiece(to);
            AddPiece(to, promotedPiece);
        }

        if (Data.IsPieceKing[(int)Squares[(int)to]])
        {
            KingSquare[(int)Side] = to;
        }

        Sides currentSide = Side; 
        Side ^= Sides.Black;
        PositionKey ^= HashKey.SideKey;

        if (IsSquareAttacked(KingSquare[(int)currentSide], Side))
        {
            TakeMove();
            return false;
        }
        return true;
    }

    public void TakeMove()
    {
        HisPly--;
        Ply--;
        int move = History[HisPly].Move;
        Position from = (Position) Move.From(move);
        Position to = (Position) Move.To(move);

        PositionKey = History[HisPly].PositionKey;
        CastlePerm = History[HisPly].CastlePerm;
        FiftyMoveCount = History[HisPly].FiftyMove;
        EnPassantSquare = History[HisPly].EnPassant;

        Side ^= Sides.Black;

        // EnPasssant
        if (Move.IsEnPassant(move))
        {
            if (Side == Sides.White)
            {
                AddPiece(to - 10, Pieces.BlackPawn);
            }
            else
            {
                AddPiece(to + 10, Pieces.WhitePawn);
            }
        }
        // Castling
        else if (Move.IsCastle(move))
        {
            switch (to)
            {
                case Position.c1:
                    MovePiece(Position.d1, Position.a1);
                    break;
                case Position.c8:
                    MovePiece(Position.d8, Position.a8);
                    break;
                case Position.g1:
                    MovePiece(Position.f1, Position.h1);
                    break;
                case Position.g8:
                    MovePiece(Position.f8, Position.h8);
                    break;
                default:
                    break;
            }
        }

        MovePiece(to, from);

        if (Data.IsPieceKing[(int)Squares[(int)from]])
        {
            KingSquare[(int)Side] = from;
        }

        Pieces captured = (Pieces)Move.Captured(move);
        if (captured != Pieces.None)
        {
            AddPiece(to, captured);
        }
        if (Move.IsPromotion(move))
        {
            ClearPiece(from);
            AddPiece(from, Side == Sides.White ? Pieces.WhitePawn: Pieces.BlackPawn);
        }
    }
}
