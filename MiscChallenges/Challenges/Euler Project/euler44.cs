using NumberTheoryLong;
using static System.Console;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
	{
        [Challenge("Euler Project", "Prob 44","https://projecteuler.net/problem=44")]

        public class Euler44 : IChallenge
        {
            public void Solve()
            {
                var fFoundPair = false;
                for (var iLarge = 0; !fFoundPair; iLarge++)
                {
                    for (var iSmall = iLarge - 1; iSmall > 0 && !fFoundPair; iSmall--)
                    {
                        // ReSharper disable once AssignmentInConditionalExpression
                        if (fFoundPair = ValidPair(iLarge, iSmall))
                        {
                            SeekSmaller(ref iLarge, ref iSmall);
                            WriteLine(IndexToPent(iLarge) - IndexToPent(iSmall));
                        }
                    }
                }
            }
            public string RetrieveSampleInput() { return null; }
            public string RetrieveSampleOutput()
            {
                return @"
5482660
";
            }


			private static void SeekSmaller(ref int iLarge, ref int iSmall)
            {
                var baseDiff = PentagDiff(iLarge, iSmall);
                for (var iLargeTrial = iLarge + 1;; iLargeTrial++)
                {
                    var firstDiff = PentagDiff(iLargeTrial, iLargeTrial - 1);

                    if (firstDiff > baseDiff)
                    {
                        return;
                    }

                    for (var iSmallTrial = iLargeTrial - 1;
                        PentagDiff(iLargeTrial, iSmallTrial) < baseDiff;
                        iSmallTrial--)
                    {
                        if (ValidPair(iLargeTrial, iSmallTrial))
                        {
                            SeekSmaller(ref iLargeTrial, ref iSmallTrial);
                            iLarge = iLargeTrial;
                            iSmall = iSmallTrial;
                            return;
                        }
                    }
                }
            }

            public static long PentagDiff(int m, int n)
            {
                return IndexToPent(m) - IndexToPent(n);
            }

            public static bool IsPentagonal(long n)
            {
                var disc = 24 * n + 1;
                var sqrt = disc.IntegerSqrt();
                return sqrt * sqrt == disc && sqrt % 6 == 5;
            }

            public static long IndexToPent(int n)
            {
                return n * (3L * n - 1) / 2L;
            }

            public static bool ValidPair(int index1, int index2)
            {
                var pent1 = IndexToPent(index1);
                var pent2 = IndexToPent(index2);

                return (IsPentagonal(pent1 + pent2) && IsPentagonal(pent1 - pent2));
            }
        }
    }
}
