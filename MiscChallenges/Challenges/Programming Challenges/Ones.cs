using System;
using System.Linq;
using System.Text;
using System.IO;
using static System.Console;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("ProgChallenges", "Ones",
			"https://onlinejudge.org/external/101/p10127.pdf")]
		public class Ones : IChallenge
		{
			public void Solve()
			{
				var ret = new StringBuilder();

				while (true)
                {
                    string input;
                    if ((input = ReadLine()) == null)
                    {
                        break;
                    }
					var n = int.Parse(input);
					FindOnes(n, ret);
				}
				Write(ret.ToString());
			}

			private static void FindOnes(int n, StringBuilder ret)
			{
				var cOnes = Iterate(1, i => (i * 10 + 1) % n, i => i == 0).Count();
				ret.Append(string.Format("{0}" + Environment.NewLine, cOnes));
			}

			public string RetrieveSampleInput()
			{
				return @"
3
7
9901
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
3
6
12
";
			}
		}
	}
}
