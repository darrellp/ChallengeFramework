using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		/// <summary>
		/// </summary>
		[Challenge("UVA", "Subsequence",
			"https://uva.onlinejudge.org/index.php?option=com_onlinejudge&Itemid=8&category=246&page=show_problem&problem=3562")]
		// ReSharper disable once InconsistentNaming
		public class Subsequence : IChallenge
		{
			public void Solve()
			{
				while (true)
				{
					var vals = GetVals();
					if (vals == null)
					{
						break;
					}
					var totalRequired = vals[1];
					Console.WriteLine("{0}", GetSeqValue(totalRequired, GetVals()));
				}
			}

			private int GetSeqValue(int totalRequired, List<int> sequence)
			{
				var iFront = -1;
				var iTail = 0;
				var sum = 0;
				var longestRun = int.MaxValue;

				while (true)
				{
					if (++iFront >= sequence.Count)
					{
						break;
					}
					sum += sequence[iFront];
					if (sum >= totalRequired)
					{
						while (true)
						{
							if (sum - sequence[iTail] < totalRequired)
							{
								break;
							}
							sum -= sequence[iTail++];
						}
						longestRun = Math.Min(longestRun, iFront - iTail + 1);
					}
				}
				return longestRun == int.MaxValue ? 0 : longestRun;
			}

			public string RetrieveSampleInput()
			{
				return @"
10 15
5 1 3 5 10 7 4 9 2 8
5 11
1 2 3 4 5
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
2
3
";
			}
		}
	}
}
