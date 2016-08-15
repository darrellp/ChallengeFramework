using static System.Console;
using static System.Math;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("ProgChallenges", "Archeologist's Dilemma",
			"http://www.programming-challenges.com/pg.php?page=downloadproblem&probid=110503&format=html")]
		public class ArcheologistsDilemma : IChallenge
		{
			private readonly double _nlog10 = Log(10);
			private readonly double _log2 = Log(2)/ Log(10);

			public void Solve()
			{
				int testCase;
				int length;

				while ((testCase = NextCase(out length)) > 0)
				{
					SolvePuzzle(testCase, length);
				}
			}

			static int NextCase(out int length)
			{
				// ReSharper disable once PossibleNullReferenceException
				var line = ReadLine();
				if (line == null)
				{
					length = 0;
					return -1;
				}
				length = line.Trim().Length;

				return int.Parse(line);
			}

			private void SolvePuzzle(int puzzle, int size)
			{
				var logPuzzleLow = Log(puzzle) / _nlog10;
				var logPuzzleHigh = Log(puzzle + 1) / _nlog10;

                // We need to find a power of 2 whose base 10 log has a fractional part
                // between fracPuzzleLow and fracPuzzleHigh
				var fracPuzzleLow = logPuzzleLow - Truncate(logPuzzleLow);
				var fracPuzzleHigh = logPuzzleHigh - Truncate(logPuzzleHigh);
				// ReSharper disable once CompareOfFloatsByEqualityOperator
                // fracPuzzleHigh always has to be larger than fracPuzzleLow...
				if (fracPuzzleHigh == 0.0)
				{
					fracPuzzleHigh = 1.0;
				}
				var fracCur = 0.0;

				// Smallest number possible is three digits (i.e., 2^7=128) since our input will be at least one digit.
				for (var exp = 0; ; exp++)
				{
					if (fracPuzzleLow <= fracCur && fracCur < fracPuzzleHigh)
					{
						var cDigits = (int)Floor(exp * _log2) + 1;
						if (cDigits >= 2 * size + 1)
						{
                            WriteLine(exp);
							return;
						}
					}
					fracCur = fracCur + _log2;
					if (fracCur >= 1.0)
					{
						fracCur -= 1.0;
					}
				}
			}

			public string RetrieveSampleInput()
			{
				return @"
1
2
10
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
7
8
20
";
			}
		}
	}
}