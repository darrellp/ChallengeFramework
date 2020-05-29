using System;
using System.Linq;
using System.Text;
using static System.Console;


namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("ProgChallenges", "MineSweeper",
			"https://onlinejudge.org/external/101/10189.pdf")]
		public class MineSweeper : IChallenge
		{
			public void Solve()
			{
				var ret = new StringBuilder();
				var firstTest = true;
				for (var i = 1; ; i++)
				{
					var nextTest = GetTest();
					if (nextTest == null)
					{
						break;
					}
					if (!firstTest)
					{
						ret.Append(Environment.NewLine);
					}
					ret.Append(SolveTest(nextTest, i));
					firstTest = false;
				}
				Write(ret.ToString());
			}

			private string SolveTest(Test test, int iTest)
			{
				var sb = new StringBuilder("Field #" + iTest + ":" + Environment.NewLine);
				sb.Append(test.GetBoard());
				return sb.ToString();
			}

			private static Test GetTest()
			{
				// ReSharper disable PossibleNullReferenceException
				var dims = ReadLine().Split(' ').
					Select(int.Parse).
					ToList();
				// ReSharper restore PossibleNullReferenceException

				if (dims[0] == 0 && dims[1] == 0)
				{
					return null;
				}

				var mines = new bool[dims[0]][];

				for (var i = 0; i < dims[0]; i++)
				{
					// ReSharper disable AssignNullToNotNullAttribute
					mines[i] =
						ReadLine().
						Select(ch => ch == '*').
						ToArray();
					// ReSharper restore AssignNullToNotNullAttribute
				}
				return new Test(dims[0], dims[1], mines);
			}

			internal class Test
			{
				public int Rows { get; private set; }
				public int Cols { get; private set; }
				public bool[][] Mines { get; private set; }

				public Test(int rows, int cols, bool[][] mines)
				{
					Rows = rows;
					Cols = cols;
					Mines = mines;
				}

				public string GetBoard()
				{
					var sb = new StringBuilder();
					for (var iRow = 0; iRow < Rows; iRow++)
					{
						sb.Append(new string(Enumerable.Range(0, Cols).Select(c => CountAt(iRow, c)).ToArray()));
						sb.Append(Environment.NewLine);
					}
					return sb.ToString();
				}

				private char CountAt(int row, int col)
				{
					var count = '0';

					if (Mines[row][col])
					{
						return '*';
					}
					for (var iRow = row - 1; iRow <= row + 1; iRow++)
					{
						if (iRow < 0 || iRow >= Rows)
						{
							continue;
						}
						for (var iCol = col - 1; iCol <= col + 1; iCol++)
						{
							if (iCol < 0 || iCol >= Cols)
							{
								continue;
							}
							count += (char)(Mines[iRow][iCol] ? 1 : 0);
						}
					}

					return count;
				}
			}
			public string RetrieveSampleInput()
			{
				return @"
4 4
*...
....
.*..
....
3 5
**...
.....
.*...
0 0
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
Field #1:
*100
2210
1*10
1110

Field #2:
**100
33200
1*100
";
			}
		}
	}
}
