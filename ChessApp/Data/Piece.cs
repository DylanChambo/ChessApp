namespace ChessApp.Data
{
    public enum Piece
    {
        None,
        WhiteKing,
        WhitePawn,
        WhiteKnight,
        WhiteBishop,
        WhiteRook,
        WhiteQueen,
        BlackKing,
        BlackPawn,
        BlackKnight,
        BlackBishop,
        BlackRook,
        BlackQueen,
        Null
    }

    public class PieceInstance
    {
        public PieceInstance(char file, int rank, Piece piece)
        {
            this.file = file;
            this.rank = rank;
            this.piece = piece;
        }
        public char file { get; set; }
        public int rank { get; set; }
        public Piece piece { get; set; }

    }
}
