using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        /// <summary>
        /// </summary>
        [Challenge("Sphere", "Adding Reversed Numbers", "http://www.spoj.com/problems/ADDREV/")]
        // ReSharper disable once InconsistentNaming
        public class AddingReversedNumbers : IChallenge
        {
            public void Solve()
            {
                ReadLine();
                List<int> nextVals;
                while ((nextVals = GetVals()) != null)
                {
                    var v1 = int.Parse(new string(nextVals[0].ToString().Reverse().ToArray()));
                    var v2 = int.Parse(new string(nextVals[1].ToString().Reverse().ToArray()));
                    var answer = int.Parse(new string((v1 + v2).ToString().Reverse().ToArray()));

                    WriteLine(answer);
                }
            }

            public string RetrieveSampleInput()
            {
                return @"
3
24 1
4358 754
305 794
";
            }

            public string RetrieveSampleOutput()
            {
                return @"
34
1998
1
";
            }
        }
    }
}
