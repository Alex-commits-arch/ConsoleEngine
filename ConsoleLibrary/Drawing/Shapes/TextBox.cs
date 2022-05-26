using ConsoleLibrary.Structures;
using ConsoleLibrary.TextExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLibrary.Drawing.Shapes
{
    public enum BreakMode
    {
        Word,
        Character
    }

    public struct TextBox : IShape
    {
        public int width, height;
        char[,] data;

        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }

        public char[,] GetData() => data;
        public void SetData(char[,] data) => this.data = data;

        public TextBox(int width, int height)
        {
            this.width = width;
            this.height = height;
            data = new char[width, height];
            data = new char[height, width];
        }

        private int GetRightmostEmpty()
        {
            int rightMost = data.GetLength(1);

            for (int x = data.GetLength(1) - 1; x >= 0; x--)
            {
                for (int y = 0; y < data.GetLength(0); y++)
                {
                    if (data[y, x] != '\0')
                        return rightMost;
                }
                rightMost = x;
            }
            return rightMost;
        }

        private int GetBottommostEmpty()
        {
            int bottommost = data.GetLength(0);

            for (int y = data.GetLength(0) - 1; y >= 0; y--)
            {
                for (int x = 0; x < data.GetLength(1); x++)
                {
                    if (data[y, x] != '\0')
                        return bottommost;
                }
                bottommost = y;
            }
            return bottommost;
        }

        private Structures.Point GetLastNonEmpty()
        {
            Structures.Point last = new Structures.Point(0, 0);

            for (int y = data.GetLength(0) - 1; y >= 0; y--)
            {
                for (int x = data.GetLength(1) - 1; x >= 0; x--)
                {
                    if (data[y, x] != '\0')
                        return new Structures.Point(x, y);
                }
            }
            return last;
        }

        public void Text(string s, BreakMode mode = BreakMode.Word)
        {
            //var test = data.Length;
            //var test1 = data.LongLength;
            //Buffer.BlockCopy(s.ToArray(), 0, data, 0, Math.Min(s.Length, data.Length) * 2);
            if (mode == BreakMode.Word && s.Contains(' '))
            {
                string[] strings = s.Split(' ', '\n');
                char[] spaces = ' '.Repeat(strings.Length - 1).ToCharArray();
                var combined = strings.Zip(
                    spaces,
                    (t1, t2) => t1 + t2)
                    .Concat(
                    strings.Skip(spaces.Count()));

                int x = 0;
                int y = 0;

                for (int i = 0; i < strings.Length; i++)
                {
                    string str = strings[i];

                    if (x + str.Length > width)
                    {
                        x = 0;
                        y++;
                        if (y >= height)
                        {
                            //[x, y]
                            //data[width - 3, height - 1] = '.';
                            //data[width - 2, height - 1] = '.';
                            //data[width - 1, height - 1] = '.';

                            //[y, x]
                            data[height - 1, width - 3] = '.';
                            data[height - 1, width - 2] = '.';
                            data[height - 1, width - 1] = '.';
                            //data[height - 1, width - 1] = '…';
                            break;
                        }
                    }

                    for (int j = 0; j < str.Length; j++)
                    {
                        //[x, y]
                        //data[x + j, y] = str[j];

                        //[y, x]
                        data[y, x + j] = str[j];
                    }

                    x += str.Length + 1;
                }
            }
            else
            {

            }

            int rightmost = GetRightmostEmpty();
            int bottommost = GetBottommostEmpty();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    //[x,y]
                    //if (data[x, y] == '\0')
                    //    data[x, y] = ' ';

                    //[y, x]
                    if (data[y, x] == '\0' && x < rightmost && y < bottommost)
                        data[y, x] = ' ';
                }
            }
        }
    }
}
