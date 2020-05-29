using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Console;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("ProgChallenges", "Knights of the Round Table",
			"https://onlinejudge.org/external/13/1364.pdf")]
		public class KinghtsOfTheRoundTable : IChallenge
		{
			public void Solve()
			{
				var sbRet = new StringBuilder();
				List<double> vals;

				while ((vals = GetDblVals()) != null)
				{
					sbRet.Append(SolveCase(vals) + Environment.NewLine);
				}
				Write(sbRet.ToString());
			}

			private string SolveCase(List<double> vals)
			{
				var sp = vals.Sum() / 2.0;
				var triangleArea = Math.Sqrt(
					sp *
					(sp - vals[0]) *
					(sp - vals[1]) *
					(sp - vals[2]));
				return "The radius of the round table is: " + (triangleArea / sp).ToString("F3");
			}

			public string RetrieveSampleInput()
			{
				return @"
12.0 12.0 8.0
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
The radius of the round table is: 2.828
";
			}
		}
	}
}
