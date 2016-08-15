using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Console;

// ReSharper disable LocalizableElement

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("Dr Ecco", "Small Change for Mujhikistan",
			"https://books.google.com/books/about/Doctor_Ecco_s_Cyberpuzzles_36_Puzzles_fo.html?id=Y0JXuG3ZDgQC")]
		public class SmallChange : IChallenge
		{

			// ReSharper disable once UnusedParameter.Local
			public void Solve()
			{
				DenominationSet denLow = null;
				var smallTot = int.MaxValue;
				int sumExceededAt;

				for (var i1 = 2; i1 < 24; i1++)
				{
					for (var i2 = i1 + 1; i2 < 25; i2++)
					{
						var den = new DenominationSet(1, i1, i2);
						var tot = den.Sum0ToN(99, smallTot, out sumExceededAt);
						if (tot < 0)
						{
							if (sumExceededAt < i2)
							{
								break;
							}
						}
						else if (tot < smallTot)
						{
							smallTot = tot;
							denLow = den;
						}
					}
				}
                WriteLine("The smallest avg is {0} achieved by {1}", smallTot/99.0, denLow);

				smallTot = int.MaxValue;
				denLow = null;
				for (var i1 = 2; i1 < 28; i1++)
				{
					for (var i2 = i1 + 1; i2 < 29; i2++)
					{
						for (var i3 = i2 + 1; i3 < 30; i3++)
						{
							var den = new DenominationSet(1, i1, i2, i3);
							var tot = den.Sum0ToN(99, smallTot, out sumExceededAt);
							if (tot < 0)
							{
								if (sumExceededAt < i3)
								{
									break;
								}
							}
							else if (tot < smallTot)
							{
								smallTot = tot;
								denLow = den;
							}
						}
					}
				}
                WriteLine("The smallest avg is {0} achieved by {1}", smallTot / 99.0, denLow);

				smallTot = int.MaxValue;
				denLow = null;
				for (var i1 = 2; i1 < 31; i1++)
				{
					for (var i2 = i1 + 1; i2 < 32; i2++)
					{
						for (var i3 = i2 + 1; i3 < 33; i3++)
						{
							for (var i4 = i3 + 1; i4 < 34; i4++)
							{
								var den = new DenominationSet(1, i1, i2, i3, i4);
								var tot = den.Sum0ToN(99, smallTot, out sumExceededAt);
								if (tot < 0)
								{
									if (sumExceededAt < i4)
									{
										break;
									}
								}
								else if (tot < smallTot)
								{
									smallTot = tot;
									denLow = den;
								}
							}
						}
					}
				}
                WriteLine("The smallest avg is {0} achieved by {1}", smallTot / 99.0, denLow);

				smallTot = int.MaxValue;
				denLow = null;
				for (var i1 = 2; i1 < 34; i1++)
				{
					for (var i2 = i1 + 1; i2 < 35; i2++)
					{
						for (var i3 = i2 + 1; i3 < 36; i3++)
						{
							for (var i4 = i3 + 1; i4 < 37; i4++)
							{
								for (var i5 = i4 + 1; i5 < 38; i5++)
								{
									var den = new DenominationSet(1, i1, i2, i3, i4, i5);
									var tot = den.Sum0ToN(99, smallTot, out sumExceededAt);
									if (tot < 0)
									{
										if (sumExceededAt < i5)
										{
											break;
										}
									}
									else if (tot < smallTot)
									{
										smallTot = tot;
										denLow = den;
									}
								}
							}
						}
					}
				}
                WriteLine("The smallest avg is {0} achieved by {1}", smallTot / 99.0, denLow);

				smallTot = int.MaxValue;
				denLow = null;
				var smvMax = 40;
				for (var i1 = 2; i1 < smvMax; i1++)
				{
					for (var i2 = i1 + 1; i2 < smvMax + 1; i2++)
					{
						for (var i3 = i2 + 1; i3 < smvMax + 2; i3++)
						{
							for (var i4 = i3 + 1; i4 < smvMax + 3; i4++)
							{
								for (var i5 = i4 + 1; i5 < smvMax + 4; i5++)
								{
									for (var i6 = i5 + 1; i6 < smvMax + 5; i6++)
									{
										var den = new DenominationSet(1, i1, i2, i3, i4, i5, i6);
										var tot = den.Sum0ToN(99, smallTot, out sumExceededAt);
										if (tot < 0)
										{
											if (sumExceededAt < i6)
											{
												break;
											}
										}
										else if (tot < smallTot)
										{
											smallTot = tot;
											denLow = den;
										}
									}
								}
							}
						}
					}
				}
                WriteLine("The smallest avg is {0} achieved by {1}", smallTot / 99.0, denLow);
			}

			public string RetrieveSampleInput() { return null; }

			public string RetrieveSampleOutput() { return null; }

			public class DenominationSet
			{
				private readonly int[] _dens;

				public int Length => _dens.Length;

			    public DenominationSet(params int[] dens)
				{
					var set = new HashSet<int>(dens);
					_dens = set.ToArray();
					Array.Sort(_dens);
					if (_dens[0] != 1)
					{
						throw new ArgumentException("Invalid denominations in DenominationSet");
					}
				}

				public int Sum0ToN(int n, int maxSum, out int sumExceededAt)
				{
					var table = MakeChangeTable(n, maxSum, out sumExceededAt);
					if (table != null)
					{
						return table.Sum();
					}
					return -1;
				}

				public int Value(IEnumerable<int> counts)
				{
					return counts.Select((c, i) => c * _dens[i]).Sum();
				}

				public List<int> GreedySet(int n, IEnumerable<int> dens)
				{
					var ret = Enumerable.Repeat(0, Length).ToList();
					var densList = dens.ToList();
					var remaining = n;
					var iDen = Length - 1;

					while (remaining != 0)
					{
						ret[iDen] = remaining / _dens[iDen];
						remaining -= ret[iDen] * densList[iDen];
						iDen--;
					}
					return ret;
				}

				public List<int> GreedySet(int n)
				{
					return GreedySet(n, _dens);
				}

				private static bool IsCanonical3(int c2, int c3)
				{
					var r = c3 % c2;
					var q = c3 / c2;
					return !(0 < r && r < c2 - q);
				}

				private bool First3Canonical()
				{
					return IsCanonical3(_dens[1], _dens[2]);
				}

				public int[] MakeChangeTable(int max, int maxSum, out int sumExceededAt)
				{
					var ret = new int[max + 1];
					ret[0] = 0;
					ret[1] = 1;
					var idenStart = 0;
					var sum = 1;
					sumExceededAt = -1;

					for (var iValue = 2; iValue <= max; iValue++)
					{
						if (idenStart < Length - 1 && iValue == _dens[idenStart + 1])
						{
							ret[iValue] = 1;
							idenStart++;
							continue;
						}
						var min = int.MaxValue;
						for (var iCur = 0; iCur <= idenStart; iCur++)
						{
							var minWithICur = 1 + ret[iValue - _dens[iCur]];
							if (minWithICur < min)
							{
								min = minWithICur;
							}
						}
						ret[iValue] = min;
						sum += min;
						if (sum > maxSum)
						{
							sumExceededAt = iValue;
							return null;
						}
					}
					return ret;
				}
				///-------------------------------------------------------------------------------------------------
				/// <summary>	Query if this denomination set is canonical. </summary>
				///
				/// <remarks>	Canonical means that the greedy algorithm always produces the optimal set of
				/// 			coins.  The specific cases for 3, 4 or 5 coins are taken from cai's article.
				/// 			Pearson't algorithm for the general case is taken from Shallit's article.
				/// 			Darrell, 5/20/2015. </remarks>
				///
				/// <returns>	true if canonical, false if not. </returns>
				///-------------------------------------------------------------------------------------------------

				public bool IsCanonical()
				{
					if (_dens[0] != 1)
					{
						return false;
					}

					switch (_dens.Length)
					{
						case 1:
							return true;

						case 2:
							return true;

						case 3:
							return First3Canonical();

						case 4:
							if (First3Canonical())
							{
								var k = _dens[3] / _dens[2];
								return GreedySet((k + 1) * _dens[2]).Sum() <= k + 1;
							}
							return false;

						case 5:
							if (First3Canonical() && !(
								_dens[1] == 2 &&
								_dens[3] == _dens[2] + 1 &&
								_dens[4] == 2 * _dens[2]))
							{
								var k = _dens[4] / _dens[3];
								return GreedySet((k + 1) * _dens[3]).Sum() <= k + 1;
							}
							return false;

						default:
							return GeneralCanonical() != null;
					}
				}

				///-------------------------------------------------------------------------------------------------
				/// <summary>	Determines the minimal counterexample if this denomination set is non-canonical </summary>
				///
				/// <remarks>	From "What This Country Needs is an 18¢ Piece" Darrell, 5/20/2015. </remarks>
				///
				/// <returns>	List of counts for minimal counterexample. </returns>
				///-------------------------------------------------------------------------------------------------

				public List<int> GeneralCanonical()
				{
					var ret = int.MaxValue;
					List<int> counterSet = null;

					// i and j indices seem to be reversed here but keeping them to
					// closer reflect the text in the article
					for (var j = 0; j < Length - 1; j++)
					{
						for (var i = j; i < Length - 1; i++)
						{
							var trialOptimal = GreedySet(_dens[i + 1] - 1);
							trialOptimal[j]++;
							var trialCounterExample = Enumerable.Range(j, Length - j).Select(indx => trialOptimal[indx] * _dens[indx]).Sum();
							if (trialCounterExample < ret)
							{
								var greedy = GreedySet(trialCounterExample);
								if (greedy.Sum() > trialOptimal.Skip(j).Sum())
								{
									counterSet = trialOptimal;
									for (var iZero = 0; iZero < j; iZero++)
									{
										counterSet[iZero] = 0;
									}
									ret = trialCounterExample;
								}
							}
						}
					}
					return counterSet;
				}

				public override string ToString()
				{
					StringBuilder sbld = new StringBuilder();
					sbld.Append("[");
					for (int iDen = 0; iDen < Length; iDen++)
					{
						if (iDen != 0)
						{
							sbld.Append(", ");
						}
						sbld.Append(_dens[iDen]);
					}
					sbld.Append("]");
					return sbld.ToString();
				}
			}
		}
	}
}
