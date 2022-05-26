using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLibrary.Forms.Controls
{
    public class ScrollableTextBox : Control
    {
        VerticalScrollbar scrollbar;
        FlexibleTextBox textBox;


        public override int Height
        {
            get => base.Height; 
            set
            {
                int height = Math.Max(2, value);
                base.Height = height;
                textBox.Height = height;
            }
        }
        public override int Width
        {
            get => base.Width;
            set
            {
                base.Width = Math.Max(1, value);
                textBox.Width = value - 1;
            }
        }

        public ScrollableTextBox(ControlManager manager) : base(manager)
        {
            scrollbar = new VerticalScrollbar(manager);
            textBox = new FlexibleTextBox(manager);
        }

        protected override void RefreshBuffer()
        {
            base.RefreshBuffer();

            

            buffer.Draw(textBox.Buffer, 0, 0);
            buffer.Draw(scrollbar.Buffer, Width - 1, 0);
        }
    }
}
