//using ConsoleLibrary;
//using ConsoleLibrary.Graphics.Drawing;
//using ConsoleLibrary.Graphics.Shapes;
//using ConsoleLibrary.Structures;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WindowsWrapper.Constants;
////using static ConsoleLibrary.InputManager;


using ConsoleLibrary.Drawing;
//using ConsoleLibrary.Forms.Components;
using ConsoleLibrary.Game;
using ConsoleLibrary.Structures;
using ConsoleLibrary.TextExtensions;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Xml.Schema;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;

namespace ConsoleApiTest.Chess
{
    public class ChessApp : GameApp
    {
        Point tileSize;
        Point borderSize;
        Point center;
        Point boardSize;
        Point boardCenter;
        Point boardPosition;
        Point markerPos;
        Point selectedPiece;
        Rectangle boardBounds;

        //PieceColor currentPlayer = PieceColor.White;

        Point[] possibleMoves;

        string horizontalGlyphs = "ABCDEFGH";
        string verticalGlyphs = "87654321";
        float backgroundAngle = 0.0f;


        //Button restartButton;
        GameOverPanel gameOverPanel;
        PromotionPanel promitionPanel;
        Border selectionMarker;
        Border moveMarker;
        BufferArea backgroundBuffer;
        BufferArea boardBuffer;
        //BufferArea gameOverBuffer;
        ChessGame game;

        Dictionary<PieceColor, Dictionary<PieceType, char>> pieceChars = new Dictionary<PieceColor, Dictionary<PieceType, char>>
        {
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
        };


        public ChessApp(int width, int height) : base(width, height) { }

        public override void Initialize(Harness harness)
        {
            base.Initialize(harness);

            game = new ChessGame();
            game.LastRank += LastRankHandler;
            game.GameOver += GameOver;
            center = new Point(width / 2, height / 2);
            selectionMarker = new DoubleBorder();
            moveMarker = new DoubleBorder();

            Title = "Chess";
            SetFont("MS Gothic", 9, 18);
            InitBoard();
            InitBackground();

            gameOverPanel = new GameOverPanel(game);
            gameOverPanel.Visible = false;
            gameOverPanel.Position = center - gameOverPanel.Center;
            gameOverPanel.Restart += Reset;

            //restartButton = new Button("Restart");
            //restartButton.Position = center - (restartButton.Text.Length / 2, -4);
            //restartButton.Click += Reset;

            //promitionPanel = new PromotionPanel();
            //promitionPanel.Visible = false;
            //promitionPanel.Position = center - promitionPanel.Center;
            //promitionPanel.Promotion += PromotionHandler;
        }

        private void LastRankHandler(Point location)
        {
            selectedPiece = location;
            //Draw(ConsoleRenderer.ActiveBuffer);
            var pieceType = new PromotionPrompt().GetResult();
            game.Promote(selectedPiece, pieceType);
            //promitionPanel.Visible = true;
        }

        //private void PromotionHandler(PieceType pieceType)
        //{
        //    game.Promote(selectedPiece, pieceType);
        //    promitionPanel.Visible = false;
        //}

        private void InitBackground()
        {
            backgroundBuffer = new BufferArea(width, height);
            var white = CharAttribute.BackgroundWhite;
            var lightGray = CharAttribute.BackgroundGray;
            var darkGray = CharAttribute.BackgroundDarkGray;
            var black = CharAttribute.BackgroundBlack;
            var blue = CharAttribute.BackgroundBlue;
            var darkBlue = CharAttribute.BackgroundDarkBlue;
            var darkMagenta = CharAttribute.BackgroundDarkMagenta;
            var magenta = CharAttribute.BackgroundMagenta;
            Gradient gradient = new Gradient(black, darkBlue, black);
            gradient = new Gradient(black, darkBlue, blue, darkBlue, black);
            //gradient = new Gradient(CharAttribute.BackgroundDarkRed, CharAttribute.BackgroundDarkMagenta, CharAttribute.BackgroundCyan);
            //gradient.Reverse();
            backgroundBuffer.Draw(gradient, 0, 0, width, height, true);

            for (int i = 0; i < 8; i++)
            {
                backgroundBuffer.Draw(horizontalGlyphs[i], boardPosition.X + tileSize.X * i + borderSize.X + tileSize.X / 2 - 1, boardPosition.Y + boardSize.Y + 1);
                backgroundBuffer.Draw(verticalGlyphs[7 - i], boardPosition.X - 2, boardPosition.Y + boardSize.Y - tileSize.Y * i - borderSize.Y - tileSize.Y / 2 - 1);
            }

            //backgroundBuffer.Draw(gradient, 0, 0, width / 2, height);
            //gradient.Reverse();
            //backgroundBuffer.Draw(gradient, width / 2, 0, width / 2, height);
        }

        private void InitBoard()
        {
            int tileWidth = 6;
            tileSize = new Point(tileWidth, tileWidth / 2);
            int borderWidth = 1;
            borderSize = new Point(borderWidth * 2, borderWidth);

            boardSize = tileSize * 8 + borderSize * 2;
            boardCenter = boardSize / 2;
            boardPosition = center - boardCenter;
            boardBuffer = new BufferArea(boardSize.X, boardSize.Y);
            boardBounds = new Rectangle(boardPosition + borderSize, boardSize - borderSize * 2 - tileSize + new Point(1, 1));

            boardBuffer.Fill(new CharInfo { UnicodeChar = ShadingCharacter.Dark, Attributes = CharAttribute.ForegroundDarkGreen });

            for (int by = 0; by < 8; by++)
            {
                for (int bx = 0; bx < 8; bx++)
                {
                    CharInfo info = new CharInfo
                    {
                        UnicodeChar = (bx + by) % 2 == 0 ? ShadingCharacter.Dark : '\0',
                        Attributes = (bx + by) % 2 == 0 ? CharAttribute.BackgroundGreen : CharAttribute.BackgroundBlack
                    };
                    boardBuffer.FillRect(bx * tileSize.X + borderSize.X, by * tileSize.Y + borderSize.Y, tileSize.X, tileSize.Y, info);
                }
            }
        }

        private void Reset()
        {
            gameOverPanel.Visible = false;
            game.Reset();
        }

        public override void Update(float deltaTime)
        {
            InputManager.Update();

            //if (InputManager.IsFirstPressed(VirtualKey.A))
            //    game.gameOver = true;

            if (InputManager.IsPressed(VirtualKey.LeftControl) && InputManager.IsPressed(VirtualKey.C))
                Exit();

            Point mousePos = InputManager.GetMousePosition();
            markerPos = (mousePos - boardPosition - borderSize) / tileSize;
            var b = new Rectangle(0, 0, 8, 8);
            markerPos.initialized = new Rectangle(boardPosition + borderSize, boardSize - borderSize * 2).ContainsPoint(mousePos);
            markerPos.Bound(new Rectangle(0, 0, 8, 8));


            if (game.gameOver)
            {

                //new PromotionPrompt().GetResult();
                gameOverPanel.Update();
                //restartButton.Update();
            }
            else
            {
                if (InputManager.IsFirstPressed(VirtualKey.LeftButton))
                {
                    if (possibleMoves != null && possibleMoves.Length > 0 && game.board.ColorAt(markerPos) != game.currentPlayer)
                    {
                        foreach (var move in possibleMoves)
                        {
                            if (move == markerPos)
                            {
                                game.Move(selectedPiece, move);
                                possibleMoves = null;
                            }
                        }
                    }
                    else if (game.board.ColorAt(markerPos) == game.currentPlayer)
                    {
                        if (markerPos == selectedPiece && possibleMoves != null)
                            possibleMoves = null;
                        else
                            possibleMoves = game.board.GetMoves(markerPos);

                        if (possibleMoves != null)
                            selectedPiece = markerPos;
                    }
                }
                //promitionPanel.Update();
            }

            harness.RequestRedraw();
        }

        private void GameOver()
        {
            gameOverPanel.Visible = true;
        }


        private void DrawBackground(BufferArea mainBuffer)
        {
            //backgroundBuffer.Clear(CharAttribute.BackgroundDarkGray);
            //var black = CharAttribute.BackgroundBlack;
            //var blue = CharAttribute.BackgroundBlue;
            //var darkBlue = CharAttribute.BackgroundDarkBlue;
            //Gradient gradient = new Gradient(black, darkBlue, black);
            //gradient = new Gradient(black, darkBlue, blue, darkBlue, black);
            ////Gradient gradient = new Gradient(CharAttribute.BackgroundDarkBlue, CharAttribute.BackgroundBlack, CharAttribute.BackgroundDarkBlue);
            //var pos = center - boardCenter;
            ////backgroundBuffer.Draw(gradient, 0, 0, width, height, true);
            ////backgroundBuffer.DrawRotatedGradient(gradient, 0, 0, width, height, backgroundAngle);
            //backgroundBuffer.DrawRotatedGradientC(gradient, 0, 0, width, height, backgroundAngle);
            //backgroundBuffer.DrawRotatedGradientC(gradient, 4 * 2, 2 * 2, width - 8 * 2, height - 4 * 2, backgroundAngle);
            //backgroundBuffer.DrawRotatedGradient(
            //    gradient,
            //    pos.X,
            //    pos.Y,
            //    boardSize.X,
            //    boardSize.Y,
            //    backgroundAngle);
            //backgroundBuffer.Draw(backgroundAngle.ToString(), 0, 0);
            mainBuffer.Draw(backgroundBuffer, 0, 0);
        }

        private void DrawBoard(BufferArea mainBuffer, Point position)
        {
            var options = new DrawingOptions();
            options.X = position.X;
            options.Y = position.Y;
            mainBuffer.Draw(boardBuffer, options);
        }

        private void DrawPieces(BufferArea mainBuffer, Point position)
        {
            var board = game.board.GetBoard();

            for (int y = 0; y < board.GetLength(0); y++)
            {
                for (int x = 0; x < board.GetLength(1); x++)
                {
                    var piece = board[x, y];

                    if (piece.type != PieceType.None)
                    {
                        Point offset = borderSize + tileSize / 2 + new Point(-1, 0);
                        Point pos = position + tileSize * new Point(x, y) + offset;

                        CharAttribute attribute = piece.color == PieceColor.White
                            ? CharAttribute.ForegroundWhite
                            : CharAttribute.ForegroundDarkGray;
                        attribute |= CharAttribute.LeadingByte;

                        //mainBuffer.Draw(pieceChars[piece.color][piece.type], pos.X, pos.Y, attribute | CharAttribute.TrailingByte);
                        //mainBuffer.Draw(pieceChars[piece.color][piece.type], pos.X + 1, pos.Y, attribute | CharAttribute.LeadingByte);
                        mainBuffer.Draw(pieceChars[piece.color][piece.type], pos.X, pos.Y, attribute);
                        //mainBuffer.Draw(pieceChars[piece.color][piece.type], pos.X + 1, pos.Y, attribute | CharAttribute.LeadingByte);
                    }
                }
            }
        }

        private void DrawMarker(BufferArea mainBuffer)
        {
            if (!markerPos.initialized || game.gameOver)
                return;

            Point markerDrawPos = markerPos * tileSize + boardPosition + borderSize;
            mainBuffer.Draw(selectionMarker, markerDrawPos.X, markerDrawPos.Y, tileSize.X, tileSize.Y, CharAttribute.ForegroundMagenta);
        }

        private void DrawInfo(BufferArea mainBuffer)
        {
            if (game.gameOver)
                return;

            if (markerPos.initialized)
            {
                var type = game.board.TypeAt(markerPos);
                mainBuffer.Draw($"{type}", boardPosition.X + boardSize.X + 1, boardPosition.Y);
                mainBuffer.Draw($"{horizontalGlyphs[markerPos.X]}{verticalGlyphs[markerPos.Y]}", boardPosition.X + boardSize.X + 1, boardPosition.Y + 1);
            }

            if (game.gameOver)
                mainBuffer.Draw("Game Over!", boardPosition.X + boardCenter.X - 5, boardPosition.Y - 2);


            mainBuffer.Draw("Current player: " + game.currentPlayer, 0, 0);
        }

        private void DrawMoves(BufferArea mainBuffer)
        {
            if (possibleMoves == null)
                return;

            foreach (Point move in possibleMoves)
            {
                mainBuffer.Draw(moveMarker, boardPosition + borderSize + move * tileSize, tileSize, CharAttribute.ForegroundYellow);
            }
        }

        public override void Draw(BufferArea buffer)
        {
            buffer.Clear();
            DrawBackground(buffer);
            DrawBoard(buffer, boardPosition);
            DrawPieces(buffer, boardPosition);
            if (!game.gameOver)
            {
                DrawMarker(buffer);
                DrawMoves(buffer);
                DrawInfo(buffer);
            }
            gameOverPanel.Draw(buffer);
            //promitionPanel.Draw(buffer);
        }
    }

    class GameOverPanel : Panel
    {
        public event Action Restart;

        private ChessGame game;
        public GameOverPanel(ChessGame game)
        {
            this.game = game;

            Title = "Game Over!";
            Size = (60, 15);

            Button restartButton = new Button("Restart");
            restartButton.Click += () => Restart?.Invoke();
            restartButton.Position = (Center.X - restartButton.Text.Width() / 2, 10);
            Components.Add(restartButton);
        }
    }

    class PromotionPanel : Panel
    {
        public delegate void PromotionEventHandler(PieceType pieceType);

        public event PromotionEventHandler Promotion;

        public PromotionPanel()
        {
            Title = "Piece Promotion";
            Size = (Title.Width() + 4, 13);

            Button queenButton = new Button("Queen: ♛");
            queenButton.Position = (Size.X / 2 - queenButton.Text.Width() / 2, 3);
            queenButton.Click += () => Promotion?.Invoke(PieceType.Queen);
            Components.Add(queenButton);

            Button rookButton = new Button("Rook: ♜");
            rookButton.Position = (Size.X / 2 - rookButton.Text.Width() / 2, 5);
            rookButton.Click += () => Promotion?.Invoke(PieceType.Rook);
            Components.Add(rookButton);

            Button bishopButton = new Button("Bishop: ♝");
            bishopButton.Position = (Size.X / 2 - bishopButton.Text.Width() / 2, 7);
            bishopButton.Click += () => Promotion?.Invoke(PieceType.Bishop);
            Components.Add(bishopButton);

            Button knightButton = new Button("Knight: ♞");
            knightButton.Position = (Size.X / 2 - knightButton.Text.Width() / 2, 9);
            knightButton.Click += () => Promotion?.Invoke(PieceType.Knight);
            Components.Add(knightButton);
        }
    }

    class Panel : Component
    {
        public string Title { get => title; set => title = value.PadLeft(value.Length + 2).PadRight(value.Length + 4); }

        private string title;

        public Panel()
        {
            Visible = true;
        }

        public Panel(string title) : this()
        {
            Title = title;
        }

        public override void Draw(BufferArea buffer)
        {
            if (!Visible) return;

            buffer.FillRect(new Rectangle(Position, Size));
            buffer.Draw(DoubleBorder.Instance, Position, Size);
            buffer.Draw(title, Position.X + Size.X / 2 - title.Length / 2, Position.Y, CharAttribute.ForegroundYellow);
            base.Draw(buffer);
        }
    }

    class Button : InputComponent
    {
        public string Text { get => text; set => SetText(value); }

        //public event Action Click;

        protected string text;
        //private int width;
        //private bool mouseOver;
        //private bool mouseDown;
        private CharAttribute[] attributes =
        {
            ConsoleRenderer.DefaultAttributes,
            CharAttribute.BackgroundBlue | CharAttribute.ForegroundWhite,
            CharAttribute.BackgroundDarkBlue,
            CharAttribute.BackgroundDarkBlue
        };

        public Button() { }

        public Button(string text)
        {
            Text = text;
        }

        protected virtual void SetText(string value)
        {
            text = value;
            Width = value.Width();
        }

        protected CharAttribute GetColor()
        {
            return attributes[Convert.ToInt32(mouseOver) | Convert.ToInt32(mouseDown) << 1];
        }

        public override void Draw(BufferArea buffer)
        {
            CharAttribute attribute = GetColor();
            buffer.Draw(text, ClientPosition.X, ClientPosition.Y, attribute);
        }
    }

    class PromotionPrompt : Prompt<PieceType>
    {
        public PromotionPrompt()
        {
            Title = "Piece Promotion";
            Size = (Title.Width() + 4, 13);
            Position = ConsoleRenderer.GetConsoleSize() / 2 - Center;

            Button queenButton = new Button("Queen: ♛");
            queenButton.Position = (Size.X / 2 - queenButton.Text.Width() / 2, 3);
            queenButton.Click += () => result = PieceType.Queen;
            Components.Add(queenButton);

            Button rookButton = new Button("Rook: ♜");
            rookButton.Position = (Size.X / 2 - rookButton.Text.Width() / 2, 5);
            rookButton.Click += () => result = PieceType.Rook;
            Components.Add(rookButton);

            Button bishopButton = new Button("Bishop: ♝");
            bishopButton.Position = (Size.X / 2 - bishopButton.Text.Width() / 2, 7);
            bishopButton.Click += () => result = PieceType.Bishop;
            Components.Add(bishopButton);

            Button knightButton = new Button("Knight: ♞");
            knightButton.Position = (Size.X / 2 - knightButton.Text.Width() / 2, 9);
            knightButton.Click += () => result = PieceType.Knight;
            Components.Add(knightButton);
        }
    }

    abstract class Prompt<T> : InputComponent where T : struct, Enum
    {
        public string Title { get => title; set => title = value.PadLeft(value.Length + 2).PadRight(value.Length + 4); }

        //protected List<Component> components = new List<Component>();
        private string title;
        protected T? result;

        public T GetResult()
        {
            while (result == null)
            {
                Update();
                Draw(ConsoleRenderer.ActiveBuffer);
                System.Threading.Thread.Sleep(1);
            }
            return result ?? default;
        }

        public override void Update()
        {
            InputManager.Update();
            base.Update();
            foreach (var component in Components)
            {
                component.Update();
            }
        }

        public override void Draw(BufferArea buffer)
        {
            buffer.FillRect(new Rectangle(Position, Size));
            buffer.Draw(DoubleBorder.Instance, Position, Size);
            buffer.Draw(title, Position.X + Size.X / 2 - title.Length / 2, Position.Y, CharAttribute.ForegroundYellow);
            foreach (var component in Components)
            {
                component.Draw(buffer);
            }
            ConsoleRenderer.RenderOutput();
        }

    }

    abstract class InputComponent : Component
    {
        public event Action Click;

        protected bool mouseOver;
        protected bool mouseDown;

        public override void Update()
        {
            var mousePosition = InputManager.GetMousePosition();
            mouseOver = PointIntersects(mousePosition);

            if (mouseOver && InputManager.IsFirstPressed(VirtualKey.LeftButton))
                mouseDown = true;

            if (mouseOver && mouseDown && InputManager.IsFirstReleased(VirtualKey.LeftButton))
                Click?.Invoke();

            if (InputManager.IsReleased(VirtualKey.LeftButton))
                mouseDown = false;
        }
    }

    abstract class Component
    {
        public class ComponentCollection : List<Component>
        {
            private readonly Component owner;

            public ComponentCollection(Component owner)
            {
                this.owner = owner;
            }

            public new void Add(Component component)
            {
                component.Parent?.Components.Remove(component);
                component.parent = owner;
                base.Add(component);
            }
        }

        public Component Parent => parent;
        public ComponentCollection Components => components;
        public bool Visible { get; set; }
        public Point Position { get => position; set => UpdatePosition(value); }
        public Point ClientPosition => parent == null ? position : parent.position + position;
        public int Left { get => Position.X; set => UpdatePosition((value, Position.Y)); }
        public int Top { get => Position.Y; set => UpdatePosition((Position.X, value)); }

        public Point Size { get => size; set => size = (Math.Max(1, value.X), Math.Max(1, value.Y)); }
        public int Width { get => size.X; set => Size = (value, size.Y); }
        public int Height { get => size.Y; set => Size = (size.X, value); }

        public Point Center => size / 2;

        private Point position;
        private Point size;
        private Component parent;
        private readonly ComponentCollection components;

        public Component()
        {
            components = new ComponentCollection(this);
        }

        protected virtual void UpdatePosition(Point newPosition)
        {
            position = newPosition;
        }

        public virtual void Update()
        {
            if (!Visible) return;
            foreach (var component in Components)
                component.Update();
        }
        public virtual void Draw(BufferArea buffer)
        {
            if (!Visible) return;
            foreach (var component in Components)
                component.Draw(buffer);
        }

        protected bool PointIntersects(Point p)
        {
            Point position = ClientPosition;
            return p.Y >= position.Y && p.Y < position.Y + size.Y &&
                   p.X >= position.X && p.X < position.X + size.X;
        }
    }
}