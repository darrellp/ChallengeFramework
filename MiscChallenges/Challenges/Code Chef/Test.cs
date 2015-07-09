using System;

#if CHALLENGE_RUNNER
namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("Code Chef", "Test - CS", "http://www.codechef.com/problems/TEST")]
		public class ChefTest : IChallenge
		{
			public class LocalTest
			{
				public static void Solve()
#else
			public class LocalTest
			{
				public static void Main()
#endif
				{
					var input = Console.ReadLine();
					while (input != "42")
					{
						Console.WriteLine(input);
						input = Console.ReadLine();
					}
				}
			}
#if CHALLENGE_RUNNER
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

			public void Solve()
			{
				LocalTest.Solve();
			}
		}
	}
}
#endif
