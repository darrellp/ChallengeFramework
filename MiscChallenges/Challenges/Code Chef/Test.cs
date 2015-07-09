using System;
using System.IO;

#if CHALLENGE_RUNNER
namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("Code Chef", "Test - CS", "http://www.codechef.com/problems/TEST")]
		public class ChefTest : IChallenge
		{
			public string Solve(StringReader stm)
			{
				Console.SetIn(stm);
				var sw = new StringWriter();
				Console.SetOut(sw);
				LocalTest.MainTest();
				return sw.ToString();
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
			public class LocalTest
			{
				internal static void MainTest()
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
		}
	}
}
