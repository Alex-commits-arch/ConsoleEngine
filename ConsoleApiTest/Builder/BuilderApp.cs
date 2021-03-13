using ConsoleLibrary.Forms;
using ConsoleLibrary.Forms.Components;
using ConsoleLibrary.Drawing;
using ConsoleLibrary.Input;
using ConsoleLibrary.Input.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsWrapper.Enums;

namespace ConsoleApiTest.Builder
{
    public class BuilderApp : FormApp
    {
        Border border;

        public BuilderApp(int width, int height) : base(width, height)
        {
        }

        public override void Init()
        {
            base.Init();


            var (width, height) = ConsoleRenderer.GetConsoleSize();

            border = new SingleBorder();
            border.Left = -1;
            border.Top = -1;
            border.Width = 20;
            border.Height = height + 2;
            border.Hide();

            //Components.Add(border);

            ConsoleInput.KeyPressed += OnKeyPressed;
        }

        private void OnKeyPressed(KeyEventArgs keyEventArgs)
        {
            var key = keyEventArgs.Key;
            var ctrlPressed = keyEventArgs.ControlKeyState.HasFlag(ControlKeyState.LeftCtrlPressed | ControlKeyState.RightCtrlPressed);

            

            switch (key)
            {
                case ConsoleKey.T:
                    ToggleToolbox();
                    break;
                case ConsoleKey.R:
                    
                    break;
                default:
                    break;
            }
        }

        private void Redraw()
        {
            ConsoleRenderer.Clear();
        }

        private void ToggleToolbox()
        {
            if (border.Visible)
                border.Hide();
            else
                border.Show();

            Redraw();
        }
    }
}
