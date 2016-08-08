using System;
using System.Collections.Generic;
using System.Linq;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("Code Chef", "Drumbf For President", "https://www.codechef.com/problems/STUDVOTE")]
        public class DrumbfForPresident : IChallenge
        {
            public void Solve()
            {
                var cCases = GetVal();

                for (int iCase = 0; iCase < cCases; iCase++)
                {
                    var parmVals = GetVals();
                    var votes = GetVals();

                    SolveCase(parmVals[0], parmVals[1], votes);
                }
            }

            private void SolveCase(int cStudents, int cRequiredVotes, List<int> votes)
            {
                var voteCounts = new int[cStudents];

                for (var iStudent = 0; iStudent < cStudents; iStudent++)
                {
                    voteCounts[votes[iStudent] - 1]++;
                }
                var cStudentGov = Enumerable.
                    Range(0, cStudents).
                    Count(i => voteCounts[i] >= cRequiredVotes && votes[i] - 1 != i);
                Console.WriteLine(cStudentGov);
            }

            public string RetrieveSampleInput()
            {
                return @"
2
3 2
2 1 2
2 1
1 2
";
            }

            public string RetrieveSampleOutput()
            {
                return @"
1
0
";
            }
        }
    }
}
