using System.Diagnostics;

namespace ChessApp.Scripts.Chess;

public static class MoveGenerator
{
    static readonly Pieces[,] LoopSlidePiece = { { Pieces.WhiteBishop, Pieces.WhiteRook, Pieces.WhiteQueen }, { Pieces.BlackBishop, Pieces.BlackRook, Pieces.BlackQueen } };
    static readonly Pieces[,] LoopNonSlidePiece = { { Pieces.WhiteKnight, Pieces.WhiteKing }, { Pieces.BlackKnight, Pieces.BlackKing } };

    static readonly int[,] PieceDirections = { 
        { 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0 },
        { -8, -19, -21, -12, 8, 19, 21, 12 },
        { -9, -11, 9, 11, 0, 0, 0, 0 },
        {-1, -10, 1, 10, 0, 0, 0, 0 },
        { -1, -10, 1, 10, -9, -11, 9, 11 },
        { -1, -10, 1, 10, -9, -11, 9, 11 },
        { 0, 0, 0, 0, 0, 0, 0, 0 },
        { -8, -19, -21, -12, 8, 19, 21, 12 },
        { -9, -11, 9, 11, 0, 0, 0, 0 },
        {-1, -10, 1, 10, 0, 0, 0, 0 },
        { -1, -10, 1, 10, -9, -11, 9, 11 },
        { -1, -10, 1, 10, -9, -11, 9, 11 }
};

    static readonly int[] PieceDirectionNumber = { 0, 0, 8, 4, 4, 8, 8, 0, 8, 4, 4, 8, 8 };

    public static void GenerateAllMoves(Board board, MoveList list)
    {
        list.count = 0;

        // White Pawn Move Generation
        if (board.Side == Sides.White)
        {
            for (int i = 0; i < board.PieceNum[(int)Pieces.WhitePawn]; i++)
            {
                Position position = board.PieceList[(int)Pieces.WhitePawn, i];
                if (board.Squares[(int)position + 10] == Pieces.None)
                {
                    AddWhitePawnMove(board, (int)position, (int)position + 10, list);
                    if (Data.Ranks[(int)position] == Ranks.r2 && board.Squares[(int)position + 20] == Pieces.None)
                    {
                        AddQuietMove(board, new Move((int)position, (int)position + 20, flag: (int)MoveFlags.PawnStart), list);
                    }
                }
                if (Data.Files[(int)position + 9] != Files.None && Data.PieceColour[(int)board.Squares[(int)position + 9]] == Sides.Black)
                {
                    AddWhitePawnCaptureMove(board, (int)position, (int)position + 9, (int)board.Squares[(int)position + 9], list);
                }
                if (Data.Files[(int)position + 11] != Files.None && Data.PieceColour[(int)board.Squares[(int)position + 11]] == Sides.Black)
                {
                    AddWhitePawnCaptureMove(board, (int)position, (int)position + 11, (int)board.Squares[(int)position + 11], list);
                }
                if (position + 9 == board.EnPassantSquare)
                {
                    AddEnPassantMove(board, new Move((int)position, (int)position + 9, flag: (int)MoveFlags.EnPassant), list);
                }
                if (position + 11 == board.EnPassantSquare)
                {
                    AddEnPassantMove(board, new Move((int)position, (int)position + 11, flag: (int)MoveFlags.EnPassant), list);
                }
            }
            // White Castling Moves
            if ((board.CastlePerm & Castling.WhiteKingSide) != 0)
            {
                if (board.Squares[(int)Position.f1] == Pieces.None && board.Squares[(int)Position.g1] == Pieces.None)
                {
                    if (!board.IsSquareAttacked(Position.e1, Sides.Black) && !board.IsSquareAttacked(Position.f1, Sides.Black))
                    {
                        AddQuietMove(board, new Move((int)Position.e1, (int)Position.g1, flag: (int)MoveFlags.Castle), list);
                    }
                }

            }
            if ((board.CastlePerm & Castling.WhiteQueenSide) != 0)
            {
                if (board.Squares[(int)Position.d1] == Pieces.None && board.Squares[(int)Position.c1] == Pieces.None && board.Squares[(int)Position.b1] == Pieces.None)
                {
                    if (!board.IsSquareAttacked(Position.e1, Sides.Black) && !board.IsSquareAttacked(Position.d1, Sides.Black))
                    {
                        AddQuietMove(board, new Move((int)Position.e1, (int)Position.c1, flag: (int)MoveFlags.Castle), list);
                    }
                }
            }
        }
        else
        // Black Pawn Move Generation
        {
            for (int i = 0; i < board.PieceNum[(int)Pieces.BlackPawn]; i++)
            {
                Position position = board.PieceList[(int)Pieces.BlackPawn, i];
                if (board.Squares[(int)position - 10] == Pieces.None)
                {
                    AddBlackPawnMove(board, (int)position, (int)position - 10, list);
                    if (Data.Ranks[(int)position] == Ranks.r7 && board.Squares[(int)position - 20] == Pieces.None)
                    {
                        AddQuietMove(board, new Move((int)position, (int)position - 20, flag: (int)MoveFlags.PawnStart), list);
                    }
                }
                if (Data.Files[(int)position - 9] != Files.None && Data.PieceColour[(int)board.Squares[(int)position - 9]] == Sides.White)
                {
                    AddBlackPawnCaptureMove(board, (int)position, (int)position - 9, (int)board.Squares[(int)position - 9], list);
                }
                if (Data.Files[(int)position - 11] != Files.None && Data.PieceColour[(int)board.Squares[(int)position - 11]] == Sides.White)
                {
                    AddBlackPawnCaptureMove(board, (int)position, (int)position - 11, (int)board.Squares[(int)position - 11], list);
                }
                if (position - 9 == board.EnPassantSquare)
                {
                    AddEnPassantMove(board, new Move((int)position, (int)position - 9, flag: (int)MoveFlags.EnPassant), list);
                }
                if (position - 11 == board.EnPassantSquare)
                {
                    AddEnPassantMove(board, new Move((int)position, (int)position - 11, flag: (int)MoveFlags.EnPassant), list);
                }
                
                
            }
            // Black Castling Moves
            if ((board.CastlePerm & Castling.BlackKingSide) != 0)
            {
                if (board.Squares[(int)Position.f8] == Pieces.None && board.Squares[(int)Position.g8] == Pieces.None)
                {
                    if (!board.IsSquareAttacked(Position.e8, Sides.White) && !board.IsSquareAttacked(Position.f8, Sides.White))
                    {
                        AddQuietMove(board, new Move((int)Position.e8, (int)Position.g8, flag: (int)MoveFlags.Castle), list);
                    }
                }

            }
            if ((board.CastlePerm & Castling.BlackQueenSide) != 0)
            {
                if (board.Squares[(int)Position.d8] == Pieces.None && board.Squares[(int)Position.c8] == Pieces.None && board.Squares[(int)Position.b8] == Pieces.None)
                {
                    if (!board.IsSquareAttacked(Position.e8, Sides.White) && !board.IsSquareAttacked(Position.d8, Sides.White))
                    {
                        AddQuietMove(board, new Move((int)Position.e8, (int)Position.c8, flag: (int)MoveFlags.Castle), list);
                    }
                }
            }
        }
        // Sliding Pieces Move Generation
        Pieces piece;
        for (int i = 0; i < 3; i++)
        {
            piece = LoopSlidePiece[(int)board.Side, i];
            for (int j = 0; j < board.PieceNum[(int)piece]; j++)
            {
                Position pos = board.PieceList[(int)piece, j];
                for (int k = 0; k < PieceDirectionNumber[(int)piece]; k++)
                {
                    int direction = PieceDirections[(int)piece, k];
                    Position target = pos + direction;

                    while (Data.Files[(int)target] != Files.None)
                    {
                        if (board.Squares[(int)target] != Pieces.None)
                        {
                            if (Data.PieceColour[(int)board.Squares[(int)target]] == (board.Side ^ Sides.Black))
                            {
                                AddCaptureMove(board, new Move((int)pos, (int)target, (int)board.Squares[(int)target]), list);
                            }
                            break;
                        }
                        AddQuietMove(board, new Move((int)pos, (int)target), list);
                        target += direction;
                    }
                }
            }
        }
        // Non Sliding Pieces Move Generation
        for (int i = 0; i < 2; i++)
        {
            piece = LoopNonSlidePiece[(int)board.Side, i];

            for (int j = 0; j < board.PieceNum[(int)piece]; j++)
            {
                Position pos = board.PieceList[(int)piece, j];

                for (int k = 0; k < PieceDirectionNumber[(int)piece]; k++)
                {
                    int direction = PieceDirections[(int)piece, k];
                    Position target = pos + direction;

                    if (Data.Files[(int)target] == Files.None)
                    {
                        continue;
                    }
                    if (board.Squares[(int)target] != Pieces.None)
                    {
                        if (Data.PieceColour[(int)board.Squares[(int)target]] == (board.Side ^ Sides.Black))
                        {
                            AddCaptureMove(board, new Move((int)pos, (int)target, (int)board.Squares[(int)target]), list);
                        }
                        continue;
                    }
                    AddQuietMove(board, new Move((int)pos, (int)target), list);
                }
            }
        }
    }
            
    public static void AddQuietMove(Board board, Move move, MoveList list)
    {
        if (board.SearchKillers[0, board.Ply] == move.move)
        {
            move.score = 900000;
        } else if (board.SearchKillers[1, board.Ply] == move.move)
        {
            move.score = 800000;
        } else
        {
            move.score = board.SearchHistory[(int)board.Squares[Move.From(move.move)], Move.To(move.move)];
        }
        list.moves[list.count] = move;
        list.count++;
    }

    public static void AddCaptureMove(Board board, Move move, MoveList list)
    {
        move.score = Data.MvvLvaScores[Move.Captured(move.move), (int)board.Squares[Move.From(move.move)]] + 1000000;
        list.moves[list.count] = move;
        list.count++;
    }

    public static void AddEnPassantMove(Board board, Move move, MoveList list)
    {
        move.score = 105 + 1000000;
        list.moves[list.count] = move;
        list.count++;
        list.count++;
    }

    public static void AddWhitePawnCaptureMove(Board board, int from, int to, int capture, MoveList list)
    {
        if (Data.Ranks[from] == Ranks.r7)
        {
            AddCaptureMove(board, new Move(from, to, capture, (int)Pieces.WhiteQueen), list);
            AddCaptureMove(board, new Move(from, to, capture, (int)Pieces.WhiteKnight), list);
            AddCaptureMove(board, new Move(from, to, capture, (int)Pieces.WhiteRook), list);
            AddCaptureMove(board, new Move(from, to, capture, (int)Pieces.WhiteBishop), list);
        } else
        {
            AddCaptureMove(board, new Move(from, to, capture), list);
        }
    }

    public static void AddWhitePawnMove(Board board, int from, int to, MoveList list)
    {
        if (Data.Ranks[from] == Ranks.r7)
        {
            AddQuietMove(board, new Move(from, to, promotion: (int)Pieces.WhiteQueen), list);
            AddQuietMove(board, new Move(from, to, promotion: (int)Pieces.WhiteKnight), list);
            AddQuietMove(board, new Move(from, to, promotion: (int)Pieces.WhiteRook), list);
            AddQuietMove(board, new Move(from, to, promotion: (int)Pieces.WhiteBishop), list);
        }
        else
        {
            AddQuietMove(board, new Move(from, to), list);
        }
    }

    public static void AddBlackPawnCaptureMove(Board board, int from, int to, int capture, MoveList list)
    {
        if (Data.Ranks[from] == Ranks.r2)
        {
            AddCaptureMove(board, new Move(from, to, capture, (int)Pieces.BlackQueen), list);
            AddCaptureMove(board, new Move(from, to, capture, (int)Pieces.BlackKnight), list);
            AddCaptureMove(board, new Move(from, to, capture, (int)Pieces.BlackRook), list);
            AddCaptureMove(board, new Move(from, to, capture, (int)Pieces.BlackBishop), list);
        }
        else
        {
            AddCaptureMove(board, new Move(from, to, capture), list);
        }
    }

    public static void AddBlackPawnMove(Board board, int from, int to, MoveList list)
    {
        if (Data.Ranks[from] == Ranks.r2)
        {
            AddQuietMove(board, new Move(from, to, promotion: (int)Pieces.BlackQueen), list);
            AddQuietMove(board, new Move(from, to, promotion: (int)Pieces.BlackKnight), list);
            AddQuietMove(board, new Move(from, to, promotion: (int)Pieces.BlackRook), list);
            AddQuietMove(board, new Move(from, to, promotion: (int)Pieces.BlackBishop), list);
        }
        else
        {
            AddQuietMove(board, new Move(from, to), list);
        }
    }
}

