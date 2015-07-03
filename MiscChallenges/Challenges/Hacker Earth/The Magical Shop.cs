using System;
using System.IO;
using System.Linq;
using System.Text;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("HackerEarth", "The Magical Shop", "https://www.hackerearth.com/problem/algorithm/the-magical-shop/")]
		public class MagicalShop : IChallenge
		{
			static void MainTest(string[] args)
			{
				var ab = Console.ReadLine();
				var vals = ab.Split(' ').Select(n => long.Parse(n)).ToList();
				var a = vals[0];
				var b = vals[1];
				var onString = Console.ReadLine().Select(ch => ch == '1').ToList();
				var sum = 0L;

				foreach (var isOn in onString)
				{
					if (isOn)
					{
						sum = (sum + a)%b;
					}
					a = (a*a)%b;
				}
				Console.WriteLine(sum);
			}

			public string Solve(StringReader stm)
			{
				return SolveInOut(stm, MainTest);
			}

			public string RetrieveSampleInput()
			{
				return @"
5 100
101
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
30
";
			}
		}
	}
}
