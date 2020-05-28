using System.Numerics;
using System;
using System.Linq;
using System.Text;
using static System.Console;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("ProgChallenges", "How Many Fibs?",

			"https://onlinejudge.org/index.php?option=onlinejudge&page=show_problem&problem=1124")]
		public class HowManyFibs : IChallenge
		{
			public void Solve()
			{
				var ret = new StringBuilder();
				while (true)
				{
					var solver = new FibSolver();
					var solution = solver.Fibs();
					if (solution < 0)
					{
						break;
					}
					ret.Append(solution + Environment.NewLine);
				}
				Write(ret.ToString());
			}

			public string RetrieveSampleInput()
			{
				return @"
10 100
1234567890 9876543210
0 0
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
5
4
";
			}

			class FibSolver
			{
				private static readonly double Sqrt5 = Math.Sqrt(5.0);
				private static readonly double Phi = (1 + Sqrt5) / 2;
				private static readonly double LnPhi = Math.Log(Phi);
				private static readonly double LnSqrt5 = Math.Log(5.0) / 2;

				private readonly BigInteger _low;
				private readonly BigInteger _high;

				internal FibSolver()
				{
					// ReSharper disable once PossibleNullReferenceException
					var line = ReadLine().Split(' ').
						Select(BigInteger.Parse).
						ToList();
					_low = line[0];
					_high = line[1];
				}

				private static Double IndexFromFib(BigInteger fib)
				{
					return (BigInteger.Log(fib) + LnSqrt5) / LnPhi;
				}

				private static BigInteger FibFromIndex(int index)
				{
					return new BigInteger(Math.Pow(Phi, index) / Sqrt5 + 0.5);
				}

				internal int Fibs()
				{
					if (_low == 0 && _high == 0)
					{
						return -1;
					}
					var lowIndex = (int)Math.Floor(IndexFromFib(_low));
					var highIndex = (int)Math.Ceiling(IndexFromFib(_high));
					if (FibFromIndex(lowIndex) < _low)
					{
						lowIndex++;
					}
					if (FibFromIndex(highIndex) > _high)
					{
						highIndex--;
					}
					return highIndex - lowIndex + 1;
				}
			}
		}
	}
}
