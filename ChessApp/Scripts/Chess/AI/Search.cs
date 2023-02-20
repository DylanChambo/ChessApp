namespace ChessApp.Scripts.Chess.AI;

public static class Search
{
    public static void SearchPosition(Board board)
    {
        DateTime time = DateTime.Now;
        MoveList moveList = new MoveList();
        MoveGenerator.GenerateAllMoves(board, moveList);
        //Console.WriteLine($"Depth: {6}) Leaf Moves: {board.Perft(6)}, Time: {DateTime.Now - time}");
       
    }

    private static bool IsRepetition(Board board)
    {
        for (int i = board.HisPly - board.FiftyMoveCount; i < board.HisPly - 1; i++)
        {
            if (board.PositionKey == board.History[i].PositionKey)
            {
                return true;
            }
        }
        return false;
    }

    private static int SearchMoves(int depth, int alpha, int beta, int board)
    {
        return 0;
    }

    private static int Quiescence(int alpha, int beta, int board)
    {
        return 0;
    }
}

public struct SearchInfo
{
    DateTime StartTime;
    DateTime finishTime;
}
