#define PARALLEL
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("Euler Project", "Prob 401", "https://projecteuler.net/problem=401")]
		// ReSharper disable once InconsistentNaming
		public class Euler401 : IChallenge
		{
			private const long M = (long)1E15;
			private const long R = (long)1E9;
			private static long _sqrM;

			public void Solve()
			{
				_sqrM = (long)Math.Floor(Math.Sqrt(M));
#if PARALLEL
				var results = new long[_sqrM];
				Parallel.For(1, _sqrM + 1, d =>
				{
					var upper = (long)Math.Floor(M / (double)d);
					var dsq = (d * d) % R;
					var dsqPart = (dsq * (upper % R)) % R;
					var lower = (long)Math.Floor(M / (double)(d + 1)) + 1;
					var divPart = (SquareSum(lower, upper) * d) % R;
					results[d - 1] = dsqPart + divPart;
				});

				var sum = results.Sum() % R;
#else
			var lower = M + 1;

			// I have a Mathematica notebook that gives
			// the explanation for this
			for (long d = 1; d <= sqrM; d++)
			{
				var upper = lower - 1;
				var dsq = (d * d) % R;
				var dsqPart = (dsq * (upper % R)) % R;
				lower = (long) Math.Floor(M / (double) (d + 1)) + 1;
				var divPart = (SquareSum(lower, upper) * d) % R;
				sum = (sum + dsqPart + divPart);
			}
#endif
				Console.WriteLine(sum % R);
			}

			private static long SquareSum(long lower, long upper)
			{
				BigInteger biLower = lower;
				BigInteger biUpper = upper;

				var mult1 = 1 - biLower + biUpper;
				var sum1 = (biLower * (2 * biLower - 1));
				var sum2 = (biUpper * (1 + 2 * (biLower + biUpper)));
				return (long)((mult1 * (sum1 + sum2) / 6) % R);
			}

			public string RetrieveSampleInput() { return null; }
			public string RetrieveSampleOutput()
			{
				return @"
281632621
";
			}
		}
	}
}
