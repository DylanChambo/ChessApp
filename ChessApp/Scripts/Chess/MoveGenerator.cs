using System.Diagnostics;

namespace ChessApp.Scripts.Chess;

public static class MoveGenerator
{
    public static void GenerateAllMoves(Board board, MoveList list)
    {
        list.count = 0;

        // White Pawn Move Generation
        if (board.Side == Sides.White)
        {
            for(int i = 0; i < board.PieceNum[(int)Pieces.WhitePawn]; i++) 
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
                    AddCaptureMove(board, new Move((int)position, (int)position + 9, flag: (int)MoveFlags.EnPassant), list);
                }
                if (position + 11 == board.EnPassantSquare)
                {
                    AddCaptureMove(board, new Move((int)position, (int)position + 11, flag: (int)MoveFlags.EnPassant), list);
                }
            }
        }
    }
    public static void AddQuietMove(Board board, Move move, MoveList list)
    {
        list.moves[list.count] = move;
        list.count++;
    }

    public static void AddCaptureMove(Board board, Move move, MoveList list)
    {
        list.moves[list.count] = move;
        list.count++;
    }

    public static void AddEnPassantMove(Board board, Move move, MoveList list)
    {
        list.moves[list.count] = move;
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
}

