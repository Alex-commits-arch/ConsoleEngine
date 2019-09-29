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

        public Location[] GetMoves(Location location)
        {
            ChessPiece piece = board[location.x, location.y];
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

        public bool IsKing(Location location)
        {
            return board[location.x, location.y].type == PieceType.King;
        }

        public bool CanMove(PieceColor player, Location location)
        {
            return ContainsPieceAt(location) && ColorAt(location) == player;
        }

        private PieceColor ColorAt(Location location)
        {
            return board[location.x, location.y].color;
        }

        public PieceType TypeAt(Location location)
        {
            return board[location.x, location.y].type;
        }

        private bool ContainsPieceAt(Location location)
        {
            return board[location.x, location.y].type != PieceType.None;
        }

        public void Move(Location from, Location to)
        {
            board[to.x, to.y] = board[from.x, from.y];
            board[from.x, from.y] = new ChessPiece(PieceType.None, PieceColor.Black);
        }

        public Location[] GetLocations(PieceColor color)
        {
            List<Location> locations = new List<Location>();
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (board[x, y].type != PieceType.None && board[x, y].color == color)
                        locations.Add(new Location(x, y));
                }
            }
            return locations.ToArray();
        }

        private class ChessLogic
        {
            public static Location[] GetMoves(ChessPiece piece, Location location, ChessPiece[,] board)
            {
                int direction = piece.color == PieceColor.White ? -1 : 1;

                switch (piece.type)
                {
                    case PieceType.Pawn:
                        return GetPawnMoves(location, board, piece.color);
                    case PieceType.King:
                        return GetJumps(location, new Location[] { new Location(-1, -1), new Location(0, -1), new Location(1, -1),
                                                                   new Location(-1,  0),                      new Location(1,  0),
                                                                   new Location(-1,  1), new Location(0,  1), new Location(1,  1) }, board, piece.color);
                    case PieceType.Knight:
                        return GetJumps(location, new Location[] { new Location(-2, -1), new Location(-1, -2), new Location(1, -2), new Location(2, -1),
                                                                   new Location(-2,  1), new Location(-1,  2), new Location(1,  2), new Location(2,  1) }, board, piece.color);
                    case PieceType.Bishop:
                        return GetPaths(location, new Location[] { new Location(-1, -1), new Location(1, -1),
                                                                   new Location(-1, +1), new Location(1,  1) }, board, piece.color);
                    case PieceType.Rook:
                        return GetPaths(location, new Location[] { new Location( 0, -1), new Location(0, 1),
                                                                   new Location(-1,  0), new Location(1, 0) }, board, piece.color);
                    case PieceType.Queen:
                        return GetPaths(location, new Location[] { new Location( 0, -1), new Location(0,  1),
                                                                   new Location(-1,  0), new Location(1,  0),
                                                                   new Location(-1, -1), new Location(1, -1),
                                                                   new Location(-1, +1), new Location(1,  1) }, board, piece.color);
                    default:
                        return null;
                }
            }

            private static Location[] GetPawnMoves(Location location, ChessPiece[,] board, PieceColor color)
            {
                int direction = color == PieceColor.White ? -1 : 1;
                List<Location> locations = new List<Location>();
                Location[] directions = {               //relative to direction
                    new Location(0, direction),         //forward one step
                    new Location(0, direction * 2),     //forward two steps
                    new Location(direction, direction), //forward left
                    new Location(-direction, direction) //forward right
                };

                Location dir = location + directions[0];

                if (board.WithinBounds(dir) && board[dir.x, dir.y].type == PieceType.None)
                {
                    locations.Add(dir);
                    dir = location + directions[1];

                    if (board.WithinBounds(dir)
                     && board[dir.x, dir.y].type == PieceType.None
                     && ((direction < 0 && location.y == 6) || location.y == 1) && !ContainsFriend(board, dir, color) && !ContainsEnemy(board, dir, color))
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

            private static Location[] GetPaths(Location location, Location[] directions, ChessPiece[,] board, PieceColor color)
            {
                List<Location> locations = new List<Location>();

                for (int i = 0; i < directions.Length; i++)
                {
                    Location sampleLocation = location + directions[i];
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

            private static bool ContainsFriend(ChessPiece[,] board, Location location, PieceColor color)
            {
                return board[location.x, location.y].type != PieceType.None
                    && board[location.x, location.y].color == color;
            }

            private static bool ContainsEnemy(ChessPiece[,] board, Location location, PieceColor color)
            {
                return board[location.x, location.y].type != PieceType.None
                    && board[location.x, location.y].color != color;
            }

            private static Location[] GetJumps(Location location, Location[] offsets, ChessPiece[,] board, PieceColor color)
            {
                List<Location> locations = new List<Location>();

                for (int i = 0; i < offsets.Length; i++)
                {
                    Location sampleLocation = location + offsets[i];
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
    public static bool WithinBounds(this ChessPiece[,] board, Location location)
    {
        if(location.y == -1)
        {

        }
        return location.x >= 0 && location.x <= board.GetUpperBound(0)
            && location.y >= 0 && location.y <= board.GetUpperBound(1);
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
    White,
    Black
}