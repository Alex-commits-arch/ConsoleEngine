using ConsoleLibrary.Drawing;
using ConsoleLibrary.Input.Events;
using System.Diagnostics;
using WindowsWrapper.Enums;

namespace ConsoleLibrary.Forms.Controls
{
    public class Button : Control
    {
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                //Width = value.Length;
            }
        }


        private string _text;
        private bool hover = false;
        private bool pressed = false;

        public Button(Control container) : base(container)
        {

        }

        protected internal override void HandleMousePressed(MouseEventArgs args)
        {
            base.HandleMousePressed(args);

            BeginUpdate();
            pressed = true;
            EndUpdate();
        }

        protected internal override void HandleMouseReleased(MouseEventArgs args)
        {
            base.HandleMouseReleased(args);

            BeginUpdate();
            pressed = false;
            EndUpdate();
        }

        protected internal override void HandleMouseEnter(MouseEventArgs args)
        {
            base.HandleMouseEnter(args);

            BeginUpdate();
            hover = true;
            EndUpdate();
        }

        protected internal override void HandleMouseLeave(MouseEventArgs args)
        {
            base.HandleMouseLeave(args);

            BeginUpdate();
            hover = false;
            EndUpdate();
        }

        protected override void RefreshBuffer()
        {
            base.RefreshBuffer();

            CharAttribute attribute = pressed
                ? CharAttribute.BackgroundDarkBlue | CharAttribute.ForegroundGrey
                : hover
                    ? CharAttribute.BackgroundBlue | CharAttribute.ForegroundWhite
                    : CharAttribute.ForegroundGrey;

            buffer.Draw(Text, Left, Top, attribute);
        }
        //public override void Draw(BufferArea drawingBuffer)
        //{
        //    drawingBuffer.Draw(Text, Left, Top);
        //}
    }
}
