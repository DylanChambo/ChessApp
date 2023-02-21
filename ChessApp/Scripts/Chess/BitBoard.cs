using System.Diagnostics;

namespace ChessApp.Scripts.Chess;

public struct BitBoard
{
    public ulong bitBoard;

    public BitBoard()
    {
        bitBoard = 0UL;
    }
    public void PrintBitBoard()
    {
        for (int rank = (int)Ranks.r8; rank >= (int)Ranks.r1; rank--)
        {
            for (int file = (int)Files.A; file <= (int)Files.H; file++)
            {
                int square = Conversion.ConvertFRTo64(file, rank);
                if ((bitBoard & 1UL << square) != 0UL)
                {
                    Debug.Write("1");
                }
                else
                {
                    Debug.Write("0");
                }
            }
            Debug.Write("\n");
        }
        Debug.Write("\n");
    }

    public void Reset()
    {
        bitBoard = 0UL;
    }

    public int Count()
    {
        ulong bb = bitBoard;
        int r;
        for (r = 0; bb != 0; r++) { bb &= bb - 1UL; }
        return r;
    }

    public void SetBit(int square)
    {
        bitBoard |= 1UL << square;
    }

    public void ClearBit(int square)
    {
        bitBoard &= ~(1UL << square);
    }
}
