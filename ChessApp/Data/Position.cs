namespace ChessApp.Data;

public class Position
{
    public Position(char file, int rank)
    {
        File = file;
        Rank = rank;
    }

    public char File { get; set; }
    public int Rank { get; set; }
}
