using System;
using NumberTheoryLong;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("ProgChallenges", "Summation of Four Primes",
            "http://www.programming-challenges.com/pg.php?page=downloadproblem&probid=110705&format=html")]
        public class FourPrimes : IChallenge
        {
            public void Solve()
            {
                while (true)
                {
                    var val = Console.ReadLine();
                    if (val == null)
                    {
                        break;
                    }
                    FourPrimesSolver(long.Parse(val));
                }
            }

            static long NextSmallestPrime(long n)
            {
                for (; !n.IsPrime(); n--)
                {
                }

                return n;
            }

            static long Goldbach(long even)
            {
                if (even == 4)
                {
                    return 2;
                }
                if ((even & 1) != 0 || even < 5)
                {
                    throw new ArgumentException("Oops - Goldbach only works with even values >= 4");
                }
                var i = even - 3;
                for (; !i.IsPrime() || !(even - i).IsPrime(); i -= 2)
                {
                }
                return i;
            }

            private static void FourPrimesSolver(long n)
            {
                if (n < 8)
                {
                    Console.WriteLine(@"Impossible.");
                    return;
                }
                // ReSharper disable JoinDeclarationAndInitializer
                long p1, p2, p3, p4;
                // ReSharper restore JoinDeclarationAndInitializer

                if ((n & 1) == 1)
                {
                    if (n == 9)
                    {
                        // Scheme doesn't quite work for 9 so just return answer directly...
                        Console.WriteLine( @"2 2 2 3" );
                        return;
                    }
                    p1 = 2;
                    p2 = NextSmallestPrime(n - 7);
                }
                else
                {
                    p1 = p2 = NextSmallestPrime(n / 2 - 2);
                }
                p3 = Goldbach(n - p1 - p2);
                p4 = n - p1 - p2 - p3;
                Console.WriteLine(@"{0} {1} {2} {3}", p1, p2, p3, p4);
            }

            public string RetrieveSampleInput()
            {
                return @"
24
36
46
7
9
23
1624535
1624536
";
            }

            public string RetrieveSampleOutput()
            {
                return @"
7 7 7 3
13 13 7 3
19 19 5 3
Impossible.
2 2 2 3
2 13 5 3
2 1624523 7 3
812257 812257 19 3
";
            }
        }
    }
}
