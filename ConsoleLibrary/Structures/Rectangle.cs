﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLibrary.Structures
{
    public class Rectangle
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Right => X + Width;
        public int Bottom => Y + Height;

        public Rectangle()
        {

        }

        public Rectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Rectangle(Point p1, Point p2)
        {
            X = p1.X;
            Y = p1.Y;
            Width = p2.X;
            Height = p2.Y;
        }
    }
}
