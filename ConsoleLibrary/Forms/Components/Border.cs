using ConsoleLibrary.Graphics.Drawing;
using System;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;

namespace ConsoleLibrary.Forms.Components
{
    public class Border : Component
    {
        const char DEFAULT_CHAR = '#';
        protected char upperLeftCorner = DEFAULT_CHAR;
        protected char upperRightCorner = DEFAULT_CHAR;
        protected char lowerRightCorner = DEFAULT_CHAR;
        protected char lowerLeftCorner = DEFAULT_CHAR;
        protected char upper = DEFAULT_CHAR;
        protected char right = DEFAULT_CHAR;
        protected char lower = DEFAULT_CHAR;
        protected new char left = DEFAULT_CHAR;

        public Border() : base() { }

        public override void Draw()
        {
            var topSide = new CharInfo[1, Width];
            var rightSide = new CharInfo[Height - 2, 1];
            var bottomSide = new CharInfo[1, Width];
            var leftSide = new CharInfo[Height - 2, 1];

            topSide[0, 0] = new CharInfo { Attributes = Attributes, UnicodeChar = upperLeftCorner };
            topSide[0, Width - 1] = new CharInfo { Attributes = Attributes, UnicodeChar = upperRightCorner };

            bottomSide[0, 0] = new CharInfo { Attributes = Attributes, UnicodeChar = lowerLeftCorner };
            bottomSide[0, Width - 1] = new CharInfo { Attributes = Attributes, UnicodeChar = lowerRightCorner };

            for (int i = 1; i <= topSide.GetUpperBound(1) - 1; i++)
            {
                topSide[0, i] = new CharInfo { Attributes = Attributes, UnicodeChar = upper };
                bottomSide[0, i] = new CharInfo { Attributes = Attributes, UnicodeChar = lower };
            }
            for (int i = 0; i <= leftSide.GetUpperBound(0); i++)
            {
                leftSide[i, 0] = new CharInfo { Attributes = Attributes, UnicodeChar = left };
                rightSide[i, 0] = new CharInfo { Attributes = Attributes, UnicodeChar = right };
            }

            ConsoleRenderer.DrawCharInfos(topSide, Left, Top);
            ConsoleRenderer.DrawCharInfos(rightSide, Left + Width - 1, Top + 1);
            ConsoleRenderer.DrawCharInfos(bottomSide, Left, Top + Height - 1);
            ConsoleRenderer.DrawCharInfos(leftSide, Left, Top + 1);
        }
    }

    public class SingleBorder : Border
    {
        public SingleBorder(bool heavy = false) : base()
        {
            upperLeftCorner = heavy ? '┏' : '┌';
            upperRightCorner = heavy ? '┓' : '┐';
            lowerRightCorner = heavy ? '┛' : '┘';
            lowerLeftCorner = heavy ? '┗' : '└';
            upper = lower = heavy ? '━' : '─';
            right = left = heavy ? '┃' : '│';
        }
    }

    public class RoundedBorder : SingleBorder
    {
        [Flags]
        public enum Rounded
        {
            UpperLeft = 1,
            UpperRight = 1 << 1,
            LowerRight = 1 << 2,
            LowerLeft = 1 << 3,
            All = ~0
        }

        public RoundedBorder(Rounded rounded = Rounded.All) : base()
        {
            if (rounded.HasFlag(Rounded.UpperLeft)) upperLeftCorner = '╭';
            if (rounded.HasFlag(Rounded.UpperRight)) upperRightCorner = '╮';
            if (rounded.HasFlag(Rounded.LowerRight)) lowerRightCorner = '╯';
            if (rounded.HasFlag(Rounded.LowerLeft)) lowerLeftCorner = '╰';
        }
    }

    public class DoubleBorder : Border
    {
        public DoubleBorder() : base()
        {
            upperLeftCorner = '╔';
            upperRightCorner = '╗';
            lowerRightCorner = '╝';
            lowerLeftCorner = '╚';
            upper = lower = '═';
            right = left = '║';
        }
    }

    public class DashedBorder : SingleBorder
    {
        public DashedBorder(bool heavy = false) : base(heavy)
        {
            upper = lower = heavy ? '┅' : '┄';
            right = left = heavy ? '┇' : '┆';
        }
    }

    public class SquigglyBorder : Border
    {
        public SquigglyBorder() : base()
        {
            left = 'ⸯ';
            right = 'ⸯ';
        }
    }
}
