using static System.Console;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        /// <summary>
        /// </summary>
        [Challenge("Sphere", "Series",
            "http://www.spoj.com/INDCNT1/problems/SEPR/")]
        public class Series : IChallenge
        {
            public void Solve()
            {
                ReadLine();
                var i = 1;
                while (true)
                {
                    var inLine = ReadLine();
                    if (inLine == null)
                    {
                        break;
                    }
                    var testVal = (long) int.Parse(inLine);
                    var output = testVal * (testVal + 1) * (testVal + 2) / 6;
                    WriteLine($"Query {i++}: {output}");
                }
            }

            public string RetrieveSampleInput()
            {
                return @"
3
1
2
5
";
            }

            public string RetrieveSampleOutput()
            {
                return @"
Query 1: 1
Query 2: 4
Query 3: 35
";
            }
        }
    }
}
