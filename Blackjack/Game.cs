using System;
using System.Runtime.CompilerServices;

namespace Playground;

public class Game
{
    private enum GameState
    {
        DrawingCard,
        WaitingPlayerDecision,
        PlayerHit,
        PlayerStand,
        DealerHit,
        DetermineWinner,
        Done
    }
    private Deck _deck { get; set; }
    private Player _player { get; set; }
    private Dealer _dealer { get; set; }
    private GameState _gameState { get; set; }
    public Game(string playerName)
    {
        _deck = new Deck();
        _player = new Player(playerName);
        _dealer = new Dealer("Dealer");
        _gameState = GameState.DrawingCard;

    }

    public void StartGame()
    {
        Console.Clear();
        while (_gameState != GameState.Done)
        {
            switch (_gameState)
            {
                case GameState.DrawingCard:
                    _player.DrawCard(_deck);
                    _dealer.DrawCard(_deck);
                    _player.DrawCard(_deck);
                    _dealer.DrawCard(_deck);

                    Console.WriteLine($"Dealer Hand: {_dealer.ShowDealerFirstCard()}");
                    Console.WriteLine($"Player Hand: {_player.ShowHand()} ({_player.CalculateHand()})");

                    if (_dealer.Hand[0].Rank == Card.Ranks.Ten || _dealer.Hand[0].Rank == Card.Ranks.Ace)
                    {
                        if (_dealer.CalculateHand() == 21)
                        {
                            Console.Clear();
                            Console.WriteLine($"Dealer Hand: {_dealer.ShowHand()} ({_dealer.CalculateHand()})");
                            Console.WriteLine($"Player Hand: {_player.ShowHand()} ({_player.CalculateHand()})");
                            Console.WriteLine("Dealer Win");
                            _gameState = GameState.Done;
                            return;
                        }
                    }

                    _gameState = GameState.WaitingPlayerDecision;

                    break;

                case GameState.WaitingPlayerDecision:
                    Console.Write("\nAction: Hit or Stand? (h/s): ");
                    string? playerDecision = Console.ReadLine();
                    ProcessPlayerDecision(playerDecision);
                    break;

                case GameState.PlayerHit:
                    Console.Clear();
                    Console.WriteLine($"Dealer Hand: {_dealer.ShowDealerFirstCard()}");
                    Console.WriteLine($"Player Hand: {_player.ShowHand()} ({_player.CalculateHand()})");
                    if (_player.CalculateHand() > 21)
                    {
                        Console.WriteLine("Player Busted, Dealer Win");
                        _gameState = GameState.Done;
                    }
                    else
                    {
                        _gameState = GameState.WaitingPlayerDecision;
                    }
                    break;

                case GameState.PlayerStand:
                    Console.Clear();
                    Console.WriteLine($"Dealer Hand: {_dealer.ShowHand()} ({_dealer.CalculateHand()})");
                    Console.WriteLine($"Player Hand: {_player.ShowHand()} ({_player.CalculateHand()})");
                    if (_dealer.CalculateHand() < 17)
                    {
                        _gameState = GameState.DealerHit;
                    }
                    else
                    {
                        _gameState = GameState.DetermineWinner;
                    }
                    break;

                case GameState.DealerHit:
                    while (_dealer.CalculateHand() < 17)
                    {
                        _dealer.DrawCard(_deck);
                    }
                    Console.Clear();
                    Console.WriteLine($"Dealer Hand: {_dealer.ShowHand()} ({_dealer.CalculateHand()})");
                    Console.WriteLine($"Player Hand: {_player.ShowHand()} ({_player.CalculateHand()})");
                    _gameState = GameState.DetermineWinner;
                    break;

                case GameState.DetermineWinner:
                    DetermineWinner();
                    _gameState = GameState.Done;
                    break;
                default:
                    break;
            }
        }
    }

    private void DetermineWinner()
    {
        if (_dealer.CalculateHand() > _player.CalculateHand() && _dealer.CalculateHand() <= 21)
        {
            Console.WriteLine("Dealer Win");
        }
        else if (_dealer.CalculateHand() == _player.CalculateHand())
        {
            Console.WriteLine("Draw");
        }
        else
        {
            Console.WriteLine("Player Win");
        }
    }

    private void ProcessPlayerDecision(string playerDecision)
    {
        switch (playerDecision.ToLower())
        {
            case "hit":
            case "h":
                _player.DrawCard(_deck);
                _gameState = GameState.PlayerHit;
                break;

            case "stand":
            case "s":
                _gameState = GameState.PlayerStand;
                break;

            default:
                Console.WriteLine("Unknown Action, try again");
                break;
        }
    }
}
