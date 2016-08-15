using System;
using System.Linq;
using NumberTheoryLong;
using static System.Console;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("ProgChallenges", "Factovisors",
            "http://www.programming-challenges.com/pg.php?page=downloadproblem&probid=110704&format=html")]
        public class Factovisors : IChallenge
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
                    var fIsDivisible = IsDivisible(nextCase.Item2, nextCase.Item1);
                    WriteLine($"{nextCase.Item2} {(fIsDivisible ? "divides" : "does not divide")} {nextCase.Item1}!");
                }
            }

            private static bool IsDivisible(int n, int m)
            {
                foreach (var factor in Factoring.Factor(n))
                {
                    var val = (int)factor.Prime;
                    var valPower = val;
                    var expInFactorial = 0;
                    while (valPower <= m)
                    {
                        expInFactorial += m / valPower;
                        valPower *= val;
                    }
                    if (expInFactorial < factor.Exp)
                    {
                        return false;
                    }
                }
                return true;
            }

            private Tuple<int, int> GetCase()
            {
                var line = ReadLine();
                if (line == null)
                {
                    return null;
                }
                var vals = line.Split(' ').
                    Select(int.Parse).
                    ToList();
                return new Tuple<int, int>(vals[0], vals[1]);
            }

            public string RetrieveSampleInput()
            {
                // We eliminate this first newline in the caller so that the uninterrupted input
                // can go at the left hand column.
                return @"
6 9
6 27
20 10000
20 100000
1000 1009
";
            }

            public string RetrieveSampleOutput()
            {
                // Caller will eliminate first newline...
                return @"
9 divides 6!
27 does not divide 6!
10000 divides 20!
100000 does not divide 20!
1009 does not divide 1000!
";
            }
        }
    }
}