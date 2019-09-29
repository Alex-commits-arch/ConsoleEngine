using System.Text;
using WindowsWrapper;
using WindowsWrapper.Constants;

namespace ConsoleLibrary.TextExtensions
{
    public static class StringExtensions
    {
        public static string FormatUnicode(this string s)
        {
            var bytes = Encoding.Unicode.GetBytes(s);
            return Encoding.ASCII.GetString(bytes);
        }

        public static CharInfo[] ToCharInfoArray(this string s)
        {
            CharInfo[] output = new CharInfo[s.Length];

            for (int i = 0; i < s.Length; i++)
            {
                output[i].UnicodeChar = s[i];
                output[i].AsciiChar = (byte)s[i];
                output[i].Attributes = (ushort)(Colors.FOREGROUND_GREEN | Colors.FOREGROUND_INTENSITY);
            }
            return output;
        }
    }
}
