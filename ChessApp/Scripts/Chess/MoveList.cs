﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace ChessApp.Scripts.Chess;

public class MoveList
{
    const int MAX_MOVES = 256;
    public Move[] moves;
    public int count;
    public MoveList()
    {
        moves = new Move[MAX_MOVES];

    }

    public void Display()
    {
        Debug.WriteLine($"Total Moves: {count}");
        for (int i = 0; i < count; i++)
        {
            Debug.WriteLine($"From: {(Position)Move.From(moves[i].move)} To: {(Position)Move.To(moves[i].move)}, {(Pieces) Move.Captured(moves[i].move)}, {(Pieces) Move.Promoted(moves[i].move)}, {(MoveFlags)Move.MoveFlag(moves[i].move)}");
        }
    }
}
