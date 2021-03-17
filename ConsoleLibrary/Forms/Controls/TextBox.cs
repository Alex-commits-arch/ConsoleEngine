using ConsoleLibrary.Drawing;
using ConsoleLibrary.TextExtensions;
using System;
using System.Linq;
using System.Text;
using WindowsWrapper.Structs;

namespace ConsoleLibrary.Forms.Controls
{
    public class TextBox : Control
    {
        public string Text { get; set; }
        public WordBreak WordBreak { get; set; }
        public TextAlign TextAlign { get; set; }

        public TextBox(ControlManager container) : base(container)
        {

        }

        protected override void RefreshBuffer()
        {
            base.RefreshBuffer();

            if (Height > 0)
            {
                var lines = new StringBuilder[Height];

                for (int i = 0; i < lines.Length; i++)
                    lines[i] = new StringBuilder(Width);

                var words = Text.Split(' ').ToList();

                int lineIndex = 0;
                for (int wordIndex = 0; wordIndex < words.Count; wordIndex++)
                {
                    var word = words[wordIndex];
                    var line = lines[lineIndex];
                    int lineLength = line.Length + word.Length;

                    if (lineLength > Width)
                    {
                        if (word.Length > Width || WordBreak == WordBreak.Hard)
                        {
                            int leftLength = Width - line.Length;

                            var leftPart = word.Substring(0, leftLength);
                            var rightPart = word.Substring(leftLength, word.Length - leftLength);

                            word = leftPart;
                            words.Insert(wordIndex + 1, rightPart);
                        }
                        else
                        {
                            words.Insert(wordIndex + 1, word);
                            word = "";
                        }
                    }

                    line.Append(word);
                    if (line.Length + 1 < Width)
                    {
                        line.Append(' ');
                    }
                    else
                    {
                        lineIndex++;
                        if (lineIndex >= lines.Length)
                            break;
                    }
                }

                var renderedLines = new string[Height];
                for (int i = 0; i < renderedLines.Length; i++)
                    renderedLines[i] = lines[i].ToString();

                renderedLines.PadAll(Width, TextAlign);

                buffer.Draw(renderedLines, 0, 0, attributes);
            }
        }
    }
}
