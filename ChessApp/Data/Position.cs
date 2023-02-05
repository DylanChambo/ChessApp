namespace ChessApp.Data;

public struct Position
{
    public Position(char file, int rank)
    {
        File = file;
        Rank = rank;
    }

    public char File { get; set; }
    public int Rank { get; set; }

}
