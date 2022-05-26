using WindowsWrapper.Enums;

namespace ConsoleLibrary.Drawing
{
    public struct DrawArgs
    {
        public int x;
        public int y;
        public bool transparency;
        public CharAttribute attributes;
        public char hiddenChar;
        public bool skipBuffer;

        public DrawArgs(int x, int y, CharAttribute attributes, bool skipBuffer = false, bool transparency = false, char hiddenChar = 'Ö')
        {
            this.x = x;
            this.y = y;
            this.transparency = transparency;
            this.attributes = attributes;
            this.hiddenChar = hiddenChar;
            this.skipBuffer = skipBuffer;
        }
    }
}
