using ConsoleLibrary.Drawing;
using System;

namespace ConsoleLibrary.Forms.Controls
{
    public class TextBox : Control
    {
        private string text;

        public string Text
        {
            get => text;
            set
            {

                text = value;
            }
        }

        public TextBox(ControlManager container) : base(container)
        {
        }

        public override void Draw()
        {
            var lines = new char[height][];
            for (int i = 0; i < height; i++)
                lines[i] = new char[width];
            var words = text?.Split(' ') ?? new string[0];

            int x = 0;
            int y = 0;
            foreach (var word in words)
            {
                if (word.Length <= width)
                {
                    if (x + word.Length > width)
                    {
                        x = 0;
                        y++;
                    }

                    if(x < width && y < height)
                    Array.Copy(word.ToCharArray(), 0, lines[y], x, word.Length);

                    x += word.Length + 1;
                }
            }

            var renderedLines = new string[height];
            for (int i = 0; i < renderedLines.Length; i++)
                renderedLines[i] = new string(lines[i]);

            ConsoleRenderer.Draw(renderedLines, new DrawArgs(left, top, attributes));
        }
    }
}
