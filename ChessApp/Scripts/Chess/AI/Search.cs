﻿using ChessApp.Shared.ChessBoard;

namespace ChessApp.Scripts.Chess.AI;

public class Search
{
    const int Infinity = 9999999;
    const int SearchDepth = 5;
    const int MaxDepth = 64;
    const int Mate = 5000;

    Board board { get; set; }
    bool abortSearch = false;

    public Search(Board board)
    {
        this.board = board;
    }
    
    public int SearchPosition()
    {
        DateTime time = DateTime.Now;
        board.Ply = 0;
        int bestScore = -Infinity;
        int bestMove = 0;
        int depth;

        for (depth = 1; depth <= SearchDepth; depth++)
        {
            bestScore = SearchMoves(depth, -Infinity, Infinity);
            bestMove = board.positionTable.GetMove(board.PositionKey);
            if (abortSearch)
            {
                depth++;
                break;
            }
        }

        Console.WriteLine($"Depth: {--depth}) Best Move: {(Position)Move.From(bestMove)}{(Position)Move.To(bestMove)} Eval: {bestScore}, Time: {DateTime.Now - time}");
        return bestMove;
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
                    return beta;
                }
                alpha = Score;
                bestMove = list.moves[i].move;
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

        if (alpha != oldAlpha)
        {
            board.positionTable.StoreMove(board.PositionKey, bestMove);
        }
        return alpha;
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
