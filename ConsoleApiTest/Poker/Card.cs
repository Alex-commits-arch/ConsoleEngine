using System;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApiTest.Poker
{
    public struct Card
    {
        public Suit Suit { get; set; }
        public Rank Rank { get; set; }

        public Card(Suit suit, Rank rank) : this()
        {
            Suit = suit;
            Rank = rank;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", Suit, Rank);
        }
    }

    public enum Suit
    {
        Diamonds,
        Clubs,
        Hearts,
        Spades
    }

    public enum Rank
    {
        Ace,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King
    }
}
