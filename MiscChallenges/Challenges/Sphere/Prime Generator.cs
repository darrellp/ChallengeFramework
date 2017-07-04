using System;
using System.Collections;
using System.Collections.Generic;
using static System.Console;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        /// <summary>
        /// </summary>
        [Challenge("Sphere", "Prime Generator",
            "http://www.spoj.com/problems/PRIME1/")]
        // ReSharper disable once InconsistentNaming
        public class PrimeGenerator : IChallenge
        {
            static int _highWaterMark = 2;
            static readonly BitArray IsCompositeList = new BitArray(3);

            public void Solve()
            {
                bool isFirstTest = true;
                IsCompositeList[0] = IsCompositeList[1] = true;
                ReadLine();
                List<int> vals;
                while ((vals = GetVals()) != null)
                {
                    var low = vals[0];
                    var high = vals[1];
                    if (low <= 1)
                    {
                        low = 2;
                    }
                    if (high <= 1)
                    {
                        high = 2;
                    }
                    var composites = new BitArray(high - low + 1);

                    Sieve(composites, low, high);

                    if (!isFirstTest)
                    {
                        WriteLine();
                    }
                    for (long i = low; i <= high; i++)
                    {
                        if (!composites[(int)(i - low)])
                        {
                            WriteLine(i);
                        }
                    }
                    isFirstTest = false;
                }
            }

            static void SieveValueFromRange(BitArray composites, long offset, long low, long high, int prime)
            {
                var rem = low % prime;
                if (rem == 0)
                {
                    rem = low;
                }
                var valCur = low + prime - rem;
                if (valCur == prime)
                {
                    valCur *= 2;
                }
                var index = (int)(valCur - offset);
                while (index <= high - offset)
                {
                    composites[index] = true;
                    index += prime;
                }
            }

            void ExtendPrimeSieveList(int high)
            {
                if (high <= _highWaterMark)
                {
                    return;
                }
                var oldHighWater = _highWaterMark;
                _highWaterMark = high;

                IsCompositeList.Length = high + 1;
                var iCurPrime = 2;
                var limit = (int)(Math.Sqrt(high) + 1);
                while (iCurPrime <= limit)
                {
                    SieveValueFromRange(IsCompositeList, 0, oldHighWater, _highWaterMark, iCurPrime++);
                    while (IsCompositeList[iCurPrime] && ++iCurPrime <= high)
                    {
                    }
                }
            }

            void Sieve(BitArray composites, long low, long high)
            {
                var limit = (int)Math.Sqrt(high);
                ExtendPrimeSieveList(limit);
                var iCurPrime = 2;
                while (iCurPrime <= limit)
                {
                    SieveValueFromRange(composites, low, low, high, iCurPrime++);
                    while (iCurPrime <= _highWaterMark && IsCompositeList[iCurPrime])
                    {
                        iCurPrime++;
                    }
                }
            }

            public string RetrieveSampleInput()
            {
                return @"
3
4 6
1 10
3 5
";
            }

            public string RetrieveSampleOutput()
            {
                return @"
5

2
3
5
7

3
5
";
            }
        }
    }
}
