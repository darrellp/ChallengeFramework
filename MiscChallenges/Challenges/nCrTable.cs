using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("Hacker Rank", "nCr Table", "https://www.hackerrank.com/challenges/ncr-table")]
		// ReSharper disable once InconsistentNaming
		public class nCrTable : IChallenge
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
			static void MainTest(String[] args)
			{
				// ReSharper disable AssignNullToNotNullAttribute
				var nCases = int.Parse(Console.ReadLine());

				for (var i = 0; i < nCases; i++)
				{

					var n = int.Parse(Console.ReadLine());
					var vals = new List<long>{1};
					var multipliers = new List<long>();
					for (var r = 0L; r < n / 2; r++)
					{
						multipliers.Add(n - r);
						var divisor = r + 1;
						var newMults = new List<long>();
						for (var iDiv = 0; iDiv < multipliers.Count; iDiv++)
						{
							var gcd = GCD(multipliers[iDiv], divisor);
							var newMult = multipliers[iDiv]/gcd;
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
						
						var curVal = multipliers.Aggregate<long, long>(1, (current, mult) => current*mult % 1000000000L);
						vals.Add(curVal);
					}
					for (var r = (n - 1) / 2; r >= 0; r--)
					{
						vals.Add(vals[r]);
					}
					var firstTime = true;
					foreach (var val in vals)
					{
						Console.Write((firstTime ? "" : " ") + val);
						firstTime = false;
					}
					Console.WriteLine();
				}
				// ReSharper restore AssignNullToNotNullAttribute
			}

			public string Solve(StringReader stm)
			{
				return SolveInOut(stm, MainTest);
			}

			public string RetrieveSampleInput()
			{
				return @"
3
2
4
5
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
1 2 1
1 4 6 4 1
1 5 10 10 5 1
";
			}
		}
	}
}
