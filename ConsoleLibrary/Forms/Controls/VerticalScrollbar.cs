using ConsoleLibrary.Input;
using ConsoleLibrary.Structures;
using ConsoleLibrary.TextExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsWrapper.Enums;

namespace ConsoleLibrary.Forms.Controls
{
    public class VerticalScrollbar : Control
    {
        private const char bigArrowUp = '\u25b2';
        private const char bigArrowDown = '\u25bc';
        private const char smallArrowUp = '\u25b4';
        private const char smallArrowDown = '\u25be';
        private const char handleSymbol = '\u2588';

        private readonly CharAttribute buttonAttributes = CharAttribute.BackgroundDarkGrey | CharAttribute.ForegroundWhite | CharAttribute.LeadingByte;
        private readonly int width = 1;

        private bool active = false;
        private int handleOffset = 0;

        public override int Width => width;
        public override int Height
        {
            get => rectangle.Height;
            set => rectangle.Height = Math.Max(3, value);
        }

        public VerticalScrollbar(ControlManager manager) : base(manager)
        {
            width = 2;
            rectangle.Width = width;
            MousePressed += OnMouseDown;
            MouseDoubleClick += OnMouseDown;
        }

        private void OnMouseDown(object sender, Input.Events.MouseEventArgs e)
        {
            var relative = GetRelativeLocation(e.Location);

            if (relative.Y == 0 && handleOffset > 0)
            {
                BeginUpdate();
                handleOffset--;
                EndUpdate();
            }
            else if (WithinScrollArea(relative))
            {
                active = true;
                ConsoleInput.MouseDragged += OnMouseDragged;
                ConsoleInput.MouseReleased += OnMouseUp;
                if (handleOffset != relative.Y - 1)
                {
                    BeginUpdate();
                    handleOffset = relative.Y - 1;
                    EndUpdate();
                }
            }
            else if (relative.Y == Height - 1 && handleOffset < Height - 3)
            {
                BeginUpdate();
                handleOffset++;
                EndUpdate();
            }
        }

        private void OnMouseDragged(object sender, Input.Events.MouseEventArgs e)
        {
            var relative = GetRelativeLocation(e.Location);

            if (active && WithinScrollArea(relative))
            {
                BeginUpdate();
                handleOffset = relative.Y - 1;
                EndUpdate();
            }
        }

        private void OnMouseUp(object sender, Input.Events.MouseEventArgs e)
        {
            active = false;
            ConsoleInput.MouseDragged -= OnMouseDragged;
            ConsoleInput.MouseReleased -= OnMouseUp;
        }

        private bool WithinScrollArea(Point p) => p.Y > 0 && p.Y < Height - 1;

        protected override void RefreshBuffer()
        {
            base.RefreshBuffer();

            buffer.Draw(bigArrowUp, 0, 0, buttonAttributes);
            buffer.Draw(handleSymbol.Repeat(2), 0, handleOffset + 1);
            buffer.Draw(bigArrowDown, 0, Height - 1, buttonAttributes);
        }
    }
}
