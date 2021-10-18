using ConsoleLibrary.Drawing;
using ConsoleLibrary.Structures;
using ConsoleLibrary.TextExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;

namespace ConsoleLibrary.Forms.Controls
{
    public class VerticalScrollbar : Scrollbar
    {
        //private const char bigArrowUp = '\u25b2';
        private const char bigArrowUp = '\u2b06';
        //private const char bigArrowDown = '\u25bc';
        private const char bigArrowDown = '\u2b07';
        private const char smallArrowUp = '\u25b4';
        private const char smallArrowDown = '\u25be';
        private const char handleSymbol = '\u2588';

        private bool doubleWidth = true;
        private readonly char decreaseSymbol;
        private readonly char increaseSymbol;

        public override int Width => thickness;
        public override int Height
        {
            get => rectangle.Height;
            set => rectangle.Height = Math.Max(2, value);
        }

        public VerticalScrollbar(ControlManager manager) : base(manager)
        {
            handleSize = 5;
            buttonAttributes &= doubleWidth ? buttonAttributes : ~CharAttribute.LeadingByte;
            decreaseSymbol = doubleWidth ? bigArrowUp : smallArrowUp;
            increaseSymbol = doubleWidth ? bigArrowDown : smallArrowDown;
            thickness = doubleWidth ? 2 : 1;
            rectangle.Width = thickness;
        }

        protected override bool OnDecreaseButton(Point p) => p.Y == 0;
        protected override bool WithinScrollArea(Point p) => p.Y > 0 && p.Y < Height - 1;
        protected override bool OnIncreaseButton(Point p) => p.Y == Height - 1;
        protected override int ScrollPosition(Point p) => p.Y - 1;
        protected override int ScrollEnd() => Height - 2 - handleSize;

        protected override void RefreshBuffer()
        {
            base.RefreshBuffer();

            buffer.Draw(decreaseSymbol, 0, 0, buttonAttributes);
            //buffer.Draw(handleSymbol.Repeat(2), 0, handleOffset + 1);
            buffer.FillRect(0, handleOffset + 1, 2, handleSize, new CharInfo { UnicodeChar = handleSymbol, Attributes = ConsoleRenderer.DefaultAttributes } );
            buffer.Draw(increaseSymbol, 0, Height - 1, buttonAttributes);
        }
    }
}
