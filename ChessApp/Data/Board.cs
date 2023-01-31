namespace ChessApp.Data.Board
{
    public class Board
    {
        public static readonly string DefaultFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

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

        public PieceInstance movingPiece = new PieceInstance('0', 0, Piece.None);
        public bool selected = false;
        public Boolean isFlipped { get; set; }

        public Board() {
            isFlipped = false;
            LoadFenPosition(DefaultFen);
        }

        public Piece GetPiece(int file, int rank)
        {
            return board[(file - 'a') + (8 * (rank - 1))];
        }

        public void SetPiece(int file, int rank, Piece piece = Piece.None)
        {
            board[(file - 'a') + (8 * (rank - 1))] = piece;
        }

        public void LoadFenPosition(string fen)
        {
            board = new Piece[64];
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
                    file += (char) char.GetNumericValue(symbol);
                } else
                {
                    SetPiece(file, rank, pieceFromSymbol[symbol]);
                    file++;
                }
            }
        }

        public void DisplayBoard()
        {
            Dictionary<Piece, char> symbolFromPiece = new Dictionary<Piece, char>()
            {
                [Piece.None] = ' ',
                [Piece.WhiteKing] = 'K', 
                [Piece.WhitePawn] = 'P',
                [Piece.WhiteBishop] = 'B',
                [Piece.WhiteKnight] = 'N',
                [Piece.WhiteRook] = 'R',
                [Piece.WhiteQueen] = 'Q' ,
                [Piece.BlackKing] = 'k',
                [Piece.BlackPawn] = 'p',
                [Piece.BlackBishop] = 'b',
                [Piece.BlackKnight] = 'n',
                [Piece.BlackRook] = 'r',
                [Piece.BlackQueen] = 'q'
            };  
            
            for (int rank = 8; rank >= 1; rank--)
            {
                for (char file = 'a'; file <= 'h'; file++)
                {
                    Console.Write(symbolFromPiece[GetPiece(file, rank)]);
                }
                Console.WriteLine("");
            }
            Console.WriteLine("\n");

        }
        
    }
}
