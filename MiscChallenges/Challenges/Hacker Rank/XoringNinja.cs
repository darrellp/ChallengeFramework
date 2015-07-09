using System;
using System.Linq;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		/// <summary>
		///  Strategy is to figure out how many times the bit for 2^n is added in to the sum.  Easiest to explain with an example - how many times
		/// is 2^0 = 1 added to the sum?  It happens for every subset which has a 1 in the units position.  Any such subset will have an odd
		/// number of values with 1's in their low bit.  How many of those are there?  Suppose we have 5 values with 1 bits in their low bit and
		/// 3 values with 0 there.  Any subset that has an odd number of values with low bits of 1 will have a 1 in the low bit of their xor.
		/// Just concentrating on these values, we can create odd sets of them in any of C(5,1) + C(5,3) + C(5,5) which is actually 2^(5-1).
		/// For any of these sets, we can pick the values with 0 in their low bits however we desire - 2^3 = 8 ways.  Thus, we have a total of
		///		2^(5-1) * 2^3 = 2^(8-1)
		/// ways - and since the digit being added is the 1's digit, we can multiply by 2^0 to get the total contributed by all these low bits:
		///		2^(8-1) * 2^0 = 2^(8-1)
		/// This only works if there is at least one value which has a 1 in the low bit.  If all the numbers have zero there then the contribution
		/// is zero
		/// 
		/// Following this same logic in general, if there are n values, the amount contributed by the 2^i bit is just
		///		2^(n-1) * 2^(i) * Z(i)
		/// where Z(i) is the i'th bit of the OR of all the values - i.e., it's zero if they all have zeros at that position and one otherwise.
		/// Summing over all i would give us
		///		2^(n-1)*OR
		/// where OR is the OR of all the values.
		/// 
		/// Wow - didn't expect that!
		/// 
		/// </summary>
		[Challenge("Hacker Rank", "Xoring Ninja", "https://www.hackerrank.com/challenges/xoring-ninja")]
		// ReSharper disable once InconsistentNaming
		public class XoringNinja : IChallenge
		{
			private const int Mod = 1000000007;

			// ReSharper disable once UnusedParameter.Local
			public void Solve()
			{
				// ReSharper disable AssignNullToNotNullAttribute
				var cases = int.Parse(Console.ReadLine());
				for (var iCase = 0; iCase < cases; iCase++)
				{
					var cVals = int.Parse(Console.ReadLine());
					var ret = Console.ReadLine().Split(' ').Select(int.Parse).Aggregate((a1, a2) => a1 | a2);
					// ReSharper restore AssignNullToNotNullAttribute
					for (var iBit = 0; iBit < cVals - 1; iBit++)
					{
						ret = (ret*2)%Mod;
					}
				
					Console.WriteLine(ret);
				}
			}

			public string RetrieveSampleInput()
			{
				return @"
1
3
1 2 3
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
