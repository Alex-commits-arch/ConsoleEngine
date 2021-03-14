using ConsoleLibrary.Drawing;
using System;
using System.Text;

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
                if (value != text) Invalidate();
                text = value;
            }
        }

        public TextBox(ControlManager container) : base(container)
        {
        }

        private void UpdateText()
        {
            var lines = new string[Height];
            var words = text.Split(' ');

            //System.Windows.TextWrapping.

            StringBuilder sb = new StringBuilder();

        }

        public override void Draw()
        {
            if (isInvalid)
            {
                // Update local buffer
            }
            // Draw local buffer to screen buffer

            var lines = new char[Height][];
            for (int i = 0; i < Height; i++)
                lines[i] = new char[Width];
            var words = text?.Split(' ') ?? new string[0];

            int x = 0;
            int y = 0;
            foreach (var word in words)
            {
                if (word.Length <= Width)
                {
                    if (x + word.Length > Width)
                    {
                        x = 0;
                        y++;
                    }

                    if (x < Width && y < Height)
                        Array.Copy(word.ToCharArray(), 0, lines[y], x, word.Length);

                    x += word.Length + 1;
                }
            }

            var renderedLines = new string[Height];
            for (int i = 0; i < renderedLines.Length; i++)
                renderedLines[i] = new string(lines[i]);

            //ConsoleRenderer.Draw(renderedLines, new DrawArgs(left, top, attributes));
        }
    }
}
