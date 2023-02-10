using BlazorState;
using ChessApp.Data;
using System.ComponentModel;

namespace ChessApp.Features.Chess;

public partial class ChessState
{
    public class StartGameAction : IAction
    {

        public Player White { get; set; }
        public Player Black { get; set; }
        public StartGameAction(Player White = Player.This, Player Black = Player.This) {
            this.White = White;
            this.Black = Black;
        }

        
    }
}

