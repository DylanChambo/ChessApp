namespace ChessApp.Data
{
    public struct Move
    {
        public Position StartSquare;
        public Position TargetSquare;
        public Move(Position startSquare, Position targetSquare)
        {
            StartSquare = startSquare;
            TargetSquare = targetSquare;
        }

        public Move()
        {
            StartSquare = new Position();
            TargetSquare = new Position();
        }
    }
}
