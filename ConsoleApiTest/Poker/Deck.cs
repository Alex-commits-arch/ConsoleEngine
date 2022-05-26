using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ConsoleApiTest.Poker
{
    public class Deck
    {
        Stack<Card> cards;
        int suitCount = 4;
        int rankCount = 13;

        public Deck(int count = 1)
        {
            cards = new Stack<Card>();
            Reset(count);
        }

        public Card Draw()
        {
            return cards.Pop();
        }
        
        public void Reset(int count = 1)
        {
            cards.Clear();
            foreach (var card in Generate(count).OrderBy(c => PokerGame.Random.Next(suitCount * rankCount * count)))
                cards.Push(card);
            Debug.WriteLine(cards.Count);
        }

        private IEnumerable<Card> Generate(int count = 1)
        {
            for (int c = 0; c < count; c++)
                for (int s = 0; s < suitCount; s++)
                    for (int r = 0; r < rankCount; r++)
                        yield return new Card((Suit)s, (Rank)r);
        }
    }
}
