namespace ChessApp.Data;

public struct Position
{
    public Position(char file, int rank)
    {
        File = file;
        Rank = rank;
    }

    public Position()
    {
        File = '0';
        Rank = 0;
    }

    public char File { get; set; }
    public int Rank { get; set; }

    public override string ToString()
    {
        return File.ToString() + Rank.ToString();
    }

}
