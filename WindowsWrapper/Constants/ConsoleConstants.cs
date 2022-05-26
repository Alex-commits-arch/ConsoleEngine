namespace WindowsWrapper.Constants
{
    public static class ConsoleConstants
    {
        public const short ENABLE_WINDOW_INPUT = 0x0008;
        public const short ENABLE_MOUSE_INPUT = 0x0010;
        public const short ENABLE_QUICK_EDIT_MODE = 0x0040;
        public const short ENABLE_EXTENDED_FLAGS = 0x0080;

        public const short STD_INPUT_HANDLE = -10;
        public const short STD_OUTPUT_HANDLE = -11;

        public const int KEY_EVENT = 1;
        public const int MOUSE_EVENT = 2;
        public const int RESIZE_EVENT = 4;

        public const int MOUSE_MOVED = 0x0001;
        public const int DOUBLE_CLICK = 0x0002;
        public const int MOUSE_WHEELED = 0x0004;
        public const int MOUSE_HWHEELED = 0x0008;

        public const int LEFT_MOUSE_BUTTON = 0x0001;
        public const int RIGHT_MOUSE_BUTTON = 0x0002;
        public const int MIDDLE_MOUSE_BUTTON = 0x0004;
    }
}
