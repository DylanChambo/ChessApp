namespace ChessApp.Data;

public struct Move
{
    public Position StartSquare;
    public Position TargetSquare;
    public MoveFlag MoveFlag;
    public Move(Position startSquare, Position targetSquare, MoveFlag moveFlag = MoveFlag.None)
    {
        StartSquare = startSquare;
        TargetSquare = targetSquare;
        MoveFlag = moveFlag;
    }

    public Move()
    {
        StartSquare = new Position();
        TargetSquare = new Position();
        MoveFlag = MoveFlag.None;
    }
}

public enum MoveFlag
{
    None,
    EnPassant,
    PawnDoubleMove,
    Castling,
    PromoteToQueen,
    PromoteToKnight,
    PromoteToRook,
    PromoteToBishop
}
