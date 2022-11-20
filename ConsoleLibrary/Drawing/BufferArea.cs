using ConsoleLibrary.Drawing.Shapes;
using ConsoleLibrary.Structures;
using ConsoleLibrary.TextExtensions;
using System;
using System.Linq;
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

        #region Management

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

        #endregion

        #region Drawing

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

        public void FillRect(DrawingOptions options, CharInfo charInfo)
        {
            int startX = SafeSourceStart(options.X);
            int startY = SafeSourceStart(options.Y);
            int endX = SafeSourceEnd(options.X, options.Width, width);
            int endY = SafeSourceEnd(options.Y, options.Height, height);

            for (int yy = startY; yy < endY; yy++)
                for (int xx = startX; xx < endX; xx++)
                    content[options.Y + yy, options.X + xx] = charInfo;
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

        public void Draw(Gradient gradient, int x, int y, int width, int height, bool vertical = false)
        {
            if (vertical)
                DrawVerticalGradient(gradient, x, y, width, height);
            else
                DrawHorizontalGradient(gradient, x, y, width, height);
        }

        private Point RotatePoint(Point p, double angle, Point origin)
        {
            Point translated = p - origin;
            return origin + new Point
            {
                X = (int)Math.Round(translated.X * Math.Cos(angle) - translated.Y * Math.Sin(angle)),
                Y = (int)Math.Round(translated.X * Math.Sin(angle) + translated.Y * Math.Cos(angle))
            };
        }

        private Rectangle CalculateBoundingBox(Point p0, Point p1, Point p2, Point p3)
        {
            int[] xVals = new[] { p0.X, p1.X, p2.X, p3.X };
            int[] yVals = new[] { p0.Y, p1.Y, p2.Y, p3.Y };
            int smallestX = Enumerable.Min(xVals);
            int smallestY = Enumerable.Min(yVals);
            int largestX = Enumerable.Max(xVals);
            int largestY = Enumerable.Max(yVals);
            return new Rectangle(smallestX, smallestY, largestX - smallestX + 1, largestY - smallestY + 1);
        }

        private Point Intersect(Point p0, Point p1, Point p2, Point p3)
        {
            long x1 = p0.X;
            long y1 = p0.Y;
            long x2 = p1.X;
            long y2 = p1.Y;
            long x3 = p2.X;
            long y3 = p2.Y;
            long x4 = p3.X;
            long y4 = p3.Y;
            double d = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

            if (d == 0)
                return default;

            int x = (int)Math.Round(((x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4)) / d);
            int y = (int)Math.Round(((x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4)) / d);

            return new Point(x, y);
        }

        private Point ClosestPointOnLine(Point p1, Point p2, Point p3)
        {
            int dy = p2.Y - p1.Y;
            int dx = p2.X - p1.X;
            float m = dx != 0 ? dy / (float)dx : 0;
            float reciprocal = m != 0 ? -(1 / m) : 0;
            float x1 = 0;
            float y1 = reciprocal * x1 + p3.Y - reciprocal * p3.X;
            float x2 = p3.X;
            float y2 = reciprocal * x2 + p3.Y - reciprocal * p3.X;
            Point p4 = new Point(
                (int)Math.Round(x1), (int)Math.Round(y1));
            Point p5 = new Point(
                (int)Math.Round(x2), (int)Math.Round(y2));
            return Intersect(p1, p2, p4, p5);
        }

        public void DrawRotatedGradient(Gradient gradient, int x, int y, int width, int height, float angle)
        {
            //y -= height / 2;
            //height *= 2;
            Rectangle area = new Rectangle(x, y, width, height);
            Point origin = new Point(x, y) + new Point(width - 1, height - 1) / 2;
            Point p0 = RotatePoint(new Point(x, y), angle, origin);
            Point p1 = RotatePoint(new Point(x + width - 1, y), angle, origin);
            Point p2 = RotatePoint(new Point(x + width - 1, y + height - 1), angle, origin);
            Point p3 = RotatePoint(new Point(x, y + height - 1), angle, origin);

            Point p4 = (p0 + (p1 - p0) / 2).Bound(area);
            Point p5 = (p1 + (p2 - p1) / 2).Bound(area);
            Point p6 = (p2 + (p3 - p2) / 2).Bound(area);
            Point p7 = (p3 + (p0 - p3) / 2).Bound(area);

            Rectangle boundingBox = CalculateBoundingBox(p0, p1, p2, p3);
            Rectangle sampleBox = CalculateBoundingBox(p4, p5, p6, p7);

            FillRect(boundingBox, CharAttribute.BackgroundDarkRed);
            FillRect(sampleBox, CharAttribute.BackgroundDarkGreen);
            Draw('+', p0.X, p0.Y, CharAttribute.ForegroundMagenta);
            Draw('+', p1.X, p1.Y, CharAttribute.ForegroundYellow);
            Draw('+', p2.X, p2.Y, CharAttribute.ForegroundCyan);
            Draw('+', p3.X, p3.Y, CharAttribute.ForegroundGreen);

            Draw('+', p4.X, p4.Y, CharAttribute.ForegroundMagenta);
            Draw('+', p5.X, p5.Y, CharAttribute.ForegroundYellow);
            Draw('+', p6.X, p6.Y, CharAttribute.ForegroundCyan);
            Draw('+', p7.X, p7.Y, CharAttribute.ForegroundGreen);

            //return;

            int startX = SafeSourceStart(x);
            int startY = SafeSourceStart(y);
            int endX = SafeSourceEnd(x, width, this.width);
            int endY = SafeSourceEnd(y, height, this.height);

            for (int xx = startX; xx < endX; xx++)
            {
                for (int yy = startY; yy < endY; yy++)
                {
                    //(int tx, int ty) = RotatePoint(new Point(xx, yy), angle, origin) + new Point(0, height / 4);
                    (int tx, int ty) = RotatePoint(new Point(xx, yy), angle, origin).Bound(boundingBox);
                    int colorIndex = (int)(ty / (boundingBox.Height / (float)gradient.Palette.Length));
                    colorIndex = Math.Max(0, Math.Min(gradient.Palette.Length - 1, colorIndex));
                    CharInfo color = gradient.Palette[colorIndex];
                    content[y + yy, x + xx] = color;
                }
            }
        }

        public void DrawRotatedGradientB(Gradient gradient, int x, int y, int width, int height, float angle)
        {
            Point origin = new Point(x, y) + new Point(width - 1, height - 1) / 2;
            Point p0 = RotatePoint(new Point(x, y), angle, origin);
            Point p1 = RotatePoint(new Point(x + width - 1, y + height - 1), angle, origin);

            int s0 = Point.Distance(p0, p1);

            int startX = SafeSourceStart(x);
            int startY = SafeSourceStart(y);
            int endX = SafeSourceEnd(x, width, this.width);
            int endY = SafeSourceEnd(y, height, this.height);

            for (int xx = startX; xx < endX; xx++)
            {
                for (int yy = startY; yy < endY; yy++)
                {
                    Point p2 = new Point(xx, yy);
                    int s1 = Point.Distance(p0, p2);
                    int s2 = Point.Distance(p1, p2);
                    Math.Acos(s0 / s1);
                    int s = (s0 + s1 + s2) / 2;
                    int distance = (int)Math.Round(2 * Math.Sqrt(s * (s - s0) * (s - s1) * (s - s2)) / s0);

                    int colorIndex = (int)(distance / (s0 / (float)gradient.Palette.Length));
                    colorIndex = Math.Max(0, Math.Min(gradient.Palette.Length - 1, colorIndex));
                    CharInfo color = gradient.Palette[colorIndex];
                    content[y + yy, x + xx] = color;
                }
            }
        }

        public void DrawRotatedGradientC(Gradient gradient, int x, int y, int width, int height, double angle)
        {
            Rectangle area = new Rectangle(x, y, width, height);
            Point origin = new Point(x, y) + new Point(width - 1, height - 1) / 2;
            //Point p0 = RotatePoint(new Point(x, y + height / 2), angle, origin);
            //Point p0 = RotatePoint(new Point(x, y), angle, origin);//.Bound(area);
            Point p1 = RotatePoint(new Point(x + width, y + height / 2), angle, origin);
            //Point p1 = RotatePoint(new Point(x + width, y + height), angle, origin);//.Bound(area);
            p1 = Game.InputManager.GetMousePosition();
            //p1.Y = origin.Y;
            Point p0 = RotatePoint(p1, Math.PI, origin);
            p0.X = 0;
            p0.Y = 0;


            Point test = Intersect(
                p0,
                p1,
                new Point(width - 5, height),
                new Point(10, height - 10)
            );

            Point test1 = ClosestPointOnLine(new Point(10, height - 10), new Point(width - 5, height), p0);

            if (test1.initialized)
            {
                Draw('A', p0.X, p0.Y, CharAttribute.BackgroundRed);
                Draw('p', test1.X, test1.Y, CharAttribute.ForegroundBlue);
                Draw('B', p1.X, p1.Y, CharAttribute.BackgroundGreen);

                Draw('D', 10, height - 10, CharAttribute.BackgroundMagenta);
                Draw('C', width - 5, height, CharAttribute.BackgroundCyan);
            }

            //return;

            int s0 = Point.Distance(p0, p1);

            int startX = SafeSourceStart(x);
            int startY = SafeSourceStart(y);
            int endX = SafeSourceEnd(x, width, this.width);
            int endY = SafeSourceEnd(y, height, this.height);

            for (int xx = startX; xx < endX; xx++)
            {
                for (int yy = startY; yy < endY; yy++)
                {
                    Point p2 = new Point(xx, yy);
                    var t = ClosestPointOnLine(p0, p1, p2);

                    int distance = t.initialized ? Point.Distance(p0, t) : xx;

                    int colorIndex = (int)(distance / (s0 / (float)gradient.Palette.Length));// % gradient.Palette.Length;
                    colorIndex = Math.Max(0, Math.Min(gradient.Palette.Length - 1, colorIndex));
                    CharInfo color = gradient.Palette[colorIndex];
                    content[y + yy, x + xx] = color;
                }
            }
        }

        private void DrawHorizontalGradient(Gradient gradient, int x, int y, int width, int height)
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

        private void DrawVerticalGradient(Gradient gradient, int x, int y, int width, int height)
        {
            int startX = SafeSourceStart(x);
            int startY = SafeSourceStart(y);
            int endX = SafeSourceEnd(x, width, this.width);
            int endY = SafeSourceEnd(y, height, this.height);

            for (int yy = startY; yy < endY; yy++)
            {
                CharInfo color = gradient.Palette[(int)(yy / (height / (float)gradient.Palette.Length)) % gradient.Palette.Length];
                for (int xx = startX; xx < endX; xx++)
                {
                    content[y + yy, x + xx] = color;
                }
            }
        }

        public void Draw(Border border, int x, int y, int width, int height, CharAttribute attributes = ConsoleRenderer.DefaultAttributes)
        {
            Draw(border.UpperLeftCorner, x, y, attributes);
            Draw(border.UpperRightCorner, x + width - 1, y, attributes);
            Draw(border.LowerRightCorner, x + width - 1, y + height - 1, attributes);
            Draw(border.LowerLeftCorner, x, y + height - 1, attributes);

            for (int xx = 1; xx < width - 1; xx++)
                Draw(border.Upper, x + xx, y, attributes);
            for (int yy = 1; yy < height - 1; yy++)
                Draw(border.Right, x + width - 1, y + yy, attributes);
            for (int xx = 1; xx < width - 1; xx++)
                Draw(border.Lower, x + xx, y + height - 1, attributes);
            for (int yy = 1; yy < height - 1; yy++)
                Draw(border.Left, x, y + yy, attributes);
        }

        public void Draw(BufferArea bufferArea, DrawingOptions options)
        {
            Draw(bufferArea.Content, options);
        }

        public void Draw(BufferArea bufferArea, int x, int y)
        {
            Draw(bufferArea.Content, x, y);
        }


        public void Draw(CharInfo[,] info, DrawingOptions options)
        {
            Draw(info, options.X, options.Y, options.WithTransparency, options.TransparentCharacter);
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

        #endregion

        #region UTILITY
        [MethodImpl]
        private int SafeSourceStart(int index) => index < 0 ? -index : 0;
        private int SafeSourceEnd(int destinationIndex, int sourceSize, int destinationSize) => Math.Min(destinationSize - destinationIndex, sourceSize);
        private int BoundToWidth(int x) => Math.Max(0, Math.Min(width, x));
        private int BoundToHeight(int y) => Math.Max(0, Math.Min(height, y));
        private bool IsBoundedIndex(int index, int size) => index >= 0 && index < size;
        #endregion
    }

    public struct DrawingOptions
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public bool WithTransparency;
        public CharAttribute Attributes;
        public char TransparentCharacter;
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
