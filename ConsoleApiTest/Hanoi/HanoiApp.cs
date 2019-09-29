using ConsoleLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleLibrary.Graphics.Shapes;
using static ConsoleLibrary.InputManager;
using ConsoleLibrary.Graphics.Drawing;
using WindowsWrapper.Constants;

namespace ConsoleApiTest
{
    class HanoiApp : ConsoleApp
    {
        public HanoiApp(int width, int height) : base(width, height)
        {
            context.HideCursor();
        }

        ushort[] blockColors =
        {
            (ushort)(Colors.BACKGROUND_RED | Colors.BACKGROUND_INTENSITY | Colors.FOREGROUND_MAGENTA | Colors.FOREGROUND_INTENSITY),
            (ushort)(Colors.BACKGROUND_BLUE | Colors.BACKGROUND_INTENSITY | Colors.FOREGROUND_GREEN | Colors.FOREGROUND_INTENSITY),
            (ushort)(Colors.BACKGROUND_YELLOW | Colors.BACKGROUND_INTENSITY | Colors.FOREGROUND_RED | Colors.FOREGROUND_INTENSITY),
        };

        int blockHeight = 2;
        int pegWidth = 1;
        int pegHeight = 16;
        int pegIndex = 0;
        int numPegs = 3;
        int numBlocks = 4;
        int minWidth = 6;
        int maxWidth = 20;

        Rect heldBlock;
        Peg[] pegs;
        //Dictionary<Rect, >

        bool exit = false;

        int x = 0;
        int y = 0;

        public override void Init()
        {
            base.Init();
            RegisterInputs();

            pegs = new Peg[numPegs];
            InitPegs();
            //pegs.Initialize();

            var background = context.CreateBuffer("background");
            var pegsBuffer = context.CreateBuffer("sticks");
            var blocks = context.CreateBuffer("blocks");
            var backRect = new Rect(context.width, context.height);
            var sprites = context.CreateBuffer("sprites");

            var player = new Rect(1, 1);
            player.Fill('V');

            var sprite = new Rect(20, 10);
            sprite.Fill('░');

            var pegShape = new Rect(pegWidth, pegHeight);
            pegShape.Fill('░');

            DrawPegs(pegsBuffer, pegShape);

            pegsBuffer.Draw(pegShape, Colors.FOREGROUND_WHITE, 0, 10);

            while (!exit)
            {

                pegsBuffer.Clear();
                DrawPegs(pegsBuffer, pegShape);

                blocks.Clear();
                DrawBlocks(blocks);

                sprites.Clear();
                sprites.Draw(player, Colors.FOREGROUND_WHITE | Colors.FOREGROUND_INTENSITY, GetPos(pegIndex+1), 2);
                if (heldBlock.GetData() != null)
                {
                    sprites.Draw(
                        heldBlock,
                        Convert.ToInt32(blockMeta[heldBlock].Where(m => m.Key == "attributes").FirstOrDefault().Value),
                        GetPos(pegIndex + 1) - heldBlock.width / 2,
                        3
                    );
                }


                context.RenderFrame();
                //Thread.Sleep(1);
            }
        }

        private void InitPegs()
        {
            for (int i = 0; i < numPegs; i++)
            {
                pegs[i] = new Peg(numBlocks);
            }

            InitBlocks();
        }

        Dictionary<Rect, List<KeyValuePair<string, object>>> blockMeta;
        Random rnd = new Random();
        private void InitBlocks()
        {
            blockMeta = new Dictionary<Rect, List<KeyValuePair<string, object>>>();
            for (int i = 0; i < numBlocks; i++)
            {
                Rect block = new Rect(
                    Lerp(maxWidth, minWidth, (float)i / (numBlocks - 1)),
                    blockHeight
                );
                block.Fill('▓');
                //block.Fill(' ');

                blockMeta[block] = new List<KeyValuePair<string, object>>();
                //blockMeta[block].Add(new KeyValuePair<string, object>("attributes", Colors.BACKGROUND_GREEN | Colors.FOREGROUND_RED | Colors.FOREGROUND_INTENSITY));
                //blockMeta[block].Add(new KeyValuePair<string, object>("attributes", rnd.Next(255) + 1));
                blockMeta[block].Add(new KeyValuePair<string, object>("attributes", blockColors[i % blockColors.Length]));
                blockMeta[block].Add(new KeyValuePair<string, object>("index", 0));
                //blockMeta[block].Where(m => m.Key == "attributes").FirstOrDefault().Value

                pegs[0].Place(block);
            }
        }

        int spacing = 0;
        private void DrawPegs(ScreenBuffer pegsBuffer, Rect pegShape)
        {
            int n = 3;
            int off = 6;
            for (int i = 1; i <= numPegs; i++)
            {
                //if(i % 2 != 0)
                pegsBuffer.Draw(
                    pegShape,
                    Colors.FOREGROUND_CYAN,
                    GetPos(i) - pegWidth / 2,
                    height - pegShape.height
                );
            }
        }

        private int GetPos(int i)
        {
            return (width / (numPegs + 1)) * i + spacing * (i - 2);
        }

        private void DrawBlocks(ScreenBuffer blocksBuffer)
        {
            for (int i = 1; i <= numPegs; i++)
            {
                Rect[] blocks = pegs[i-1].GetBlocks().ToArray();
                for (int j = blocks.Length - 1; j >= 0; j--)
                {
                    var test = (width / (numPegs + 1)) * i + spacing * (i - 2);
                    Rect block = blocks[j];
                    blocksBuffer.Draw(
                        block,
                        Convert.ToInt32(blockMeta[block].Where(m => m.Key == "attributes").FirstOrDefault().Value),
                        GetPos(i) - block.width / 2 + pegWidth / 2,
                        height - blockHeight * (blocks.Length - j));
                    //blocksBuffer.Draw(
                    //    block,
                    //    (int)blockMeta[block].Where(m => m.Key == "attributes").FirstOrDefault().Value,
                    //    20,
                    //    height - blockHeight * (blocks.Length - j));
                    //blocksBuffer.Draw(
                    //    block,
                    //    (ushort)blockMeta[block].Where(m => m.Key == "attributes").FirstOrDefault().Value,
                    //    (width / (numPegs + 1)) * i + spacing * (i - 2) + 20,
                    //    0);
                }
            }
        }

        private void RegisterInputs()
        {
            //inputManager.Register(KeyCode.Space, new KeyHandler(KeyState.Pre, deltaTime => MoveLeft(deltaTime)));
            inputManager.Register(KeyCode.Left, new KeyHandler(KeyState.Pressed, deltaTime => MoveLeft(deltaTime)));
            inputManager.Register(KeyCode.Right, new KeyHandler(KeyState.Pressed, deltaTime => MoveRight(deltaTime)));
            inputManager.Register(KeyCode.Up, new KeyHandler(KeyState.Pressed, deltaTime => Take()));
            inputManager.Register(KeyCode.Down, new KeyHandler(KeyState.Pressed, deltaTime => Place()));
            inputManager.Register(KeyCode.Escape, new KeyHandler(KeyState.Pressed, deltaTime => base.Exit()));
            inputManager.Register(KeyCode.PageUp, new KeyHandler(KeyState.Held, deltaTime => IncreaseSpacing(deltaTime)));
            inputManager.Register(KeyCode.PageDown, new KeyHandler(KeyState.Held, deltaTime => DecreaseSpacing(deltaTime)));
        }

        private void Take()
        {
            if(heldBlock.GetData() == null && pegs[pegIndex].CanTake())
                heldBlock = pegs[pegIndex].Take();
        }

        private void Place()
        {

            var test = heldBlock;
            if (heldBlock.GetData() != null && pegs[pegIndex].CanPlace(heldBlock))
            {
                pegs[pegIndex].Place(heldBlock);
                heldBlock.SetData(null);
            }
        }

        private void MoveLeft(double deltaTime)
        {
            pegIndex = Math.Max(0, pegIndex - 1);
            //pegIndex--;
            //if (pegIndex < 0)
            //    pegIndex = numPegs - 1;
            //x -= (int)Math.Round(100 * deltaTime);
        }

        private void MoveRight(double deltaTime)
        {
            pegIndex = Math.Min(numPegs - 1, pegIndex + 1);
            //pegIndex = (pegIndex + 1) % numPegs;
        }

        private void MoveUp(double deltaTime)
        {
            y -= (int)Math.Round(50 * deltaTime);
        }

        private void MoveDown(double deltaTime)
        {
            y += (int)Math.Round(50 * deltaTime);
        }

        private void IncreaseSpacing(double deltaTime)
        {
            spacing += (int)Math.Round(40 * deltaTime);
        }

        private void DecreaseSpacing(double deltaTime)
        {
            spacing -= (int)Math.Round(40 * deltaTime);
        }

        private float Lerp(float from, float to, float by)
        {
            return from * (1 - by) + to * by;
        }

        private int Lerp(int from, int to, float by)
        {
            return (int)Math.Round(from * (1 - by) + to * by);
        }

        //private void DrawBoard()
        //{
        //    int wCenter = drawingContext.width / 2;
        //    int hCenter = drawingContext.height / 2;

        //    int tileWidth = 6;
        //    int tileHeight = 3;

        //    int boardHeight = tileHeight * 8;
        //    int boardWidth = tileWidth * 8;

        //    int xOffset = wCenter - boardWidth / 2;
        //    int yOffset = hCenter - boardHeight / 2;

        //    drawingContext.FillRect(
        //        '#',
        //        xOffset - 1,
        //        yOffset - 1,
        //        boardWidth + 2,
        //        boardHeight + 2);

        //    for (int x = 0; x < 8; x++)
        //    {
        //        for (int y = 0; y < 8; y++)
        //        {
        //            bool checker = (x + y) % 2 == 0;

        //            drawingContext.FillRect(
        //                checker ? '░' : ' ',
        //                x * tileWidth + xOffset,
        //                y * tileHeight + yOffset,
        //                tileWidth,
        //                tileHeight);
        //        }
        //    }
        //}
    }
}
