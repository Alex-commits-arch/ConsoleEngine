using ConsoleLibrary;
using ConsoleLibrary.Graphics.Drawing;
using ConsoleLibrary.Graphics.Shapes;
using ConsoleLibrary.Structures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsWrapper.Constants;
using static ConsoleLibrary.InputManager;

namespace ConsoleApiTest.Chess
{
    class ChessApp : ConsoleApp
    {
        public ChessApp(int width = 40, int height = 30) : base(width, height) { }

        const char PAWN = '♙';
        const char ROOK = '♖';
        const char KNIGHT = '♘';
        const char BISHOP = '♗';
        const char QUEEN = '♕';
        const char KING = '♔';
        // ♙♖♘♗♕♔
        // ♟♜♞♝♛♚
        Dictionary<PieceColor, Dictionary<PieceType, char>> pieceChars = new Dictionary<PieceColor, Dictionary<PieceType, char>>
        {
            //{ PieceType.Pawn,   '\u2659' },
            //{ PieceType.Pawn,   PAWN },
            //{ PieceType.Pawn,   'P' },
            //{ PieceType.Rook,   'R' },
            //{ PieceType.Knight, 'N' },
            //{ PieceType.Bishop, 'B' },
            //{ PieceType.Queen,  'Q' },
            //{ PieceType.King,   'K' },
            { PieceColor.Black, new Dictionary<PieceType, char>{
                { PieceType.Pawn, '♙' },
                { PieceType.Rook, '♖' },
                { PieceType.Knight, '♘' },
                { PieceType.Bishop, '♗' },
                { PieceType.Queen, '♕' },
                { PieceType.King, '♔' },
            } },
            { PieceColor.White, new Dictionary<PieceType, char>{
                { PieceType.Pawn, '♟' },
                { PieceType.Rook, '♜' },
                { PieceType.Knight, '♞' },
                { PieceType.Bishop, '♝' },
                { PieceType.Queen, '♛' },
                { PieceType.King, '♚' },
            } }

            //{PieceColor.Black  { PieceType.Pawn,   PAWN },
            //{ PieceType.Rook,   ROOK },
            //{ PieceType.Knight, KNIGHT },
            //{ PieceType.Bishop, BISHOP },
            //{ PieceType.Queen,  QUEEN },
            //{ PieceType.King,   KING },}
            
        };

        Dictionary<PieceColor, int> pieceColors = new Dictionary<PieceColor, int>
        {
            { PieceColor.White, Colors.BACKGROUND_WHITE | Colors.COMMON_LVB_REVERSE_VIDEO },
            { PieceColor.Black, Colors.FOREGROUND_INTENSITY },
        };

        int tilesX = 8;
        int tilesY = 8;
        int tileWidth = 6;
        int tileHeight = 3;

        Location windowCenter;
        Location boardCenter;
        Location tileCenter;
        Location tileSize;
        Location boardSize;
        Location selected;
        Location playerPos;

        //int playerX = 0;
        //int playerY = 0;

        readonly int boardColor = Colors.FOREGROUND_GREEN | Colors.FOREGROUND_INTENSITY;

        //ChessBoard board;
        ChessGame game;

        public override void Init()
        {
            base.Init();
            playerPos = new Location(0, 0);
            windowCenter = new Location(width, height) / 2;
            tileSize = new Location(tileWidth, tileHeight);
            tileCenter = tileSize / 2;
            boardSize = new Location(tilesX, tilesY) * tileSize;
            boardCenter = boardSize / 2;

            InitControls();
            InitLayers();
            InitGame();

            base.Loop(Update);
        }

        private void InitGame()
        {
            game = new ChessGame(true);
            //board = new ChessBoard();
            //board.GetMoves(new Location(0, 1));
            DrawBoard();
        }

        public void Update()
        {
            DrawPieces();
            DrawText();
            DrawPlayer();
            DrawWinner();
            context.RenderFrame();
            //context.DrawChar('♙', 1, 0);
            //context.DrawString("♙", 0, 0);
        }

        private void DrawBoard()
        {
            var tileShape = new Rect(tileWidth, tileHeight);
            var borderShape = new Rect(boardSize + new Location(2, 2));

            var boardBuffer = context["board"];
            tileShape.Fill('░');
            borderShape.Border('╔', '╗', '╚', '╝', '║', '═');
            //borderShape.Border('░');

            boardBuffer.Draw(borderShape, boardColor, windowCenter - boardCenter + new Location(-1, -1));
            for (int x = 0; x < tilesX; x++)
            {
                boardBuffer.Draw("ABCDEFGH"[x], boardColor, windowCenter + new Location(tileWidth / 2 + tileWidth * x - boardCenter.x, boardCenter.y + tileCenter.y));
                boardBuffer.Draw("12345678"[x], boardColor, windowCenter + new Location(-boardCenter.x - tileCenter.x, boardCenter.y - tileHeight / 2 - tileHeight * x - 1));

                for (int y = 0; y < tilesY; y++)
                {
                    if ((x + y) % 2 != 0)
                        boardBuffer.Draw(tileShape, boardColor, windowCenter - boardCenter + new Location(x, y) * tileSize);
                }
            }
        }

        private void DrawPlayer()
        {
            var playerShape = new Rect(tileWidth, tileHeight);
            var playerColor = Colors.FOREGROUND_MAGENTA | Colors.FOREGROUND_INTENSITY;
            playerShape.Corners('╔', '╗', '╚', '╝');
            //playerShape.Sides('|', '-');
            playerShape.Sides('║', '-');

            var playerBuffer = context["player"];
            playerBuffer.Clear();
            playerBuffer.Draw(playerShape, playerColor, windowCenter - boardCenter + playerPos * tileSize);

            playerBuffer.Draw('>', playerColor, windowCenter + new Location(-boardCenter.x - tileCenter.x - 2, -boardCenter.y + (tileHeight / 2) + tileHeight * playerPos.y));
            playerBuffer.Draw("/\\", playerColor, windowCenter + new Location(tileWidth / 2 + tileWidth * playerPos.x - boardCenter.x, boardCenter.y + tileCenter.y + 2));
        }

        private void DrawPieces()
        {
            var pieceShape = new Rect(1, 1);
            var piecesBuffer = context["pieces"];
            var field = game.board.GetBoard();

            piecesBuffer.Clear();
            for (int x = 0; x < tilesX; x++)
            {
                for (int y = 0; y < tilesY; y++)
                {
                    ChessPiece piece = field[x, y];
                    if (piece.type != PieceType.None)
                    {
                        pieceShape.Fill(pieceChars[piece.color][piece.type]);
                        piecesBuffer.Draw(
                            pieceShape,
                            pieceColors[piece.color],
                            windowCenter - boardCenter + new Location(x * tileWidth, y * tileHeight) + tileCenter);
                    }
                }
            }
        }

        private void DrawWinner()
        {
            if (game.gameOver)
            {
                var playerBuffer = context["player"];
                string winString = $"{game.currentPlayer} won the game!";
                playerBuffer.Draw(winString, Colors.FOREGROUND_YELLOW | Colors.FOREGROUND_INTENSITY, windowCenter + new Location(-winString.Length / 2, 0));
            }
        }

        private void DrawText()
        {
            string playerString = $"Player: {game.currentPlayer}";
            string posString = $"{"ABCDEFGH"[playerPos.x]}{playerPos.y + 1}";
            string completeString = $"{playerString}\n{posString}";
            string pieceString = $"Piece: {game.board.TypeAt(playerPos)}".PadRight(20);
            var currentPlayerText = new TextBox(playerString.Length, 1);
            //currentPlayerText.Text("This is a longer text to test the capabilities of the text box :)");
            currentPlayerText.Text(playerString);
            //currentPlayerText.Text("This is a longer text to test the capabilities of the");

            var textBuffer = context["text"];

            //textBuffer.Draw(currentPlayerText, boardColor, windowCenter + boardCenter + new Location(2, -boardCenter.y - 5));
            textBuffer.Draw(playerString, boardColor, windowCenter + boardCenter + new Location(2, -boardCenter.y - 5));
            textBuffer.Draw(pieceString, boardColor, windowCenter + boardCenter + new Location(2, -boardCenter.y - 4));
            textBuffer.Draw(posString, boardColor, windowCenter + new Location(-boardCenter.x - posString.Length - 1, boardCenter.y + 1));
        }

        private void DrawMoves(Location[] locations)
        {
            var markerShape = new Rect(tileWidth, tileHeight);
            var markersBuffer = context["markers"];
            markerShape.Border('╔', '╗', '╚', '╝', '║', '═');
            markerShape.Border('▓');
            //markerShape.Border(' ');

            markersBuffer.Clear();
            if (locations != null)
            {
                for (int i = 0; i < locations.Length; i++)
                {
                    Location location = locations[i];
                    //markersBuffer.Draw(markerShape, Colors.FOREGROUND_YELLOW /*| Colors.FOREGROUND_INTENSITY*/ | Colors.BACKGROUND_YELLOW | Colors.BACKGROUND_INTENSITY, windowCenter - boardCenter + location * tileSize);
                    //markersBuffer.Draw(markerShape, Colors.FOREGROUND_YELLOW | Colors.FOREGROUND_INTENSITY | Colors.BACKGROUND_WHITE ^ Colors.BACKGROUND_INTENSITY /*| Colors.BACKGROUND_INTENSITY*/, windowCenter - boardCenter + location * tileSize);
                    //markersBuffer.Draw(markerShape, Colors.FOREGROUND_YELLOW | Colors.FOREGROUND_INTENSITY | Colors.BACKGROUND_WHITE /*| Colors.BACKGROUND_INTENSITY*/, windowCenter - boardCenter + location * tileSize * 2);
                    markersBuffer.Draw(markerShape, Colors.FOREGROUND_YELLOW | Colors.FOREGROUND_INTENSITY | Colors.BACKGROUND_INTENSITY, windowCenter - boardCenter + location * tileSize);
                    //markersBuffer.Draw(markerShape, Colors.FOREGROUND_YELLOW |  | Colors.BACKGROUND_INTENSITY, windowCenter - boardCenter + location * tileSize);
                }
            }
        }

        Location[] moves;
        private void Select()
        {
            if ((game.currentPlayer == PieceColor.White || !game.againstComputer) && !game.gameOver)
            {
                //Location playerPos = new Location(playerX, playerY);

                if (!selected.initialized)
                {
                    if (game.board.CanMove(game.currentPlayer, playerPos))
                    {
                        moves = game.board.GetMoves(playerPos);
                        if (moves != null && moves.Length > 0)
                            selected = playerPos;
                    }
                }
                else
                {
                    if ((bool)moves?.Contains(playerPos))
                    {
                        game.Move(selected, playerPos);
                        selected.initialized = false;
                        moves = null;
                    }
                    else if (playerPos == selected)
                    {
                        selected.initialized = false;
                        moves = null;
                    }
                }
                DrawMoves(moves);
            }
        }

        public void InitLayers()
        {
            context.CreateBuffer("board");
            context.CreateBuffer("pieces");
            context.CreateBuffer("markers");
            context.CreateBuffer("text");
            context.CreateBuffer("player");
        }

        public void InitControls()
        {
            inputManager.Register(KeyCode.Escape, new KeyHandler(KeyState.Pressed, deltaTime => Exit()));
            inputManager.Register(KeyCode.Space, new KeyHandler(KeyState.Pressed, deltaTime => Select()));

            inputManager.Register(KeyCode.Right, new KeyHandler(KeyState.Pressed, deltaTime =>
            {
                playerPos.x = Math.Min(tilesX - 1, playerPos.x + 1);
            }));
            inputManager.Register(KeyCode.Left, new KeyHandler(KeyState.Pressed, deltaTime =>
            {
                playerPos.x = Math.Max(0, playerPos.x - 1);
            }));
            inputManager.Register(KeyCode.Up, new KeyHandler(KeyState.Pressed, deltaTime =>
            {
                playerPos.y = Math.Max(0, playerPos.y - 1);
            }));
            inputManager.Register(KeyCode.Down, new KeyHandler(KeyState.Pressed, deltaTime =>
            {
                playerPos.y = Math.Min(tilesY - 1, playerPos.y + 1);
            }));
        }
    }
}
