using System;
using System.Collections.Generic;
using System.Linq;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("Hacker Rank", "Merge Lists", "https://www.hackerrank.com/challenges/merge-list")]
		// ReSharper disable once InconsistentNaming
		public class MergeLists : IChallenge
		{
			static long GCD(long n1, long n2)
			{
				var r = n2;
				var rLast = 1L;
				while (r != 0)
				{
					rLast = r;
					r = n1 - (n1 / n2) * n2;
					n1 = n2;
					n2 = r;
				}
				return rLast;
			}

			// ReSharper disable once UnusedParameter.Local
			public void Solve()
			{
				// ReSharper disable AssignNullToNotNullAttribute
				var nCases = int.Parse(Console.ReadLine());

				for (var i = 0; i < nCases; i++)
				{

					// ReSharper disable once PossibleNullReferenceException
					var nm = Console.ReadLine().Split(' ').Select(int.Parse).ToList();
					var n = Math.Min(nm[0], nm[1]);
					var m = Math.Max(nm[0], nm[1]);
					var multipliers = new List<long>();

					for (var r = 0L; r < n; r++)
					{
						multipliers.Add(m + n - r);
						var divisor = r + 1;
						var newMults = new List<long>();
						for (var iDiv = 0; iDiv < multipliers.Count; iDiv++)
						{
							var gcd = GCD(multipliers[iDiv], divisor);
							var newMult = multipliers[iDiv] / gcd;
							if (newMult != 1)
							{
								newMults.Add(newMult);
							}
							divisor /= gcd;
							if (divisor == 1)
							{
								newMults.AddRange(multipliers.GetRange(iDiv + 1, multipliers.Count - iDiv - 1));
								multipliers = newMults;
								break;
							}
						}
					}
					var curVal = multipliers.Aggregate<long, long>(1, (current, mult) => current * mult % 1000000007L);
					Console.WriteLine(curVal);
				}
				// ReSharper restore AssignNullToNotNullAttribute
			}

			public string RetrieveSampleInput()
			{
				return @"
1
2 2
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
6
";
			}
		}
	}
}
