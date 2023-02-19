namespace ChessApp.Scripts.Chess;

public struct Undo
{
    public int Move;
    public int CastlePerm;
    public Position EnPassant;
    public int FiftyMove;
    public ulong PositionKey;
}
