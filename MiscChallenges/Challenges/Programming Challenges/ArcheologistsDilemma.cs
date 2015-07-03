using System;
using System.IO;
using System.Text;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("ProgChallenges", "Archeologist's Dilemma",
			"http://www.programming-challenges.com/pg.php?page=downloadproblem&probid=110503&format=html")]
		public class ArcheologistsDilemma : IChallenge
		{
			private readonly double _nlog10 = Math.Log(10);
			private readonly double _log2 = Math.Log(2)/Math.Log(10);

			public string Solve(StringReader str)
			{
				var ret = new StringBuilder();
				int testCase;
				int length;

				while ((testCase = NextCase(str, out length)) > 0)
				{
					SolvePuzzle(testCase, length, ret);
				}
				return ret.ToString();
			}

			static int NextCase(StringReader str, out int length)
			{
				// ReSharper disable once PossibleNullReferenceException
				var line = str.ReadLine();
				if (line == null)
				{
					length = 0;
					return -1;
				}
				length = line.Trim().Length;

				return int.Parse(line);
			}

			private void SolvePuzzle(int puzzle, int size, StringBuilder strBuilder)
			{
				var logPuzzleLow = Math.Log(puzzle) / _nlog10;
				var logPuzzleHigh = Math.Log(puzzle + 1) / _nlog10;
				var fracPuzzleLow = logPuzzleLow - Math.Truncate(logPuzzleLow);
				var fracPuzzleHigh = logPuzzleHigh - Math.Truncate(logPuzzleHigh);
				// ReSharper disable once CompareOfFloatsByEqualityOperator
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
						var cDigits = (int)Math.Floor(exp * _log2) + 1;
						if (cDigits >= 2 * size + 1)
						{
							strBuilder.Append(exp + Environment.NewLine);
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
				// We eliminate this first newline in the caller so that the uninterrupted input
				// can go at the left hand column.
				return @"
1
2
10
";
			}

			public string RetrieveSampleOutput()
			{
				// Caller will eliminate first newline...
				return @"
7
8
20
";
			}
		}
	}
}