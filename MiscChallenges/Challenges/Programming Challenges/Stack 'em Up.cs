//// ReSharper disable AssignNullToNotNullAttribute
//// ReSharper disable PossibleNullReferenceException
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.IO;

//namespace Programming_Challenges.Challenges
//{
//	public static partial class ChallengeClass
//	{
//		[Challenge("2.8.5", "Stack 'em Up")]
//		public class StackEmUp : IChallenge
//		{
//			public string Solve(StringReader stm)
//			{
//				var ret = new StringBuilder();
//				var cCases = int.Parse(stm.ReadLine());
//				var fFirstCase = true;
//				stm.ReadLine();
//				for (var i = 0; i < cCases; i++)
//				{
//					if (!fFirstCase)
//					{
//						ret.Append(Environment.NewLine);
//					}
//					fFirstCase = false;
//					var shuffler = new Shuffler(stm);
//					ret.Append(shuffler.Solve());
//				}
//				return ret.ToString();
//			}


//			public class Shuffler
//			{
//				private readonly List<List<int>> _shuffles = new List<List<int>>();
//				private readonly List<int> _shuffleIndices = new List<int>();
//				private List<Card> _deck = new List<Card>(); 

//				public Shuffler(StringReader stm)
//				{
//					var cShuffles = int.Parse(stm.ReadLine());
//					var allElements = new List<int>();
//					while (allElements.Count() != cShuffles * 52)
//					{
//						var elements = stm.ReadLine().Split(new[] {' '}).Select(int.Parse).ToList();
//						allElements.AddRange(elements);
//					}

//					for (var iShuffle = 0; iShuffle < cShuffles; iShuffle++)
//					{
//						_shuffles.Add(allElements.Take(52).ToList());
//						allElements = allElements.Skip(52).ToList();
//					}

//					string shuffleIndexString;

//					while ((shuffleIndexString = stm.ReadLine()) != string.Empty && shuffleIndexString != null)
//					{
//						_shuffleIndices.Add(int.Parse(shuffleIndexString) - 1);
//					}

//					var suits = new List<char> {'C', 'D', 'H', 'S'};
//					foreach (var iSuit in suits)
//					{
//						for (byte iRank = 1; iRank <= 13; iRank++)
//						{
//							_deck.Add(new Card(iSuit, iRank));
//						}
//					}
//				}

//				internal string Solve()
//				{
//					foreach (var iShuffle in _shuffleIndices)
//					{
//						Shuffle(_shuffles[iShuffle]);
//					}
//					var ret = new StringBuilder();
//					foreach (var card in _deck)
//					{
//						ret.Append(card + Environment.NewLine);
//					}
//					return ret.ToString();
//				}

//				private void Shuffle(List<int> shuffle)
//				{
//					var tempDeck = new List<Card>();
//					for (var i = 0; i < 52; i++)
//					{
//						tempDeck.Add(_deck[shuffle[i] - 1]);
//					}
//					_deck = tempDeck;
//				}
//			}

//			public string RetrieveSampleInput()
//			{
//				return @"
//1

//2
//2 1 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26
//27 28 29 30 31 32 33 34 35 36 37 38 39 40 41 42 43 44 45 46 47 48 49 50 52 51
//52 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26
//27 28 29 30 31 32 33 34 35 36 37 38 39 40 41 42 43 44 45 46 47 48 49 50 51 1
//1
//2
//";
//			}

//			public string RetrieveSampleOutput()
//			{
//				return @"
//King of Spades
//2 of Clubs
//4 of Clubs
//5 of Clubs
//6 of Clubs
//7 of Clubs
//8 of Clubs
//9 of Clubs
//10 of Clubs
//Jack of Clubs
//Queen of Clubs
//King of Clubs
//Ace of Clubs
//2 of Diamonds
//3 of Diamonds
//4 of Diamonds
//5 of Diamonds
//6 of Diamonds
//7 of Diamonds
//8 of Diamonds
//9 of Diamonds
//10 of Diamonds
//Jack of Diamonds
//Queen of Diamonds
//King of Diamonds
//Ace of Diamonds
//2 of Hearts
//3 of Hearts
//4 of Hearts
//5 of Hearts
//6 of Hearts
//7 of Hearts
//8 of Hearts
//9 of Hearts
//10 of Hearts
//Jack of Hearts
//Queen of Hearts
//King of Hearts
//Ace of Hearts
//2 of Spades
//3 of Spades
//4 of Spades
//5 of Spades
//6 of Spades
//7 of Spades
//8 of Spades
//9 of Spades
//10 of Spades
//Jack of Spades
//Queen of Spades
//Ace of Spades
//3 of Clubs
//";
//			}
//		}
//	}
//}
