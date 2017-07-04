using static System.Console;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        /// <summary>
        /// </summary>
        [Challenge("Sphere", "Life, the Universe and Everything",
            "http://www.spoj.com/problems/TEST/")]
        // ReSharper disable once InconsistentNaming
        public class LifeTheUniverseAndEverything : IChallenge
        {
            public void Solve()
            {
                while (true)
                {
                    var inLine = ReadLine();
                    if (inLine == null || inLine == "42")
                    {
                        break;
                    }
                    WriteLine(inLine);
                }
            }

            public string RetrieveSampleInput()
            {
                return @"
1
2
88
42
99
";
            }

            public string RetrieveSampleOutput()
            {
                return @"
1
2
88
";
            }
        }
    }
}
