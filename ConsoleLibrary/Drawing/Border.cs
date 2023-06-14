using ConsoleLibrary.Drawing;
using System;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;

namespace ConsoleLibrary.Drawing
{
    public class Border
    {
        const char DEFAULT_CHAR = '#';

        public static Border Instance => instance ??= new();
        private static Border instance;

        public char UpperLeftCorner => upperLeftCorner;
        public char UpperRightCorner => upperRightCorner;
        public char LowerRightCorner => lowerRightCorner;
        public char LowerLeftCorner => lowerLeftCorner;
        public char Upper => upper;
        public char Right => right;
        public char Lower => lower;
        public char Left => left;

        protected char upperLeftCorner = DEFAULT_CHAR;
        protected char upperRightCorner = DEFAULT_CHAR;
        protected char lowerRightCorner = DEFAULT_CHAR;
        protected char lowerLeftCorner = DEFAULT_CHAR;
        protected char upper = DEFAULT_CHAR;
        protected char right = DEFAULT_CHAR;
        protected char lower = DEFAULT_CHAR;
        protected char left = DEFAULT_CHAR;

        public Border() { }
    }

    public class CustomBorder : SquigglyBorder
    {
        public CustomBorder(char c) { 
        
        }
    }

    public class SingleBorder : SquigglyBorder
    {
        public static new SingleBorder Instance => instance ??= new();
        private static SingleBorder instance;

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
        public static new RoundedBorder Instance => instance ??= new();
        private static RoundedBorder instance;

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

    public class DoubleBorder : SquigglyBorder
    {
        public static new DoubleBorder Instance => instance ??= new();
        private static DoubleBorder instance;

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
        public static new DashedBorder Instance => instance ??= new();
        private static DashedBorder instance;

        public DashedBorder(bool heavy = false) : base(heavy)
        {
            upper = lower = heavy ? '┅' : '┄';
            right = left = heavy ? '┇' : '┆';
        }
    }

    public class SquigglyBorder : Border
    {
        public static SquigglyBorder Instance => instance ??= new();
        private static SquigglyBorder instance;

        public SquigglyBorder() : base()
        {
            left = 'ⸯ';
            right = 'ⸯ';
        }
    }
}
