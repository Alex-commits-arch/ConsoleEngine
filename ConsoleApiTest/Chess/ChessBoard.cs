using ConsoleLibrary.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApiTest.Chess
{
    class ChessBoard
    {
        private PieceType[] firstRank =
        {
            PieceType.Rook,
            PieceType.Knight,
            PieceType.Bishop,
            PieceType.Queen,
            PieceType.King,
            PieceType.Bishop,
            PieceType.Knight,
            PieceType.Rook
        };

        ChessPiece[,] board;
        public ChessBoard()
        {
            ResetBoard();
        }

        public Point[] GetMoves(Point location)
        {
            ChessPiece piece = board[location.X, location.Y];
            return ChessLogic.GetMoves(piece, location, board);
        }

        public ChessPiece[,] GetBoard()
        {
            return board;
        }

        public void ResetBoard()
        {
            board = new ChessPiece[8, 8];
            PlacePieces();
        }

        private void PlacePieces()
        {
            for (int i = 0; i < firstRank.Length; i++)
            {
                board[i, 0] = new ChessPiece(firstRank[i], PieceColor.Black);
                board[i, 1] = new ChessPiece(PieceType.Pawn, PieceColor.Black);
                board[i, 6] = new ChessPiece(PieceType.Pawn, PieceColor.White);
                board[i, 7] = new ChessPiece(firstRank[i], PieceColor.White);
            }
        }

        public bool IsKing(Point location)
        {
            return board[location.X, location.Y].type == PieceType.King;
        }

        public bool CanMove(PieceColor player, Point location)
        {
            return ContainsPieceAt(location) && ColorAt(location) == player;
        }

        public PieceColor ColorAt(Point location)
        {
            return board[location.X, location.Y].color;
        }

        public PieceType TypeAt(Point location)
        {
            return board[location.X, location.Y].type;
        }

        public void TypeAt(Point location, PieceType pieceType)
        {
            board[location.X, location.Y].type = pieceType;
        }

        public bool IsLastRank(Point location)
        {
            PieceColor color = ColorAt(location);

            if (color == PieceColor.White)
                return location.Y == 0;
            if (color == PieceColor.Black)
                return location.Y == 7;
            return false;
        }

        public bool ContainsPieceAt(Point location)
        {
            return board[location.X, location.Y].type != PieceType.None;
        }

        public void Move(Point from, Point to)
        {
            board[to.X, to.Y] = board[from.X, from.Y];
            board[from.X, from.Y] = new ChessPiece(PieceType.None, PieceColor.Black);
        }

        public Point[] GetLocations(PieceColor color)
        {
            List<Point> locations = new List<Point>();
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (board[x, y].type != PieceType.None && board[x, y].color == color)
                        locations.Add(new Point(x, y));
                }
            }
            return locations.ToArray();
        }

        private class ChessLogic
        {
            public static Point[] GetMoves(ChessPiece piece, Point location, ChessPiece[,] board)
            {
                int direction = piece.color == PieceColor.White ? -1 : 1;

                switch (piece.type)
                {
                    case PieceType.Pawn:
                        return GetPawnMoves(location, board, piece.color);
                    case PieceType.King:
                        return GetJumps(location, new Point[] { new Point(-1, -1), new Point(0, -1), new Point(1, -1),
                                                                   new Point(-1,  0),                      new Point(1,  0),
                                                                   new Point(-1,  1), new Point(0,  1), new Point(1,  1) }, board, piece.color);
                    case PieceType.Knight:
                        return GetJumps(location, new Point[] { new Point(-2, -1), new Point(-1, -2), new Point(1, -2), new Point(2, -1),
                                                                   new Point(-2,  1), new Point(-1,  2), new Point(1,  2), new Point(2,  1) }, board, piece.color);
                    case PieceType.Bishop:
                        return GetPaths(location, new Point[] { new Point(-1, -1), new Point(1, -1),
                                                                   new Point(-1, +1), new Point(1,  1) }, board, piece.color);
                    case PieceType.Rook:
                        return GetPaths(location, new Point[] { new Point( 0, -1), new Point(0, 1),
                                                                   new Point(-1,  0), new Point(1, 0) }, board, piece.color);
                    case PieceType.Queen:
                        return GetPaths(location, new Point[] { new Point( 0, -1), new Point(0,  1),
                                                                   new Point(-1,  0), new Point(1,  0),
                                                                   new Point(-1, -1), new Point(1, -1),
                                                                   new Point(-1, +1), new Point(1,  1) }, board, piece.color);
                    default:
                        return null;
                }
            }

            private static Point[] GetPawnMoves(Point location, ChessPiece[,] board, PieceColor color)
            {
                int direction = color == PieceColor.White ? -1 : 1;
                List<Point> locations = new List<Point>();
                Point[] directions = {               //relative to direction
                    new Point(0, direction),         //forward one step
                    new Point(0, direction * 2),     //forward two steps
                    new Point(direction, direction), //forward left
                    new Point(-direction, direction) //forward right
                };

                Point dir = location + directions[0];

                if (board.WithinBounds(dir) && board[dir.X, dir.Y].type == PieceType.None)
                {
                    locations.Add(dir);
                    dir = location + directions[1];

                    if (board.WithinBounds(dir)
                     && board[dir.X, dir.Y].type == PieceType.None
                     && ((direction < 0 && location.Y == 6) || location.Y == 1) && !ContainsFriend(board, dir, color) && !ContainsEnemy(board, dir, color))
                        locations.Add(dir);
                }

                //capturing
                dir = location + directions[2];
                if (board.WithinBounds(dir) && ContainsEnemy(board, dir, color))
                    locations.Add(dir);
                dir = location + directions[3];
                if (board.WithinBounds(dir) && ContainsEnemy(board, dir, color))
                    locations.Add(dir);

                return locations.ToArray();
            }

            private static Point[] GetPaths(Point location, Point[] directions, ChessPiece[,] board, PieceColor color)
            {
                List<Point> locations = new List<Point>();

                for (int i = 0; i < directions.Length; i++)
                {
                    Point sampleLocation = location + directions[i];
                    while (board.WithinBounds(sampleLocation) && !ContainsFriend(board, sampleLocation, color))
                    {
                        locations.Add(sampleLocation);
                        if (ContainsEnemy(board, sampleLocation, color))
                            break;
                        sampleLocation += directions[i];
                    }
                }
                return locations.ToArray();
            }

            private static bool ContainsFriend(ChessPiece[,] board, Point location, PieceColor color)
            {
                return board[location.X, location.Y].type != PieceType.None
                    && board[location.X, location.Y].color == color;
            }

            private static bool ContainsEnemy(ChessPiece[,] board, Point location, PieceColor color)
            {
                return board[location.X, location.Y].type != PieceType.None
                    && board[location.X, location.Y].color != color;
            }

            private static Point[] GetJumps(Point location, Point[] offsets, ChessPiece[,] board, PieceColor color)
            {
                List<Point> locations = new List<Point>();

                for (int i = 0; i < offsets.Length; i++)
                {
                    Point sampleLocation = location + offsets[i];
                    if (board.WithinBounds(sampleLocation) && !ContainsFriend(board, sampleLocation, color))
                    {
                        locations.Add(sampleLocation);
                    }
                }
                return locations.ToArray();
            }
        }

    }
}

static class BoardExtensions
{
    public static bool WithinBounds(this ChessPiece[,] board, Point location)
    {
        if (location.Y == -1)
        {

        }
        return location.X >= 0 && location.X <= board.GetUpperBound(0)
            && location.Y >= 0 && location.Y <= board.GetUpperBound(1);
    }
}

struct ChessPiece
{
    public PieceType type;
    public PieceColor color;

    public ChessPiece(PieceType type, PieceColor color)
    {
        this.type = type;
        this.color = color;
    }
}

enum PieceType
{
    None,
    Pawn,
    Bishop,
    Knight,
    Rook,
    Queen,
    King
}

enum PieceColor
{
    None,
    White,
    Black
}