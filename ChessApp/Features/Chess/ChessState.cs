using BlazorState;
using ChessApp.Data;
using ChessApp.Scripts.Chess;
using ChessApp.Scripts.Chess.AI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using static System.Formats.Asn1.AsnWriter;

namespace ChessApp.Features.Chess;

public partial class ChessState : State<ChessState>
{
    public bool IsFlipped { get; private set; }
    public Board Board { get; private set; }
    public MoveList MoveList { get; private set; }

    public GameState GameState = GameState.None;

    public Player BlackPlayer;
    public Player WhitePlayer;

    public Position lastMoveFrom = 0;
    public Position lastMoveTo = 0;

    public override void Initialize()
    { 
        IsFlipped = false;
    }

    public void Start(Player white = Player.You, Player black = Player.You)
    {
        lastMoveFrom = 0;
        lastMoveTo = 0;
        Board = new Board();
        MoveList = new MoveList();
        WhitePlayer = white;
        BlackPlayer = black;
        GameState = GameState.Playing;

        UpdateMoveList();
    }

    public void UpdateMoveList()
    {
        MoveList.count = 0;
        if (Board.Side == Sides.White && WhitePlayer == Player.You)
        {
            MoveGenerator.GenerateAllMoves(Board, MoveList);
        }
        else if (Board.Side == Sides.Black && BlackPlayer == Player.You)
        {
            MoveGenerator.GenerateAllMoves(Board, MoveList);
        }
    }

    public void CheckGameState()
    {
        MoveList list = new MoveList();
        MoveGenerator.GenerateAllMoves(Board, list);

        int legal = 0;
        for (int i = 0; i < list.count; i++)
        {
            if (!Board.MakeMove(list.moves[i].move))
            {
                continue;
            }
            legal++;   
            Board.TakeMove();
        }

        if (legal != 0)
        {
            return;
        }

        Sides side = Board.Side;
        if (Board.IsSquareAttacked(Board.KingSquare[(int)side], side ^ Sides.Black))
        {
            if (side == Sides.White)
            {
                GameState = GameState.BlackWin;
            } else
            {
                GameState = GameState.WhiteWin;
            }
        } else
        {
            GameState = GameState.Draw;
        }
    }


}

