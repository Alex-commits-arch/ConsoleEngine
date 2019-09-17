using ConsoleLibrary.Api.WinApi;
using ConsoleLibrary.Api.WinApi.Structs;
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

namespace ConsoleLibrary.Graphics.Drawing
{
    public class DrawingContext
    {
        public int width;
        public int height;
        SafeFileHandle consoleHandle;

        public DrawingContext(int width, int height)
        {
            this.width = width;
            this.height = height;
            consoleHandle = WinApi.CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Create, 0, IntPtr.Zero);
            buffers = new Dictionary<string, ScreenBuffer>();
            HideCursor();
        }

        public void HideCursor() { Console.CursorVisible = false; }
        public void ShowCursor() { Console.CursorVisible = true; }


        Dictionary<string, ScreenBuffer> buffers;

        public ScreenBuffer this[string name] => buffers[name];

        public ScreenBuffer CreateBuffer(string name)
        {
            buffers.Add(name, new ScreenBuffer(width, height));
            return buffers[name];
        }

        public void RenderFrame()
        {
            SmallRect smallRect = new SmallRect(0, 0, (short)width, (short)height);
            WinApi.WriteConsoleOutputW(
                consoleHandle,
                SmashBuffers(),
                new Coord((short)width, (short)height),
                new Coord(0, 0),
                ref smallRect
            );
        }

        private CharInfo[] SmashBuffers()
        {
            CharInfo[] output = new CharInfo[width * height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int i = buffers.Count - 1; i >= 0; i--)
                    {
                        ScreenBuffer buffer = buffers.ElementAt(i).Value;
                        if (buffer[x, y].UnicodeChar != '\0')
                        {
                            output[y * width + x] = buffer[x, y];

                            if(buffer[x, y].UnicodeChar == '♙')
                            {
                                var test = buffer[x, y].UnicodeChar.ToString().FormatUnicode();
                            }
                            //output[y * width + x + 1].UnicodeChar = buffer[x, y].UnicodeChar;
                            //output[y * width + x + 1].AsciiChar = buffer[x, y].AsciiChar;
                            //output[y * width + x + 1].AsciiChar = (byte)'\0';
                            break;
                        }
                    }
                }
            }

            //for (int i = 0; i < 100; i++)
            //{
            //    output[i].UnicodeChar = '♙';
            //    output[i].AsciiChar = 0x41;
            //    output[i].Attributes = 255;
            //}

            return output;
        }

        public void DrawChar(char c, int x, int y)
        {
            int n = 0;
            //WinApi.WriteConsoleOutputCharacterW(consoleHandle, c.FormatUnicode().ToString().ToCharArray(), 1, new Coord((short)x, (short)y), out n);
            WinApi.WriteConsoleOutputCharacterW(consoleHandle, c.ToString().FormatUnicode().ToCharArray(), 1, new Coord((short)x, (short)y), out n);
            //WinApi.WriteConsoleOutputCharacterW(consoleHandle, c.FormatUnicode().ToString().FormatUnicode().ToCharArray(), 1, new Coord((short)x, (short)y), out n);
            //WinApi.WriteConsoleOutputCharacterW(consoleHandle, new char['\u2659'.FormatUnicode()], 1, new Coord((short)x, (short)y), out n);
            //WinApi.WriteConsoleOutputCharacterW(consoleHandle, new char['\u2659'.FormatUnicode()], 1, new Coord((short)x, (short)y), out n);
        }

        public void DrawString(string s, int x, int y)
        {
            int n = 0;
            WinApi.WriteConsoleOutputCharacterW(consoleHandle, s.FormatUnicode().ToCharArray(), s.Length, new Coord((short)x, (short)y), out n);
        }

        public void DrawRect(char c, int x, int y, int w, int h)
        {
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                    if (i == 0 || j == 0 || i == w - 1 || j == h - 1)
                        DrawChar(c, x + i, y + j);
        }

        public void Clear()
        {
            SmallRect smallRect = new SmallRect(0, 0, (short)width, (short)height);
            WinApi.WriteConsoleOutputW(
                consoleHandle,
                ' '.Repeat(width, height).ToCharInfoArray(),
                //c.Repeat(width, height),
                new Coord((short)width, (short)height),
                new Coord(0, 0),
                ref smallRect
            );
        }

        public void DrawRect(Rect rect, int x, int y)
        {
            //rect.shape
            short width = Math.Abs((short)(rect.width + x - 1));
            short height = Math.Abs((short)(rect.height + y - 1));
            SmallRect smallRect = new SmallRect((short)x, (short)y, width, height);
            var test = rect.GetData().ToCharInfoArray();
            WinApi.WriteConsoleOutputW(
                consoleHandle,
                rect.GetData().ToCharInfoArray(),
                //.ToCharInfoArray(),
                new Coord((short)rect.width, (short)rect.height),
                new Coord(0, 0),
                ref smallRect
            );
        }

        public void FillRect(char c, int x, int y, int w, int h)
        {
            short width = Math.Abs((short)(w + x));
            short height = Math.Abs((short)(h + y));
            SmallRect smallRect = new SmallRect((short)x, (short)y, width, height);
            WinApi.WriteConsoleOutputW(
                consoleHandle,
                c.Repeat(w, h).ToCharInfoArray(),
                //c.Repeat(width, height),
                new Coord((short)w, (short)h),
                new Coord(0, 0),
                ref smallRect
            );
        }
    }
}
