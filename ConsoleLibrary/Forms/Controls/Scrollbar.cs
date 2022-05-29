using ConsoleLibrary.Input;
using ConsoleLibrary.Structures;
using System;
using WindowsWrapper.Enums;

namespace ConsoleLibrary.Forms.Controls
{
    public abstract class Scrollbar : Control
    {
        private bool active = false;
        protected int thickness = 1;
        protected int handleOffset = 0;
        protected int handleSize = 1;
        protected CharAttribute buttonAttributes = CharAttribute.BackgroundDarkGray | CharAttribute.ForegroundWhite | CharAttribute.LeadingByte;

        public Scrollbar(Control manager) : base(manager)
        {
            MousePressed += OnMouseDown;
            MouseDoubleClick += OnMouseDown;
        }

        protected abstract bool OnDecreaseButton(Point p);
        protected abstract bool WithinScrollArea(Point p);
        protected abstract bool OnIncreaseButton(Point p);
        protected abstract int ScrollPosition(Point p);
        protected abstract int ScrollEnd();

        private int CalculateHandleOffset(Point p) => Math.Max(0, Math.Min(ScrollEnd(), ScrollPosition(p) - handleSize / 2));

        private void OnMouseDown(object sender, Input.Events.MouseEventArgs e)
        {
            var relative = GetRelativeLocation(e.Location);

            if (OnDecreaseButton(relative) && handleOffset > 0)
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
                int calulatedOffset = CalculateHandleOffset(relative);

                if (handleOffset != calulatedOffset)
                {
                    BeginUpdate();
                    handleOffset = calulatedOffset;
                    EndUpdate();
                }
            }
            else if (OnIncreaseButton(relative) && handleOffset < ScrollEnd())
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
                handleOffset = CalculateHandleOffset(relative);
                EndUpdate();
            }
        }

        private void OnMouseUp(object sender, Input.Events.MouseEventArgs e)
        {
            active = false;
            ConsoleInput.MouseDragged -= OnMouseDragged;
            ConsoleInput.MouseReleased -= OnMouseUp;
        }
    }
}
