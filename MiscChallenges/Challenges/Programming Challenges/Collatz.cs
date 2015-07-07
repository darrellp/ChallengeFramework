using System;
using System.IO;
using System.Linq;
using System.Text;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("ProgChallenges", "The 3n + 1 Problem",
			"http://www.programming-challenges.com/pg.php?page=downloadproblem&probid=110101&format=html")]
		public class Collatz : IChallenge
		{
			public string Solve(StringReader stm)
			{
				var ret = new StringBuilder();
				while (true)
				{
					var nextCase = GetCase(stm);
					if (nextCase == null)
					{
						break;
					}

					var low = Math.Min(nextCase.Item1, nextCase.Item2);
					var high = Math.Max(nextCase.Item1, nextCase.Item2);
					var c = Enumerable.Range(low, high - low + 1).
						Select(CollatzCount).
						Max();
					ret.Append(string.Format("{0} {1} {2}" + Environment.NewLine, nextCase.Item1, nextCase.Item2, c));
				}
				return ret.ToString();
			}

			private int CollatzCount(int i)
			{
				long v = i;
				var count = 1;

				while (v != 1)
				{
					count++;
					if ((v & 1) == 0)
					{
						v = v >> 1;
					}
					else
					{
						v = (v << 1) + v + 1;
					}
				}
				return count;
			}

			private Tuple<int, int> GetCase(StringReader stm)
			{
				var line = stm.ReadLine();
				if (line == null)
				{
					return null;
				}
				var vals = line.Split(' ').
					Select(int.Parse).
					ToList();
				return new Tuple<int, int>(vals[0], vals[1]);
			}

			public string RetrieveSampleInput()
			{
				return @"
1 10
100 200
201 210
900 1000
1 1
10 1
210 201
113383 113383
999999 1
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
1 10 20
100 200 125
201 210 89
900 1000 174
1 1 1
10 1 20
210 201 89
113383 113383 248
999999 1 525
";
			}
		}
	}
}
