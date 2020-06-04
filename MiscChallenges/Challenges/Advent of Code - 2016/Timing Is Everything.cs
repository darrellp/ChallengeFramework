using System.Collections.Generic;
using System.Text.RegularExpressions;
using RegexStringLibrary;
using NumberTheoryLong;
using static System.Console;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("Advent of Code 2016", "Timing Is Everything (15)", "https://adventofcode.com/2016/day/15")]
        public class TimingIsEverything : IChallenge
        {
            public void Solve()
            {
                string nextLine;
                var regexPattern =
                    Stex.Begin +
                    "Disc #" +
                    Stex.Integer("Index") +
                    " has " +
                    Stex.Integer("Positions") +
                    " positions; at time=0, it is at position " +
                    Stex.Integer("Start") +
                    "." +
                    Stex.End;
                var rgx = new Regex(regexPattern);

                var a = new List<long>();
                var mods = new List<long>();

                while ((nextLine = ReadLine()) != null)
                {
                    var match = rgx.Match(nextLine);
                    var index = int.Parse(match.Groups["Index"].Value);
                    var mod = int.Parse(match.Groups["Positions"].Value);
                    var start = int.Parse(match.Groups["Start"].Value);
                    a.Add((100 * mod - start - index) % mod);
                    mods.Add(mod);
                }
                WriteLine(ChineseRemainder.CRT(a.ToArray(), mods.ToArray()));
                a.Add(11 - a.Count - 1);
                mods.Add(11);
                WriteLine(ChineseRemainder.CRT(a.ToArray(), mods.ToArray()));
            }

			public string RetrieveSampleInput()
            {
                return @"
Disc #1 has 13 positions; at time=0, it is at position 11.
Disc #2 has 5 positions; at time=0, it is at position 0.
Disc #3 has 17 positions; at time=0, it is at position 11.
Disc #4 has 3 positions; at time=0, it is at position 0.
Disc #5 has 7 positions; at time=0, it is at position 2.
Disc #6 has 19 positions; at time=0, it is at position 17.
";
            }

            public string RetrieveSampleOutput()
            {
                return null;
            }
        }
	}
}
