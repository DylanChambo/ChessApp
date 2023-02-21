namespace ChessApp.Scripts.Chess;

public class PositonTable
{
    Dictionary<UInt64, int> Entries { get; set; }
        
    public PositonTable()
    {
        Entries = new Dictionary<UInt64, int>();
        Init();
    }

    public void Init()
    {
        Entries.Clear();
    }

    public void StoreMove(UInt64 poskey,  int move)
    {
        Entries[poskey] = move;
    }

    public int GetMove(UInt64 poskey)
    {
        try { return Entries[poskey];}
        catch { return 0; }
    }
}
