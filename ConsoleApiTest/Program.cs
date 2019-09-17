using ConsoleLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApiTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new Chess.ChessApp(86, 32);
            app.Init();
        }
    }
}
