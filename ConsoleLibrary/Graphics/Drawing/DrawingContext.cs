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
    public class DrawingContext
    {
        public int width;
        public int height;
        SafeFileHandle consoleHandle;
        CharInfo[] buffer;

        public DrawingContext(int width, int height)
        {
            this.width = width;
            this.height = height;
            buffer = new CharInfo[width * height];
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
            SMALL_RECT smallRect = new SMALL_RECT(0, 0, (short)width, (short)height);
            WinApi.WriteConsoleOutputW(
                consoleHandle,
                SmashBuffers(),
                new COORD((short)width, (short)height),
                new COORD(0, 0),
                ref smallRect
            );
        }

        public void Clear(Color col = Color.FG_WHITE)
        {
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    Draw(x, y, ' ', col);
        }

        void Fill(int x1, int y1, int x2, int y2, char c = (char)PixelType.Solid, Color col = Color.FG_WHITE)
        {
            x1 = Math.Max(Math.Min(x1, width), 0);
            y1 = Math.Max(Math.Min(y1, height), 0);
            x2 = Math.Max(Math.Min(x2, width), 0);
            y2 = Math.Max(Math.Min(y2, height), 0);

            for (int x = x1; x < x2; x++)
                for (int y = y1; y < y2; y++)
                    Draw(x, y, c, col);
        }

        public void Draw(int x, int y, char c = (char)PixelType.Solid, Color col = Color.FG_WHITE)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                buffer[y * width + x].UnicodeChar = c;
                buffer[y * width + x].Attributes = (short)col;
            }
        }

        public void Render()
        {
            SMALL_RECT smallRect = new SMALL_RECT(0, 0, (short)width, (short)height);
            WinApi.WriteConsoleOutputW(
                consoleHandle,
                buffer,
                new COORD((short)width, (short)height),
                new COORD(0, 0),
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
                            break;
                        }
                    }
                }
            }
            
            return output;
        }

        public void DrawChar(char c, int x, int y)
        {
            int n = 0;
            //WinApi.WriteConsoleOutputCharacterW(consoleHandle, c.FormatUnicode().ToString().ToCharArray(), 1, new Coord((short)x, (short)y), out n);
            WinApi.WriteConsoleOutputCharacterW(consoleHandle, c.ToString().FormatUnicode().ToCharArray(), 1, new COORD((short)x, (short)y), out n);
            //WinApi.WriteConsoleOutputCharacterW(consoleHandle, c.FormatUnicode().ToString().FormatUnicode().ToCharArray(), 1, new Coord((short)x, (short)y), out n);
            //WinApi.WriteConsoleOutputCharacterW(consoleHandle, new char['\u2659'.FormatUnicode()], 1, new Coord((short)x, (short)y), out n);
            //WinApi.WriteConsoleOutputCharacterW(consoleHandle, new char['\u2659'.FormatUnicode()], 1, new Coord((short)x, (short)y), out n);
        }

        public void DrawString(string s, int x, int y)
        {
            int n = 0;
            WinApi.WriteConsoleOutputCharacterW(consoleHandle, s.FormatUnicode().ToCharArray(), s.Length, new COORD((short)x, (short)y), out n);
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
            SMALL_RECT smallRect = new SMALL_RECT(0, 0, (short)width, (short)height);
            WinApi.WriteConsoleOutputW(
                consoleHandle,
                ' '.Repeat(width, height).ToCharInfoArray(),
                //c.Repeat(width, height),
                new COORD((short)width, (short)height),
                new COORD(0, 0),
                ref smallRect
            );
        }

        public void DrawRect(Rect rect, int x, int y)
        {
            //rect.shape
            short width = Math.Abs((short)(rect.width + x - 1));
            short height = Math.Abs((short)(rect.height + y - 1));
            SMALL_RECT smallRect = new SMALL_RECT((short)x, (short)y, width, height);
            var test = rect.GetData().ToCharInfoArray();
            WinApi.WriteConsoleOutputW(
                consoleHandle,
                rect.GetData().ToCharInfoArray(),
                //.ToCharInfoArray(),
                new COORD((short)rect.width, (short)rect.height),
                new COORD(0, 0),
                ref smallRect
            );
        }

        public void FillRect(char c, int x, int y, int w, int h)
        {
            short width = Math.Abs((short)(w + x));
            short height = Math.Abs((short)(h + y));
            SMALL_RECT smallRect = new SMALL_RECT((short)x, (short)y, width, height);
            WinApi.WriteConsoleOutputW(
                consoleHandle,
                c.Repeat(w, h).ToCharInfoArray(),
                //c.Repeat(width, height),
                new COORD((short)w, (short)h),
                new COORD(0, 0),
                ref smallRect
            );
        }
    }
}
