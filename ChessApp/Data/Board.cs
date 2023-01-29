namespace ChessApp.Data.Board
{
    public class Board
    {
        public static readonly string DefaultFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";

        public static string GetSquareColour(char file, int rank)
        {
            if ((rank + file) % 2 == 0)
            {
                return "dark";
            }
            else
            {
                return "light";
            }
        }

        private Piece[] board = new Piece[64];
        public Boolean isFlipped { get; set; }

        public Board() {
            isFlipped = false;
            LoadFenPosition(DefaultFen);
        }

        public Piece GetPiece(int file, int rank)
        {
            return board[(file - 'a') + (8 * (rank - 1))];
        }

        public void SetPiece(int file, int rank, Piece piece)
        {
            board[(file - 'a') + (8 * (rank - 1))] = piece;
        }

        public void LoadFenPosition(string fen)
        {
            Dictionary<char, Piece> pieceFromSymbol = new Dictionary<char, Piece>()
            {
                ['K'] = Piece.WhiteKing,
                ['P'] = Piece.WhitePawn,
                ['B'] = Piece.WhiteBishop,
                ['N'] = Piece.WhiteKnight,
                ['R'] = Piece.WhiteRook,
                ['Q'] = Piece.WhiteQueen,
                ['k'] = Piece.BlackKing,
                ['p'] = Piece.BlackPawn,
                ['b'] = Piece.BlackBishop,
                ['n'] = Piece.BlackKnight,
                ['r'] = Piece.BlackRook,
                ['q'] = Piece.BlackQueen
            };

            string fenBoard = fen.Split(' ')[0];
            char file = 'a';
            int rank = 8;

            foreach (char symbol in fenBoard)
            {
                if (symbol == '/')
                {
                    file = 'a';
                    rank--;
                } else if(char.IsDigit(symbol)) {
                    Console.WriteLine($"{file} {symbol}");
                    file += (char) char.GetNumericValue(symbol);
                    Console.WriteLine($"{file}");
                } else
                {
                    SetPiece(file, rank, pieceFromSymbol[symbol]);
                    file++;
                }
            }
        }

        
    }
}
