namespace ChessApp.Scripts.Chess;

public struct MoveList
{
    const int MAX_MOVES = 256;
    public Move[] moves;
    public int count;
    public MoveList()
    {
        moves = new Move[MAX_MOVES];

    }
}
