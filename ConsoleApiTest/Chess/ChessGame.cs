using ConsoleLibrary.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApiTest.Chess
{
    class ChessGame
    {
        public ChessBoard board;
        public PieceColor currentPlayer;
        public bool againstComputer;
        public bool gameOver = false;

        private ChessAi ai;

        public ChessGame(bool againstComputer = true)
        {
            board = new ChessBoard();
            this.againstComputer = againstComputer;
            
            if (againstComputer)
                ai = new ChessAi(this);
        }

        public void Move(Location from, Location to)
        {
            if (board.IsKing(to))
            {
                gameOver = true;
            }

            board.Move(from, to);
            ChangePlayer();
        }

        private void ChangePlayer()
        {
            if (!gameOver)
            {
                currentPlayer = currentPlayer == PieceColor.Black ? PieceColor.White : PieceColor.Black;

                if (againstComputer && currentPlayer == PieceColor.Black)
                {
                    ai.MakeMove();
                }
            }
        }
    }
}
