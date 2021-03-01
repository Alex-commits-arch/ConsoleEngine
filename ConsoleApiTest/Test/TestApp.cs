using ConsoleLibrary.Forms;
using ConsoleLibrary.Forms.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApiTest.Test
{
    public class TestApp : FormApp
    {
        public TestApp(int width = 80, int height = 16) : base(width, height)
        {

        }

        public override void Init()
        {
            base.Init();

            var b = new Button("Test");
            b.Left = Width / 2;
            b.Top = Height / 2;
            b.Click += B_Click;
            b.Hide();

            //Components.Add(b);

            //DrawComponents();
        }

        private void B_Click(object sender, ConsoleLibrary.Input.Events.MouseEventArgs e)
        {
            Debug.WriteLine("works");
        }
    }
}
