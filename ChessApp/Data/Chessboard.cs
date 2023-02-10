﻿using AnyClone;
using ChessApp.Features.Chess;
using System;

namespace ChessApp.Data;

public class Chessboard
{
    public Piece[] Board { get; set; }
    public Side SideToMove { get; set; }
    public GameState GameState { get; set; }
    public Castling WhiteCastling { get; set; }
    public Castling BlackCastling { get; set; }
    public char epFile { get; set; } 
    public int HalfmoveClock { get; set; }
    public int FullmoveCount { get; set; }
    public List<Move> Moves { get; set; }
    public bool Check { get; set; }
    public Material Material = new Material();

    public Player WhitePlayer { get; set; }
    public Player BlackPlayer { get; set; }

    public Chessboard() 
    {
        Board = new Piece[64];
        WhiteCastling = new Castling();
        BlackCastling = new Castling();
        GameState = GameState.None;
        FenUtils.PopulateDefaultBoard(this);
        Material = PieceUtils.CalcMaterial(this);
        MoveGenerator.GenerateMoves(this);
    }

    public Chessboard(Chessboard board)
    {
        board.CloneTo(this);
        GameState = GameState.None;
    }

    public Piece GetPiece(int file, int rank)
    {
        if ('a' <= file && file <= 'h')
        {
            if (1 <= rank && rank <= 8)
            {
                return Board[(file - 'a') + (8 * (rank - 1))];
            }
        }
        return Piece.Null;
    }

    public void SetPiece(int file, int rank, Piece piece = Piece.None)
    {
        if ('a' <= file && file <= 'h')
        {
            if (1 <= rank && rank <= 8)
            {
                Board[(file - 'a') + (8 * (rank - 1))] = piece;
            }
        }
    }

    public void MovePiece(Move move)
    {
        Piece piece = GetPiece(move.StartSquare.File, move.StartSquare.Rank);
        Piece targetPiece = GetPiece(move.StartSquare.File, move.StartSquare.Rank);

        if (targetPiece != Piece.None)
        {
            if (SideToMove == Side.White)
            {
                Material.Black -= PieceUtils.GetPieceValue(targetPiece);
            } else
            {
                Material.White -= PieceUtils.GetPieceValue(targetPiece);
            }
        }

        SetPiece(move.StartSquare.File, move.StartSquare.Rank);
        SetPiece(move.TargetSquare.File, move.TargetSquare.Rank, piece);
    }

    public void Move(Move move, bool tempMove = false)
    {
        MovePiece(move);

        epFile = '-';
        switch (move.MoveFlag)
        {
            case MoveFlag.PawnDoubleMove:
                epFile = move.TargetSquare.File;
                break;
            case MoveFlag.EnPassant:
                if (SideToMove == Side.White)
                {
                    Console.WriteLine(GetPiece(move.TargetSquare.File, move.TargetSquare.Rank - 1));
                    SetPiece(move.TargetSquare.File, move.TargetSquare.Rank - 1);
                } else
                {
                    SetPiece(move.TargetSquare.File, move.TargetSquare.Rank + 1);
                }
                break;
            case MoveFlag.Castling:
                Piece rook = SideToMove == Side.White? Piece.WhiteRook : Piece.BlackRook;
                if (move.TargetSquare.File == 'g')
                {
                    SetPiece('h', move.TargetSquare.Rank);
                    SetPiece('f', move.TargetSquare.Rank, rook);
                } else
                {
                    SetPiece('a', move.TargetSquare.Rank);
                    SetPiece('d', move.TargetSquare.Rank, rook);
                }
                break;
            case MoveFlag.PromoteToQueen:
                Piece queen = SideToMove == Side.White ? Piece.WhiteQueen : Piece.BlackQueen;
                SetPiece(move.TargetSquare.File, move.TargetSquare.Rank, queen);
                if (SideToMove == Side.White)
                {
                    Material.White += PieceUtils.Queen - PieceUtils.Pawn;
                }
                else
                {
                    Material.Black += PieceUtils.Queen - PieceUtils.Pawn;
                }
                break;
            case MoveFlag.PromoteToKnight:
                Piece knight = SideToMove == Side.White ? Piece.WhiteKnight : Piece.BlackKnight;
                SetPiece(move.TargetSquare.File, move.TargetSquare.Rank, knight);
                if (SideToMove == Side.White)
                {
                    Material.White += PieceUtils.Knight - PieceUtils.Pawn;
                }
                else
                {
                    Material.Black += PieceUtils.Knight - PieceUtils.Pawn;
                }
                break;
            case MoveFlag.PromoteToRook:
                rook = SideToMove == Side.White ? Piece.WhiteRook : Piece.BlackRook;
                SetPiece(move.TargetSquare.File, move.TargetSquare.Rank, rook);
                if (SideToMove == Side.White)
                {
                    Material.White += PieceUtils.Rook - PieceUtils.Pawn;
                }
                else
                {
                    Material.Black += PieceUtils.Rook - PieceUtils.Pawn;
                }
                break;
            case MoveFlag.PromoteToBishop:
                Piece bishop = SideToMove == Side.White ? Piece.WhiteBishop : Piece.BlackBishop;
                SetPiece(move.TargetSquare.File, move.TargetSquare.Rank, bishop);
                if (SideToMove == Side.White)
                {
                    Material.White += PieceUtils.Bishop - PieceUtils.Pawn;
                }
                else
                {
                    Material.Black += PieceUtils.Bishop - PieceUtils.Pawn;
                }
                break;
            default:
                break;
        }

        SideToMove = SideToMove == Side.White ? Side.Black : Side.White;

        if (!tempMove)
        {
            
            MoveGenerator.GenerateMoves(this);
            isCheck();
            Console.WriteLine($"Move, {Check}");

            DisplayBoard();
            if (Moves.Count == 0)
            {
                if (Check)
                {
                    Console.WriteLine("Checkmate");
                    GameState = SideToMove == Side.White ? GameState.BlackWin : GameState.WhiteWin;
                }
                else
                {
                    Console.WriteLine("Stalemate");
                    GameState = GameState.Draw;
                }
            } 
        } 
    }

   public Move OpponentMove(Player player)
    {
        Console.WriteLine(player);
        
        if (player == Player.RandomBot)
        {
            Thread.Sleep(200);
            Chessboard board = new Chessboard(this);
            Moves.Clear();
            Random random = new Random();
            int randomNum = random.Next(board.Moves.Count);
            return board.Moves[randomNum];
        }
        else if (player == Player.SmartBot)
        {
            Chessboard board = new Chessboard(this);
            Moves.Clear();

            return AI.CalcMove(board, 7);
        }
        return new Move();
    }

    public void isCheck()
    {
        Chessboard temp = new Chessboard(this);
        temp.SideToMove = temp.SideToMove == Side.White ? Side.Black : Side.White;
        Check = !MoveGenerator.NotCheck(temp);
    }

    public void StartGame(Player White = Player.This, Player Black = Player.This)
    {
        WhitePlayer = White;
        BlackPlayer = Black;
        GameState = GameState.Playing;
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
            [Piece.WhiteQueen] = 'Q',
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
        Console.WriteLine("--------");

    }
    public static string GetSquareColour(Position pos)
    {
        if ((pos.Rank + pos.File) % 2 == 0)
        {
            return "dark";
        }
        else
        {
            return "light";
        }
    }
}



public enum Player
{
    This,
    RandomBot,
    SmartBot,
    OnlinePlayer
}

