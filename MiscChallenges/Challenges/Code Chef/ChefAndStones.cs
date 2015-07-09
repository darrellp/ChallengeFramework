using System;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("Code Chef", "Chef and Stones", "http://www.codechef.com/JAN15/problems/CHEFSTON")]
		// ReSharper disable once InconsistentNaming
		public class ChefSton : IChallenge
		{
			public void Solve()
			{
				var cTests = GetVal();
				for (var iTest = 0; iTest < cTests; iTest++)
				{
					var vals = GetVals();
					var cTypes = vals[0];
					var time = vals[1];
					var typeTimes = GetVals();
					var typeProfits = GetVals();
					var maxProfit = int.MinValue;

					for (int iType = 0; iType < cTypes; iType++)
					{
						var count = time/typeTimes[iType];
						var profit = count*typeProfits[iType];
						if (profit > maxProfit)
						{
							maxProfit = profit;
						}
					}
					Console.WriteLine(maxProfit.ToString());
				}
			}

			public string RetrieveSampleInput()
			{
				return @"
1
3 10
3 4 5
4 4 5
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
12
";
			}
		}
	}
}
