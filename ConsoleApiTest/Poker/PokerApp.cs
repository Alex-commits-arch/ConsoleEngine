using ConsoleLibrary;
using ConsoleLibrary.Forms;
using ConsoleLibrary.Forms.Components;
using ConsoleLibrary.Drawing;
using ConsoleLibrary.Input;
using ConsoleLibrary.Input.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsWrapper.Enums;
using WindowsWrapper.Structs;

namespace ConsoleApiTest.Poker
{
    class PokerApp : FormApp
    {
        Button startButton;
        Button placeholder;
        Border border;
        int sideMargin = 10;

        int width;
        int height;
        int centerX;
        int centerY;

        //COORD 

        public PokerApp(int width = 180, int height = 41) : base(width, height)
        {
            Title = "Console Poker";
        }

        public override void Init()
        {
            base.Init();

            (width, height) = ConsoleRenderer.GetConsoleSize();
            (centerX, centerY) = ConsoleRenderer.GetWindowCenter();

            InitStart();
            ConsoleInput.KeyPressed += OnKeyPressed;
            ConsoleInput.MousePressed += OnMousePressed;
            ConsoleInput.MouseDoubleClick += OnMousePressed;

            //DrawComponents();
            Renderer.DrawMainTitle();
            //Cursor = true;
        }

        #region INITIALIZERS

        private void InitStart()
        {
            startButton = new Button("Single Player");
            startButton.Left = width / 2 - startButton.Width / 2;
            startButton.Top = height / 2 - startButton.Height / 2;
            startButton.Click += OnStartPressed;
            //Components.Add(startButton);

            placeholder = new Button("[Multiplayer]");
            placeholder.Left = width / 2 - placeholder.Width / 2;
            placeholder.Top = height / 2 - placeholder.Height / 2 + 3;
            placeholder.Disable();
            //placeholder.Click += Placeholder_Click;
            //Components.Add(placeholder);

            border = new DoubleBorder();
            border.Left = sideMargin;
            border.Width = width - border.Left * 2;
            border.Height = height;
            border.Attributes = CharAttribute.ForegroundDarkBlue;
            //Components.Add(border);
        }

        private void Placeholder_Click(object sender, MouseEventArgs e)
        {
            Renderer.DrawMainTitle();
        }

        #endregion

        #region DRAW CALLS

        private void DrawStart()
        {
            Renderer.DrawMainTitle();
            //DrawComponents();
        }

        private void DrawMain()
        {
            border.Draw();
            int cardHeight = 9;
            int cardWidth = Renderer.GetCardWidth(cardHeight);
            //int cardWidth = 
            Renderer.DrawCard(
                new Card(Suit.Clubs, Rank.Ace),
                width - sideMargin - cardWidth - 2,
                height - cardHeight - 1,
                cardHeight,
                cardWidth
            );
            Renderer.DrawCard(
                new Card(Suit.Clubs, Rank.Ace),
                width - sideMargin - cardWidth * 2 - 3,
                height - cardHeight - 1,
                cardHeight,
                cardWidth
            );


            Renderer.DrawTable(centerX, centerY, width / 2, height / 2);
            //Renderer.DrawCard(new Card(Suit.Clubs, Rank.Ace), width - 40, height - cardHeight - 1, cardHeight);
        }

        #endregion

        #region EVENT HANDLERS

        private void OnStartPressed(object sender, MouseEventArgs e)
        {
            ConsoleRenderer.Clear();
            startButton.HideAndDisable();
            placeholder.Hide();
            DrawMain();
        }

        private void OnMousePressed(object sender, MouseEventArgs e)
        {
            //var suits = Enum.GetValues(typeof(Suit));
            //var ranks = Enum.GetValues(typeof(Rank));
            //int cx = e.Location.X;
            //int cy = e.Location.Y;
            //Renderer.DrawCard(
            //    new Card(
            //        (Suit)PokerGame.Random.Next(suits.Length),
            //        (Rank)PokerGame.Random.Next(ranks.Length)
            //    ),
            //    cx - 3,
            //    cy - 2,
            //    5
            //);
            //var info = ConsoleRenderer.GetCharInfo(0, 0);
        }

        private void OnKeyPressed(KeyEventArgs args)
        {
            bool ctrlPressed = args.ControlKeyState.HasFlag(ControlKeyState.LeftCtrlPressed);
            if (args.Key == ConsoleKey.R && ctrlPressed)
            {
                ConsoleRenderer.Clear();
                startButton.ShowAndEnable();
                placeholder.Show();
                DrawStart();
            }
            else if (args.Key == ConsoleKey.Q && ctrlPressed)
            {
                //Cursor = !Cursor;
            }
        }
        #endregion
    }
}
