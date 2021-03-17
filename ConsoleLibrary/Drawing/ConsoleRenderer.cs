using ConsoleLibrary.Drawing.Shapes;
using ConsoleLibrary.Structures;
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

namespace ConsoleLibrary.Drawing
{
    public static class ConsoleRenderer
    {
        private const string DefaultBufferName = "default";
        private static readonly Dictionary<string, ScreenBuffer> buffers = new Dictionary<string, ScreenBuffer>();
        private static ScreenBuffer activeBuffer = CreateScreenBuffer(DefaultBufferName);

        public const CharAttribute DefaultAttributes = CharAttribute.BackgroundBlack | CharAttribute.ForegroundGrey;

        public static ScreenBuffer ActiveBuffer => activeBuffer;

        public static Point GetFontSize() => MyConsole.GetFontSize();
        public static Point GetWindowSize() => MyConsole.GetClientSize();
        public static Point GetConsoleSize() => MyConsole.GetConsoleBufferSize();

        public static Point GetWindowCenter()
        {
            var (w, h) = MyConsole.GetConsoleBufferSize();
            return new Point(w / 2, h / 2);
        }

        public static ScreenBuffer CreateScreenBuffer(string name = "ScreenBuffer")
        {
            if (buffers.ContainsKey(name))
                throw new ArgumentException($"A screen buffer with the name \"{name}\" already exists.");

            var buffer = new ScreenBuffer(name);
            buffers.Add(name, buffer);

            return buffer;
        }

        public static void SetActiveScreenBuffer(string name)
        {
            activeBuffer = buffers.ContainsKey(name)
                ? buffers[name]
                : buffers[DefaultBufferName];
        }

        public static void SetActiveScreenBuffer(int index)
        {
            activeBuffer = index >= 0 && index < buffers.Count
                ? buffers.ElementAt(index).Value
                : buffers[DefaultBufferName];
        }

        public static void Resize(int width, int height)
        {
            activeBuffer.Resize(width, height);
        }

        public static void Clear(CharAttribute attributes = DefaultAttributes)
        {
            MyConsole.Clear(attributes);
        }

        public static void RenderOutput()
        {
            var chars = activeBuffer.Content;
            MyConsole.WriteOutput(
                chars,
                new Point(chars.GetLength(1), chars.GetLength(0)),
                new Point(0, 0)
            );
        }
    }
}
