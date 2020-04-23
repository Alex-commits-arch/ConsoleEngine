using ConsoleLibrary.Forms.Components;
using ConsoleLibrary.Forms.Interfaces;
using ConsoleLibrary.Graphics.Drawing;
using ConsoleLibrary.Input;
using ConsoleLibrary.Input.Events;
using System.Diagnostics;
using System.Linq;
using WindowsWrapper;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;

namespace ConsoleLibrary.Forms
{
    public class FormApp : ConsoleApp
    {
        private bool cursor;
        private int prevMouseX;
        private int prevMouseY;
        private CharInfo prevCharInfo;
        private bool justLeftButton;
        private string title;

        public ComponentCollection Components { get; set; }
        public bool Cursor
        {
            get => cursor;
            set
            {
                if (value && !cursor)
                    ConsoleInput.MouseMoved += OnMouseMoved;
                else if (cursor && !value)
                {
                    ConsoleRenderer.Draw(ConsoleRenderer.GetCharInfo(prevMouseX, prevMouseY), new DrawArgs
                    {
                        x = prevMouseX,
                        y = prevMouseY,
                        skipBuffer = true
                    });
                    ConsoleInput.MouseMoved -= OnMouseMoved;
                }
                cursor = value;
            }
        }
        public string Title
        {
            get => title;
            set
            {
                MyConsole.SetTitle(value);
                title = value;
            }
        }

        public FormApp(int width, int height) : base(width, height) { }

        public override void Init()
        {
            base.Init();
            Components = new ComponentCollection();
        }

        protected void DrawComponents()
        {
            foreach (var component in Components)
            {
                if (component.Visible)
                    component.Draw();
            }
        }



        private void OnMouseMoved(object sender, MouseEventArgs args)
        {
            if (cursor)
            {
                var (currX, currY) = args.Location;
                if (prevMouseX != currX || prevMouseY != currY)
                {
                    System.Threading.Thread.Sleep(40);
                    ConsoleRenderer.Draw(ConsoleRenderer.GetCharInfo(prevMouseX, prevMouseY), new DrawArgs
                    {
                        x = prevMouseX,
                        y = prevMouseY,
                        skipBuffer = true
                    });
                    ConsoleRenderer.Draw(
                        ConsoleRenderer.GetCharInfo(currX, currY).UnicodeChar,
                        new DrawArgs
                        {
                            x = currX,
                            y = currY,
                            attributes = CharAttribute.BackgroundWhite | CharAttribute.ForegroundBlack,
                            skipBuffer = true
                        }
                    );
                    prevMouseX = currX;
                    prevMouseY = currY;
                }
            }
        }
    }
}
