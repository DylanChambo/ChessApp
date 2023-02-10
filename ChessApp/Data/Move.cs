using System.Diagnostics.CodeAnalysis;

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

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj != null)
        {
            if (obj.GetType() == this.GetType())
            {
                Move objMove = (Move)obj;
                return this.StartSquare.Equals(objMove.StartSquare) && this.TargetSquare.Equals(objMove.TargetSquare);
            }
        }
        return false;
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

public class Castling
{
    public bool KingSide { get; set; }
    public bool QueenSide { get; set; }
}
