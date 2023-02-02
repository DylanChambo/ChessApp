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

    public override bool Equals(object? obj)
    {
        if (obj != null)
        {
            if (obj.GetType() == this.GetType())
            { 
                Position objPos = (Position)obj;
                return this.File == objPos.File && this.Rank == objPos.Rank;
            }
        }
        return false;
    }
}
