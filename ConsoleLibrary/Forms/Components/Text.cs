using ConsoleLibrary.Drawing;
using ConsoleLibrary.Forms.Components;

namespace ConsoleLibrary.Forms.Components
{
    public class Text : Component
    {
        public string content;

        public Text() : base() { }

        public override void Draw()
        {
            int xOffset = 0;
            int yOffset = 0;

            var words = content.Split(' ');
            foreach (var word in words)
            {
                if (word.Length + xOffset > Width)
                {
                    yOffset++;
                    if (yOffset > Height - 1)
                        break;
                    xOffset = 0;
                }
                //context.DrawString(word, Left + xOffset, Top + yOffset);
                xOffset += word.Length + 1;
            }
        }
    }

    public class BorderedText : Component
    {
        public override int Top { get => base.Top; set => SetTop(value); }
        public override int Left { get => base.Left; set => SetLeft(value); }
        public override int Width { get => base.Width; set => SetWidth(value); }
        public override int Height { get => base.Height; set => SetHeight(value); }
        public Border Border { get => border; set => SetBorder(value); }
        public string Content { get => content; set => SetContent(value); }

        private Border border;
        private Text text;
        private string content;


        public BorderedText() : base()
        {
            border = new Border();
            text = new Text();
        }

        public override void Draw()
        {
            border.Draw();
            text.Draw();
        }

        private void SetTop(int top)
        {
            base.Top = top;
            border.Top = top;
            text.Top = top + 1;
        }

        private void SetLeft(int left)
        {
            base.Left = left;
            border.Left = left;
            text.Left = left + 1;
        }

        private void SetWidth(int width)
        {
            base.Width = width;
            border.Width = width;
            text.Width = width - 2;
        }

        private void SetHeight(int height)
        {
            base.Height = height;
            border.Height = height;
            text.Height = height - 2;
        }

        private void SetBorder(Border border)
        {
            border.Left = Left;
            border.Top = Top;
            border.Width = Width;
            border.Height = Height;
            this.border = border;
        }

        private void SetContent(string content)
        {
            this.content = content;
            text.content = content;
        }
    }
}
