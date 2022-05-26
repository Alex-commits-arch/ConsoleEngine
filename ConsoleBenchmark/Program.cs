using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using ConsoleLibrary.Drawing;
using System;
using WindowsWrapper.Structs;

namespace ConsoleBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine();

            BenchmarkRunner.Run<Benchmarks>();
            //Console.ReadLine();
        }
    }

    [MemoryDiagnoser]
    public class Benchmarks
    {
        //readonly BufferArea area = new BufferArea(50, 50);
        const int sourceSize = 10;
        const int destSize = 100;
        const int range = 100;

        [Benchmark]
        public void TestA()
        {
            for (int i = 0; i <= range; i++)
                A(i, sourceSize, destSize);
        }

        [Benchmark]
        public void TestC()
        {
            for (int i = 0; i <= range; i++)
                B(i, sourceSize, destSize);
        }

        private int A(int a, int b, int c) => Math.Min(c - a, b);

        private int B(int a, int b, int c) => (c - a) < b ? c - a : b;

        //[Benchmark]
        //public void FastClear()
        //{
        //    area.Clear();
        //}

        //[Benchmark]
        //public void SlowClear()
        //{
        //    area.ClearOld();
        //}

        //[Benchmark]
        //public void SlowDraw()
        //{
        //    CharInfo[,] infos = new CharInfo[50, 50];
        //    for (int i = 0; i < infos.GetLength(0); i++)
        //        for (int j = 0; j < infos.GetLength(1); j++)
        //            infos[i, j].Attributes = WindowsWrapper.Enums.CharAttribute.ForegroundGrey;
        //    area.Draw(infos, 0, 0);
        //}
        //readonly ScreenBuffer buffer = new ScreenBuffer(84, 42);

        //[Benchmark]
        //public void Draw()
        //{
        //    CharInfo[,] infos = new CharInfo[50, 50];
        //    int ui = infos.GetLength(0);
        //    int uj = infos.GetLength(1);
        //    for (int i = 0; i < ui; i++)
        //        for (int j = 0; j < uj; j++)
        //            infos[i, j].Attributes = WindowsWrapper.Enums.CharAttribute.ForegroundGrey;
        //    area.Draw(infos, 0, 0);
        //}

        //[Benchmark]
        //public void DrawNoTransparency()
        //{
        //    CharInfo[,] infos = new CharInfo[50, 50];
        //    int ui = infos.GetLength(0);
        //    int uj = infos.GetLength(1);
        //    for (int i = 0; i < ui; i++)
        //        for (int j = 0; j < uj; j++)
        //            infos[i, j].Attributes = WindowsWrapper.Enums.CharAttribute.ForegroundGrey;
        //    area.DrawNoTransparency(infos, 0, 0);
        //}

        //[Benchmark]
        //public void DrawCacheInfo()
        //{
        //    CharInfo[,] infos = new CharInfo[50, 50];
        //    int ui = infos.GetLength(0);
        //    int uj = infos.GetLength(1);
        //    for (int i = 0; i < ui; i++)
        //        for (int j = 0; j < uj; j++)
        //            infos[i, j].Attributes = WindowsWrapper.Enums.CharAttribute.ForegroundGrey;
        //    area.DrawCacheInfo(infos, 0, 0);
        //}


        //[Benchmark]
        //public void DrawNoTransparencyCacheInfo()
        //{
        //    CharInfo[,] infos = new CharInfo[50, 50];
        //    int ui = infos.GetLength(0);
        //    int uj = infos.GetLength(1);
        //    for (int i = 0; i < ui; i++)
        //        for (int j = 0; j < uj; j++)
        //            infos[i, j].Attributes = WindowsWrapper.Enums.CharAttribute.ForegroundGrey;
        //    area.DrawNoTransparencyCacheInfo(infos, 0, 0);
        //}

        //[Benchmark]
        //public void DrawNoTransparencyInlining()
        //{
        //    CharInfo[,] infos = new CharInfo[50, 50];
        //    int ui = infos.GetLength(0);
        //    int uj = infos.GetLength(1);
        //    for (int i = 0; i < ui; i++)
        //        for (int j = 0; j < uj; j++)
        //            infos[i, j].Attributes = WindowsWrapper.Enums.CharAttribute.ForegroundGrey;
        //    area.DrawNoTransparencyInlining(infos, 0, 0);
        //}









        //[Benchmark]
        //public void Test3()
        //{
        //    CharInfo[,] infos = new CharInfo[50, 50];
        //    int ui = infos.GetLength(0);
        //    int uj = infos.GetLength(1);
        //    for (int i = 0; i < ui; i++)
        //        for (int j = 0; j < uj; j++)
        //            infos[i, j].Attributes = WindowsWrapper.Enums.CharAttribute.ForegroundGrey;
        //    area.Draw(infos, 0, 0);
        //}
    }
}
