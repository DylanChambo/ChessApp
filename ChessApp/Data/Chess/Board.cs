using System.Diagnostics;

namespace ChessApp.Data.Chess;

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
        Pawns = new BitBoard[2];
        KingSquare = new Position[2];
        PieceNum = new int[13];
        BigPiece = new int[2];
        MajorPiece = new int[2];
        MinorPiece = new int[2];
        Material = new int[2];
        History = new Undo[MaxGameMoves];
        PieceList = new Position[13,10];
        Fen.PopulateBoardFromFen(this);
    }

    public void ResetBoard()
    {
        int i;
        for(i = 0; i < VirtualBoardSize; i++)
        {
            Squares[i] = Pieces.Offboard;
        }

        for (i = 0; i < BoardSize; i++)
        {
            Squares[Conversion.To120(i)] = Pieces.None;
        }

        for (i = 0; i < 2; i++)
        {
            BigPiece[i] = 0;
            MajorPiece[i] = 0;
            MinorPiece[i] = 0;
            Pawns[i].Reset();
        }

        for (i = 0; i < 13; i++)
        {
            PieceNum[i] = 0;
        }

        KingSquare[(int)Sides.White] = KingSquare[(int)Sides.Black] = Position.No_Square;
        Side = Sides.Both;
        EnPassantSquare = Position.No_Square;
        FiftyMoveCount= 0;
        Ply = 0;
        HisPly= 0;
        CastlePerm = 0;
        HashKey= 0UL;
    }
    
    public void UpdateMaterialLists()
    {
        for (int i = 0; i < VirtualBoardSize; i++)
        {
            Pieces piece = Squares[i];
            if (piece != Pieces.Offboard || piece != Pieces.None )
            {
                Sides side = Data.PieceColour[(int)piece];
                if (Data.PieceBig[(int) piece])
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
                PieceList[(int)piece, PieceNum[(int)piece]] = (Position) i;
                PieceNum[(int)piece]++;

                if (piece == Pieces.WhiteKing)
                {
                    KingSquare[(int)Sides.White] = (Position) i;
                }
                else if (piece == Pieces.BlackKing)
                {
                    KingSquare[(int)Sides.Black] = (Position) i;
                }
            }
        }
    }
}
