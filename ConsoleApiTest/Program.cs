using ConsoleLibrary.Forms;
using ConsoleLibrary.Forms.Controls;
using ConsoleLibrary.Drawing;
using ConsoleLibrary.Input;
using ConsoleLibrary.Input.Events;
using System;
using System.Diagnostics;
using System.Drawing;
using WindowsWrapper.Enums;
namespace ConsoleApiTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int scale = 4;
            //new TestApp(9*scale, 4*scale).Init();
            new TestApp(90, 40).Init();
            ConsoleInput.ReadInput();
        }
    }

    public class TestApp : FormApp
    {
        private readonly string lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

        ScreenBuffer buffer;
        BufferArea square;
        int squareX = 0;
        int squareY = 0;
        TextBox text;

        public TestApp(int width = 90, int height = 40) : base(width, height)
        {
            Title = "Control Testing";
        }

        public override void Init()
        {
            base.Init();

            buffer = ConsoleRenderer.CreateScreenBuffer();
            ConsoleRenderer.SetActiveBuffer(buffer);

            

            text = new TextBox(controlManager)
            {
                Left = Width / 2,
                Top = Height / 2,
                Text = lorem,
                Width = 20,
                Height = 5,
                Attributes = CharAttribute.ForegroundWhite
            };

            buffer.Clear(CharAttribute.BackgroundBlack);
            square = buffer.GetArea(0, 0, buffer.Width/2, buffer.Height/2);

            Draw();

            ConsoleInput.KeyPressed += ConsoleInput_KeyPressed;

            ConsoleInput.KeyHeld += ConsoleInput_KeyPressed;

            ConsoleInput.MouseDragged += ConsoleInput_MouseDragged;
        }

        private void ConsoleInput_MouseDragged(object sender, MouseEventArgs e)
        {
            (int x, int y) = e.Location;
            //(squareX  , squareY) = e.Location;
            squareX = x - square.Width / 2;
            squareY = y - square.Height / 2;
            Draw();
            //squareX = x;

        }

        private void ConsoleInput_KeyPressed(KeyEventArgs e)
        {
            //if (e.Key.HasFlag(System.ConsoleKey.S))
            //    text.Height++;
            //if (e.Key.HasFlag(System.ConsoleKey.D))
            //    text.Width++;

            if (e.Key == ConsoleKey.Escape)
                Environment.Exit(0);

            switch (e.Key)
            {
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    squareX--;
                    break;
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    squareY--;
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    squareX++;
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    squareY++;
                    break;
                default:
                    break;
            }
            Draw();
            //switch (e.Key)
            //{
            //    case System.ConsoleKey.W:
            //        text.Height--;
            //        break;
            //    case System.ConsoleKey.A:
            //        text.Width--;
            //        break;
            //    case System.ConsoleKey.S:
            //        text.Height++;
            //        break;
            //    case System.ConsoleKey.D:
            //        text.Width++;
            //        break;
            //    case System.ConsoleKey.LeftArrow:
            //        text.Left--;
            //        break;
            //    case System.ConsoleKey.UpArrow:
            //        text.Top--;
            //        break;
            //    case System.ConsoleKey.RightArrow:
            //        text.Left++;
            //        break;
            //    case System.ConsoleKey.DownArrow:
            //        text.Top++;
            //        break;
            //}
            //Draw();
        }

        private void Draw()
        {
            buffer.Clear(CharAttribute.BackgroundWhite);
            buffer.Draw(square, squareX, squareY);
            ConsoleRenderer.DrawBuffers();

            (int cw, int ch) = FontSize;
            using (var p = new System.Drawing.Pen(System.Drawing.Color.Red, 1))
            using (var g = System.Drawing.Graphics.FromHwnd(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle))
            {
                g.DrawEllipse(p, squareX * cw, squareY * ch, square.Width * cw, square.Height * ch);
            }

            //controlManager.DrawControls();
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

    public enum BackgroundShade
    {
        None,
        Light,
        Medium,
        Dark
    }
}
