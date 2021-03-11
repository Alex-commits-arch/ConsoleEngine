using ConsoleLibrary.Drawing;
using System.Text;
using WindowsWrapper;
using WindowsWrapper.Constants;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;

namespace ConsoleLibrary.TextExtensions
{
    public static class StringExtensions
    {
        public static string FormatUnicode(this string s)
        {
            var bytes = Encoding.Unicode.GetBytes(s);
            return Encoding.ASCII.GetString(bytes);
        }

        public static CharInfo[] ToCharInfoArray(this string s, CharAttribute attributes = ConsoleRenderer.defaultAttributes)
        {
            CharInfo[] output = new CharInfo[s.Length];

            for (int i = 0; i < s.Length; i++)
            {
                output[i].UnicodeChar = s[i];
                output[i].Attributes = attributes;
            }
            return output;
        }
    }
}
