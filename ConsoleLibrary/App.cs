using ConsoleLibrary.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsWrapper.Constants;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;

namespace ConsoleLibrary
{
    public abstract class App
    {
        public int width, height;

        protected Harness harness;

        public App(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public virtual void Initialize(Harness harness)
        {
            this.harness = harness;
            harness.RequestRedraw();
        }

        public virtual void Update(float deltaTime) { }

        //public virtual void Draw(DrawingContext context) { }
    }



    public class TestApp : App
    {
        float px, py, vx, vy;

        public TestApp(int width, int height) : base(width, height) { }

        public override void Initialize(Harness harness)
        {

            vy = 10;
            base.Initialize(harness);
        }

        public override void Update(float deltaTime)
        {
            vx = 0;

            //if (ConsoleInput.IsFirstPressed(VirtualKeys.Space))
            //    vy = -15f;
            //if (ConsoleInput.IsPressed(VirtualKeys.D))
            //    vx = 15f;
            //if (ConsoleInput.IsPressed(VirtualKeys.A))
            //    vx -= 15f;

            py += vy * deltaTime;
            px += vx * deltaTime;
            if (py >= height)
            {
                py = height - 1;
                vy = 0;
            }
            vy += 9.81f * 2 * deltaTime;

            //MyConsole.SetTitle($"FPS: {(int)(1 / deltaTime)}");
            //MyConsole.SetTitle($"Mouse: {new { test = Input.GetMousePosition() }}");
            //POINT mousePos = ConsoleInput.GetMousePosition();
            //Debug.WriteLine(new { mousePos.X, mousePos.Y });

            harness.RequestRedraw();
        }

        //public override void Draw(DrawingContext context)
        //{
        //    //context.Clear(Color.BG_BLACK);
        //    //context.Draw((int)px, (int)py, (char)PixelType.Solid);
        //    //context.Render();
        //}
    }



    public class Harness
    {
        bool running = true;
        bool pendingDraw = false;

        App app;
        //DrawingContext context;

        public void Strap(App app)
        {
            this.app = app;

            //context = new DrawingContext(app.width, app.height);
            MyConsole.SetFontSize(8, 8);
            MyConsole.SetSize(app.width, app.height);
            MyConsole.SetMode(ConsoleConstants.ENABLE_EXTENDED_FLAGS | ConsoleConstants.ENABLE_WINDOW_INPUT | ConsoleConstants.ENABLE_MOUSE_INPUT);
            MyConsole.DisableResize();

            app.Initialize(this);
        }

        public void RequestRedraw()
        {
            pendingDraw = true;
        }

        public void Run()
        {
            if (app != null)
            {
                while (running)
                {
                    app.Update(GetDeltaTime());
                    if (pendingDraw)
                    {
                        //app.Draw(context);
                        pendingDraw = false;
                    }
                    System.Threading.Thread.Sleep(1);
                }
            }
        }

        DateTime currentTime = DateTime.Now;
        DateTime prevTime = DateTime.Now;
        private float GetDeltaTime()
        {
            currentTime = DateTime.Now;
            float deltaTime = (float)(currentTime - prevTime).TotalSeconds;
            prevTime = currentTime;

            return deltaTime;
        }
    }
}
