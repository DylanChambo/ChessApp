using System.ComponentModel.DataAnnotations;
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
            Debug.WriteLine($"From: {(Position)moves[i].From()} To: {(Position)moves[i].To()}, {(Pieces) moves[i].Captured()}, {(Pieces) moves[i].Promoted()}, {(MoveFlags)moves[i].MoveFlag()}");
        }
    }
}
