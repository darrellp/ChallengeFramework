using System;
using System.IO;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("Euler Project", "Prob 173", "https://projecteuler.net/problem=173")]
		public class Euler173 : IChallenge
		{
			public static class Static173
			{
				public static int Laminae(int holeSize, int tileCount)
				{
					return Math.Max(0, (int)Math.Floor((Math.Sqrt((double)holeSize * holeSize + tileCount) - holeSize) / 2.0));
				}
			}

			public string Solve(StringReader stm)
			{
				var totalLaminae = 0;
				var tileCount = 1000000;

				for (var holeLayers = 1; holeLayers <= tileCount / 4; holeLayers++)
				{
					totalLaminae += Static173.Laminae(holeLayers, tileCount);
				}
				return totalLaminae.ToString() + Environment.NewLine;
			}

			public string RetrieveSampleInput() { return null; }
			public string RetrieveSampleOutput()
			{
				return @"
1572729
";
			}
		}
	}
}
