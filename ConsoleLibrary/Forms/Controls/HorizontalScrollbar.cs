using ConsoleLibrary.Structures;
using ConsoleLibrary.TextExtensions;
using System;
using WindowsWrapper.Enums;

namespace ConsoleLibrary.Forms.Controls
{
    public class HorizontalScrollbar : Scrollbar
    {
        //private const char bigArrowLeft = '\u2b05';
        private const char bigArrowLeft = '<';
        //private const char bigArrowRight = '\u27a1';
        private const char bigArrowRight = '>';
        private const char smallArrowUp = '\u25b4';
        private const char smallArrowDown = '\u25be';
        private const char handleSymbol = '\u2588';

        private const int buttonSize = 2;

        public override int Height => thickness;
        public override int Width
        {
            get => base.Width;
            set => base.Width = Math.Max(6, value);
        }

        public HorizontalScrollbar(ControlManager manager) : base(manager)
        {
            handleSize = 10;
            thickness = 1;
            base.Height = thickness;
        }

        protected override bool OnDecreaseButton(Point p) => p.X < buttonSize;
        protected override bool WithinScrollArea(Point p) => p.X >= buttonSize && p.X < Width - buttonSize;
        protected override bool OnIncreaseButton(Point p) => p.X >= Width - buttonSize;
        protected override int ScrollPosition(Point p) => p.X - buttonSize;
        protected override int ScrollEnd() => Width - (buttonSize * 2) - handleSize;

        protected override void RefreshBuffer()
        {
            base.RefreshBuffer();

            buffer.Draw(bigArrowLeft, 0, 0, buttonAttributes);
            buffer.Draw(handleSymbol.Repeat(handleSize), handleOffset + buttonSize, 0);
            buffer.Draw(bigArrowRight, Width - buttonSize, 0, buttonAttributes);
        }
    }
}
