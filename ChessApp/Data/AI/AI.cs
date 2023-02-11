using System.Diagnostics;
using System.Net;

namespace ChessApp.Data;

public class AI
{
    const int immediateMateScore = 100000;
    const int positiveInfinity = 9999999;
    const int negativeInfinity = -positiveInfinity;
    public Chessboard Board { get; set; }
    Move bestMoveThisIteration;
    int bestEvalThisIteration;
    Move bestMove;
    int bestEval;
    bool abortSearch = false;

    public AI(Chessboard board)
    {
        Board = board;
    }
    public Move CalcMove(int depth = 10)
    {
        bestMove = bestMoveThisIteration = new Move();
        bestEval = bestEvalThisIteration = 0;
        for (int searchDepth = 1; searchDepth <= 1; searchDepth++)
        {
            SearchMoves(3, 0, negativeInfinity, positiveInfinity, Board);
            if (abortSearch)
            {
                break;
            }
            else
            {
                bestMove = bestMoveThisIteration;
                bestEval = bestEvalThisIteration;

                // Exit search if found a mate
                if (IsMateScore(bestEval))
                {
                    break;
                }
            }
        }
        Debug.WriteLine($"Best Move: {bestMove}, Eval: {bestEval}");
        return bestMove;
    }

    public int SearchMoves(int depth, int plyFromRoot, int alpha, int beta, Chessboard board)
    {
        if (abortSearch)
        {
            return 0;
        }

        if (plyFromRoot > 0)
        {
            // Skip this position if a mating sequence has already been found earlier in
            // the search, which would be shorter than any mate we could find from here.
            // This is done by observing that alpha can't possibly be worse (and likewise
            // beta can't  possibly be better) than being mated in the current position.
            alpha = Math.Max(alpha, -immediateMateScore + plyFromRoot);
            beta = Math.Min(beta, immediateMateScore - plyFromRoot);
            if (alpha >= beta)
            {
                return alpha;
            }
        }

        if (depth == 0)
        {
            return Evaluate(board);
        }

        if (board.Moves.Count == 0)
        {
            if (board.Check)
            {
                return -immediateMateScore;
            }
            else
            {
                return 0;
            }
        }
        foreach (Move move in board.Moves)
        {
            Chessboard newBoard = new Chessboard(board);
            newBoard.Move(move);
            int evaluation = -SearchMoves(depth - 1, plyFromRoot + 1, -beta,  -alpha, newBoard);
            
            // Move was *too* good, so opponent won't allow this position to be reached
            if (evaluation >= beta)
            {
                return beta;
            }

            // Found a new best move in this position
            if (evaluation > alpha)
            {
                alpha = evaluation;
                if (plyFromRoot == 0)
                {
                    bestMoveThisIteration = move;
                    bestEvalThisIteration = evaluation;
                }
               
            }
        }
        return alpha;
    }

    public int Evaluation(Chessboard board, int alpha, int beta)
    {
        int evaluation = Evaluate(board);
        Debug.WriteLine($"{evaluation}");
        if (evaluation >= beta)
        {
            return beta;
        }
        if (evaluation > alpha)
        {
            alpha = evaluation;
        }

        foreach (Move move in board.Moves)
        {
            Chessboard newBoard = new Chessboard(board);
            newBoard.Move(move);
            evaluation = -Evaluation(newBoard, -beta, -alpha);

            if (evaluation >= beta)
            {
                return beta;
            }

            // Found a new best move in this position
            if (evaluation > alpha)
            {
                alpha = evaluation;
            }
        }

        return alpha;
    }
    public static bool IsMateScore(int score)
    {
        const int maxMateDepth = 1000;
        return Math.Abs(score) > immediateMateScore - maxMateDepth;
    }

    public static int Evaluate(Chessboard board)
    {
        int whiteEval = board.Material.White;
        int blackEval = board.Material.Black;

        int perspective = (board.SideToMove == Side.White ? 1 : -1);
        return (whiteEval - blackEval) * perspective;
    }
}
