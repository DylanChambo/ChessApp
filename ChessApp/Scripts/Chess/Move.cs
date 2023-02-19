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
    public static int From(int move)
    {
        return (move >> FromLSB) & 0xFF;
    }

    public static int To(int move)
    {
        return (move >> ToLSB) & 0xFF;
    }

    public static int Promoted(int move)
    {
        return (move >> PromotionLSB) & 0xF;
    }

    public static int Captured(int move)
    {
        return (move >> CaptureLSB) & 0xF;
    }

    public static int MoveFlag(int move)
    {
        return (move >> EnPassantBit) & 0xF;
    }

    public static bool IsCapture(int move)
    {
        return ((move >> CaptureLSB) & 0x1F) > 0;
    }

    public static bool IsPromotion(int move)
    {
        return Promoted(move) > 0;
    }
    public static bool IsEnPassant(int move)
    {
        return ((move >> EnPassantBit) & 1) > 0;
    }
    public static bool IsPawnStart(int move)
    {
        return ((move >> PawnStartBit) & 0x1F) > 0;
    }

    public static bool IsCastle(int move)
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