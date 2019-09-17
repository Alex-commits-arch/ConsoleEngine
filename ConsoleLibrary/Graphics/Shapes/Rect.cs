using ConsoleLibrary.Structures;
using ConsoleLibrary.TextExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLibrary.Graphics.Shapes
{
    public struct Rect : IShape
    {
        public int width, height;
        char[,] data;

        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }

        public char[,] GetData() => data;
        public void SetData(char[,] data) => this.data = data;

        public Rect(Location l)
        {
            width = l.x;
            height = l.y;
            data = new char[width, height];
        }

        public Rect(int width, int height)
        {
            this.width = width;
            this.height = height;
            data = new char[width, height];
        }

        public void Fill(char c)
        {
            SetData(c.Repeat(width, height));
        }

        public void Border(char c)
        {
            Corners(c);
            Sides(c);
        }

        //public void Border(char tl, char hor, char tr, char ver, char br, char bl)
        public void Border(char tl, char tr, char bl, char br, char ver, char hor)
        {
            Corners(tl, tr, bl, br);
            Sides(ver, hor);
        }

        public void Sides(char c)
        {
            Sides(c, c);
        }

        public void Corners(char c)
        {
            Corners(c, c, c, c);
        }

        public void Sides(char ver, char hor)
        {
            for (int y = 1; y < height - 1; y++)
            {
                data[0, y] = ver;
                data[width - 1, y] = ver;
            }
            for (int x = 1; x < width - 1; x++)
            {
                data[x, 0] = hor;
                data[x, height - 1] = hor;
            }
        }

        public void Corners(char tl, char tr, char bl, char br)
        {
            data[0, 0] = tl;
            data[0, height - 1] = bl;
            data[width - 1, 0] = tr;
            data[width - 1, height - 1] = br;
        }
    }
}
