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
        private static char[] shadeSequence = new char[]
        {
            ShadingCharacter.None,
            ShadingCharacter.Light,
            ShadingCharacter.Medium,
            ShadingCharacter.Dark,
            ShadingCharacter.Medium,
            ShadingCharacter.Light,
            ShadingCharacter.None,
        };

        private CharInfo[] pallette = new CharInfo[shadeSequence.Length];

        public CharInfo[] Pallette => pallette;

        /// <summary>
        /// Generates a gradient from Background* to Foreground*
        /// </summary>
        /// <param name="color"></param>
        public Gradient(CharAttribute color)
        {
            if (color >= CharAttribute.LeadingByte)// || to >= CharAttribute.LeadingByte)
                throw new Exception("Both parameters have to be valid colors");

            CharInfo shade = new CharInfo { Attributes = color };

            for (int i = 0; i < shadeSequence.Length; i++)
            {
                shade.UnicodeChar = shadeSequence[i];
                if (i > shadeSequence.Length / 2)
                    shade.Attributes |= CharAttribute.Reverse;
                pallette[i] = shade;
            }
        }

        public Gradient(params CharAttribute[] colors)
        {
            int palletteLength = shadeSequence.Length * (colors.Length - 1) - (colors.Length -2);

            pallette = new CharInfo[palletteLength];

            for (int colorIndex = 0; colorIndex < colors.Length - 1; colorIndex++)
            {
                CharAttribute from = colors[colorIndex];
                CharAttribute to = colors[colorIndex + 1];

                if (from <= CharAttribute.ForegroundWhite)
                    from = (CharAttribute)((int)from << 4);
                if(to > CharAttribute.ForegroundWhite)
                    to = (CharAttribute)((int)to >> 4);

                CharInfo shade = new CharInfo { Attributes = from | to };

                for (int shadeIndex = 0; shadeIndex < shadeSequence.Length; shadeIndex++)
                {
                    shade.UnicodeChar = shadeSequence[shadeIndex];
                    if (shadeIndex > shadeSequence.Length / 2)
                        shade.Attributes |= CharAttribute.Reverse;
                    pallette[shadeIndex - colorIndex + colorIndex * shadeSequence.Length] = shade;
                }
            }
        }

        public void Reverse()
        {
            Array.Reverse(pallette);
            //pallette = pallette.Reverse();
        }

        public void AppendColor(CharAttribute color)
        {
            CharInfo[] newPallette = new CharInfo[pallette.Length + shadeSequence.Length - 1];
            Array.Copy(pallette, newPallette, pallette.Length);
            //Array.Reverse()
            //if(pallette[pallette.Length-1].Attributes <= CharAttribute.ForegroundWhite)

        }
    }
}
