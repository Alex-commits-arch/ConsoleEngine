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
        private bool _cursor;
        private int _prevMouseX;
        private int _prevMouseY;
        private CharInfo prevCharInfo;
        private bool justLeftButton;
        private string _title;

        public ComponentCollection Components { get; set; }
        public bool Cursor
        {
            get => _cursor;
            set
            {
                if (value && !_cursor)
                    ConsoleInput.MouseMoved += OnMouseMoved;
                else if (_cursor && !value)
                {
                    ConsoleRenderer.Draw(ConsoleRenderer.GetCharInfo(_prevMouseX, _prevMouseY), new DrawArgs
                    {
                        x = _prevMouseX,
                        y = _prevMouseY,
                        skipBuffer = true
                    });
                    ConsoleInput.MouseMoved -= OnMouseMoved;
                }
                _cursor = value;
            }
        }
        public string Title
        {
            get => _title;
            set
            {
                MyConsole.SetTitle(value);
                _title = value;
            }
        }

        public FormApp(int width, int height) : base(width, height) { }

        public override void Init()
        {
            base.Init();
            Components = new ComponentCollection();
            ConsoleInput.MouseMoved += OnMouseMoved;
            ConsoleInput.KeyPressed += OnKeyPressed;
        }

        private void ChangeCursor()
        {
            MyConsole.SetCursor(IDC_STANDARD_CURSORS.IDC_CROSS);
        }

        protected void DrawComponents()
        {
            foreach (var component in Components)
            {
                if (component.Visible)
                    component.Draw();
            }
        }

        private void OnKeyPressed(KeyEventArgs keyEventArgs)
        {
            var key = keyEventArgs.Key;

            if (key == System.ConsoleKey.A)
            {
                MyConsole.SetCursor(IDC_STANDARD_CURSORS.IDC_ARROW);
            }
            if (key == System.ConsoleKey.S)
            {
                MyConsole.SetCursor(IDC_STANDARD_CURSORS.IDC_WAIT);
            }
            if (key == System.ConsoleKey.D)
            {
                MyConsole.SetCursor(IDC_STANDARD_CURSORS.IDC_HAND);
            }
            
        }

        private void OnMouseMoved(object sender, MouseEventArgs args)
        {
            if (_cursor)
            {
                var (currX, currY) = args.Location;
                if (_prevMouseX != currX || _prevMouseY != currY)
                {
                    System.Threading.Thread.Sleep(40);
                    ConsoleRenderer.DrawCursor(_prevMouseX, _prevMouseY, currX, currY);
                    _prevMouseX = currX;
                    _prevMouseY = currY;
                }
            }
        }
    }
}
