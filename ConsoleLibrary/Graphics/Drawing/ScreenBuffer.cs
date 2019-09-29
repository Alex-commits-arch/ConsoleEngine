using ConsoleLibrary.Api.WinApi.Structs;
using ConsoleLibrary.Graphics.Shapes;
using ConsoleLibrary.Structures;
using ConsoleLibrary.TextExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLibrary.Graphics.Drawing
{
    public class ScreenBuffer
    {
        //char[,] content;
        CharInfo[] content;
        int width, height;

        public ScreenBuffer(int width, int height)
        {
            content = new CharInfo[width * height];
            this.width = width;
            this.height = height;
        }

        public CharInfo this[int x, int y] => content[y * width + x];

        public void Draw(IShape s, int a, Location l)
        {
            Draw(s, (ushort)a, l.x, l.y);
        }
        public void Draw(IShape s, ushort a, Location l)
        {
            Draw(s, a, l.x, l.y);
        }
        public void Draw(IShape s, int a, int x = 0, int y = 0)
        {
            Draw(s, (ushort)a, x, y);
        }
        public void Draw(IShape s, ushort a, int x = 0, int y = 0)
        {
            char[,] shape = s.GetData();

            for (int i = 0; i < s.Width; i++)
            {
                for (int j = 0; j < s.Height; j++)
                {
                    if (x + i >= 0 && x + i < width
                     && y + j >= 0 && y + j < height)
                    {
                        //[x, y]
                        //Draw(shape[i, j], a, x + i, y + j);

                        //[y, x]
                        Draw(shape[j, i], a, x + i, y + j);
                    }
                }
            }
        }

        public void Draw(string s, int a, Location l)
        {
            Draw(s, (ushort)a, l.x, l.y);
        }

        public void Draw(string s, ushort a, Location l)
        {
            Draw(s, a, l.x, l.y);
        }

        public void Draw(string s, int a, int x = 0, int y = 0)
        {
            Draw(s, (ushort)a, x, y);
        }

        public void Draw(string s, ushort a, int x = 0, int y = 0)
        {
            for (int i = 0; i < s.Length; i++)
            {
                Draw(s[i], a, x+i, y);
            }
        }



        public void Draw(char c, int a, Location l)
        {
            Draw(c, (ushort)a, l.x, l.y);
        }
        public void Draw(char c, ushort a, Location l)
        {
            Draw(c, a, l.x, l.y);
        }
        public void Draw(char c, int a, int x = 0, int y = 0)
        {
            Draw(c, (ushort)a, x, y);
        }
        public void Draw(char c, ushort a, int x = 0, int y = 0)
        {
            int index = y * width + x;
            content[index].UnicodeChar = c;
            content[index].AsciiChar = (byte)c;
            content[index].Attributes = a;
        }

        public void Fill()
        {

        }

        public void Clear()
        {
            content = '\0'.Repeat(width, height).ToCharInfoArray();
        }
    }
}
