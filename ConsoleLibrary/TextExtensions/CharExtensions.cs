using System.Text;
using WindowsWrapper;
using WindowsWrapper.Constants;
using WindowsWrapper.Structs;

namespace ConsoleLibrary.TextExtensions
{
    public static class CharExtensions
    {
        public static string Repeat(this char c, int n)
        {
            return new string(c, n);
        }

        public static char[,] Repeat(this char c, int w, int h)
        {
            char[,] output = new char[w, h];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    output[x, y] = c;
                }
            }
            return output;
        }

        public static CharInfo[] ToCharInfoArray(this char[,] cArr)
        {
            int w = cArr.GetLength(0);
            int h = cArr.GetLength(1);
            CharInfo[] output = new CharInfo[w * h];

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    int i = y * w + x;
                    output[i].UnicodeChar = cArr[x, y];//.FormatUnicode();
                    output[i].AsciiChar = (byte)cArr[x, y];
                    output[i].Attributes = (short)(Colors.FOREGROUND_GREEN | Colors.FOREGROUND_INTENSITY);
                }
            }
            return output;
        }

        public static char FormatUnicode(this char c)
        {
            var bytes = Encoding.Unicode.GetBytes(c.ToString());
            var res = Encoding.Unicode.GetString(bytes)[0];
            return Encoding.ASCII.GetString(bytes)[0];
        }
    }
}
