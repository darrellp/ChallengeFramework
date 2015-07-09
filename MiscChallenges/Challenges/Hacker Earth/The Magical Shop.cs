using System;
using System.Linq;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("HackerEarth", "The Magical Shop", "https://www.hackerearth.com/problem/algorithm/the-magical-shop/")]
		public class MagicalShop : IChallenge
		{
			public void Solve()
			{
				var ab = Console.ReadLine();
				// ReSharper disable once PossibleNullReferenceException
				var vals = ab.Split(' ').Select(long.Parse).ToList();
				var a = vals[0];
				var b = vals[1];
				// ReSharper disable once AssignNullToNotNullAttribute
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
