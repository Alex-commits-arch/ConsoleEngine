using ConsoleLibrary;
using ConsoleLibrary.Forms;
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
            //var harness = new Harness();
            //harness.Strap(new TestApp(40, 40));
            //harness.Run();

            //var form = new TestForm();
            //form.Init();
            //MyConsole
            new TestApp().Init();
            Console.ReadKey();

            //var app = new Chess.ChessApp(86, 34);
            //app.Init();
        }
    }

    class TestApp : ConsoleApp
    {
        public TestApp(int width = 80, int height = 30) : base(width, height)
        {
        }

        public override void Init()
        {
            base.Init();
            Loop(() =>
            {

            });
        }

        private void Update()
        {

        }

        //public override void Loop(Action update)
        //{
        //    base.Loop(update);
        //}
    }
}
