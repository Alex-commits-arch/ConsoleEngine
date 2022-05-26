using ConsoleLibrary.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApiTest.Chess
{
    class ChessAi
    {
        private ChessGame game;

        public ChessAi(ChessGame game)
        {
            this.game = game;
        }

        Random rnd = new Random();
        public void MakeMove()
        {
            var locations = game.board.GetLocations(PieceColor.Black);
            Point[] moves = new Point[0];
            Point from;
            do
            {
                from = locations.ElementAt(rnd.Next(locations.Length));
                moves = game.board.GetMoves(from);
            } while (moves.Length == 0);
            Point to = moves.ElementAt(rnd.Next(moves.Length));

            game.Move(from, to);
        }
    }
}
