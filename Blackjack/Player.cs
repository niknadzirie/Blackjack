using System;

namespace Playground;

public class Player
{
    public Player(string name)
    {
        Name = name;
    }

    private List<Card> _hand { get; set; } = [];
    public IReadOnlyList<Card> Hand => _hand;
    public string Name { get; }

    public void DrawCard(Deck deck)
    {
        _hand.Add(deck.DrawCard());
    }

    public string ShowHand()
    {
        return String.Join(", ", Hand);
    }

    public int CalculateHand()
    {
        int totalCardValue = 0;
        int totalAces = 0;
        foreach (Card card in _hand)
        {

            switch (card.Rank)
            {
                // edge cases for ace value => 1 or 11
                case Card.Ranks.Ace:
                    totalCardValue += 11;
                    totalAces++;
                    break;

                // edge cases for 10, J, Q, K => all value are 10
                case Card.Ranks.Ten:
                case Card.Ranks.Jack:
                case Card.Ranks.Queen:
                case Card.Ranks.King:
                    totalCardValue += 10;

                    break;

                default:
                    totalCardValue += (int)card.Rank;
                    break;

            }

        }

        while (totalCardValue > 21 && totalAces > 0)
        {
            totalCardValue -= 10; //make ace value = 1
            totalAces--;
        }

        return totalCardValue;
    }

}

public class Dealer : Player
{
    public Dealer(string name) : base(name)
    {

    }

    public string ShowDealerFirstCard()
    {
        return $"{Hand[0]}, Unknown ({DealerFirstCardValue()})";
    }

    public int DealerFirstCardValue()
    {
        switch (Hand[0].Rank)
        {
            // edge cases for ace value => 1 or 11
            case Card.Ranks.Ace:
                    return 11;

            // edge cases for 10, J, Q, K => all value are 10
            case Card.Ranks.Ten:
            case Card.Ranks.Jack:
            case Card.Ranks.Queen:
            case Card.Ranks.King:
                return 10;

            default:
                return (int)Hand[0].Rank;
        }
    }
}
