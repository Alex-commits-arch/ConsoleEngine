using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;

namespace ConsoleLibrary.Drawing
{
    public class Gradient
    {
        private static readonly char[] shadeSequence = new char[]
        {
            ShadingCharacter.None,
            ShadingCharacter.Light,
            ShadingCharacter.Medium,
            ShadingCharacter.Dark,
            ShadingCharacter.Medium,
            ShadingCharacter.Light,
            ShadingCharacter.None,
        };

        private CharInfo[] palette;

        public CharInfo[] Palette => palette;

        public Gradient(params CharAttribute[] colors)
        {
            int paletteLength = shadeSequence.Length * (colors.Length - 1) - (colors.Length - 2);

            palette = new CharInfo[paletteLength];

            for (int colorIndex = 0; colorIndex < colors.Length - 1; colorIndex++)
            {
                CharAttribute from = colors[colorIndex];
                CharAttribute to = colors[colorIndex + 1];

                if (from <= CharAttribute.ForegroundWhite)
                    from = (CharAttribute)((int)from << 4);
                if (to > CharAttribute.ForegroundWhite)
                    to = (CharAttribute)((int)to >> 4);

                CharInfo shade = new CharInfo { Attributes = from | to };

                for (int shadeIndex = 0; shadeIndex < shadeSequence.Length; shadeIndex++)
                {
                    shade.UnicodeChar = shadeSequence[shadeIndex];
                    if (shadeIndex > shadeSequence.Length / 2)
                        shade.Attributes |= CharAttribute.Reverse;
                    palette[shadeIndex - colorIndex + colorIndex * shadeSequence.Length] = shade;
                }
            }
        }

        public void Reverse()
        {
            Array.Reverse(palette);
        }

        public void AppendColor(CharAttribute color)
        {
            CharInfo[] newPalette = new CharInfo[palette.Length + shadeSequence.Length - 1];
            Array.Copy(palette, newPalette, palette.Length);
            palette = newPalette;
        }
    }
}
