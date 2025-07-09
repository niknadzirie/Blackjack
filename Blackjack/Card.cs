using System;
using System.Net.NetworkInformation;

namespace Playground;

public class Card
{
    public enum Ranks
    {
        Ace = 1,
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
    public enum Suites
    {
        Spade,
        Heart,
        Club,
        Diamond
    }

    public Ranks Rank { get; set; }
    public Suites Suite { get; set; }

    public Card(Ranks rank, Suites suite)
    {
        Rank = rank;
        Suite = suite;
    }

    public override string ToString()
    {
        return $"{Rank} of {Suite}";
    }
}

public class Deck
{
    private static Random _random = new Random();
    private List<Card> _cards { get; set; }
    public IReadOnlyList<Card> Cards => _cards;
    public Deck()
    {
        _cards = GenerateDeck();
        ShuffleDeck();
    }

    private List<Card> GenerateDeck()
    {
        var deck = new List<Card>();
        foreach (Card.Ranks rank in Enum.GetValues(typeof(Card.Ranks)))
        {
            foreach (Card.Suites suite in Enum.GetValues(typeof(Card.Suites)))
            {
                deck.Add(new Card(rank, suite));
            }
        }

        return deck;
    }

    public Card DrawCard()
    {
        Card card = _cards[0];
        _cards.RemoveAt(0);
        return card;
    }

    private void ShuffleDeck()
    {
        int n = _cards.Count;
        while (n > 1)
        {
            n--;
            int k = _random.Next(n + 1);
            var value = _cards[k];
            _cards[k] = _cards[n];
            _cards[n] = value;
        }
    }
}
