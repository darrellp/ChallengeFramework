using System.Text;
using System.Linq;
using System.Security.Cryptography;
using static System.Console;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        // Warning: This one took almost 3 minutes the last time I ran it.
        [Challenge("Advent of Code 2016", "How About a Nice Game of Chess? (5)", "https://adventofcode.com/2016/day/5")]
        public class HowAboutANiceGameOfChess : IChallenge
        {
            public void Solve()
            {
                var prefix = "ffykfhsq";
                var initVals = Enumerable
                    .Range(0, int.MaxValue)
                    .Select(n => new {n, v = CalculateMd5Hash(prefix + n) })
                    .Where(s => s.v.StartsWith("00000"))
                    .Take(8)
                    .ToArray();

                var chars = initVals.
                    Select(t => t.v[5])
                    .ToArray();

                WriteLine(new string(chars));

                char[] charsArray = "        ".ToCharArray();

                var interesting = initVals
                    .Select(t => t.v).Concat(Enumerable
                        .Range(initVals[7].n + 1, 100000000)
                        .Select(n => CalculateMd5Hash(prefix + n))
                        .Where(s => s.StartsWith("00000")))
                    .Where(s => '0' <= s[5] && s[5] < '8' && charsArray[s[5] - '0'] == ' ');

                foreach (var val in interesting)
                {
                    charsArray[val[5] - '0'] = val[6];
                    if (charsArray.All(c => c != ' '))
                    {
                        break;
                    }
                }

                WriteLine(new string(charsArray));
            }

            public string RetrieveSampleInput()
            {
                return @"
";
            }

            public string RetrieveSampleOutput()
            {
                return null;
            }
        }
    }
}
