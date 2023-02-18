namespace ChessApp.Scripts.Chess;

public static class MoveGenerator
{
    public static void GenerateAllMoves(Board board, MoveList list)
    {
        list.count = 0;
    }
    public static void AddQuietMove(Board board, int move, MoveList list)
    {
        list.moves[list.count].move = move;
        list.moves[list.count].move = 0;
        list.count++;
    }

    public static void AddCaptureMove(Board board, int move, MoveList list)
    {
        list.moves[list.count].move = move;
        list.moves[list.count].move = 0;
        list.count++;
    }

    public static void AddEnPassantMove(Board board, int move, MoveList list)
    {
        list.moves[list.count].move = move;
        list.moves[list.count].move = 0;
        list.count++;
    }
}

