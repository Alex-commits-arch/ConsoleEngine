using ConsoleLibrary.TextExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLibrary.Graphics.Shapes
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
        }

        public void Text(string s, BreakMode mode = BreakMode.Word)
        {
            if (mode == BreakMode.Word && s.Contains(' '))
            {
                string[] strings = s.Split(' ');
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
                            data[width - 3, height - 1] = '.';
                            data[width - 2, height - 1] = '.';
                            data[width - 1, height - 1] = '.';
                            break;
                        }
                    }

                    for (int j = 0; j < str.Length; j++)
                    {
                        data[x + j, y] = str[j];
                    }

                    x += str.Length + 1;
                }
            }
            else
            {

            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (data[x, y] == '\0')
                        data[x, y] = ' ';
                }
            }
        }
    }
}
