﻿using ConsoleLibrary.Input.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLibrary.Forms.Controls
{
    public class ScrollableTextBox : Control
    {
        VerticalScrollbar scrollbar;
        TextBox textBox;

        public ScrollableTextBox(Control parent) : base(parent)
        {
            Width = 20;
            Height = 10;
            Top = 1;
            Name = "Scrollable";

            textBox = new TextBox(this);

            textBox.Width = Width - 2;
            textBox.Height = Height;
            textBox.Text = "Hello";
            textBox.Attributes = WindowsWrapper.Enums.CharAttribute.BackgroundDarkBlue | WindowsWrapper.Enums.CharAttribute.ForegroundWhite;

            scrollbar = new VerticalScrollbar(this);
            scrollbar.Height = Height;
            scrollbar.Left = Width - 2;

            scrollbar.MouseEnter += (object obj, MouseEventArgs args) => {
                Debug.WriteLine("Scrollbar entered");
            };
        }

        //protected internal override void HandleResized(int width, int height)
        //{
        //    Width = width / 2;
        //    Left = width / 4;
        //    //Height = 20;
        //    //textBox.BeginUpdate
        //    textBox.Height = Height;
        //    textBox.Width = Width - 1;
        //    scrollbar.Height = Height;
        //    scrollbar.Left = Width - scrollbar.Width;
        //}

        protected override void RefreshBuffer()
        {
            base.RefreshBuffer();


            textBox.Draw(buffer);
            scrollbar.Draw(buffer);
        }
    }
}
