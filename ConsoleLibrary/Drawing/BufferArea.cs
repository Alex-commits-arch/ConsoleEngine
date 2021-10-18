using ConsoleLibrary.Drawing.Shapes;
using ConsoleLibrary.Structures;
using ConsoleLibrary.TextExtensions;
using System;
using System.Runtime.CompilerServices;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;

namespace ConsoleLibrary.Drawing
{
    public class BufferArea
    {
        protected CharInfo[,] content;
        protected int width;
        protected int height;

        public CharInfo[,] Content => content;
        public int Width => width;
        public int Height => height;

        public BufferArea(int width, int height)
        {
            this.width = width;
            this.height = height;
            content = new CharInfo[height, width];
        }

        public BufferArea(CharInfo[,] content)
        {
            width = content.GetLength(1);
            height = content.GetLength(0);
            this.content = content;
        }

        public void Resize(int width, int height)
        {
            this.width = width;
            this.height = height;
            content = new CharInfo[height, width];
        }

        public void ResizePreserve(int width, int height)
        {
            this.width = width;
            this.height = height;
            var oldContent = content;
            content = new CharInfo[height, width];
            Draw(oldContent, 0, 0);
        }

        public BufferArea GetBuffer(Rectangle rect)
        {
            return GetBuffer(rect.Left, rect.Top, rect.Width, rect.Height);
        }

        public BufferArea GetBuffer(int x, int y, int w, int h)
        {
            return new BufferArea(GetArea(x, y, w, h));
        }

        public CharInfo[,] GetArea(Rectangle rect)
        {
            return GetArea(rect.Left, rect.Top, rect.Width, rect.Height);
        }

        public CharInfo[,] GetArea(int x, int y, int w, int h)
        {
            int safeX = Math.Max(0, Math.Min(width, x));
            int safeY = Math.Max(0, Math.Min(height, y));

            int safeWidth = SafeSourceEnd(safeX, Math.Min(w - (safeX - x), w), width);
            int safeHeight = SafeSourceEnd(safeY, Math.Min(h - (safeY - y), h), height);

            CharInfo[,] area = (safeWidth < 0 || safeHeight < 0)
                ? new CharInfo[0, 0]
                : new CharInfo[safeHeight, safeWidth];

            for (int areaY = 0; areaY < safeHeight; areaY++)
                for (int areaX = 0; areaX < safeWidth; areaX++)
                    area[areaY, areaX] = content[(safeY + areaY), (safeX + areaX)];

            return area;
        }

        public CharInfo[,] GetArea(ref Rectangle rect)
        {
            int x = rect.Left;
            int y = rect.Top;
            int w = rect.Width;
            int h = rect.Height;

            int safeX = Math.Max(0, Math.Min(width, x));
            int safeY = Math.Max(0, Math.Min(height, y));

            int safeWidth = SafeSourceEnd(safeX, Math.Min(w - (safeX - x), w), width);
            int safeHeight = SafeSourceEnd(safeY, Math.Min(h - (safeY - y), h), height);

            CharInfo[,] area = (safeWidth < 0 || safeHeight < 0)
                ? new CharInfo[0, 0]
                : new CharInfo[safeHeight, safeWidth];

            for (int areaY = 0; areaY < safeHeight; areaY++)
                for (int areaX = 0; areaX < safeWidth; areaX++)
                    area[areaY, areaX] = content[(safeY + areaY), (safeX + areaX)];

            rect.Left = safeX;
            rect.Top = safeY;
            rect.Width = safeWidth;
            rect.Height = safeHeight;
            return area;
        }

        public void Clear(CharAttribute attributes = ConsoleRenderer.DefaultAttributes)
        {
            CharInfo clearChar = new CharInfo
            {
                Attributes = attributes,
                UnicodeChar = '\0'
            };

            Fill(clearChar);
        }

        public void Clear(Rectangle rect, CharAttribute attributes = ConsoleRenderer.DefaultAttributes)
        {
            CharInfo clearChar = new CharInfo
            {
                Attributes = attributes,
                UnicodeChar = '\0'
            };

            FillRect(rect, clearChar);
        }

        public void Fill(CharInfo charInfo)
        {
            int ux = content.GetLength(1);
            int uy = content.GetLength(0);
            for (int y = 0; y < uy; y++)
                for (int x = 0; x < ux; x++)
                    content[y, x] = charInfo;
        }

        public void FillRect(Rectangle rect, CharAttribute attributes = ConsoleRenderer.DefaultAttributes)
        {
            FillRect(rect.Left, rect.Top, rect.Width, rect.Height, new CharInfo { UnicodeChar = '\0', Attributes = attributes });
        }

        public void FillRect(Rectangle rect, CharInfo charInfo)
        {
            FillRect(rect.Left, rect.Top, rect.Width, rect.Height, charInfo);
        }

        public void FillRect(int x, int y, int width, int height, CharAttribute attributes = ConsoleRenderer.DefaultAttributes)
        {
            FillRect(x, y, width, height, new CharInfo { UnicodeChar = '\0', Attributes = attributes });
        }

        public void FillRect(int x, int y, int width, int height, CharInfo charInfo)
        {
            int startX = SafeSourceStart(x);
            int startY = SafeSourceStart(y);
            int endX = SafeSourceEnd(x, width, this.width);
            int endY = SafeSourceEnd(y, height, this.height);

            for (int yy = startY; yy < endY; yy++)
                for (int xx = startX; xx < endX; xx++)
                    content[y + yy, x + xx] = charInfo;
        }

        public void Draw(char c, int x, int y, CharAttribute attributes = ConsoleRenderer.DefaultAttributes)
        {
            if (IsBoundedIndex(x, width) && IsBoundedIndex(y, height))
                content[y, x] = new CharInfo
                {
                    UnicodeChar = c,
                    Attributes = attributes
                };
        }

        public void Draw(string s, int x, int y, CharAttribute attributes = ConsoleRenderer.DefaultAttributes)
        {
            int safeHeight = SafeSourceEnd(y, 1, height);

            if (safeHeight > 0)
            {
                int safeStart = SafeSourceStart(x);
                int safeEnd = SafeSourceEnd(x, s.Length, width);

                var infos = s.ToCharInfoArray(attributes);

                for (int index = safeStart; index < safeEnd; index++)
                {
                    content[y, x + index] = infos[index];
                }
            }
        }

        public void Draw(string[] strings, int x, int y, CharAttribute attributes = ConsoleRenderer.DefaultAttributes)
        {
            int safeHeight = SafeSourceEnd(y, strings.Length, height);

            if (safeHeight > 0)
            {
                int safeStartX = SafeSourceStart(x);
                int safeStartY = SafeSourceStart(y);

                int safeEndY = SafeSourceEnd(y, strings.Length, height);

                var infos = strings.ToCharInfoArray(attributes);

                for (int indexY = safeStartY; indexY < safeEndY; indexY++)
                {
                    int safeEndX = SafeSourceEnd(x, infos[indexY].Length, width);

                    for (int indexX = safeStartX; indexX < safeEndX; indexX++)
                    {
                        content[y + indexY, x + indexX] = infos[indexY][indexX];
                    }
                }
            }
        }

        public void Draw(ColorfulString s, int x, int y)
        {
            int safeHeight = SafeSourceEnd(y, 1, height);

            if (safeHeight > 0)
            {
                int safeStart = SafeSourceStart(x);
                int safeEnd = SafeSourceEnd(x, s.Length, width);

                var infos = s.ToCharInfoArray();

                for (int index = safeStart; index < safeEnd; index++)
                {
                    content[y, x + index] = infos[index];
                }
            }
        }

        public void Draw(Gradient gradient, int x, int y, int width, int height)
        {
            int startX = SafeSourceStart(x);
            int startY = SafeSourceStart(y);
            int endX = SafeSourceEnd(x, width, this.width);
            int endY = SafeSourceEnd(y, height, this.height);

            for (int xx = startX; xx < endX; xx++)
            {
                CharInfo color = gradient.Palette[(int)(xx / (width / (float)gradient.Palette.Length)) % gradient.Palette.Length];
                for (int yy = startY; yy < endY; yy++)
                {
                    content[y + yy, x + xx] = color;
                }
            }
        }

        public void Draw(BufferArea bufferArea, int x, int y)
        {
            Draw(bufferArea.Content, x, y);
        }

        public void Draw(CharInfo[,] info, int x, int y, bool withTransparancy = false, char transparentCharacter = '\0')
        {
            int areaWidth = info.GetLength(1);
            int areaHeight = info.GetLength(0);

            int safeX = SafeSourceStart(x);
            int safeY = SafeSourceStart(y);

            int safeWidth = SafeSourceEnd(x, areaWidth, width);
            int safeHeight = SafeSourceEnd(y, areaHeight, height);

            for (int areaY = safeY; areaY < safeHeight; areaY++)
            {
                for (int areaX = safeX; areaX < safeWidth; areaX++)
                {
                    if (!withTransparancy || info[areaY, areaX].UnicodeChar != transparentCharacter)
                    {
                        int drawX = x + areaX;
                        int drawY = y + areaY;
                        content[drawY, drawX] = info[areaY, areaX];
                    }
                }
            }
        }

        #region UTILITY
        [MethodImpl]
        private int SafeSourceStart(int index) => index < 0 ? -index : 0;
        private int SafeSourceEnd(int destinationIndex, int sourceSize, int destinationSize) => Math.Min(destinationSize - destinationIndex, sourceSize);
        private int BoundToWidth(int x) => Math.Max(0, Math.Min(width, x));
        private int BoundToHeight(int y) => Math.Max(0, Math.Min(height, y));
        private bool IsBoundedIndex(int index, int size) => index >= 0 && index < size;
        #endregion
    }

    public class ScreenBuffer : BufferArea
    {
        public string Name { get; }

        public ScreenBuffer(string name) : base(MyConsole.Width, MyConsole.Height)
        {
            Name = name;
        }
    }
}
