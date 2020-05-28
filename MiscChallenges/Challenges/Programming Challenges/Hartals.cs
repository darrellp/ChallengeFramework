using System.Collections.Generic;
using System;
using System.Text;
using static System.Console;


namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("ProgChallenges", "Hartals",
			"https://onlinejudge.org/index.php?option=onlinejudge&Itemid=8&page=show_problem&problem=991")]
		public class Hartals : IChallenge
		{
			public void Solve()
			{
				var ret = new StringBuilder();
				var cCases = int.Parse(ReadLine());
				for (var iCase = 0; iCase < cCases; iCase++)
				{
					var solver = new HartalSolver();
					ret.Append(solver.LostDays() + Environment.NewLine);
				}
				Write(ret.ToString());
			}

			public string RetrieveSampleInput()
			{
				return @"
2
14
3
3
4
8
100
4
12
15
25
40
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
5
15
";
			}

			class HartalSolver
			{
				private readonly int _simulationDays;
				private readonly List<int> _periods = new List<int>();

				internal HartalSolver()
				{
					_simulationDays = int.Parse(ReadLine());
					var periodCount = int.Parse(ReadLine());
					for (var i = 0; i < periodCount; i++)
					{
						_periods.Add(int.Parse(ReadLine()));
					}
				}

				internal int LostDays()
				{
					var lostDays = new HashSet<int>();

					foreach (var period in _periods)
					{
						for (var hartalDay = period; hartalDay <= _simulationDays; hartalDay += period)
						{
							var dow = hartalDay % 7;
							if (dow != 0 && dow != 6)
							{
								lostDays.Add(hartalDay);
							}
						}
					}
					return lostDays.Count;
				}
			}
		}
	}
}
