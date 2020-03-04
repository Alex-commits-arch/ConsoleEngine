using ConsoleLibrary.Graphics.Drawing;
using ConsoleLibrary.Graphics.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsWrapper.Constants;
using ConsoleLibrary.Forms.Components;
using ConsoleLibrary.Structures;
using static ConsoleLibrary.Forms.Components.RoundedBorder;

namespace ConsoleLibrary.Forms
{
    public abstract class Form
    {
        private int width = 40;
        private int height = 20;
        private string title = "Form";
        protected DrawingContext drawingContext;
        private List<Component> components;

        public int Width { get => width; set => Resize(value, height); }
        public int Height { get => height; set => Resize(width, value); }
        public string Title { get => title; set => SetTitle(value); }

        public Form()
        {
            components = new List<Component>();
            drawingContext = new DrawingContext(width, height);
            MyConsole.SetMode(ConsoleConstants.ENABLE_EXTENDED_FLAGS | ConsoleConstants.ENABLE_WINDOW_INPUT | ConsoleConstants.ENABLE_MOUSE_INPUT);
            MyConsole.HideCursor();
            MyConsole.DisableResize();
        }

        public virtual void Init()
        {
            Resize(width, height);
            Render();
        }

        public virtual void Render()
        {
            foreach (var component in components)
            {
                component.Render();
            }
        }

        protected void AddComponent(Component component)
        {
            components.Add(component);
        }

        protected void Resize(int width, int height)
        {
            this.width = width;
            this.height = height;
            drawingContext.Reset(width, height);
            MyConsole.SetSize(width, height);
        }

        private void SetTitle(string title)
        {
            this.title = title;
            MyConsole.SetTitle(title);
        }
    }

    public class TestForm : Form
    {
        public TestForm() : base()
        {
            Title = "Hello";
            Width = 60;

            BorderedText borderedText = new BorderedText(drawingContext);
            borderedText.Left = 0;
            borderedText.Top = 0;
            borderedText.Width = 20;
            borderedText.Height = 10;
            borderedText.Border = new RoundedBorder(drawingContext);
            borderedText.Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Lorem ipsum dolor sit amet, consectetur adipiscing elit";
            AddComponent(borderedText);
        }

        public override void Init()
        {
            base.Init();
        }
    }
}
