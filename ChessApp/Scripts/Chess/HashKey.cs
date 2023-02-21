namespace ChessApp.Scripts.Chess;

public static class HashKey
{
    public static Random rand;
    public static ulong[,] PieceKeys { get; set; }
    public static ulong SideKey { get; set; }
    public static ulong[] CastleKey { get; set; }

    static HashKey()
    {
        rand = new Random();
        PieceKeys = new ulong[13, 120];
        CastleKey = new ulong[16];
        InitHashKeys();
    }

    public static void InitHashKeys()
    {
        int i;
        int j;
        for (i = 0; i < 13; i++)
        {
            for (j = 0; j < 120; j++)
            {
                PieceKeys[i, j] = (ulong)rand.NextInt64();
            }
        }

        SideKey = (ulong)rand.NextInt64();

        for (i = 0; i < 16; i++)
        {
            CastleKey[i] = (ulong)rand.NextInt64();
        }
    }

    public static ulong GenerateHashKey(Board board)
    {
        ulong hashkey = 0;

        Pieces piece;

        for (int i = Conversion.Square64To120[0]; i <= Conversion.Square64To120[63]; i++)
        {
            piece = board.Squares[i];
            if (piece != Pieces.Offboard && piece != Pieces.None)
            {
                hashkey ^= PieceKeys[(int)piece, i];
            }
        }

        if (board.Side == Sides.White)
        {
            hashkey ^= SideKey;
        }

        if (board.EnPassantSquare != Position.No_Square)
        {
            hashkey ^= PieceKeys[(int)Pieces.None, (int)board.EnPassantSquare];
        }

        hashkey ^= CastleKey[board.CastlePerm];

        return hashkey;
    }
}
