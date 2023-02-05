using BlazorState;
using ChessApp.Data;
using System.IO.Pipelines;
using static ChessApp.Features.Mouse.MouseState;

namespace ChessApp.Features.Mouse;

public partial class MouseState : State<MouseState>
{
    public Coord MousePos { get; private set; }
    public string Cursor { get; private set; }

    public override void Initialize()
    {
        MousePos = new Coord(0, 0); 
        Cursor = "";
    }
}