using ConsoleLibrary.Forms.Components;
using ConsoleLibrary.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsWrapper.Enums;

namespace ConsoleApiTest.Poker
{
    public static class Renderer
    {
        private static readonly double ratio = 0.71428571428;

        private static readonly char[] suits = { '♦', '♣', '♥', '♠' };
        private static readonly string[] ranks = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

        public static int GetCardWidth(int height)
        {
            var fontSize = ConsoleRenderer.GetFontSize();
            var fontRatio = fontSize.Y / fontSize.X;
            return (int)Math.Round(height * ratio * fontRatio);
        }

        public static void DrawCard(Card card, int x, int y, int height, int width = 0)
        {
            if (width <= 0)
                width = GetCardWidth(height);

            ConsoleRenderer.FillRect(' ', x + 1, y + 1, width - 2, height - 2, CharAttribute.BackgroundBlack);

            Border border = new RoundedBorder();
            border.Width = width;
            border.Height = height;
            border.Left = x;
            border.Top = y;
            border.Attributes = CharAttribute.ForegroundWhite;
            border.Draw();

            var suit = suits[(int)card.Suit];
            var rank = ranks[(int)card.Rank];

            bool isRed = card.Suit == Suit.Diamonds || card.Suit == Suit.Hearts;

            ConsoleRenderer.DrawChar(
                suit,
                x + width / 2,
                y + height / 2,
                isRed ?
                CharAttribute.ForegroundRed :
                CharAttribute.ForegroundWhite
            );

            ConsoleRenderer.DrawString(rank, x + 1, y + 1, CharAttribute.ForegroundWhite);
            ConsoleRenderer.DrawString(rank, x + width - 1 - rank.Length, y + height - 2, CharAttribute.ForegroundWhite);
        }

        public static void DrawTable(int x, int y, int w, int h)
        {
            var border = new RoundedBorder();
            border.Width = w;
            border.Height = h;
            border.Left = x;
            border.Top = y;
            border.Attributes = CharAttribute.ForegroundWhite;
            border.Draw();
            var innerBorder = new DashedBorder();
            innerBorder.Width = w - 2;
            innerBorder.Height = h - 2;
            innerBorder.Left = x + 1;
            innerBorder.Top = y + 1;
            innerBorder.Attributes = CharAttribute.ForegroundWhite;
            innerBorder.Draw();
        }

        public static void DrawMainTitle()
        {
            //var title = new string[]
            //{
            //    "  /$$$$$$                                          /$$                 /$$$$$$$           /$$                          ",
            //    " /$$__ÖÖ$$                                        | $$                | $$__  $$         | $$                          ",
            //    "|Ö$$  \\__/  /$$$$$$  /$$$$$$$   /$$$$$$$  /$$$$$$ | $$  /$$$$$$       | $$  \\ $$ /$$$$$$ | $$   /$$  /$$$$$$   /$$$$$$ ",
            //    "|Ö$$       /$$ÖÖÖÖ$$|Ö$$__ÖÖ$$ /$$_____/ /$$__  $$| $$ /$$__  $$      | $$$$$$$//$$__  $$| $$  /$$/ /$$__  $$ /$$__  $$",
            //    "|Ö$$      |Ö$$  \\Ö$$|Ö$$  \\Ö$$|ÖÖ$$$$$$ | $$  \\ $$| $$| $$$$$$$$      | $$____/| $$  \\ $$| $$$$$$/ | $$$$$$$$| $$  \\__/",
            //    "|Ö$$    $$|Ö$$  |Ö$$|Ö$$  |Ö$$ \\____ÖÖ$$| $$  | $$| $$| $$_____/      | $$     | $$  | $$| $$_  $$ | $$_____/| $$      ",
            //    "|ÖÖ$$$$$$/|ÖÖ$$$$$$/|Ö$$  |Ö$$ /$$$$$$$/|  $$$$$$/| $$|  $$$$$$$      | $$     |  $$$$$$/| $$ \\  $$|  $$$$$$$| $$      ",
            //    " \\______/  \\______/ |__/  |__/|_______/  \\______/ |__/ \\_______/      |__/      \\______/ |__/  \\__/ \\_______/|__/      ",
            //};
            var title = new string[]
            {
                "  /$$$$$$                                          /$$                 /$$$$$$$           /$$                          ",
                " /$$__ÖÖ$$                                        |Ö$$                |Ö$$__ÖÖ$$         |Ö$$                          ",
                "|Ö$$  \\__/  /$$$$$$  /$$$$$$$   /$$$$$$$  /$$$$$$ |Ö$$  /$$$$$$       |Ö$$  \\Ö$$ /$$$$$$ |Ö$$   /$$  /$$$$$$   /$$$$$$ ",
                "|Ö$$       /$$ÖÖÖÖ$$|Ö$$__ÖÖ$$ /$$_____/ /$$__ÖÖ$$|Ö$$ /$$__ÖÖ$$      |Ö$$$$$$$//$$__ÖÖ$$|Ö$$  /$$/ /$$__ÖÖ$$ /$$__ÖÖ$$",
                "|Ö$$      |Ö$$  \\Ö$$|Ö$$  \\Ö$$|ÖÖ$$$$$$ |Ö$$  \\Ö$$|Ö$$|Ö$$$$$$$$      |Ö$$____/|Ö$$  \\Ö$$|Ö$$$$$$/ |Ö$$$$$$$$|Ö$$  \\__/",
                "|Ö$$    $$|Ö$$  |Ö$$|Ö$$  |Ö$$ \\____ÖÖ$$|Ö$$  |Ö$$|Ö$$|Ö$$_____/      |Ö$$     |Ö$$  |Ö$$|Ö$$_ÖÖ$$ |Ö$$_____/|Ö$$      ",
                "|ÖÖ$$$$$$/|ÖÖ$$$$$$/|Ö$$  |Ö$$ /$$$$$$$/|ÖÖ$$$$$$/|Ö$$|ÖÖ$$$$$$$      |Ö$$     |ÖÖ$$$$$$/|Ö$$ \\ÖÖ$$|Ö $$$$$$$|Ö$$      ",
                " \\______/  \\______/ |__/  |__/|_______/  \\______/ |__/ \\_______/      |__/      \\______/ |__/  \\__/ \\_______/|__/      ",
            };

            var (windowWidth, windowHeight) = ConsoleRenderer.GetWindowSize();
            //var sw = Stopwatch.StartNew();
            ConsoleRenderer.Draw(title, new DrawArgs(
                windowWidth / 2 - title[0].Length / 2,
                windowHeight / 4 - title.Length / 2,
                CharAttribute.ForegroundGreen,
                true
            ));
            //sw.Stop();
            //Debug.WriteLine(sw.ElapsedMilliseconds);
            //ConsoleRenderer.DrawStrings(
            //    title,
            //    windowWidth / 2 - title[0].Length / 2,
            //    windowHeight / 4 - title.Length / 2,
            //    CharAttribute.ForegroundGreen
            //);
        }
    }
}
