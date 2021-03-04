using ConsoleLibrary.Forms;
using ConsoleLibrary.Forms.Controls;
using ConsoleLibrary.Graphics.Drawing;
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
            new TestApp(9*scale, 4*scale).Init();
            ConsoleInput.ReadInput();
        }
    }

    public class TestApp : FormApp
    {
        private readonly string lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

        TextBox text;

        public TestApp(int width = 90, int height = 40) : base(width, height)
        {
            Title = "Control Testing";
        }

        public override void Init()
        {
            base.Init();

            var buffer = ConsoleRenderer.CreateScreenBuffer();
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

            buffer.Clear(CharAttribute.BackgroundCyan);
            var area = buffer.GetArea(0, 0, buffer.Width/2, buffer.Height/2);
            buffer.Clear(CharAttribute.ForegroundGrey);
            buffer.Draw(area, buffer.Width / 2, buffer.Height / 2);
            //buffer.Draw(area, area.Width, area.Height);
            ConsoleRenderer.DrawBuffers();
            //Draw();

            ConsoleInput.KeyPressed += ConsoleInput_KeyPressed;

            ConsoleInput.KeyHeld += ConsoleInput_KeyPressed;
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
                case System.ConsoleKey.W:
                    text.Height--;
                    break;
                case System.ConsoleKey.A:
                    text.Width--;
                    break;
                case System.ConsoleKey.S:
                    text.Height++;
                    break;
                case System.ConsoleKey.D:
                    text.Width++;
                    break;
                case System.ConsoleKey.LeftArrow:
                    text.Left--;
                    break;
                case System.ConsoleKey.UpArrow:
                    text.Top--;
                    break;
                case System.ConsoleKey.RightArrow:
                    text.Left++;
                    break;
                case System.ConsoleKey.DownArrow:
                    text.Top++;
                    break;
            }
            //Draw();
        }

        private void Draw()
        {
            ConsoleRenderer.FastClear(CharAttribute.BackgroundWhite);
            controlManager.DrawControls();
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
