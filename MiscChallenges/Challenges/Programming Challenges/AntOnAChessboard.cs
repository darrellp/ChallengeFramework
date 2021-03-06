﻿using static System.Console;
using static System.Math;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("ProgChallenges", "Ant On a Chessboard",
			"http://www.programming-challenges.com/pg.php?page=downloadproblem&probid=111201&format=html")]
		public class AntOnAChessboard : IChallenge
		{
			public void Solve()
			{
				while (true)
				{
					int row, col;

					var nextCase = GetVal();
					if (nextCase == 0)
					{
						break;
					}

					GetValue(nextCase, out row, out col);
					WriteLine(@"{0} {1}", col, row);
				}
			}

			private static void GetValue(int nextCase, out int row, out int col)
			{
				var innerSquareRoot = (int)Floor(Sqrt(nextCase - 1));
				var innerSquare = innerSquareRoot * innerSquareRoot;

				var rest = nextCase - innerSquare;
				if (rest <= innerSquareRoot + 1)
				{
					row = rest;
					col = innerSquareRoot + 1;
				}
				else
				{
					row = innerSquareRoot + 1;
					col = 2 * innerSquareRoot + 2 - rest;
				}
				if ((innerSquareRoot & 1) == 1)
				{
					var tmp = row;
					row = col;
					col = tmp;
				}
			}

			public string RetrieveSampleInput()
			{
				return @"
8
20
25
0
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
2 3
5 4
1 5
";
			}
		}
	}
}
