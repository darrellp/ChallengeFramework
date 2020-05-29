using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace MiscChallenges.Challenges
{
	static partial class ChallengeClass
	{
        [Challenge("Euler Project", "Prob 105", "https://projecteuler.net/problem=105")]
        public class Euler105 : IChallenge
        {
            public void Solve()
            {
                long total = 0;
                using (var file = new System.IO.StreamReader("Data Files/sets.txt"))
                {
                    var seps = ",".ToCharArray();
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        var vals = line.Split(seps).Select(int.Parse).ToList();
                        vals.Sort();

                        var ssst = new SpecialSumSetTester(vals);
                        if (ssst.Check())
                        {
                            total += vals.Sum();
                        }
                    }
                }
                WriteLine(total);
            }

            public string RetrieveSampleInput()
            {
                return null;
            }

            public string RetrieveSampleOutput()
            {
                return @"
73702
";
            }
        }

		private class SpecialSumSetTester
		{
			private readonly List<int> _vals;

			public SpecialSumSetTester(List<int> vals)
			{
				_vals = vals;
			}

			public bool Check()
			{
				// Does condition 2 fail?
				if (!ValidateCondition2())
				{
					return false;
				}

				// Verify no Duplicates
				for (var i = 1; i < _vals.Count; i++)
				{
					if (_vals[i] == _vals[i - 1])
					{
						return false;
					}
				}

				// Check that no two subsets have the same sum.
				var sumsSoFar = new HashSet<int>();

				// For every possible subset size >= 2
				for (var size = 2; size <= _vals.Count / 2; size++)
				{
					// ReSharper disable once LoopCanBeConvertedToQuery
					foreach (var set in ProduceSets(_vals.Count, size))
					{
						var sum = set.Select(i => _vals[i]).Sum();
						if (sumsSoFar.Contains(sum))
						{
							return false;
						}
						sumsSoFar.Add(sum);
					}
				}
				return true;
			}

			/// <summary>
			/// Validates the second condition for _vals.
			/// </summary>
			/// <remarks>
			/// Condition 2 is that if one subset has more elements than another it must
			/// have a larger sum.  Suppose not - suppose that, for instance,
			/// a + b &gt; c + d + e + f.  First of all, we might as well throw out enough
			/// elements from the larger sum so that it only has one more element than the smaller
			/// rather than two.  If it fails for 2 elements compared to four then it fails
			/// for 2 elements compared to 3.  Secondly, we might as well make a + b come
			/// from the largest elements in _val and c, d and e come from the smallest
			/// elements.  In other words, if we fail condition 2 for any two subsets, then
			/// then we will fail by picking n+1 of the smallest elements and comparing them
			/// against n of the largest for some n.  Thus, if we don't fail for these sets,
			/// we won't fail for any sets.  This routine checks those cases.
			/// </remarks>
			/// <returns><c>true</c> if the set meets condition 2, <c>false</c> otherwise.</returns>
			private bool ValidateCondition2()
			{
				// For each potential size of the larger subset
				for (var count = 2; count < (_vals.Count + 3) / 2; count++)
				{
					// Sum that many of the smallest elements
					var sumFront = _vals.Take(count).Sum();
					// Sum one less from the largest elements
					var sumBack = _vals.Skip(_vals.Count - count + 1).Sum();

					// Is the larger set's sum is greater than the smaller set's?
					if (sumFront <= sumBack)
					{
						return false;
					}
				}
				return true;
			}

			private static IEnumerable<List<int>> ProduceSets(int size, int subsetSize)
			{
				var mask1 = (1 << subsetSize) - 1;

				// We're going to produce pairs by using bitmap masks.  The first bitmap mask will range
				// all the subsetSize sets of [0, size).  The second will range over all sets of the
				// remaining elements where the smallest element of the second set is larger than the
				// smallest element of the first.  This will guarantee not repeating set pairs.
				while (mask1 < (1 << size))
				{
					// Get indices corresponding to current mask1 (indices1)
					var indices1 = Enumerable.
						Range(0, size).
						Where(i => (mask1 & (1 << i)) != 0).
						ToList();

					yield return indices1;

					// Advance mask1
					mask1 = NextKBitNumber(mask1);
				}
			}

			private static int NextKBitNumber(int mask)
			{
				// Advance mask1 to next mask
				var u = mask & (-mask);
				var v = mask + u;
				mask = (((mask ^ v) >> 2) / u) | v;
				return mask;
			}
		}
    }
}