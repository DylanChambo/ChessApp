using ChessApp.Shared.ChessBoard;

namespace ChessApp.Scripts.Chess.AI;

public class Search
{
    const int Infinity = 9999999;
    public const int MaxDepth = 64;
    const int Mate = 5000;

    Board board { get; set; }
    bool abortSearch = false;
    int BestMove = 0;

    public Search(Board board)
    {
        this.board = board;
    }
    
    public int SearchPosition()
    {
        DateTime time = DateTime.Now;
        board.Ply = 0;
        board.SearchKillers = new int[2, MaxDepth];
        board.SearchHistory = new int[13, Board.VirtualBoardSize];
        int bestScore = -Infinity;
        int depth;

        for (depth = 1; depth <= MaxDepth; depth++)
        {
            if ((DateTime.Now - time >= new TimeSpan(10000000)))
            {
                break;
            }
            bestScore = SearchMoves(depth, -Infinity, Infinity);
            if (abortSearch)
            {
                depth++;
                break;
            }
        }

        Console.WriteLine($"Depth: {--depth}) Best Move: {(Position)Move.From(BestMove)}{(Position)Move.To(BestMove)} Eval: {bestScore}, Time: {DateTime.Now - time}");
        return BestMove;
    }

    private bool IsRepetition()
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

    private int SearchMoves(int depth, int alpha, int beta)
    {
        if (depth == 0)
        {
            return Evaluate.EvaluatePosition(board);
        }

        if (IsRepetition() || board.FiftyMoveCount >= 100) { return 0; }

        if (board.Ply >= MaxDepth - 1)
        {
            return Evaluate.EvaluatePosition(board);
        }

        MoveList list = new MoveList();
        MoveGenerator.GenerateAllMoves(board, list);
        int Score;
        int bestMove = 0;
        int legal = 0;
        int oldAlpha = alpha;

        for (int i = 0; i < list.count; i++)
        {
            GetBestMove(i, list);
            if (!board.MakeMove(list.moves[i].move))
            {
                continue;
            }
            legal++;
            
            Score = -SearchMoves(depth - 1, -beta, -alpha);
            board.TakeMove();

            if (Score > alpha)
            {
                if (Score >= beta)
                {
                    if (!Move.IsCapture(list.moves[i].move))
                    {
                        board.SearchKillers[1, board.Ply] = board.SearchKillers[0, board.Ply];
                        board.SearchKillers[0, board.Ply] = list.moves[i].move;
                    }
                    return beta;
                }
                alpha = Score;
                bestMove = list.moves[i].move;
                if (!Move.IsCapture(list.moves[i].move))
                {
                    board.SearchHistory[(int)board.Squares[Move.From(bestMove)], Move.To(bestMove)] += depth;
                }
            }
        }

        if (legal == 0)
        {
            if (board.IsSquareAttacked(board.KingSquare[(int)board.Side], board.Side ^ Sides.Black))
            {
                return -Mate + board.Ply;
            }
            else
            {
                return 0;
            }
        }

        if (alpha != oldAlpha && board.Ply == 0)
        {
            BestMove = bestMove;
        }
        return alpha;
    }

    static void GetBestMove(int moveNum,MoveList list)
    {
        Move temp = list.moves[moveNum];
        int bestScore = temp.score;
        int bestIndex = moveNum;

        for (int i = moveNum; i < list.count; i++)
        {
            if (list.moves[i].score > bestScore)
            {
                bestScore = list.moves[i].score;
                bestIndex = i;
            }
        }

        list.moves[moveNum] = list.moves[bestIndex];
        list.moves[bestIndex] = temp;
    }

    private int Quiescence(int alpha, int beta)
    {
        return 0;
    }
}

public struct SearchInfo
{
    DateTime StartTime;
    DateTime finishTime;
}
