using System.Diagnostics;

namespace ChessApp.Data.Chess;

public static class Conversion
{
    static public int[] Square120To64 { get; set; }
    static public int[] Square64To120 { get; set; }

    static Conversion()
    {
        GeneretesConversionArray();
    }

    /// <summary>
    /// Generetes lookup tables to convert coordintates as fast as possible;
    /// </summary>
    public static void GeneretesConversionArray()
    {
        Square120To64 = new int[Board.VirtualBoardSize];
        Square64To120 = new int[Board.BoardSize];

        for (int i = 0; i < Board.VirtualBoardSize; i++)
        {
            Square120To64[i] = -1;
        }

        int square64 = 0;

        for (int rank = (int)Ranks.r1; rank <= (int)Ranks.r8; rank++)
        {
            for (int file = (int)Files.A; file <= (int)Files.H; file++)
            {
                int square = ConvertFRToIndex(file, rank);
                Square64To120[square64] = square;
                Square120To64[square] = square64;
                square64++;
            }
        }
    }

    /// <summary>
    /// Converts a File and rank to its equivalent index in a 120 array board representation.
    /// </summary>
    /// <param name="file">File of the square.</param>
    /// <param name="rank">Rank of the square.</param>
    /// <returns></returns>
    public static int ConvertFRToIndex(int file, int rank)
    {
        return 21 + file + 10 * rank;
    }
}
