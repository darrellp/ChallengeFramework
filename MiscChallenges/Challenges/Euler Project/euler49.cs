using System.Collections.Generic;
using NumberTheoryLong;
using static System.Console;


namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
	{

		private static readonly bool[] IsPrime = new bool[9000];
		private static bool _found;

        [Challenge("Euler Project", "Prob 49",
            "https://projecteuler.net/problem=49")]
        public class Euler49 : IChallenge
        {
            public void Solve()
            {
                _found = false;
                for (var i = 1000; i < 10000; i++)
                {
                    IsPrime[i - 1000] = Primes.IsPrime(i);
                }

                for (var i = 1000; i < 10000; i++)
                {
                    if (i != 1487 && IsPrime[i - 1000])
                    {
                        var d0 = i % 10;
                        var d1 = (i % 100) / 10;
                        var d2 = (i % 1000) / 100;
                        var d3 = i / 1000;
                        var second =
                            Try(i, d3, d2, d0, d1) +
                            Try(i, d3, d0, d2, d1) +
                            Try(i, d3, d1, d2, d0) +
                            Try(i, d3, d1, d0, d2) +
                            Try(i, d3, d0, d1, d2) +
                            Try(i, d2, d3, d1, d0) +
                            Try(i, d2, d3, d0, d1) +
                            Try(i, d0, d3, d2, d1) +
                            Try(i, d1, d3, d2, d0) +
                            Try(i, d1, d3, d0, d2) +
                            Try(i, d0, d3, d1, d2) +
                            Try(i, d2, d0, d3, d1) +
                            Try(i, d2, d1, d3, d0) +
                            Try(i, d0, d2, d3, d1) +
                            Try(i, d1, d2, d3, d0) +
                            Try(i, d0, d1, d3, d2) +
                            Try(i, d1, d0, d3, d2) +
                            Try(i, d2, d1, d0, d3) +
                            Try(i, d2, d0, d1, d3) +
                            Try(i, d1, d2, d0, d3) +
                            Try(i, d0, d2, d1, d3) +
                            Try(i, d1, d0, d2, d3) +
                            Try(i, d0, d1, d2, d3);
                        if (second != 0)
                        {
                            var third = 2 * second - i;
                            WriteLine( i * 100000000L + second * 10000L + third);
                        }
                    }
                }
            }

            public string RetrieveSampleInput() { return null; }
            public string RetrieveSampleOutput()
            {
                return @"
296962999629
";
            }


			private static int Try(int first, int d3, int d2, int d1, int d0)
            {
                if (_found || d0 % 2 == 0)
                {
                    return 0;
                }

                var second = d0 + 10 * d1 + 100 * d2 + 1000 * d3;
                if (second <= first || !IsPrime[second - 1000])
                {
                    return 0;
                }

                var third = 2 * second - first;

                if (third >= 10000 || !IsPrime[third - 1000])
                {
                    return 0;
                }

                var t0 = third % 10;
                var t1 = (third % 100) / 10;
                var t2 = (third % 1000) / 100;
                var t3 = third / 1000;

                var dSort = new List<int> {d0, d1, d2, d3};
                var tSort = new List<int> {t0, t1, t2, t3};
                dSort.Sort();
                tSort.Sort();
                for (var i = 0; i < 4; i++)
                {
                    if (dSort[i] != tSort[i])
                    {
                        return 0;
                    }
                }

                _found = true;
                return second;
            }
        }
    }
}
