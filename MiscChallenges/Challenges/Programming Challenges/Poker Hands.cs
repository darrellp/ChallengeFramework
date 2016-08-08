using System;
using System.Collections.Generic;
using System.Linq;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("ProgChallenges", "Poker Hands",
			"http://www.programming-challenges.com/pg.php?page=downloadproblem&probid=110202&format=html")]
		public class PokerHands : IChallenge
		{
			public void Solve()
			{
				PokerDeal deal;
				while ((deal = NextDeal()) != null)
				{
					Console.WriteLine(FindWinner(deal));
				}
			}

			private object FindWinner(PokerDeal deal)
			{
				var ret = string.Empty;

				switch (deal.BlackWins())
				{
					case -1:
						ret = "White wins.";
						break;
					case 0:
						ret = "Tie.";
						break;
					case 1:
						ret = "Black wins.";
						break;
				}
				return ret;
			}

			private PokerDeal NextDeal()
			{
				var input = Console.ReadLine();
				if (input == null)
				{
					return null;
				}
				var cards = input.Split(' ').Select(s => new Card(s)).ToList();
				var blackHand = cards.Take(5).ToList();
				var whiteHand = cards.Skip(5).ToList();
				return new PokerDeal(blackHand, whiteHand);
			}

			internal class PokerDeal
			{
				internal List<Card> BlackHand { get; private set; }
				internal List<Card> WhiteHand { get; private set; }

				internal PokerDeal(List<Card> blackHand, List<Card> whiteHand)
				{
					BlackHand = blackHand;
					WhiteHand = whiteHand;
					BlackHand.Sort();
					WhiteHand.Sort();
				}

				internal int BlackWins()
				{
					var bRank = RankHand(fBlack: true);
					var wRank = RankHand(fBlack: false);
					return bRank.CompareTo(wRank);
				}

				/// <summary>
				/// Get the ranking for a hand
				/// </summary>
				/// <param name="fBlack">If true, rank the black hand - else rank white hand</param>
				/// <returns>ranking</returns>
				private int RankHand(bool fBlack)
				{
					// Run through all the rankers and return the first non-zero value
					return Enumerable.
						Range(0, Rankers.Count).
						Select(cat => Rank(cat, fBlack)).
						FirstOrDefault(rank => rank != 0);
				}

				private int Rank(int iRanker, bool fBlack)
				{
					var catRank = Rankers.Count - 1 - iRanker;
					var intraCategoryRank = Rankers[iRanker](fBlack ? BlackHand : WhiteHand);
					var interCategoryRank = intraCategoryRank == 0 ? 0 : catRank * BigRank;
					return interCategoryRank + intraCategoryRank;
				}

				// Generally the ranking works by each category of poker hands coming up with
				// a value between 0 and BigRank to distinguish hands within the category and
				// then that value is added to a suitable multiple of BigRank to distinguish
				// values between categories.
				private const int BigRank = 13 * 13 * 13 * 13 * 13;

				// NOTE: These rankers depend on being run in the order of the
				// _rankers list - i.e., each depends on the hand not falling
				// into one of the previous cases on the list - they don't stand
				// on their own.  For instance, the TwoPairRanker would recognize
				// a hand with three of a kind as a "two pair".
				// Also, they assume the cards in the hand have been sorted
				// in ascending order.
				// Finally, each ranker is intended only as an effective ranker within
				// it's own category - i.e., it's ranking is only correct against
				// another card in the same category.  Inter-category ranking is handled
				// by the Rank() function.  It should return 0 only if the hand does not
				// fall into the respective category.

				private static readonly List<Func<List<Card>, int>> Rankers =
					new List<Func<List<Card>, int>>
				{
					StraightFlushRanker,
					FourOfAKindRanker,
					FullHouseRanker,
					FlushRanker,
					StraightRanker,
					ThreeOfAKindRanker,
					TwoPairRanker,
					PairRanker,
					HighCardRanker
				};

				private static int HighCardRanker(List<Card> hand)
				{
					// Essentially returns the sorted hand as a base 13 number.
					var acc = 0;
					var powerOf13 = 1;

					for (var iCard = 0; iCard < 5; powerOf13 *= 13, iCard++)
					{
						acc += hand[iCard].Rank * powerOf13;
					}
					return acc;
				}

				private static int PairRanker(List<Card> hand)
				{
					var iPair = -1;

					// Find the pair
					for (var iCard = 1; iCard < 5; iCard++)
					{
						if (hand[iCard].Rank == hand[iCard - 1].Rank)
						{
							iPair = iCard;
							break;
						}
					}

					if (iPair < 0)
					{
						return 0;
					}

					var acc = 0;
					var powerOf13 = 1;

					// Use the three kickers as a base 13 number, excluding the pair
					for (var iCard = 0; iCard < 5; iCard++)
					{
						if (iCard == iPair - 1 || iCard == iPair)
						{
							continue;
						}
						acc += hand[iCard].Rank * powerOf13;
						powerOf13 *= 13;
					}
					// Make the pair the high digit of our 4 digit base 13 number
					return acc + 13 * 13 * 13 * hand[iPair].Rank;
				}

				private static int TwoPairRanker(List<Card> hand)
				{
					var pairCount = 0;
					var pairIndices = new int[2];

					for (var iCard = 1; iCard < 5; iCard++)
					{
						if (hand[iCard].Rank == hand[iCard - 1].Rank)
						{
							pairIndices[pairCount++] = iCard;
						}
					}
					if (pairCount != 2)
					{
						return 0;
					}

					// Determine where the odd man out index is
					var extraIndex = 0;
					if (pairIndices[0] == 0)
					{
						extraIndex = pairIndices[1] == 2 ? 4 : 2;
					}

					// High pair is the top digit, low pair is the next and kicker is low digit
					// of our returned base 13 number.
					return 13 * 13 * hand[pairIndices[1]].Rank +
						   13 * hand[pairIndices[0]].Rank +
						   hand[extraIndex].Rank;
				}

				private static int ThreeOfAKindRanker(List<Card> hand)
				{
					// Middle sorted card must belong to the three of a kind
					if (hand.Count(c => c.Rank == hand[2].Rank) == 3)
					{
						return hand[2].Rank;
					}
					return 0;
				}

				private static int StraightRanker(List<Card> hand)
				{
					if (hand.Skip(1).Select((c, i) => c.Rank - i - 1).All(d => d == hand[0].Rank))
					{
						return hand[4].Rank;
					}
					return 0;
				}

				private static int FlushRanker(List<Card> hand)
				{
					if (hand.Skip(1).Select(c => c.Suit).All(s => s == hand[0].Suit))
					{
						return hand[4].Rank;
					}
					return 0;
				}

				private static int FullHouseRanker(List<Card> hand)
				{
					if (hand[0].Rank != hand[1].Rank || hand[3].Rank != hand[4].Rank)
					{
						return 0;
					}
					if (hand[2].Rank == hand[0].Rank || hand[2].Rank == hand[4].Rank)
					{
						return hand[2].Rank;
					}
					return 0;
				}

				private static int FourOfAKindRanker(List<Card> hand)
				{
					var start = hand[0] == hand[1] ? 1 : 2;
					for (var i = start; i < start + 3; i++)
					{
						if (hand[i].Rank != hand[i - 1].Rank)
						{
							return 0;
						}
					}
					return hand[2].Rank;
				}

				private static int StraightFlushRanker(List<Card> hand)
				{
					for (var i = 1; i < hand.Count(); i++)
					{
						if (hand[i].Suit != hand[i - 1].Suit || hand[i].Rank != hand[i - 1].Rank + 1)
						{
							return 0;
						}
					}
					return hand[4].Rank;
				}
			}

		    internal class Card : IComparable
		    {
			    internal char Suit { get; private set; }
			    internal byte Rank { get; private set; }

			    internal Card(char suit, byte rank)
			    {
				    Suit = suit;
				    Rank = rank;
			    }

			    private static readonly Dictionary<char, byte> CharToRank = new Dictionary<char, byte>
			    {
				    {'2', 1},
				    {'3', 2},
				    {'4', 3},
				    {'5', 4},
				    {'6', 5},
				    {'7', 6},
				    {'8', 7},
				    {'9', 8},
				    {'T', 9},
				    {'J', 10},
				    {'Q', 11},
				    {'K', 12},
				    {'A', 13},
			    };

			    internal Card(string input)
			    {
				    Suit = input[1];
				    Rank = CharToRank[input[0]];
			    }

			    public int CompareTo(object obj)
			    {
				    var card = obj as Card;
				    if (card == null)
				    {
					    return 0;
				    }
				    var cmpRank = Rank.CompareTo(card.Rank);
				    if (cmpRank == 0)
				    {
					    return Suit.CompareTo(card.Suit);
				    }
				    return cmpRank;
			    }

			    private static readonly Dictionary<char, string> SuitToString = new Dictionary<char, string>
			    {
				    {'C', "Clubs"},
				    {'D', "Diamonds"},
				    {'H', "Hearts"},
				    {'S', "Spades"}
			    };

			    private static readonly string[] Ranks =
			    {
				    "2",
				    "3",
				    "4",
				    "5",
				    "6",
				    "7",
				    "8",
				    "9",
				    "10",
				    "Jack",
				    "Queen",
				    "King",
				    "Ace"
			    };

			    public override string ToString()
			    {
				    return Ranks[Rank - 1] + " of " + SuitToString[Suit];
			    }
		    }

			public string RetrieveSampleInput()
			{
				return @"
2H 3D 5S 9C KD 2C 3H 4S 8C AH
2H 4S 4C 2D 4H 2S 8S AS QS 3S
2H 3D 5S 9C KD 2C 3H 4S 8C KH
2H 3D 5S 9C KD 2D 3H 5C 9S KH
2H 2C 4D 5D 6D 2D 3H 3C 5H 6H
2H 2C 4D 5D 6D 2D 2H 3C 5H 6H";
			}

			public string RetrieveSampleOutput()
			{
				return @"
White wins.
Black wins.
Black wins.
Tie.
White wins.
Black wins.
";
			}
		}
	}
}
