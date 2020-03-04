using ConsoleLibrary.Graphics.Drawing;
using System;

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
        protected char left = DEFAULT_CHAR;

        public Border(DrawingContext context) : base(context) { }

        public override void Render()
        {
            context.DrawChar(upperLeftCorner, Left, Top);
            context.DrawChar(upperRightCorner, Left + Width - 1, Top);
            context.DrawChar(lowerRightCorner, Left + Width - 1, Top + Height - 1);
            context.DrawChar(lowerLeftCorner, Left, Top + Height - 1);


            for (int i = 1; i < Math.Max(Width, Height) - 1; i++)
            {
                if (i < Width - 1)
                {
                    context.DrawChar(upper, Left + i, Top);
                    context.DrawChar(lower, Left + i, Top + Height - 1);
                }

                if (i < Height - 1)
                {
                    context.DrawChar(left, Left, Top + i);
                    context.DrawChar(right, Left + Width - 1, Top + i);
                }
            }
        }
    }

    public class SingleBorder : Border
    {
        public SingleBorder(DrawingContext context) : base(context)
        {
            upperLeftCorner = '┌';
            upperRightCorner = '┐';
            lowerRightCorner = '┘';
            lowerLeftCorner = '└';
            upper = '─';
            right = '│';
            lower = '─';
            left = '│';
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

        public RoundedBorder(DrawingContext context, Rounded rounded = Rounded.All) : base(context)
        {
            if (rounded.HasFlag(Rounded.UpperLeft)) upperLeftCorner = '╭';
            if (rounded.HasFlag(Rounded.UpperRight)) upperRightCorner = '╮';
            if (rounded.HasFlag(Rounded.LowerRight)) lowerRightCorner = '╯';
            if (rounded.HasFlag(Rounded.LowerLeft)) lowerLeftCorner = '╰';
        }
    }

    public class DoubleBorder : Border
    {
        public DoubleBorder(DrawingContext context) : base(context)
        {
            upperLeftCorner = '╔';
            upperRightCorner = '╗';
            lowerRightCorner = '╝';
            lowerLeftCorner = '╚';
            upper = '═';
            right = '║';
            lower = '═';
            left = '║';
        }
    }
}
