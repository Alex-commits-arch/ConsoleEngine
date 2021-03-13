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
            new TestApp(84, 42).Init();
            //new TestApp(52, 40).Init();
            ConsoleInput.InputLoop();
        }
    }

    public class TestApp : FormApp
    {
        private readonly string lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

        BufferArea buffer;
        BufferArea backface;
        CharInfo[,] square;
        int squareX = 2;
        int squareY = 1;
        TextBox text;
        ColorfulString colorfulString;
        string[] strings;

        public TestApp(int width = 90, int height = 40) : base(width, height)
        {
            Title = "Control Testing";
        }

        public override void Init()
        {
            base.Init();

            buffer = ConsoleRenderer.ActiveBuffer;

            text = new TextBox(controlManager)
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
                Value = "LMAAAAAAOOOOOOOOOOO",
                ColorThing = ColorThing.Bounce,
                Attributes = new CharAttribute[] {
                    CharAttribute.ForegroundRed,
                    CharAttribute.ForegroundYellow,
                    CharAttribute.ForegroundGreen,
                    CharAttribute.ForegroundCyan,
                    CharAttribute.ForegroundBlue,
                    CharAttribute.ForegroundMagenta,
                    CharAttribute.ForegroundRed
                }
            };

            strings = new string[]
            {
                "Haha",
                "Strings",
                "Go",
                "Brrrrrrrrrrrrrrrrrrrrrrrrrrrrr"
            };

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


            Draw();

            ConsoleInput.KeyPressed += ConsoleInput_KeyPressed;

            ConsoleInput.KeyHeld += ConsoleInput_KeyPressed;

            ConsoleInput.MouseDragged += ConsoleInput_MouseDragged;

            ConsoleInput.Resized += delegate
            {
                Draw();
                //Debug.WriteLine("aaaaa");
            };
            //Draw();
            //ConsoleInput.MouseMoved += ConsoleInput_MouseDragged;
        }

        int prevX = 0;
        int prevY = 0;
        private void ConsoleInput_MouseDragged(object sender, MouseEventArgs e)
        {
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
                Environment.Exit(0);

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
            buffer.Clear();
            buffer.Draw(backface, squareX - 2, squareY - 1);
            buffer.Draw(square, squareX, squareY);
            buffer.Draw(colorfulString, 0, strings.Length);
            buffer.Draw(strings, 0, 0);
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
