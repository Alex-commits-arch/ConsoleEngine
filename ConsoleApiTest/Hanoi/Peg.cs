using ConsoleLibrary.Graphics.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApiTest
{
    class Peg
    {
        Stack<Rect> blocks;

        public Peg(int n)
        {
            blocks = new Stack<Rect>();
        }

        public List<Rect> GetBlocks()
        {
            return blocks.ToList();
        }

        public bool CanTake()
        {
            return blocks.Count > 0;
        }

        public bool CanPlace(Rect block)
        {
            return blocks.Count == 0 || block.width < blocks.Peek().width;
        }

        public Rect Take()
        {
            return blocks.Pop();
        }

        public void Place(Rect block)
        {
            blocks.Push(block);
        }
    }
}
