using ConsoleLibrary.Drawing.Shapes;
using ConsoleLibrary.Structures;
using ConsoleLibrary.TextExtensions;
using System;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;

namespace ConsoleLibrary.Drawing
{
    public class ScreenBuffer
    {
        CharInfo[] content;
        readonly int width;
        readonly int height;

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
                content[i] = clearChar;
        }

        public BufferArea GetArea(Rectangle rect)
        {
            return GetArea(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public BufferArea GetArea(int x, int y, int width, int height)
        {
            int safeX = Math.Max(0, Math.Min(this.width, x));
            int safeY = Math.Max(0, Math.Min(this.height, y));

            int safeWidth = Math.Min(this.width - safeX, Math.Min(width - (safeX - x), width));
            int safeHeight = Math.Min(this.height - safeY, Math.Min(height - (safeY - y), height));

            CharInfo[,] area = new CharInfo[safeHeight, safeWidth];

            for (int areaY = 0; areaY < safeHeight; areaY++)
                for (int areaX = 0; areaX < safeWidth; areaX++)
                {
                    int index = (safeX + areaX) + (safeY + areaY) * safeWidth;
                    area[areaY, areaX] = content[index];
                }

            return new BufferArea(area);
        }

        public void Draw(BufferArea bufferArea, int x, int y)
        {
            Draw(bufferArea.Area, x, y);
        }

        private void Draw(CharInfo[,] info, int x, int y, bool withTransparancy = false, char transparentCharacter = '\0')
        {
            int areaWidth = info.GetUpperBound(1) + 1;
            int areaHeight = info.GetUpperBound(0) + 1;

            int safeX = Math.Max(-x, Math.Min(0, x));
            int safeY = Math.Max(-y, Math.Min(0, y));

            int safeWidth = Math.Min(width - x, areaWidth);
            int safeHeight = Math.Min(height - y, areaHeight);

            for (int areaY = safeY; areaY < safeHeight; areaY++)
            {
                for (int areaX = safeX; areaX < safeWidth; areaX++)
                {
                    if (!withTransparancy || info[areaY, areaX].UnicodeChar != transparentCharacter)
                    {
                        int drawX = areaX + x;
                        int drawY = areaY + y;
                        int index = drawX + drawY * width;
                        content[index] = info[areaY, areaX];
                    }
                }
            }
        }
    }

    public class BufferArea
    {
        public CharInfo[,] Area { get; private set; }
        public int Width => Area.GetUpperBound(1) + 1;
        public int Height => Area.GetUpperBound(0) + 1;

        public BufferArea(CharInfo[,] area)
        {
            Area = area;
        }
    }
}
