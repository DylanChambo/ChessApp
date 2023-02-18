using System.Diagnostics;

namespace ChessApp.Scripts.Chess;

public struct Move
{
    const int FromLSB = 0;
    const int ToLSB = 8;
    const int PromotionLSB = 16;
    const int CaptureLSB = 20;
    const int EnPassantBit =24;
    const int PawnStartBit = 25;
    const int CastleBit = 26;
    public int move { get; set; }
    public int score { get; set; }
    public Move(int from, int to, int capture = (int)Pieces.None, int promotion = (int)Pieces.None, int flag = (int)MoveFlags.None)
    {
        move = (from) | (to << ToLSB) | (promotion << PromotionLSB) | (capture << CaptureLSB) | (flag << EnPassantBit);
    }

    public int From()
    {
        return (move >> FromLSB)& 0xFF;
    }

    public int To() 
    { 
        return (move >> ToLSB) & 0xFF;
    }

    public int Promoted() 
    {
        return (move >> PromotionLSB) & 0xF;
    }

    public int Captured()
    {
        return (move >> CaptureLSB)& 0xF;
    }

    public int MoveFlag()
    {
        return (move >> EnPassantBit) & 0xF;
    }

    public bool IsCapture()
    {
        return ((move >> CaptureLSB) & 0x1F) > 0;
    }

    public bool IsPromotion()
    {
        return Promoted() > 0;
    }

    public bool IsEnPassant()
    {
        return ((move >> EnPassantBit) & 1) > 0;
    }

    public bool IsPawnStart()
    {
        return ((move >> PawnStartBit) & 0x1F) > 0;
    }

    public bool IsCastle()
    {
        return ((move >> CastleBit) & 0x1F) > 0;
    }

}

public enum MoveFlags
{
    None,
    EnPassant,
    PawnStart,
    Castle = 4,
}