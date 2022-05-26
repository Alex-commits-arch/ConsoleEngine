using System;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;

namespace ConsoleLibrary.Drawing
{
    public enum ColorSelectMode
    {
        Default,
        Drag,
        Repeat,
        Bounce
    }

    public class ColorfulString
    {
        private CharInfo[] cache;
        private string prevValue;
        private CharAttribute[] attributes;

        public string Value { get; set; }
        public int Length => Value?.Length ?? 0;
        public ColorSelectMode ColorThing { get; set; }
        public CharAttribute[] Attributes { get => attributes; set => attributes = value; }

        public CharInfo[] ToCharInfoArray()
        {
            if (prevValue != Value)
            {
                cache = new CharInfo[Value.Length];
                
                Func<int, CharAttribute> colorGetter = (i) => ConsoleRenderer.DefaultAttributes;

                int length = attributes.Length;
                switch (ColorThing)
                {
                    case ColorSelectMode.Repeat:
                        colorGetter = index => attributes[index % length];
                        break;
                    case ColorSelectMode.Drag:
                        colorGetter = index => index < length
                            ? attributes[index]
                            : attributes[length - 1];
                        break;
                    case ColorSelectMode.Bounce:
                        colorGetter = index =>
                        {
                            int x = index % (length * 2);

                            if (x >= length)
                                return attributes[length - 1 - (x % length)];
                            else
                                return attributes[index % length];
                        };
                        break;
                    case ColorSelectMode.Default:
                        colorGetter = index =>
                        {
                            if (index < length)
                                return attributes[index];
                            return ConsoleRenderer.DefaultAttributes;
                        };
                        break;
                }

                for (int i = 0; i < Value.Length; i++)
                {
                    CharAttribute attribute = ConsoleRenderer.DefaultAttributes;
                    if (Attributes != null && length > 0)
                        attribute = colorGetter(i);

                    cache[i] = new CharInfo
                    {
                        UnicodeChar = Value[i],
                        Attributes = attribute
                    };
                }
                prevValue = Value;
            }

            return cache;
        }
    }
}
