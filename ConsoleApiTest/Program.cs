using ConsoleLibrary.Forms;
using ConsoleLibrary.Forms.Controls;
using ConsoleLibrary.Drawing;
using ConsoleLibrary.Input;
using ConsoleLibrary.Input.Events;
using System;
using System.Diagnostics;
//using System.Drawing;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;
using ConsoleLibrary;
using WindowsWrapper;
using ConsoleLibrary.TextExtensions;
using System.Linq;
using ConsoleLibrary.Structures;

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

            Console.WriteLine(new { hello = "test" });


            //new TestApp(80, 20).Init();
            //ConsoleInput.InputLoop();
            Console.ReadLine();
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
        TextBox dataBox;
        VerticalScrollbar vs;
        HorizontalScrollbar hs;
        ColorfulString colorfulString;
        string[] strings;

        int anchorX = 0;
        int anchorY = 0;
        bool active = false;
        bool scalingX = false;
        bool scalingY = false;

        static Rectangle r0 = new Rectangle(20, 10, 8, 4);
        static Rectangle r1 = new Rectangle(22, 11, 8, 4);
        Rectangle r2 = r0.Intersect(r1);

        public TestApp(int width = 90, int height = 40) : base(width, height)
        {
            Title = "Control Testing";
        }

        public override void Init()
        {
            base.Init();

            buffer = ConsoleRenderer.ActiveBuffer;

            textBox = new TextBox(controlManager)
            {
                Width = 34,
                Height = 13,
                Text = "Hello there",
                Attributes = CharAttribute.ForegroundWhite | CharAttribute.BackgroundDarkRed,
                WordBreak = WordBreak.Hard,
                TextAlign = TextAlign.Left
            };
            textBox.Text = lorem;
            textBox.MousePressed += TextBox_MousePressed;
            textBox.Visible = false;
            //textBox.MouseReleased += TextBox_MouseReleased;

            dataBox = new TextBox(controlManager)
            {
                Width = 20,
                Height = 2,
                Name = "hello",
                Text = textBox.Rectangle.Size.ToString(),
                Attributes = CharAttribute.ForegroundGreen
            };
            dataBox.Width = dataBox.Text.Length;
            dataBox.Visible = false;


            vs = new VerticalScrollbar(controlManager)
            {
                Height = 5,
                Left = 20,
                Top = 4
            };

            hs = new HorizontalScrollbar(controlManager)
            {
                Width = 10,
                Left = 3,
                Top = Height - 1
            };

            new ScrollableTextBox(controlManager)
            {
                Width= 20,
                Height= 5
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

            ConsoleInput.KeyHeld += ConsoleInput_KeyPressed;

            ConsoleInput.MouseReleased += TextBox_MouseReleased;

            ConsoleInput.MouseDragged += ConsoleInput_MouseDragged;

            ConsoleInput.Resized += ConsoleInput_Resized;
            //ConsoleInput.Resized += delegate
            //{
            //    Draw();
            //};
            Draw();
        }

        private void ConsoleInput_Resized(ResizedEventArgs keyEventArgs)
        {
            vs.BeginUpdate();
            vs.Top = 0;
            vs.Left = Width - vs.Width;
            vs.Height = Height - 1;
            vs.EndUpdate();

            textBox.Visible = false;
            hs.BeginUpdate();
            hs.Left = 0;
            hs.Top = Height - 1;
            hs.Width = Width - 2;
            hs.EndUpdate();
            Draw();
        }

        private void TextBox_MousePressed(object sender, MouseEventArgs e)
        {
            (int x, int y) = e.Location;
            scalingX = x == textBox.Right && e.ControlKeyState.HasFlag(ControlKeyState.ShiftPressed);
            scalingY = y == textBox.Bottom && e.ControlKeyState.HasFlag(ControlKeyState.ShiftPressed);

            if (scalingX || scalingY)
            {
                anchorX = textBox.Width;
                anchorY = textBox.Height;
            }
            else
            {
                anchorX = x - textBox.Left;
                anchorY = y - textBox.Top;
            }
            active = true;
        }

        private void TextBox_MouseReleased(object sender, MouseEventArgs e)
        {
            active = false;
        }

        private void ConsoleInput_MouseDragged(object sender, MouseEventArgs e)
        {
            if (active)
            {
                (int x, int y) = e.Location;

                textBox.BeginUpdate();

                if ((scalingX || scalingY) && e.ControlKeyState.HasFlag(ControlKeyState.ShiftPressed))
                {
                    if (scalingX)
                        textBox.Width = x - textBox.Left + 1;
                    if (scalingY)
                        textBox.Height = y - textBox.Top + 1;
                }
                else
                {
                    textBox.Left = x - anchorX;
                    textBox.Top = y - anchorY;
                }
                textBox.EndUpdate();

                dataBox.BeginUpdate();
                string line1 = "Location: " + textBox.Rectangle.UpperLeft;
                string line2 = "Size:" + textBox.Rectangle.Size;
                dataBox.Text = $"{line1} {line2}";
                dataBox.Width = Math.Max(line1.Length, line2.Length);
                dataBox.EndUpdate();
                //Draw();
            }
        }

        private void ConsoleInput_KeyPressed(KeyEventArgs e)
        {
            if (e.Key == ConsoleKey.Escape)
            {
                Exit();
            }

            bool leftControl = e.ControlKeyState.HasFlag(ControlKeyState.LeftCtrlPressed);

            if (e.Key == ConsoleKey.S && leftControl)
            {
                var area = ClientArea;
                var size = area.Size;
                using (System.Drawing.Bitmap b = new System.Drawing.Bitmap(size.X, size.Y, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(b))
                {
                    g.CopyFromScreen(area.UpperLeft, new System.Drawing.Point(0, 0), new System.Drawing.Size(size));
                    b.Save("Test.png");
                }
            }

            textBox.BeginUpdate();
            if (leftControl || e.ControlKeyState.HasFlag(ControlKeyState.ShiftPressed))
            {
                switch (e.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        textBox.Height--;
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        textBox.Width--;
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        textBox.Height++;
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        textBox.Width++;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (e.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        textBox.Top--;
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        textBox.Left--;
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        textBox.Top++;
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        textBox.Left++;
                        break;
                    default:
                        break;
                }
            }

            //Fill
            if (e.Key == ConsoleKey.F)
            {
                if (leftControl)
                {
                    textBox.Left = 0;
                    textBox.Top = 0;
                    textBox.Width = Width;
                    textBox.Height = Height;
                }
                else
                    UpdateColor();
            }

            //Reset
            if (e.Key == ConsoleKey.R && leftControl)
            {
                textBox.Left = 0;
                textBox.Top = 0;
                textBox.Width = 34;
                textBox.Height = 13;
            }

            //Center
            if (e.Key == ConsoleKey.C && leftControl)
            {
                textBox.Left = Width / 2 - textBox.Width / 2;
                textBox.Top = Height / 2 - textBox.Height / 2;
            }

            textBox.EndUpdate();

            dataBox.BeginUpdate();
            string line1 = "Location: " + textBox.Rectangle.UpperLeft;
            string line2 = "Size:" + textBox.Rectangle.Size;
            dataBox.Text = $"{line1} {line2}";
            dataBox.Width = Math.Max(line1.Length, line2.Length);
            dataBox.EndUpdate();
        }

        private void Draw()
        {
            //buffer.Clear(CharAttribute.BackgroundDarkYellow);
            Gradient gradient = new Gradient(CharAttribute.BackgroundDarkGrey, CharAttribute.ForegroundWhite, CharAttribute.BackgroundGreen, CharAttribute.BackgroundMagenta, CharAttribute.BackgroundCyan, CharAttribute.BackgroundYellow);
            //gradient = new Gradient(CharAttribute.BackgroundCyan, CharAttribute.BackgroundMagenta, CharAttribute.ForegroundYellow, CharAttribute.BackgroundBlack);
            //gradient = new Gradient(CharAttribute.BackgroundBlue, CharAttribute.BackgroundCyan, CharAttribute.BackgroundGreen);
            gradient = new Gradient(CharAttribute.BackgroundBlack, CharAttribute.BackgroundDarkGrey, CharAttribute.BackgroundGrey, CharAttribute.BackgroundWhite);
            gradient = new Gradient(CharAttribute.BackgroundBlack, CharAttribute.BackgroundDarkBlue, CharAttribute.BackgroundBlue);
            //gradient.Reverse();
            buffer.Draw(gradient, 0, 0, Width, Height / 2);
            gradient.Reverse();
            buffer.Draw(gradient, 0, Height / 2, Width, Height);

            buffer.Draw(backface, Width / 2 - backface.Width / 2, Height / 2 - backface.Height / 2);
            buffer.Draw(square, Width / 2 - square.GetLength(1) / 2, Height / 2 - square.GetLength(0) / 2);

            //buffer.Draw(strings, Width / 2 - strings[0].Length / 2, Height / 2 - strings.Length / 2, CharAttribute.ForegroundWhite);
            //buffer.Draw("Hello", 10, Height - 1);

            controlManager.DrawControls();
            buffer.FillRect(r0, CharAttribute.BackgroundCyan);
            buffer.FillRect(r1, CharAttribute.BackgroundRed);
            buffer.FillRect(r2, CharAttribute.BackgroundYellow);
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
