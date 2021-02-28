using ConsoleLibrary;
using ConsoleLibrary.Forms;
using ConsoleLibrary.Graphics.Drawing;
using ConsoleLibrary.Input;
using ConsoleLibrary.Input.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsWrapper.Enums;
namespace ConsoleApiTest
{
    class Program
    {
        static void Main(string[] args)
        {
            new TestApp().Init();
            ConsoleInput.ReadInput();
        }
    }

    public class TestApp : FormApp
    {
        public TestApp(int width = 90, int height = 40) : base(width, height)
        {
            Title = "Control Testing";
        }

        public override void Init()
        {
            base.Init();

            var container = new ControlManager();

            //var control3 = new Control(container)
            //{
            //    Width = 9,
            //    Height = 4,
            //    Name = "Control 3",
            //    Attribute = CharAttribute.BackgroundDarkCyan,
            //    FocusAttribute = CharAttribute.BackgroundCyan
            //};
            //control3.MouseEnter += Control_MouseEnter;
            //control3.MouseLeave += Control_MouseLeave;
            //control3.MouseMoved += Control_MouseMoved;

            //var control4 = new Control(container)
            //{
            //    Width = 9,
            //    Height = 4,
            //    X = 9,
            //    Name = "Control 4",
            //    Attribute = CharAttribute.BackgroundDarkMagenta,
            //    FocusAttribute = CharAttribute.BackgroundMagenta,
            //    BackgroundShade = BackgroundShade.Light
            //};

            //var control5 = new Control(container)
            //{
            //    Width = 9,
            //    Height = 4,
            //    X = 18,
            //    Name = "Control 5",
            //    Attribute = CharAttribute.BackgroundDarkYellow,
            //    FocusAttribute = CharAttribute.BackgroundYellow,
            //    BackgroundShade = BackgroundShade.Medium
            //};

            //var control6 = new Control(container)
            //{
            //    Width = 9,
            //    Height = 4,
            //    X = 27,
            //    Name = "Control 6",
            //    Attribute = CharAttribute.BackgroundDarkGreen,
            //    FocusAttribute = CharAttribute.BackgroundGreen,
            //    BackgroundShade = BackgroundShade.Dark
            //};

            //var attributes = new[]
            //{
            //    CharAttribute.BackgroundGrey | CharAttribute.ForegroundBlack,
            //    CharAttribute.BackgroundBlack | CharAttribute.ForegroundGrey,
            //    //CharAttribute.BackgroundYellow,
            //    //CharAttribute.BackgroundGreen

            //};
            int w = 9;
            int h = 4;
            //for (int i = 0; i < 8; i++)
            //{
            //    for (int j = 0; j < 8; j++)
            //    {
            //        bool white = (j + i) % 2 == 0;
            //        var control = new Control(container)
            //        {
            //            X = w * j,
            //            Y = h * i,
            //            Width = w,
            //            Height = h,
            //            Attribute = attributes[Convert.ToInt32(!white)],
            //            BackgroundShade = white ? BackgroundShade.None : BackgroundShade.Light
            //        };
            //    }
            //}

            var shades = new[]
            {
                BackgroundShade.None,
                BackgroundShade.Light,
                BackgroundShade.Medium,
                BackgroundShade.Dark,
            };

            var colors = new[]
            {
                CharAttribute.BackgroundGreen | CharAttribute.ForegroundBlack,
                CharAttribute.BackgroundBlack | CharAttribute.ForegroundGreen,
            };


            for (int i = 0; i < shades.Length; i++)
            {
                var shade = shades[i];
                var green = new Control(container)
                {
                    X = w * i,
                    Width = w,
                    Height = h,
                    Attribute = colors[0],
                    BackgroundShade = shade
                };
                var black = new Control(container)
                {
                    X = w * i ,
                    Y = h,
                    Width = w,
                    Height = h,
                    Attribute = colors[1],
                    BackgroundShade = shade
                };
            }

            container.Draw();

            ConsoleInput.Resized += delegate (ResizedEventArgs e)
            {
                ConsoleRenderer.FastClear();
                container.Draw();
            };
        }

        private void Control_MouseMoved(object sender, MouseEventArgs e)
        {
            Debug.WriteLine((sender as Control)?.Name + " moved");
        }

        private void Control_MouseLeave(object sender, MouseEventArgs e)
        {
            Debug.WriteLine((sender as Control)?.Name + " left");
        }

        private void Control_MouseEnter(object sender, MouseEventArgs e)
        {
            Debug.WriteLine((sender as Control)?.Name + " entered");
        }
    }

    public enum EventType
    {
        MouseMoved,
        MouseEnter,
        MouseLeave,
        MouseDragged,
        MousePressed,
        MouseReleased,
        MouseDoubleClick
    }

    public class ControlManager : IContainer
    {
        private List<Control> controls;
        private Control controlUnderMouse;
        private int prevMouseX;
        private int prevMouseY;
        private Dictionary<EventType, EventHandlerList> events;

        public ComponentCollection Components => new ComponentCollection(controls.ToArray());

        public ControlManager()
        {
            controls = new List<Control>();

            events = new Dictionary<EventType, EventHandlerList>
            {
                {EventType.MouseMoved,       new EventHandlerList() },
                {EventType.MouseEnter,       new EventHandlerList() },
                {EventType.MouseLeave,       new EventHandlerList() },
                {EventType.MouseDragged,     new EventHandlerList() },
                {EventType.MousePressed,     new EventHandlerList() },
                {EventType.MouseReleased,    new EventHandlerList() },
                {EventType.MouseDoubleClick, new EventHandlerList() }
            };

            ConsoleInput.MouseMoved += ConsoleInput_MouseMoved;
            ConsoleInput.MouseDragged += ConsoleInput_MouseDragged;
            ConsoleInput.MousePressed += ConsoleInput_MousePressed;
            ConsoleInput.MouseReleased += ConsoleInput_MouseReleased;
            ConsoleInput.MouseDoubleClick += ConsoleInput_MouseDoubleClick;

            //ConsoleInput.KeyHeld += ConsoleInput_KeyHeld;
            //ConsoleInput.KeyPressed += ConsoleInput_KeyPressed;
            //ConsoleInput.KeyReleased += ConsoleInput_KeyReleased;
        }

        private void ConsoleInput_KeyHeld(KeyEventArgs keyEventArgs)
        {
            throw new NotImplementedException();
        }

        private void ConsoleInput_KeyReleased(KeyEventArgs keyEventArgs)
        {
            throw new NotImplementedException();
        }

        private void ConsoleInput_KeyPressed(KeyEventArgs keyEventArgs)
        {
            throw new NotImplementedException();
        }

        public void SubscribeMouseEvent(EventType type, object subscriber, MouseEventHandler handler) => events[type].AddHandler(subscriber, handler);
        public void UnsubscribeMouseEvent(EventType type, object subscriber, MouseEventHandler handler) => events[type].RemoveHandler(subscriber, handler);

        private void ConsoleInput_MouseMoved(object _, MouseEventArgs e)
        {
            (int x, int y) = e.Location;
            if (x != prevMouseX || y != prevMouseY)
            {
                if (controlUnderMouse != null)
                {
                    if (controlUnderMouse.ContainsPoint(x, y))
                    {
                        InvokeMouseEvent(EventType.MouseMoved, controlUnderMouse, e);
                    }
                    else
                    {
                        var control = ControlUnderMouse(x, y);

                        InvokeMouseEvent(EventType.MouseEnter, control, e);
                        InvokeMouseEvent(EventType.MouseLeave, controlUnderMouse, e);

                        controlUnderMouse = control;
                    }
                }
                else
                {
                    controlUnderMouse = ControlUnderMouse(x, y);
                    InvokeMouseEvent(EventType.MouseEnter, controlUnderMouse, e);
                }
                prevMouseX = x;
                prevMouseY = y;
            }
        }

        private void ConsoleInput_MouseDragged(object _, MouseEventArgs e) => HandleMouseEvent(EventType.MouseDragged, e);
        private void ConsoleInput_MousePressed(object _, MouseEventArgs e) => HandleMouseEvent(EventType.MousePressed, e);
        private void ConsoleInput_MouseReleased(object _, MouseEventArgs e) => HandleMouseEvent(EventType.MouseReleased, e);
        private void ConsoleInput_MouseDoubleClick(object _, MouseEventArgs e) => HandleMouseEvent(EventType.MouseDoubleClick, e);

        private void HandleMouseEvent(EventType type, MouseEventArgs e)
        {
            (int x, int y) = e.Location;
            var control = ControlUnderMouse(x, y);
            InvokeMouseEvent(type, control, e);
        }

        private Control ControlUnderMouse(int x, int y)
        {
            return controls
                .Where(control => control.Visible && control.Enabled && control.ContainsPoint(x, y))
                .LastOrDefault();
        }

        private void InvokeMouseEvent(EventType type, Control control, MouseEventArgs e)
        {
            (events[type][control] as MouseEventHandler)?.Invoke(control, e);
        }

        public void Draw()
        {
            foreach (var control in controls)
                control.Draw();
        }

        #region Container Implementation
        public void Add(IComponent component)
        {
            if (component is Control control)
                controls.Add(control);
        }

        public void Add(IComponent component, string name)
        {
            throw new NotImplementedException();
        }

        public void Remove(IComponent component)
        {
            if (component is Control control)
                controls.Remove(control);
        }

        public void Dispose()
        {
            foreach (var control in controls)
                control.Dispose();
        }
        #endregion
    }

    public enum BackgroundShade
    {
        None,
        Light,
        Medium,
        Dark
    }

    public class Control : Component
    {
        private static readonly Dictionary<BackgroundShade, char> shades = new Dictionary<BackgroundShade, char>
        {
            { BackgroundShade.None, '\u0020' },
            { BackgroundShade.Light, '\u2591' },
            { BackgroundShade.Medium, '\u2592' },
            { BackgroundShade.Dark, '\u2593' },
        };

        private int x;
        private int y;
        private int width;
        private int height;
        private CharAttribute attribute = CharAttribute.BackgroundBlack;
        private CharAttribute focusAttribute = CharAttribute.BackgroundBlack;
        private BackgroundShade backgroundShade;
        private bool focused;
        private bool visible = true;
        private bool enabled = true;
        private string name;
        protected ControlManager container;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int Width { get => width; set => width = Math.Max(0, value); }
        public int Height { get => height; set => height = Math.Max(0, value); }
        public CharAttribute Attribute { get => attribute; set => attribute = value; }
        public CharAttribute FocusAttribute { get => focusAttribute; set => focusAttribute = value; }
        public BackgroundShade BackgroundShade { get => backgroundShade; set => backgroundShade = value; }
        public string Name { get => name; set => name = value; }
        public bool Visible { get => visible; set => visible = value; }
        public bool Enabled { get => enabled; set => enabled = value; }
        public bool Focused
        {
            get => focused;
            private set
            {
                focused = value;
                Draw();
            }
        }

        public event MouseEventHandler MousePressed
        {
            add => container.SubscribeMouseEvent(EventType.MousePressed, this, value);
            remove => container.UnsubscribeMouseEvent(EventType.MousePressed, this, value);
        }

        public event MouseEventHandler MouseReleased
        {
            add => container.SubscribeMouseEvent(EventType.MouseReleased, this, value);
            remove => container.UnsubscribeMouseEvent(EventType.MouseReleased, this, value);
        }

        public event MouseEventHandler MouseMoved
        {
            add => container.SubscribeMouseEvent(EventType.MouseMoved, this, value);
            remove => container.UnsubscribeMouseEvent(EventType.MouseMoved, this, value);
        }

        public event MouseEventHandler MouseEnter
        {
            add => container.SubscribeMouseEvent(EventType.MouseEnter, this, value);
            remove => container.UnsubscribeMouseEvent(EventType.MouseEnter, this, value);
        }

        public event MouseEventHandler MouseLeave
        {
            add => container.SubscribeMouseEvent(EventType.MouseLeave, this, value);
            remove => container.UnsubscribeMouseEvent(EventType.MouseLeave, this, value);
        }

        public Control(ControlManager container)
        {
            this.container = container;
            container.Add(this);

            MouseEnter += Control_MouseEnter;
            MouseLeave += Control_MouseLeave;
        }

        private void Control_MouseEnter(object sender, MouseEventArgs e)
        {
            //Focused = true;
        }

        private void Control_MouseLeave(object sender, MouseEventArgs e)
        {
            //Focused = false;
        }

        public bool ContainsPoint(int mx, int my)
        {
            return width > 0 && height > 0 &&
                   mx >= x && mx < x + width &&
                   my >= y && my < y + height;
        }

        public void Draw()
        {
            ConsoleRenderer.FillRect(shades[backgroundShade], x, y, width, height, Focused ? focusAttribute : attribute);
        }
    }
}
