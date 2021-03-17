using ConsoleLibrary.Forms;
using ConsoleLibrary.Forms.Controls;
using ConsoleLibrary.Drawing;
using ConsoleLibrary.Input;
using ConsoleLibrary.Input.Events;
using System;
using System.Diagnostics;
using System.Drawing;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;
using ConsoleLibrary;
using WindowsWrapper;
using ConsoleLibrary.TextExtensions;
using System.Linq;

namespace ConsoleApiTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //var t = (SystemMetric[])Enum.GetValues(typeof(SystemMetric));

            //foreach (var metric in t)
            //{
            //    Console.WriteLine($"{metric} = {WinApi.GetSystemMetrics(metric)}");
            //}
            //WinApi

            //bool equal = true;
            //for (int i = 0; i < 100; i++)
            //{
            //    Debug.WriteLine($"{i} => {A(i, 10, 100)}, {B(i, 10, 100)}");
            //    if (A(i, 10, 100) != B(i, 10, 100) )
            //        equal = false;
            //}
            //Console.WriteLine(equal);
            //Console.ReadLine();

            //new TestApp(84, 42).Init();
            new TestApp(80, 20).Init();
            ConsoleInput.InputLoop();
        }

        private static int A(int a, int b, int c) => Math.Min(c - a, b);

        private static int B(int a, int b, int c) => (c - a) < b ? c - a : b;
    }

    public class TestApp : FormApp
    {
        private readonly string lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

        BufferArea buffer;
        BufferArea backface;
        CharInfo[,] square;
        int squareX = 2;
        int squareY = 1;
        TextBox textBox;
        ColorfulString colorfulString;
        string[] strings;

        public TestApp(int width = 90, int height = 40) : base(width, height)
        {
            Title = "Control Testing";
        }

        public override void Init()
        {
            base.Init();

            //ConsoleInput.KeyPressed += ConsoleInput_KeyPressed;

            //return;
            buffer = ConsoleRenderer.ActiveBuffer;

            textBox = new TextBox(controlManager)
            {
                Left = Width / 2,
                Top = Height / 2,
                Text = lorem,
                Width = 20,
                Height = 5,
                Attributes = CharAttribute.ForegroundWhite
            };

            colorfulString = new ColorfulString
            {
                Value = new string(Enumerable.Repeat('A', Enum.GetValues(typeof(CharAttribute)).Length - 0).ToArray()),
                ColorThing = ColorSelectMode.Repeat,
                Attributes = (CharAttribute[])Enum.GetValues(typeof(CharAttribute))
            };


            strings = new string[]
            {
                "Haha",
                "Console",
                "Go",
                "Brrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr"
            }.NormalizeLengths()
            .PadAround(2);

            int ratioX = 2;
            int ratioY = 1;
            int scale = 2;
            int spacingX = ratioX * scale;
            int spacingY = ratioY * scale;
            int pieceWidth = 2;
            int pieceHeight = 1;
            int tileWidth = pieceWidth + spacingX * 2;
            int tileHeight = pieceHeight + spacingY * 2;
            square = new CharInfo[8 * tileHeight, 8 * tileWidth];

            for (int y = 0; y < square.GetLength(0); y++)
                for (int x = 0; x < square.GetLength(1); x++)
                    square[y, x].Attributes |= ((x / tileWidth + y / tileHeight) % 2 == 0
                        ? CharAttribute.BackgroundGrey | CharAttribute.ForegroundBlack
                        : CharAttribute.BackgroundBlack | CharAttribute.ForegroundWhite);

            for (int y = 0; y < square.GetLength(0); y++)
                for (int x = 0; x < square.GetLength(1); x++)
                {
                    if (x % tileWidth == 0 && y % tileHeight == 0)
                    {
                        int px = x + spacingX;
                        int py = y + spacingY;
                        square[py, px].UnicodeChar = '♕';
                        square[py, px].Attributes |= CharAttribute.LeadingByte;

                        var yellow = CharAttribute.BackgroundYellow | CharAttribute.ForegroundBlack;
                        var marker = new CharInfo
                        {
                            UnicodeChar = ShadingCharacter.Light,
                            Attributes = yellow
                        };
                    }
                }

            backface = new BufferArea(square.GetLength(1) + 4, square.GetLength(0) + 2);
            CharInfo backChar = new CharInfo
            {
                UnicodeChar = ShadingCharacter.Dark,
                Attributes = CharAttribute.BackgroundDarkGrey
            };
            backface.Fill(backChar);


            //Draw();

            ConsoleInput.KeyPressed += ConsoleInput_KeyPressed;

            //ConsoleInput.KeyHeld += ConsoleInput_KeyPressed;

            //ConsoleInput.MouseDragged += ConsoleInput_MouseDragged;

            ConsoleInput.Resized += delegate
            {
                Draw();
            };
            Draw();
        }

        int prevX = 0;
        int prevY = 0;
        private void ConsoleInput_MouseDragged(object sender, MouseEventArgs e)
        {
            //Debug.WriteLine("Drag");
            (int x, int y) = e.Location;

            if (x != prevX || y != prevY)
            {
                prevX = x;
                prevY = y;
                squareX = x - square.GetLength(1) / 2;
                squareY = y - square.GetLength(0) / 2;
                Draw();
            }
        }

        private void ConsoleInput_KeyPressed(KeyEventArgs e)
        {
            if (e.Key == ConsoleKey.Escape)
            {
                //ConsoleRenderer.Clear();
                Exit();
            }


            //if (e.Key == ConsoleKey.S && e.ControlKeyState.HasFlag(ControlKeyState.LeftCtrlPressed))
            //{
            //    var h = Process.GetCurrentProcess().MainWindowHandle;
            //    //using (var image = )
            //    (int ww, int wh) = ClientSize;
            //    using (Bitmap b = new Bitmap(ww, wh))
            //    {
            //        using (var p = Brushes.CornflowerBlue)
            //        {
            //            using (Graphics g = Graphics.FromImage(b))
            //            {
            //                //g.draw
            //                var points = MapToScreen(new Point[] { new Point(0, 0) });
            //                //g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size((int)g.VisibleClipBounds.Width, (int)g.VisibleClipBounds.Height));
            //                //g.
            //                b.Save("Test.png");
            //            }
            //            //g.FillEllipse(p, 0, 0, g.VisibleClipBounds.Width, g.VisibleClipBounds.Height);
            //        }
            //    }
            //}
            //switch (e.Key)
            //{
            //    case ConsoleKey.A:
            //    case ConsoleKey.LeftArrow:
            //        squareX--;
            //        break;
            //    case ConsoleKey.W:
            //    case ConsoleKey.UpArrow:
            //        squareY--;
            //        break;
            //    case ConsoleKey.D:
            //    case ConsoleKey.RightArrow:
            //        squareX++;
            //        break;
            //    case ConsoleKey.S:
            //    case ConsoleKey.DownArrow:
            //        squareY++;
            //        break;
            //    default:
            //        break;
            //}
            //Draw();

            //Draw();
            //if (e.Key == ConsoleKey.F)
            //MyConsole.Fill(new CharInfo());
            //ConsoleRenderer.Clear();
            //    buffer.Clear();
        }

        private void Draw()
        {
            //buffer.Clear(CharAttribute.BackgroundDarkYellow);
            int i = 0;
            Gradient gradient = new Gradient(CharAttribute.BackgroundDarkGrey, CharAttribute.ForegroundWhite, CharAttribute.BackgroundGreen, CharAttribute.BackgroundMagenta, CharAttribute.BackgroundCyan, CharAttribute.BackgroundYellow);
            gradient = new Gradient(CharAttribute.BackgroundCyan, CharAttribute.BackgroundMagenta, CharAttribute.ForegroundYellow, CharAttribute.BackgroundBlack);
            gradient = new Gradient(CharAttribute.BackgroundBlue, CharAttribute.BackgroundCyan, CharAttribute.BackgroundGreen);
            gradient = new Gradient(CharAttribute.BackgroundBlack, CharAttribute.BackgroundDarkGrey, CharAttribute.BackgroundGrey, CharAttribute.BackgroundWhite);
            gradient = new Gradient(CharAttribute.BackgroundBlack, CharAttribute.BackgroundDarkBlue, CharAttribute.BackgroundBlue);
            gradient.Reverse();
            //int w = (int)Math.Ceiling(Width / (double)gradient.Pallette.Length);
            //float w = Width / (float)gradient.Pallette.Length;

            //foreach (var color in gradient.Pallette)
            //{
            //    //buffer.FillRect((int)(w * i), 0, (int)(w * i++), Height, color);
            //    buffer.FillRect((int)(w * i), 0, (int)Math.Ceiling(w * i++), Height, color);
            //}

            buffer.Draw(gradient, 0, 0, Width, Height/2);
            gradient.Reverse();
            buffer.Draw(gradient, 0, Height/2, Width, Height);

            //buffer.Draw(backface, squareX - 2, squareY - 1);
            //buffer.Draw(square, squareX, squareY);
            //buffer.Draw(colorfulString, 0, strings.Length);
            //buffer.Draw('\u2020', 35, 0, CharAttribute.LeadingByte);
            //buffer.Draw('♕', 34, 10, CharAttribute.ForegroundCyan | CharAttribute.LeadingByte);
            //buffer.Draw('♕', 35, 10, CharAttribute.ForegroundCyan | CharAttribute.TrailingByte);
            //buffer.Draw('A', 35, 10, CharAttribute.ForegroundCyan | CharAttribute.TrailingByte);
            buffer.Draw(strings, Width / 2 - strings[0].Length / 2, Height / 2 - strings.Length / 2, CharAttribute.ForegroundWhite);
            //buffer.Draw(strings, Width / 2 - strings[0].Length / 2, Height - strings.Length, CharAttribute.ForegroundWhite);
            buffer.Draw("Hello", 10, Height - 1);
            //buffer.Draw()
            //controlManager.DrawControls();
            ConsoleRenderer.RenderOutput();
        }

        private void Control_MousePressed(object sender, MouseEventArgs e)
        {
            Debug.WriteLine((sender as Control)?.Name + " pressed");
        }

        private void Control_MouseMoved(object sender, MouseEventArgs e)
        {
            Debug.WriteLine((sender as Control)?.Name + " moved");
        }

        private void Control_MouseLeave(object sender, MouseEventArgs e)
        {
            Debug.WriteLine((sender as Control)?.Name + " left");
        }

        private void Control_MouseEnter(object sender, MouseEventArgs e)
        {
            Debug.WriteLine((sender as Control)?.Name + " entered");
        }
    }
}
