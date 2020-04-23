using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApiTest.Poker
{
    class PokerGame
    {
        public static Random Random = new Random();

        public PokerGame()
        {

        }

        public void Start(int opponents = 1)
        {
            if (opponents < 1)
                opponents = 1;
            else if (opponents > 7)
                opponents = 7;
        }
    }

    class Player
    {

    }
}
