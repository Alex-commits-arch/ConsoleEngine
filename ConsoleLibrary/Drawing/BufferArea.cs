using ConsoleLibrary.Drawing.Shapes;
using ConsoleLibrary.Structures;
using ConsoleLibrary.TextExtensions;
using System;
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

        public BufferArea GetBuffer(Rectangle rect)
        {
            return GetBuffer(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public BufferArea GetBuffer(int x, int y, int w, int h)
        {
            return new BufferArea(GetArea(x, y, w, h));
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

        public void Clear(CharAttribute attributes = ConsoleRenderer.DefaultAttributes)
        {
            CharInfo clearChar = new CharInfo
            {
                Attributes = attributes,
                UnicodeChar = '\0'
            };

            Fill(clearChar);
        }

        public void Fill(CharInfo charInfo)
        {
            for (int y = 0; y < content.GetLength(0); y++)
                for (int x = 0; x < content.GetLength(1); x++)
                    content[y, x] = charInfo;
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
        private int SafeSourceStart(int sourceX) => Math.Max(-sourceX, Math.Min(0, sourceX));
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

        public void Resize(int width, int height)
        {
            //this.width = width;
            (this.width, this.height) = (width, height);
            content = new CharInfo[height, width];
        }
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

            Func<int, CharAttribute> getRepeat = i => Attributes[i % Attributes.Length];

            Func<int, CharAttribute> colorGetter = (i) => ConsoleRenderer.DefaultAttributes;

            switch (ColorThing)
            {
                case ColorThing.Repeat:
                    colorGetter = i => Attributes[i % Attributes.Length];
                    break;
                case ColorThing.Drag:
                    colorGetter = i => i < Attributes.Length
                        ? Attributes[i]
                        : Attributes[Attributes.Length - 1];
                    break;
                case ColorThing.Bounce:
                    colorGetter = i =>
                    {
                        int x = i % Attributes.Length;
                        int xx = i % (Attributes.Length * 2);

                        if (xx > x)
                            return Attributes[Attributes.Length - 1 - (xx % Attributes.Length)];
                        else
                            return Attributes[x];
                    };
                    break;
                case ColorThing.Default:
                    colorGetter = i =>
                    {
                        if (i < Attributes.Length)
                            return Attributes[i];
                        return ConsoleRenderer.DefaultAttributes;
                    };
                    break;

            }

            for (int i = 0; i < Value.Length; i++)
            {
                CharAttribute attribute = ConsoleRenderer.DefaultAttributes;
                if (Attributes != null && Attributes.Length > 0)
                    attribute = colorGetter(i);
                //switch (ColorThing)
                //{
                //    case ColorThing.Repeat:
                //        attribute = Attributes[i % Attributes.Length];
                //        break;
                //    case ColorThing.Drag:
                //        attribute = i < Attributes.Length
                //            ? Attributes[i]
                //            : Attributes[Attributes.Length - 1];
                //        break;
                //    case ColorThing.Bounce:
                //        int x = i % Attributes.Length;
                //        int xx = i % (Attributes.Length * 2);

                //        if (xx > x)
                //            attribute = Attributes[Attributes.Length - 1 - (xx % Attributes.Length)];
                //        else
                //            attribute = Attributes[x];
                //        break;
                //    case ColorThing.Default:
                //        if (i < Attributes.Length)
                //            attribute = Attributes[i];
                //        break;

                //}

                infos[i] = new CharInfo
                {
                    UnicodeChar = Value[i],
                    Attributes = attribute
                };
            }

            return infos;
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
