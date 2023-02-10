using ChessApp.Shared.ChessBoard;
using System.Drawing;
using System.IO.Pipelines;

namespace ChessApp.Data;

public class MoveGenerator
{
    public static void GenerateMoves(Chessboard board, bool checking = false)
    {
        board.Moves = new List<Move>();
        board.Check = false;

        for (int rank = 1; rank <= 8; rank++)
        {
            for (char file = 'a'; file <= 'h'; file++)
            {
                Piece piece = board.GetPiece(file, rank);
                Side side;
                if (PieceUtils.IsWhite(piece) && board.SideToMove == Side.White)
                {
                    side = Side.White;
                }
                else if (PieceUtils.IsBlack(piece) && board.SideToMove == Side.Black)
                {
                    side = Side.Black;
                }
                else
                {
                    continue;
                }

                if (piece == Piece.WhitePawn || piece == Piece.BlackPawn)
                {
                    PawnMove(file, rank, side, board, checking);
                }
                else if (piece == Piece.WhiteKnight || piece == Piece.BlackKnight)
                {
                    KnightMove(file, rank, side, board, checking);
                }
                else if (piece == Piece.WhiteBishop || piece == Piece.BlackBishop)
                {
                    BishopMove(file, rank, side, board, checking);
                }
                else if (piece == Piece.WhiteRook || piece == Piece.BlackRook)
                {
                    RookMove(file, rank, side, board, checking);
                }
                else if (piece == Piece.WhiteQueen || piece == Piece.BlackQueen)
                {
                    BishopMove(file, rank, side, board, checking);
                    RookMove(file, rank, side, board, checking);
                }
                else if (piece == Piece.WhiteKing || piece == Piece.BlackKing)
                {
                    KingMove(file, rank, side, board, checking);
                }
            }
        }
    }

    public static void AddMove(Piece piece, Move move, Chessboard board, bool checking, Piece pawnPiece = Piece.None)
    {

        if (checking)
        {
            if (PieceUtils.IsKing(piece))
            {
                board.Check = true;
            }
        } else
        {
            if (NotCheck(board, move))
            {
                
                if ((pawnPiece == Piece.WhitePawn && move.TargetSquare.Rank == 8) || (pawnPiece == Piece.BlackPawn && move.TargetSquare.Rank == 1))
                {
                    move.MoveFlag = MoveFlag.PromoteToQueen;
                    board.Moves.Add(move);
                    move.MoveFlag = MoveFlag.PromoteToKnight;
                    board.Moves.Add(move);
                    move.MoveFlag = MoveFlag.PromoteToRook;
                    board.Moves.Add(move);
                    move.MoveFlag = MoveFlag.PromoteToBishop;
                    board.Moves.Add(move);
                }
                else
                {
                    board.Moves.Add(move);
                }
            }
        }
    }

    public static MoveFlag CanEnPassant(char file, int rank, Side side, Chessboard board)
    {
        // Check for En Passant
        if (side == Side.White)
        {
            if (rank == 5 && board.epFile == file)
            {
                return MoveFlag.EnPassant;
            }
        } else
        {
            if (rank == 4 && board.epFile == file)
            {
                return MoveFlag.EnPassant;
            }
        }
        return MoveFlag.None; ;
    }

    public static void PawnMove(char file, int rank, Side side, Chessboard board, bool checking = false)
    {
        // TODO: add en passant
        int direction = side == Side.White ? 1 : -1;
        Piece piece;
        Move move;

        // Check if they can move forward
        piece = board.GetPiece(file, rank + direction);
        if (piece == Piece.None && !checking)
        {
            move = new Move(new Position(file, rank), new Position(file, rank + direction));
            if (NotCheck(board, move))
            {
                if ((side == Side.White && rank + direction == 8) || (side == Side.Black && rank + direction == 1))
                {
                    move.MoveFlag = MoveFlag.PromoteToQueen;
                    board.Moves.Add(move);
                    move.MoveFlag = MoveFlag.PromoteToKnight;
                    board.Moves.Add(move);
                    move.MoveFlag = MoveFlag.PromoteToRook;
                    board.Moves.Add(move);
                    move.MoveFlag = MoveFlag.PromoteToBishop;
                    board.Moves.Add(move);
                } else
                { 
                    board.Moves.Add(move);
                }
            }
            // Double moves on first turn
            piece = board.GetPiece(file, rank + direction * 2);
            if (piece == Piece.None && ((side == Side.White && rank == 2) || (side == Side.Black && rank == 7)))
            {
                move = new Move(new Position(file, rank), new Position(file, rank + direction * 2), MoveFlag.PawnDoubleMove);
                if (NotCheck(board, move))
                {
                    board.Moves.Add(move);
                }
            }
        }

        // Check if they can move forward attacking kingside
        MoveFlag flag = CanEnPassant((char)(file + 1), rank, side, board);
        Piece pawnPiece = board.GetPiece(file, rank);
        piece = board.GetPiece(file + 1, rank + direction);
        if (IsOpposition(piece, side) || flag == MoveFlag.EnPassant)
        {
            move = new Move(new Position(file, rank), new Position((char)(file + 1), rank + direction), flag);
            AddMove(piece, move, board, checking, pawnPiece);
        }

        // Check if they can move forward attacking queenside
        flag = CanEnPassant((char)(file - 1), rank, side, board);
        piece = board.GetPiece(file - 1, rank + direction);
        if (IsOpposition(piece, side) || flag == MoveFlag.EnPassant)
        {
            move = new Move(new Position(file, rank), new Position((char)(file - 1), rank + direction), flag);
            AddMove(piece, move, board, checking, pawnPiece);
        }


    }

    public static void KnightMove(char file, int rank, Side side, Chessboard board, bool checking = false)
    {
        for (int k = 1; k <= 2; k++)
        {
            for (int i = -1; i <= 1; i += 2)
            {
                for (int j = -1; j <= 1; j += 2)
                {
                    int r = k == 1 ? 2 * i : 1 * i;
                    int f = k * j;
                    Piece piece = board.GetPiece(file + f, rank + r);
                    if (piece == Piece.None || IsOpposition(piece, side))
                    {
                        Move move = new Move(new Position(file, rank), new Position((char)(file + f), rank + r));
                        AddMove(piece, move, board, checking);
                    }
                }
            }
        }
    }

    public static void BishopMove(char file, int rank, Side side, Chessboard board, bool checking = false)
    {
        for (int i = -1; i <= 1; i += 2)
        {
            for (int j = -1; j <= 1; j += 2)
            {
                int k = 1;
                while (true)
                {
                    Piece piece = board.GetPiece(file + k * i, rank + k * j);
                    if (piece == Piece.None || IsOpposition(piece, side))
                    {
                        Move move = new Move(new Position(file, rank), new Position((char)(file + k * i), rank + k * j));
                        AddMove(piece, move, board, checking);
                        if (piece != Piece.None) { break; }
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

    public static void RookMove(char file, int rank, Side side, Chessboard board, bool checking = false)
    {
        for (int i = 0; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j += 2)
            {
                int k = 1;
                while (true)
                {
                    int l = 1 - i;
                    Piece piece = board.GetPiece(file + (k * j * i), rank + (k * j * l));
                    if (piece == Piece.None || IsOpposition(piece, side))
                    {
                        Move move = new Move(new Position(file, rank), new Position((char)(file + (k * j * i)), rank + (k * j * l)));
                        AddMove(piece, move, board, checking);
                        if (piece != Piece.None) { break; }
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

    public static void KingMove(char file, int rank, Side side, Chessboard board, bool checking = false)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) { continue; }
                Piece piece = board.GetPiece(file + i, rank + j);
                if (piece == Piece.None || IsOpposition(piece, side))
                {
                    Move move = new Move(new Position(file, rank), new Position((char)(file + i), rank + j));
                    AddMove(piece, move, board, checking);
                }
            }
        }



        // Check for Castling
        CheckCastling(board);
        int backRank = side == Side.White ? 1 : 8;
        bool canCastle = side == Side.White ? board.WhiteCastling.KingSide : board.BlackCastling.KingSide;
        // Kingside
        if (board.GetPiece('f', backRank) == Piece.None && board.GetPiece('g', backRank) == Piece.None && canCastle)
        {
            Move middleMove = new Move(new Position(file, rank), new Position((char)(file + 1), rank));
            Move move = new Move(new Position(file, rank), new Position((char)(file + 2), rank), MoveFlag.Castling);
            if (NotCheck(board, middleMove) && NotCheck(board, move)) {
                board.Moves.Add(move);
            }
        }
        // Queenside
        canCastle = side == Side.White ? board.WhiteCastling.QueenSide : board.BlackCastling.QueenSide;
        if (board.GetPiece('b', backRank) == Piece.None && board.GetPiece('c', backRank) == Piece.None && board.GetPiece('d', backRank) == Piece.None && canCastle)
        {
            Move middleMove = new Move(new Position(file, rank), new Position((char)(file - 1), rank));
            Move move = new Move(new Position(file, rank), new Position((char)(file - 2), rank), MoveFlag.Castling);
            if (NotCheck(board, middleMove) && NotCheck(board, move))
            {
                board.Moves.Add(move);
            }
        }

    }

    public static void CheckCastling(Chessboard board)
    {
        if (board.GetPiece('e', 1) != Piece.WhiteKing)
        {
            board.WhiteCastling.KingSide = false;
            board.WhiteCastling.QueenSide = false;
        } else
        {
            if (board.GetPiece('h', 1) != Piece.WhiteRook)
            {
                board.WhiteCastling.KingSide = false;
            }
            if (board.GetPiece('a', 1) != Piece.WhiteRook)
            {
                board.WhiteCastling.QueenSide = false;
            }
        }

        if (board.GetPiece('e', 8) != Piece.BlackKing)
        {
            board.BlackCastling.KingSide = false;
            board.BlackCastling.QueenSide = false;
        }
        else
        {
            if (board.GetPiece('h', 8) != Piece.BlackRook)
            {
                board.BlackCastling.KingSide = false;
            }
            if (board.GetPiece('a', 8) != Piece.BlackRook)
            {
                board.BlackCastling.QueenSide = false;
            }
        }
    }

    public static bool NotCheck(Chessboard board)
    {
        MoveGenerator.GenerateMoves(board, true);

        return !board.Check;
    }

    public static bool NotCheck(Chessboard board, Move move)
    {
        board = new Chessboard(board);
        board.Move(move, true);

        return NotCheck(board);
    }
}
