using BlazorState;
using ChessApp.Data;

namespace ChessApp.Features.Chess;

public partial class ChessState
{
    public class StartGameAction : IAction
    {
        public StartGameAction(Player white = Player.You, Player black = Player.You)
        {
           WhitePlayer = white;
           BlackPlayer = black;
        }

        public Player BlackPlayer;
        public Player WhitePlayer;
    }
}

