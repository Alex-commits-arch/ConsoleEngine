using ConsoleLibrary.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsWrapper.Constants;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;

namespace ConsoleLibrary.Game
{
    public abstract class GameApp : ConsoleApp
    {
        public int width, height;

        protected Harness harness;

        public GameApp(int width, int height) : base(width, height)
        {
            this.width = width;
            this.height = height;
        }

        public virtual void Initialize(Harness harness)
        {
            this.harness = harness;
        }

        public virtual void Update(float deltaTime) { }
        public virtual void Draw(BufferArea buffer) { }
    }

    public class Harness
    {
        bool running = true;
        bool pendingDraw = false;

        GameApp app;

        public void Strap(GameApp app)
        {
            this.app = app;

            //MyConsole.Test();
            //MyConsole.SetFontSize(9, 18);
            //MyConsole.SetFontSize(8, 8);
            MyConsole.SetMode(ConsoleModes.ENABLE_EXTENDED_FLAGS | ConsoleModes.ENABLE_WINDOW_INPUT | ConsoleModes.ENABLE_MOUSE_INPUT);
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
                //InputManager.InputLoop();
                while (running && !MyConsole.Exiting)
                {
                    //InputManager.HandleInput();
                    app.Update(GetDeltaTime());
                    if (pendingDraw)
                    {
                        MyConsole.HideCursor();
                        app.Draw(ConsoleRenderer.ActiveBuffer);
                        ConsoleRenderer.RenderOutput();
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
