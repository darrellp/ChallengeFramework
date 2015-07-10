using System;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("Code Chef", "TestNS - CS", "http://www.codechef.com/problems/TEST")]
		public class ChefTestNS : IChallenge
		{
			public void Solve()
			{
				var input = Console.ReadLine();
				while (input != "42")
				{
					Console.WriteLine(input);
					input = Console.ReadLine();
				}
			}

			public string RetrieveSampleInput()
			{
				return @"
1
2
88
42
99
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
1
2
88
";
			}
		}
	}
}
