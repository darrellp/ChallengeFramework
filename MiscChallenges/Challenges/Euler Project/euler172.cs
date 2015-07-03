using System;
using System.IO;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("Euler Project", "Prob 172", "https://projecteuler.net/problem=172")]
		// ReSharper disable once InconsistentNaming
		public class Euler172 : IChallenge
		{
			public string Solve(StringReader stm)
			{
				var ret = 0UL;
				var fact18 = Fact(18);

				for (var m3 = 0; m3 <= 6; m3++)
				{
					for (var m2 = 0; m2 <= 9; m2++)
					{
						var m1 = 18 - 3 * m3 - 2 * m2;
						if (m1 < 0 || m1 + m2 + m3 > 10)
						{
							continue;
						}
						ret += (fact18 / ((ulong)Math.Pow(6L, m3) * (ulong)Math.Pow(2L, m2))) *
							Comb(10, m1) * Comb(10 - m1, m2) * Comb(10 - m1 - m2, m3);
					}
				}
				return (ret * 9 / 10).ToString() + Environment.NewLine;
			}

			public string RetrieveSampleInput() { return null; }
			public string RetrieveSampleOutput()
			{
				return @"
227485267000992000
";
			}
		}
	}
}
