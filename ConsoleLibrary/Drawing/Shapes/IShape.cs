namespace ConsoleLibrary.Drawing.Shapes
{
    public interface IShape
    {
        int Width { get; set; }
        int Height { get; set; }
        void SetData(char[,] data);
        char[,] GetData();
    }
}
