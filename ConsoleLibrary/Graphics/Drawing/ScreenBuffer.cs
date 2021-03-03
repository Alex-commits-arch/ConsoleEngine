using ConsoleLibrary.Graphics.Shapes;
using ConsoleLibrary.Structures;
using ConsoleLibrary.TextExtensions;
using System;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;

namespace ConsoleLibrary.Graphics.Drawing
{
    public class ScreenBuffer
    {
        CharInfo[] content;
        int width;
        int height;

        public int Width { get => width; }
        public int Height { get => height; }

        public ScreenBuffer(int width, int height)
        {
            content = new CharInfo[width * height];
            this.width = width;
            this.height = height;
        }

        public void Clear(CharAttribute attributes = ConsoleRenderer.defaultAttributes)
        {
            CharInfo clearChar = new CharInfo
            {
                Attributes = attributes,
                UnicodeChar = '\0'
            };

            for (int i = 0; i < content.Length; i++)
            {
                content[i] = clearChar;
            }
        }

        public CharInfo[,] GetArea(int x, int y, int width, int height)
        {
            (int mw, int mh) = MaxSize(x, y, width, height);
            CharInfo[,] area = new CharInfo[mh, mw];

            (int ux, int uy) = UpperBounds(x, y, width, height);
            for (int ay = 0; ay <= uy; ay++)
            {
                for (int ax = 0; ax <= ux; ax++)
                {
                    int i = Index(x + ax, y + ay, this.width - width);
                    area[ay, ax] = content[i];
                }
            }

            return area;
        }

        private (int, int) MaxSize(int x, int y, int w, int h)
        {
            return (Math.Min(w, width - x), Math.Min(h, height - y));
        }

        private (int, int) LowerBounds(int x, int y)//, int w, int h)
        {
            return (
                Math.Max(x, 0),
                Math.Max(y, 0)
            );
        }

        private (int, int) UpperBounds(int x, int y, int w, int h)
        {
            return (
                Math.Min(x + w, width - x - 1),
                Math.Min(y + h, height - y - 1)
            );
        }

        private int Index(int x, int y, int w) => x + y * w;

        public CharInfo this[int x, int y] => content[y * width + x];

        public void Draw(CharInfo[,] info, int x, int y)
        {
            int areaWidth = info.GetUpperBound(1) + 1;
            int areaHeight = info.GetUpperBound(0) + 1;

            int areaMinX = Math.Max(-x, 0);
            int areaMinY = Math.Max(-y, 0);

            int areaMaxX = Math.Max(-x + areaWidth, width) - 1;
            int areaMaxY = Math.Max(-y + areaHeight, height) - 1;

            //int offsetX = -x + areaMinY;

            for (int areaY = areaMinY; areaY <= areaMaxY; areaY++)
            {
                for (int areaX = areaMinX; areaX <= areaMaxX; areaX++)
                {
                    int drawX = Math.Min(x + areaX, width - 1);
                    int drawY = Math.Min(y + areaY, Height - 1);
                    int index = drawX + drawY * width;
                    content[index] = info[areaY, areaX];
                }
            }
            //int width = info.GetUpperBound(1) + 1;
            //int height = info.GetUpperBound(0) + 1;

            //(int lax, int lay) = LowerBounds(x, y);
            //(int ux, int uy) = UpperBounds(x, y, width, height);
            //for (int ay = lay; ay < uy; ay++)
            //{
            //    for (int ax = lax; ax < ux; ax++)
            //    {
            //        int i = Index(lax  + ax, lay + ay, this.width);
            //        content[i] = info[ay, ax];
            //    }
            //}
        }

        //public void Draw(IShape s, int a, Location l)
        //{
        //    Draw(s, (ushort)a, l.x, l.y);
        //}
        //public void Draw(IShape s, ushort a, Location l)
        //{
        //    Draw(s, a, l.x, l.y);
        //}
        //public void Draw(IShape s, int a, int x = 0, int y = 0)
        //{
        //    Draw(s, (ushort)a, x, y);
        //}
        //public void Draw(IShape s, CharAttribute a, int x = 0, int y = 0)
        //{
        //    char[,] shape = s.GetData();

        //    for (int i = 0; i < s.Width; i++)
        //    {
        //        for (int j = 0; j < s.Height; j++)
        //        {
        //            if (x + i >= 0 && x + i < width
        //             && y + j >= 0 && y + j < height)
        //            {
        //                Draw(shape[j, i], a, x + i, y + j);
        //            }
        //        }
        //    }
        //}

        //public void Draw(string s, int a, Location l)
        //{
        //    Draw(s, (ushort)a, l.x, l.y);
        //}

        //public void Draw(string s, ushort a, Location l)
        //{
        //    Draw(s, a, l.x, l.y);
        //}

        //public void Draw(string s, int a, int x = 0, int y = 0)
        //{
        //    Draw(s, (ushort)a, x, y);
        //}

        //public void Draw(string s, CharAttribute a, int x = 0, int y = 0)
        //{
        //    for (int i = 0; i < s.Length; i++)
        //    {
        //        Draw(s[i], a, x+i, y);
        //    }
        //}



        //public void Draw(char c, CharAttribute a, Location l)
        //{
        //    Draw(c, a, l.x, l.y);
        //}
        //public void Draw(char c, CharAttribute a, int x = 0, int y = 0)
        //{
        //    int index = y * width + x;
        //    content[index].UnicodeChar = c;
        //    content[index].AsciiChar = (byte)c;
        //    content[index].Attributes = a;
        //}

        //public void Fill()
        //{

        //}

        //public void Clear()
        //{
        //    content = '\0'.Repeat(width, height).ToCharInfoArray();
        //}
    }

    class BufferArea
    {

    }
}
