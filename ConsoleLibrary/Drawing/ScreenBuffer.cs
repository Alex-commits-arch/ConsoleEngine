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
        CharInfo[,] content;
        readonly int width;
        readonly int height;

        public CharInfo[,] Content => content;
        public int Width { get => width; }
        public int Height { get => height; }

        public ScreenBuffer(int width, int height)
        {
            this.width = width;
            this.height = height;
            content = new CharInfo[height, width];
        }

        public ScreenBuffer(CharInfo[,] content)
        {
            width = content.GetLength(1);
            height = content.GetLength(0);
            this.content = content;
        }

        public ScreenBuffer GetBuffer(Rectangle rect)
        {
            return GetBuffer(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public ScreenBuffer GetBuffer(int x, int y, int w, int h)
        {
            return new ScreenBuffer(GetArea(x, y, w, h));
        }

        public CharInfo[,] GetArea(Rectangle rect)
        {
            return GetArea(rect.X, rect.Y, rect.Width, rect.Height);
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

        public void Clear(CharAttribute attributes = ConsoleRenderer.defaultAttributes)
        {
            CharInfo clearChar = new CharInfo
            {
                Attributes = attributes,
                UnicodeChar = '\0'
            };

            for (int y = 0; y < content.GetLength(0); y++)
                for (int x = 0; x < content.GetLength(1); x++)
                    content[y, x] = clearChar;
        }

        public void Draw(char c, int x, int y, CharAttribute attributes = ConsoleRenderer.defaultAttributes)
        {
            if (IsBoundedIndex(x, width) && IsBoundedIndex(y, height))
                content[y, x] = new CharInfo
                {
                    UnicodeChar = c,
                    Attributes = attributes
                };
        }

        public void Draw(string s, int x, int y, CharAttribute attributes = ConsoleRenderer.defaultAttributes)
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

        public void Draw(ScreenBuffer bufferArea, int x, int y)
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
        private int SafeSourceStart(int sourceX) => Math.Max(-sourceX, Math.Min(0, sourceX));
        private int SafeSourceEnd(int destinationIndex, int sourceSize, int destinationSize) => Math.Min(destinationSize - destinationIndex, sourceSize);
        private int BoundToWidth(int x) => Math.Max(0, Math.Min(width, x));
        private int BoundToHeight(int y) => Math.Max(0, Math.Min(height, y));
        private bool IsBoundedIndex(int index, int size) => index >= 0 && index < size;
        #endregion
    }

    public class ColorfulString
    {
        public string Value { get; set; }
        public int Length => Value?.Length ?? 0;
        public ColorThing ColorThing { get; set; }
        public CharAttribute[] Attributes { get; set; }

        public CharInfo[] ToCharInfoArray()
        {
            CharInfo[] infos = new CharInfo[Value.Length];


            for (int i = 0; i < Value.Length; i++)
            {
                CharAttribute attribute = ConsoleRenderer.defaultAttributes;
                if (Attributes != null && Attributes.Length > 0)
                    switch (ColorThing)
                    {
                        case ColorThing.Repeat:
                            attribute = Attributes[i % Attributes.Length];
                            break;
                        case ColorThing.Drag:
                            attribute = i < Attributes.Length
                                ? Attributes[i]
                                : Attributes[Attributes.Length - 1];
                            break;
                        case ColorThing.Bounce:
                            int x = i % Attributes.Length;
                            int xx = i % (Attributes.Length * 2);

                            if (xx > x)
                                attribute = Attributes[Attributes.Length - 1 - (xx % Attributes.Length)];
                            else
                                attribute = Attributes[x];
                            break;
                        case ColorThing.Default:
                            if (i < Attributes.Length)
                                attribute = Attributes[i];
                            break;

                    }

                infos[i] = new CharInfo
                {
                    UnicodeChar = Value[i],
                    Attributes = attribute
                };
            }

            return infos;

            //CharInfo[][] test = new CharInfo[1][];
            //test[0] = infos;
        }
    }

    public enum ColorThing
    {
        Default,
        Drag,
        Repeat,
        Bounce
    }
}
