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
        public delegate void LastRankEventHandler(Point location);

        public event LastRankEventHandler LastRank;
        public event Action GameOver;

        public ChessBoard board;
        public PieceColor currentPlayer = PieceColor.White;
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

        public void Reset()
        {
            gameOver = false;
            currentPlayer = PieceColor.White;
            board.ResetBoard();
        }

        public void Move(Point from, Point to)
        {
            if (board.IsKing(to))
            {
                gameOver = true;
                GameOver?.Invoke();
            }

            board.Move(from, to);

            if (!gameOver)
            {
                if (board.TypeAt(to) == PieceType.Pawn && board.IsLastRank(to))
                    LastRank?.Invoke(to);

                ChangePlayer();
            }
        }

        public void Promote(Point location, PieceType pieceType)
        {
            if (board.TypeAt(location) == PieceType.Pawn)
                board.TypeAt(location, pieceType);
        }

        private void ChangePlayer()
        {
            currentPlayer = currentPlayer == PieceColor.Black ? PieceColor.White : PieceColor.Black;

            if (againstComputer && currentPlayer == PieceColor.Black)
            {
                ai.MakeMove();
            }
        }
    }
}
