using System;
using System.Globalization;
using System.Linq;
using System.Text;
using static System.Console;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("ProgChallenges", "Primary Arithmetic",
			"https://onlinejudge.org/external/100/10035.pdf")]
		public class PrimaryArithmetic : IChallenge
		{
			public void Solve()
			{
				var ret = new StringBuilder();
				while (true)
				{
					// ReSharper disable once PossibleNullReferenceException
					var input = ReadLine().
						Split(new[] { ' ' }).
						Select(s => new string(s.Reverse().ToArray())).
						ToList();

					if (input[0] == "0" && input[1] == "0")
					{
						break;
					}

					var carryCur = 0;
					var cCarries = 0;

					for (var ich = 0; ich < Math.Max(input[0].Length, input[1].Length); ich++)
					{
						var digit0 = (ich < input[0].Length ? input[0][ich] : '0') - '0';
						var digit1 = (ich < input[1].Length ? input[1][ich] : '0') - '0';

						carryCur = digit0 + digit1 + carryCur > 9 ? 1 : 0;
						cCarries += carryCur;
					}

					var quantity = cCarries == 0 ? "No" : cCarries.ToString(CultureInfo.InvariantCulture);
					ret.Append(string.Format("{0} carry operation{1}." + Environment.NewLine,
						quantity,
						cCarries > 1 ? "s" : string.Empty));
				}
				Write(ret.ToString());
			}

			public string RetrieveSampleInput()
			{
				return @"
123 456
555 555
123 594
999 1
0 0
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
No carry operation.
3 carry operations.
1 carry operation.
3 carry operations.
";
			}
		}
	}
}
