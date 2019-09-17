using ConsoleLibrary;
using ConsoleLibrary.Api.WinApi.Constants;
using ConsoleLibrary.Graphics.Drawing;
using ConsoleLibrary.Graphics.Shapes;
using ConsoleLibrary.Input;
using ConsoleLibrary.Structures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleLibrary.Input.InputManager;

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

        Dictionary<PieceType, char> pieceChars = new Dictionary<PieceType, char>
        {
            { PieceType.Pawn,   '\u2659' },
            { PieceType.Rook,   'R' },
            { PieceType.Knight, 'N' },
            { PieceType.Bishop, 'B' },
            { PieceType.Queen,  'Q' },
            { PieceType.King,   'K' },
        };

        Dictionary<PieceColor, int> pieceColors = new Dictionary<PieceColor, int>
        {
            { PieceColor.White, Colors.BACKGROUND_WHITE },
            { PieceColor.Black, Colors.FOREGROUND_WHITE },
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

        int playerX = 0;
        int playerY = 0;

        readonly int boardColor = Colors.FOREGROUND_GREEN | Colors.FOREGROUND_INTENSITY;

        //ChessBoard board;
        ChessGame game;

        public override void Init()
        {
            base.Init();
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
            game = new ChessGame(false);
            //board = new ChessBoard();
            //board.GetMoves(new Location(0, 1));
            DrawBoard();
        }

        public void Update()
        {
            DrawPieces();
            DrawText();
            DrawPlayer();
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
            playerShape.Corners('╔', '╗', '╚', '╝');
            playerShape.Sides('|', '-');

            var playerBuffer = context["player"];
            playerBuffer.Clear();
            playerBuffer.Draw(playerShape, Colors.FOREGROUND_MAGENTA | Colors.FOREGROUND_INTENSITY, windowCenter - boardCenter + new Location(playerX, playerY) * tileSize);
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
                        pieceShape.Fill(pieceChars[piece.type]);
                        piecesBuffer.Draw(
                            pieceShape,
                            pieceColors[piece.color],
                            windowCenter - boardCenter + new Location(x * tileWidth, y * tileHeight) + tileCenter);
                    }
                }
            }
        }

        private void DrawText()
        {
            var currentPlayerText = new TextBox(20, 4);
            currentPlayerText.Text("This is a longer text to test the capabilities of the text box :)");

            var textBuffer = context["text"];

            textBuffer.Draw(currentPlayerText, boardColor, windowCenter - new Location(currentPlayerText.width, currentPlayerText.height) /2 );
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
                }
            }
        }

        Location[] moves;
        private void Select()
        {
            if (game.currentPlayer == PieceColor.White || !game.againstComputer)
            {
                Location playerPos = new Location(playerX, playerY);

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
                playerX = Math.Min(tilesX - 1, playerX + 1);
            }));
            inputManager.Register(KeyCode.Left, new KeyHandler(KeyState.Pressed, deltaTime =>
            {
                playerX = Math.Max(0, playerX - 1);
            }));
            inputManager.Register(KeyCode.Up, new KeyHandler(KeyState.Pressed, deltaTime =>
            {
                playerY = Math.Max(0, playerY - 1);
            }));
            inputManager.Register(KeyCode.Down, new KeyHandler(KeyState.Pressed, deltaTime =>
            {
                playerY = Math.Min(tilesY - 1, playerY + 1);
            }));
        }
    }
}
