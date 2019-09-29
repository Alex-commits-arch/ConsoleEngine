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
            var harness = new Harness();
            harness.Strap(new TestApp(40, 40));
            harness.Run();

            //var app = new Chess.ChessApp(86, 34);
            //app.Init();
        }
    }
}
