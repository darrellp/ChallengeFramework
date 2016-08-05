using System;
using System.Collections.Generic;
using System.Linq;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("ProgChallenges", "Pairsumonious Numbers",
            "http://www.programming-challenges.com/pg.php?page=downloadproblem&probid=110508&format=html")]
        public class PairsumoniousNumbers : IChallenge
        {
            public void Solve()
            {
                while (true)
                {
                    var nextCase = GetCase();
                    if (nextCase == null)
                    {
                        break;
                    }
                    nextCase.Solve();
                }
            }

            PairsumoniousNumbersCase GetCase()
            {
                List<int> vals = GetVals();
                if (vals == null)
                {
                    return null;
                }
                vals = vals.Skip(1).ToList();
                vals.Sort();
                return new PairsumoniousNumbersCase(vals);
            }

            class PairsumoniousNumbersCase
            {
                // The list of sums we have to account for in sorted order.
                private readonly List<int> _sortedSums;
                // key will be sum, value will be count.  I was keeping this as a set but sums can be repeated
                // so I had to do something a little more fancy.
                private readonly Dictionary<int, int> _availableSums = new Dictionary<int, int>();
                // The bases we're backtracking on.
                private readonly List<int> _bases = new List<int>();

                public PairsumoniousNumbersCase(List<int> sortedSums)
                {
                    _sortedSums = sortedSums;

                    // The smallest two sums are already accounted for in the first three sum.
                    foreach (var sortedSum in sortedSums.Skip(2))
                    {
                        MakeSumAvailable(sortedSum);
                    }
                }

                /// <summary>
                /// Solves this pairsumonious case.
                /// </summary>
                /// <remarks>
                /// If we know three sums which derive from three bases (I'm going to call the numbers in the original set
                /// bases) then we can determine the three bases easily enough.  If the sums are s1, s2 and s2 then the original
                /// values are:
                ///     b = (s1 + s2 - s3) / 2
                ///     a = s1 - b
                ///     c = s2 - b
                /// If b0 &lt;= b1 &lt;= b2 &lt;= b3 are the four smallest bases then the two smallest sums are b0 + b1 and b0 + b2.
                /// Sadly, all bets fail after that.  The third sum might be b1 + b2 or it might be b0 + b3.  We want to find
                /// b1 + b2 since that will be the third sum and allow us to reverse engineer to b0, b1 and b2.  Unfortunately, I
                /// don't think there's an immediate and direct way to identify the sum which is b1 + b2 so we just have to
                /// backtrack a bit.  This shouldn't be too much of a problem because we've got at most 45 numbers to deal with and
                /// each step forward eliminates or allows for many of these.
                /// 
                /// So we're going to find all third number possibilities such that at least (s1 + s2 - s3) is even.  If we do have
                /// the sums for the three smallest numbers, then the smallest sum unaccounted for must have at least one base from
                /// the three smallest bases which we know so it's base sum will be bi + b3 for i=0,1 or 2. This is the basis for our
                /// backtrack.  When we make a guess in the backtrack, b3, then we know that we must have bi+b3 for i=0,1 and 2.  We
                /// can check on these and if any of these values isn't in our set then we have to backtrack.  If they are in our set
                /// we make another guess, and so on.
                /// </remarks>
                public void Solve()
                {
                    // The third sum has to have this low bit in order to make (b0 + b2 - b1)/2 integral.
                    var thirdSumLowBit = (_sortedSums[0] ^ _sortedSums[1]) & 1;
                    var thirdSumPossibilities = _sortedSums.Skip(2).Where(s => (s & 1) == thirdSumLowBit);
                    foreach (var sum in thirdSumPossibilities)
                    {
                        // We have to establish the first three sums before we can really get the ball
                        // rolling with backtracking on the remaining sums.  Priming the pump as it were.
                        // The first two sums are b0 + b1 and b0 + b2.  We complete this triangle by searching
                        // for b1 + b2 so we just work our way through the remaining available sums to see if
                        // any of them work as b1 + b2.
                        var b0 = (_sortedSums[0] - sum + _sortedSums[1]) / 2;
                        var b1 = _sortedSums[0] - b0;
                        var b2 = _sortedSums[1] - b0;
                        AddBases(b0, b1, b2);
                        MakeSumUnavailable(sum);
                        if (SolveRest())
                        {
                            PrintResult();
                            return;
                        }
                        RemoveBases(3);
                        MakeSumAvailable(sum);
                    }

                    // Couldn't find a suitable third sum so we have to give up
                    Console.WriteLine("Impossible");
                }

                private void PrintResult()
                {
                    _bases.Sort();
                    for (var iBase = 0; iBase < _bases.Count - 1; iBase++)
                    {
                        Console.Write(_bases[iBase] + " ");
                    }
                    Console.WriteLine(_bases[_bases.Count - 1]);
                }

                private bool SolveRest()
                {
                    if (_availableSums.Count == 0)
                    {
                        return true;
                    }
                    // Try to figure out the base values that add to the smallest
                    // sum not yet accounted for.
                    var nextSum = SmallestAvailableSum();

                    // We have to keep track of these so we can "unremove" them if we back out
                    var sumsRemoved = new List<int>();

                    // Since it's the smallest sum not accounted for, it has to be the sum of one of the bases we've
                    // already discovered with a new base not yet entered into our list so try all the bases to see if
                    // any of them work.  We have to use ToArray below to clone _bases or else CLR will complain about
                    // modifying the basis of the loop from within the loop.
                    foreach (var baseVal in _bases.ToArray())
                    {
                        var newBase = nextSum - baseVal;
                        var validBase = true;
                        foreach (var testBase in _bases)
                        {
                            var testSum = testBase + newBase;

                            // See if we have the sum for testBase + newBase available
                            if (_availableSums.ContainsKey(testSum))
                            {
                                // This sum is now accounted for and is no longer available for further base sums.
                                sumsRemoved.Add(testSum);
                                MakeSumUnavailable(testSum);
                            }
                            else
                            {
                                // Oops!  A base added to the newBase didn't have a corresponding sum so back out.
                                foreach (var sum in sumsRemoved)
                                {
                                    MakeSumAvailable(sum);
                                }
                                sumsRemoved = new List<int>();
                                validBase = false;
                                break;
                            }
                        }
                        if (validBase)
                        {
                            // Okay, this choice of base didn't produce any sums that weren't available so let's
                            // recursively try to find the next available sum
                            AddBases(newBase);
                            if (SolveRest())
                            {
                                // We found the answer!
                                return true;
                            }
                            // This failed in the end so back out and try something else
                            foreach (var sum in sumsRemoved)
                            {
                                MakeSumAvailable(sum);
                            }
                            sumsRemoved = new List<int>();
                            RemoveBases();
                        }
                    }
                    return false;
                }

                private void MakeSumUnavailable(int sum)
                {
                    if (--_availableSums[sum] == 0)
                    {
                        _availableSums.Remove(sum);
                    }
                }

                private void MakeSumAvailable(int sum)
                {
                    if (!_availableSums.ContainsKey(sum))
                    {
                        _availableSums[sum] = 0;
                    }
                    _availableSums[sum]++;
                }

                private void AddBases(params int[] newBases)
                {
                    foreach (var newBase in newBases)
                    {
                        _bases.Add(newBase);
                    }
                }

                private void RemoveBases(int count = 1)
                {
                    for (var i = 0; i < count; i++)
                    {
                        _bases.RemoveAt(_bases.Count - 1);
                    }
                }

                private int SmallestAvailableSum()
                {
                    return _availableSums.Keys.Min();
                }
            }

            public string RetrieveSampleInput()
            {
                return @"
3 1269 1160 1663
3 1 1 1
5 226 223 225 224 227 229 228 226 225 227
5 216 210 204 212 220 214 222 208 216 210
5 -1 0 -1 -2 1 0 -1 1 0 -1
5 79950 79936 79942 79962 79954 79972 79960 79968 79924 79932
";
            }

            public string RetrieveSampleOutput()
            {
                return @"
383 777 886
Impossible
111 112 113 114 115
101 103 107 109 113
-1 -1 0 0 1
39953 39971 39979 39983 39989
";
            }
        }
    }
}
