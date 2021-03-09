using ConsoleLibrary.Forms.Interfaces;
using ConsoleLibrary.Drawing;
using ConsoleLibrary.Input;
using ConsoleLibrary.Input.Events;
using System.Diagnostics;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;
//using static ConsoleLibrary.Input.ConsoleInput;

namespace ConsoleLibrary.Forms.Components
{
    public class Button : InputComponent
    {
        private string text;
        private CharAttribute attributes = CharAttribute.ForegroundGrey;
        private CharAttribute activeAttributes = CharAttribute.BackgroundBlue | CharAttribute.ForegroundWhite;
        private CharAttribute pressedAttributes = CharAttribute.BackgroundDarkBlue | CharAttribute.ForegroundGrey;
        private CharAttribute disabledAttributes = CharAttribute.ForegroundDarkGrey;

        public string Text
        {
            get => text; set
            {
                text = value;
                Width = value.Length;
            }
        }

        public new CharAttribute Attributes { get => attributes; set => attributes = value; }
        public CharAttribute ActiveAttributes { get => activeAttributes; set => activeAttributes = value; }
        public CharAttribute PressedAttributes { get => pressedAttributes; set => pressedAttributes = value; }
        public CharAttribute DisabledAttributes { get => disabledAttributes; set => disabledAttributes = value; }

        public Button() { }

        public Button(string text) : this()
        {
            Text = text;
            Height = 1;
        }

        public override void Draw()
        {
            if (visible)
            {
                CharAttribute color = 
                    enabled ?
                        pressed ?
                        pressedAttributes :
                        active ?
                        activeAttributes :
                        attributes :
                    disabledAttributes;

                ConsoleRenderer.Draw(text, new DrawArgs(left, top, color));
            }
        }
    }
}
