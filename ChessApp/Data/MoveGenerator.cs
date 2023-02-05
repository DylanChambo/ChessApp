using ChessApp.Shared.ChessBoard;
using System.Drawing;
using System.IO.Pipelines;

namespace ChessApp.Data
{
    public class MoveGenerator
    {
        public static void GenerateMoves(Chessboard board)
        {
            board.Moves = new List<Move>();
            for (int rank = 1; rank <= 8; rank++)
            {
                for (char file = 'a'; file <= 'h'; file++)
                {
                    Piece piece = board.GetPiece(file, rank);
                    Side side;
                    if (IsWhite(piece) && board.SideToMove == Side.White)
                    {
                        side = Side.White;
                    }
                    else if (IsBlack(piece) && board.SideToMove == Side.Black)
                    {
                        side = Side.Black;
                    }
                    else
                    {
                        continue;
                    }

                    if (piece == Piece.WhitePawn || piece == Piece.BlackPawn)
                    {
                       PawnMove(file, rank, side, board);
                    }
                    else if (piece == Piece.WhiteKnight || piece == Piece.BlackKnight)
                    {
                        KnightMove(file, rank, side, board);
                    }
                    else if (piece == Piece.WhiteBishop || piece == Piece.BlackBishop)
                    {
                        BishopMove(file, rank, side, board);
                    }
                    else if (piece == Piece.WhiteRook || piece == Piece.BlackRook)
                    {
                        RookMove(file, rank, side, board);
                    }
                    else if (piece == Piece.WhiteQueen || piece == Piece.BlackQueen)
                    {
                        BishopMove(file, rank, side, board);
                        RookMove(file, rank, side, board);
                    }
                    else if (piece == Piece.WhiteKing || piece == Piece.BlackKing)
                    {
                        KingMove(file, rank, side, board);
                    }
                }
            }

            


        }

        public static bool IsOpposition(Piece piece, Side side)
        {
            return side == Side.White ? IsBlack(piece) : IsWhite(piece);
        }

        public static bool IsBlack(Piece piece)
        {
            return (Piece.BlackKing <= piece && piece <= Piece.BlackQueen);
        }

        public static bool IsWhite(Piece piece)
        {
            return (Piece.WhiteKing <= piece && piece <= Piece.WhiteQueen);
        }
        public static void PawnMove(char file, int rank, Side side, Chessboard board)
        {
            // TODO: add en passant
            List<Move> moves = new List<Move>();
            int direction = side == Side.White ? 1 : -1;

            // Check if they can move forward
            if (board.GetPiece(file, rank + direction) == Piece.None)
            {
                board.Moves.Add(new Move(new Position(file, rank), new Position(file, rank + direction)));
                // Double moves on first turn
                if (board.GetPiece(file, rank + direction * 2) == Piece.None && ((side == Side.White && rank == 2) || (side == Side.Black && rank == 7)))
                {
                    board.Moves.Add(new Move(new Position(file, rank), new Position(file, rank + direction * 2)));
                }
            }

            // Check if they can move forward attacking kingside
            if (IsOpposition(board.GetPiece(file + 1, rank + direction), side))
            {
                board.Moves.Add(new Move(new Position(file, rank), new Position((char)(file + 1), rank + direction)));
            }

            // Check if they can move forward attacking queenside
            if (IsOpposition(board.GetPiece(file - 1, rank + direction), side))
            {
                board.Moves.Add(new Move(new Position(file, rank), new Position((char)(file - 1), rank + direction)));
            }
        }

        public static void KnightMove(char file, int rank, Side side, Chessboard board)
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
                            board.Moves.Add(new Move(new Position(file, rank), new Position((char)(file + f), rank + r)));
                        }
                    }
                }
            }
        }

        public static void BishopMove(char file, int rank, Side side, Chessboard board)
        {
            for (int i = -1; i <= 1; i += 2)
            {
                for (int j = -1; j <= 1; j += 2)
                {
                    int k = 1;
                    while (true)
                    {
                        Piece piece = board.GetPiece(file + k * i, rank + k * j);
                        if (piece == Piece.None)
                        {
                            board.Moves.Add(new Move(new Position(file, rank), new Position((char)(file + k * i), rank + k * j)));

                        }
                        else if (IsOpposition(piece, side))
                        {
                            board.Moves.Add(new Move(new Position(file, rank), new Position((char)(file + k * i), rank + k * j)));
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

        public static void RookMove(char file, int rank, Side side, Chessboard board)
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
                        if (piece == Piece.None)
                        {
                            board.Moves.Add(new Move(new Position(file, rank), new Position((char)(file + (k * j * i)), rank + (k * j * l))));
                        }
                        else if (IsOpposition(piece, side))
                        {
                            board.Moves.Add(new Move(new Position(file, rank), new Position((char)(file + (k * j * i)), rank + (k * j * l))));
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

        public static void KingMove(char file, int rank, Side side, Chessboard board)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) { continue; }
                    Piece piece = board.GetPiece(file + i, rank + j);
                    if (piece == Piece.None || IsOpposition(piece, side))
                    {
                        board.Moves.Add(new Move(new Position(file, rank), new Position((char)(file + i), rank + j)));
                    }
                }
            }
        }

    }
}
