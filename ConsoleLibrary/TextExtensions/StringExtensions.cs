using ConsoleLibrary.Drawing;
using System.Linq;
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

        public static CharInfo[] ToCharInfoArray(this string s, CharAttribute attributes = ConsoleRenderer.DefaultAttributes)
        {
            CharInfo[] output = new CharInfo[s.Length];

            for (int i = 0; i < s.Length; i++)
            {
                output[i].UnicodeChar = s[i];
                output[i].Attributes = attributes;
            }
            return output;
        }

        public static CharInfo[][] ToCharInfoArray(this string[] strings, CharAttribute attributes = ConsoleRenderer.DefaultAttributes)
        {
            CharInfo[][] output = new CharInfo[strings.Length][];// strings[0]?.Length ?? 0];

            for (int i = 0; i < strings.Length; i++)
            {
                var str = strings[i];
                output[i] = new CharInfo[str.Length];
                for (int j = 0; j < output[i].Length; j++)
                {
                    output[i][j].UnicodeChar = str[j];
                    output[i][j].Attributes = attributes;
                }
            }

            return output;
        }

        public static string[] NormalizeLengths(this string[] strings, TextAlign textAlign = TextAlign.Center)
        {
            int longest = 0;

            foreach (var str in strings)
                if (str.Length > longest)
                    longest = str.Length;

            for (int i = 0; i < strings.Length; i++)
                strings[i] = textAlign == TextAlign.Left
                    ? strings[i].PadRight(longest)
                    : textAlign == TextAlign.Right
                    ? strings[i].PadLeft(longest)
                    : strings[i].PadBoth(longest);

            return strings;
        }

        public static string[] PadAround(this string[] strings, int padding = 1)
        {
            if (strings.Length == 0)
                return strings;

            int width = strings[0].Length;

            foreach (var str in strings)
                if (str.Length != width)
                    throw new System.Exception("All strings must be the same length");

            string[] output = new string[strings.Length + padding * 2];

            string padString = string.Empty.PadLeft(width + padding * 2);
            for (int i = 0; i < padding; i++)
            {
                output[i] = padString;
                output[output.Length - 1 - i] = padString;
            }

            for (int i = 0; i < strings.Length; i++)
            {
                output[i + padding] = strings[i].PadBoth(strings[i].Length + padding * 2);
            }

            return output;
        }

        public static string PadBoth(this string str, int totalWidth)
        {
            int spaces = totalWidth - str.Length;
            int padLeft = spaces / 2 + str.Length;
            return str.PadLeft(padLeft).PadRight(totalWidth);
        }
    }
}
