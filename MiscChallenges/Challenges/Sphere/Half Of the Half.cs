using static System.Console;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        /// <summary>
        /// </summary>
        [Challenge("Sphere", "Half of the Half",
            "http://www.spoj.com/problems/STRHH/")]
        public class HalfOfTheHalf : IChallenge
        {
            public void Solve()
            {
                ReadLine();
                string inLine;
                while ((inLine = ReadLine()) != null)
                {
                    for (int i = 0; i < inLine.Length / 2; i += 2)
                    {
                        Write(inLine[i]);
                    }
                    WriteLine();
                }
            }

            public string RetrieveSampleInput()
            {
                return @"
4
your 
progress 
is 
noticeable
";
            }

            public string RetrieveSampleOutput()
            {
                return @"
y
po
i
ntc
";
            }
        }
    }
}
