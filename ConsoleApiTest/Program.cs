using ConsoleLibrary;
using ConsoleLibrary.Forms;
using ConsoleLibrary.Input;
using ConsoleLibrary.Input.Events;
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
            //new Poker.PokerApp().Init();
            new Poker.PokerApp().Init();
            
            //WindowsWrapper.WinApi.SetCursor();
            ConsoleInput.ReadInput();
            
        }
    }

    //class TestApp : ConsoleApp
    //{
    //    public TestApp(int width = 80, int height = 30) : base(width, height) { }

    //    public override void Init()
    //    {
    //        base.Init();
    //        ConsoleInput.KeyPressed += KeyPressed;
    //    }

    //    private void KeyPressed(KeyEventArgs args)
    //    {
    //        Debug.WriteLine(args.Key);
    //    }
    //}
}
