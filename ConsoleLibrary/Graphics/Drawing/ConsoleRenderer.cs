using ConsoleLibrary.Graphics.Shapes;
using ConsoleLibrary.TextExtensions;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WindowsWrapper;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;

namespace ConsoleLibrary.Graphics.Drawing
{
    public struct DrawArgs
    {
        public int x;
        public int y;
        public bool transparency;
        public CharAttribute attributes;
        public char hiddenChar;
        public bool skipBuffer;

        public DrawArgs(int x, int y, CharAttribute attributes, bool skipBuffer = false, bool transparency = false, char hiddenChar = 'Ö')
        {
            this.x = x;
            this.y = y;
            this.transparency = transparency;
            this.attributes = attributes;
            this.hiddenChar = hiddenChar;
            this.skipBuffer = skipBuffer;
        }
    }


    public static class ConsoleRenderer
    {
        private static readonly List<ScreenBuffer> buffers = new List<ScreenBuffer>();
        private static CharInfo[,] buffer = new CharInfo[MyConsole.Height, MyConsole.Width];
        private static ScreenBuffer activeBuffer;

        public const CharAttribute defaultAttributes = CharAttribute.BackgroundBlack | CharAttribute.ForegroundGrey;

        public static ScreenBuffer ActiveBuffer { get => activeBuffer; }

        public static ScreenBuffer CreateScreenBuffer()
        {
            var buffer = new ScreenBuffer(MyConsole.Width, MyConsole.Height);

            buffers.Add(buffer);

            return buffer;
        }

        public static void SetActiveBuffer(ScreenBuffer buffer)
        {
            activeBuffer = buffer;
        }

        public static void DrawBuffers()
        {
            foreach (var buffer in buffers)
            {
                var area = buffer.GetArea(0, 0, MyConsole.Width, MyConsole.Height);

                DrawOutput(area, 0, 0);
            }
        }

        public static COORD GetFontSize()
        {
            return MyConsole.GetFontSize();
        }

        public static COORD GetWindowSize()
        {
            return MyConsole.GetConsoleSize();
        }

        public static COORD GetWindowCenter()
        {
            var (w, h) = MyConsole.GetConsoleSize();
            return new COORD((short)(w / 2), (short)(h / 2));
        }

        public static void SlowClear(CharAttribute col = CharAttribute.ForegroundWhite)
        {
            var charInfo = new CharInfo
            {
                UnicodeChar = ' ',
                Attributes = col
            };
            var charInfos = new CharInfo[MyConsole.Height, MyConsole.Width];

            for (int x = 0; x < MyConsole.Width; x++)
                for (int y = 0; y < MyConsole.Height; y++)
                    charInfos[y, x] = charInfo;

            DrawOutput(charInfos, 0, 0);
        }

        public static void FastClear(CharAttribute col = CharAttribute.ForegroundWhite)
        {
            MyConsole.Clear(col);
            Array.Clear(buffer, 0, (int)buffer.LongLength);
        }

        public static void Draw(string s, DrawArgs args)
        {
            CharInfo[,] chars = new CharInfo[1, s.Length];

            CharInfo info = new CharInfo();
            for (int i = 0; i < s.Length; i++)
            {
                if (args.transparency && s[i] != args.hiddenChar && char.IsSeparator(s[i]))
                    chars[0, i] = buffer[args.y, args.x + i];
                else
                {
                    info.UnicodeChar = s[i] != args.hiddenChar ? s[i] : ' ';
                    info.Attributes = args.attributes;
                    chars[0, i] = info;
                }
            }
            //Array.Copy()
            DrawOutput(chars, args.x, args.y);
        }

        public static void Draw(string[] lines, DrawArgs args)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                Draw(lines[i], args);
                args.y++;
            }
        }

        //public static void Draw(char c, DrawArgs args)
        //{
        //    Draw(new CharInfo { Attributes = args.attributes, UnicodeChar = c }, args);
        //}

        //public static void Draw(CharInfo info, DrawArgs args)
        //{
        //    DrawOutput(new CharInfo[,] { { info } }, args);
        //}

        //public static void DrawChar(char c, int x, int y)
        //{
        //    DrawString(c.ToString(), x, y);
        //}

        public static void DrawChar(char c, int x, int y, CharAttribute attributes)
        {
            CharInfo info = new CharInfo
            {
                UnicodeChar = c,
                Attributes = attributes
            };
            DrawCharInfo(info, x, y);
        }

        public static void DrawCharInfo(CharInfo info, int x, int y)
        {
            CharInfo[,] chars = new CharInfo[,] { { info } };
            DrawOutput(chars, x, y);
        }

        public static void DrawCharInfos(CharInfo[,] chars, int x, int y)
        {
            DrawOutput(chars, x, y);
        }

        public static CharInfo GetCharInfo(int x, int y)
        {
            return buffer[y, x];
        }

        //public static char GetChar(int x, int y)
        //{
        //    return MyConsole.GetChar(x, y);
        //}

        public static void FillRect(char fill, int x, int y, int width, int height, CharAttribute attributes)
        {
            CharInfo[,] chars = new CharInfo[height, width];

            CharInfo info = new CharInfo { Attributes = attributes, UnicodeChar = fill };
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    chars[i, j] = info;
            DrawOutput(chars, x, y);
        }

        public static void DrawString(string s, int x, int y)
        {
            MyConsole.WriteOutputCharacter(s.FormatUnicode().ToCharArray(), s.Length, new COORD((short)x, (short)y), out int n);
        }

        public static void DrawString(string s, int x, int y, CharAttribute attributes, bool transparency = false)
        {
            CharInfo[,] chars = new CharInfo[1, s.Length];

            CharInfo info = new CharInfo();
            for (int i = 0; i < s.Length; i++)
            {
                info.UnicodeChar = s[i];
                info.Attributes = attributes;
                chars[0, i] = info;
            }

            DrawOutput(chars, x, y);
        }

        //public static void DrawCursor(int prevX, int prevY, int currX, int currY)
        //{
        //        Draw(GetCharInfo(prevX, prevY), new DrawArgs
        //        {
        //            x = prevX,
        //            y = prevY,
        //            skipBuffer = true
        //        });
        //        Draw(
        //            GetCharInfo(currX, currY).UnicodeChar,
        //            new DrawArgs
        //            {
        //                x = currX,
        //                y = currY,
        //                attributes = CharAttribute.BackgroundWhite | CharAttribute.ForegroundBlack,
        //                skipBuffer = true
        //            }
        //        );
            
        //}

        private static void DrawOutput(CharInfo[,] chars, int x, int y, bool skipBuffer = false)
        {
            int ux = chars.GetUpperBound(1);
            int uy = chars.GetUpperBound(0);
            MyConsole.WriteOutput(
                chars,
                new COORD((short)(ux + 1), (short)(uy + 1)),
                new COORD((short)x, (short)y)
            );

            //if (!skipBuffer)
            //{
            //    var (w, h) = GetWindowSize();
            //    for (int cy = 0; cy <= uy; cy++)
            //    {
            //        int ry = cy + y;
            //        for (int cx = 0; cx <= ux; cx++)
            //        {
            //            int rx = cx + x;
            //            if (ry >= 0 && ry < h &&
            //                rx >= 0 && rx < w)
            //            {
            //                buffer[ry, rx] = chars[cy, cx];
            //            }
            //        }
            //    }
            //}
        }

        private static void DrawOutput(CharInfo[,] chars, DrawArgs args)
        {
            int ux = chars.GetUpperBound(1);
            int uy = chars.GetUpperBound(0);
            MyConsole.WriteOutput(
                chars,
                new COORD((short)(ux + 1), (short)(uy + 1)),
                new COORD((short)args.x, (short)args.y)
            );

            //if (!args.skipBuffer)
            //{
            //    var (w, h) = GetWindowSize();
            //    for (int cy = 0; cy <= uy; cy++)
            //    {
            //        int ry = cy + args.y;
            //        for (int cx = 0; cx <= ux; cx++)
            //        {
            //            int rx = cx + args.x;
            //            if (ry >= 0 && ry < h &&
            //                rx >= 0 && rx < w)
            //            {
            //                buffer[ry, rx] = chars[cy, cx];
            //            }
            //        }
            //    }
            //}
        }
    }
}
