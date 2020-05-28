using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using static System.Console;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("ProgChallenges", "Graphical Editor",
			"https://onlinejudge.org/index.php?option=onlinejudge&Itemid=8&page=show_problem&problem=1208")]
		public class GraphEd : IChallenge
		{
			List<List<char>> _image;

			public void Solve()
			{
				_image = new List<List<char>>();
				var ret = new StringBuilder();
				while (true)
				{
					if (!Execute(ReadLine(), ret))
					{
						break;
					}
				}
				Write(ret.ToString());
			}

			private readonly Dictionary<char, Func<string[], StringBuilder, List<List<char>>, bool>> _lookup = new Dictionary
				<char, Func<string[], StringBuilder, List<List<char>>, bool>>
			{
				{'I', New},
				{'C', Clear},
				{'L', ColorPixel},
				{'V', VerticalSegment},
				{'H', HorizontalSegment},
				{'K', DrawRectangle},
				{'F', Fill},
				{'S', Save},
				{'X', Exit}
			};

			private static bool ColorPixel(string[] parms, StringBuilder sb, List<List<char>> image)
			{
				var col = int.Parse(parms[0]) - 1;
				var row = int.Parse(parms[1]) - 1;
				var color = parms[2][0];
				image[row][col] = color;

				return true;
			}

			private static bool VerticalSegment(string[] parms, StringBuilder sb, List<List<char>> image)
			{
				var col = int.Parse(parms[0]) - 1;
				var rowStart = int.Parse(parms[1]) - 1;
				var rowEnd = int.Parse(parms[2]) - 1;
				var color = parms[3][0];

				if (rowStart > rowEnd)
				{
					var t = rowStart;
					rowStart = rowEnd;
					rowEnd = t;
				}
				for (var i = rowStart; i <= rowEnd; i++)
				{
					image[i][col] = color;
				}

				return true;
			}

			private static bool HorizontalSegment(string[] parms, StringBuilder sb, List<List<char>> image)
			{
				var colStart = int.Parse(parms[0]) - 1;
				var colEnd = int.Parse(parms[1]) - 1;
				var row = int.Parse(parms[2]) - 1;
				var color = parms[3][0];

				if (colStart > colEnd)
				{
					var t = colStart;
					colStart = colEnd;
					colEnd = t;
				}
				for (var i = colStart; i <= colEnd; i++)
				{
					image[row][i] = color;
				}

				return true;
			}

			private static bool DrawRectangle(string[] parms, StringBuilder sb, List<List<char>> image)
			{
				var colStart = int.Parse(parms[0]) - 1;
				var colEnd = int.Parse(parms[1]) - 1;
				var rowStart = int.Parse(parms[2]) - 1;
				var rowEnd = int.Parse(parms[3]) - 1;
				var color = parms[4][0];

				if (colStart > colEnd)
				{
					var t = colStart;
					colStart = colEnd;
					colEnd = t;
				}
				if (rowStart > rowEnd)
				{
					var t = rowStart;
					rowStart = rowEnd;
					rowEnd = t;
				}
				for (var iCol = colStart; iCol <= colEnd; iCol++)
				{
					for (var iRow = rowStart; iRow <= rowEnd; iRow++)
						image[iRow][iCol] = color;
				}

				return true;
			}

			private static bool Fill(string[] parms, StringBuilder sb, List<List<char>> image)
			{
				var pCol = int.Parse(parms[0]) - 1;
				var pRow = int.Parse(parms[1]) - 1;
				var color = parms[2][0];
				FillRegion(pCol, pRow, color, image);
				return true;
			}

			private static void FillRegion(int pCol, int pRow, char color, List<List<char>> image)
			{
				var originalColor = image[pRow][pCol];
				if (color == originalColor)
				{
					return;
				}
				image[pRow][pCol] = color;
				if (ShouldFill(image, pCol + 1, pRow, originalColor))
				{
					FillRegion(pCol + 1, pRow, color, image);
				}
				if (ShouldFill(image, pCol, pRow + 1, originalColor))
				{
					FillRegion(pCol, pRow + 1, color, image);
				}
				if (ShouldFill(image, pCol - 1, pRow, originalColor))
				{
					FillRegion(pCol - 1, pRow, color, image);
				}
				if (ShouldFill(image, pCol, pRow - 1, originalColor))
				{
					FillRegion(pCol, pRow - 1, color, image);
				}
			}

			private static bool ShouldFill(List<List<char>> image, int pCol, int pRow, char originalColor)
			{
				return
					pCol >= 0 &&
					pCol < image[0].Count() &&
					pRow >= 0 &&
					pRow < image.Count() &&
					image[pRow][pCol] == originalColor;
			}

			private static bool Save(string[] parms, StringBuilder sb, List<List<char>> image)
			{
				var name = parms[0];
				var rows = image.Count();
				var cols = image[0].Count();

				sb.Append(name + Environment.NewLine);
				for (var iRow = 0; iRow < rows; iRow++)
				{
					for (var iCol = 0; iCol < cols; iCol++)
					{
						sb.Append(image[iRow][iCol]);
					}
					sb.Append(Environment.NewLine);
				}
				return true;
			}

			private static bool Exit(string[] parms, StringBuilder sb, List<List<char>> image)
			{
				return false;
			}

			private static bool Clear(string[] parms, StringBuilder sb, List<List<char>> image)
			{
				var cols = image[0].Count();
				var rows = image.Count();

				for (var iRow = 0; iRow < rows; iRow++)
				{
					image[iRow] = Enumerable.Repeat('O', cols).ToList();
				}
				return true;
			}

			private static bool New(string[] parms, StringBuilder sb, List<List<char>> image)
			{
				var cols = int.Parse(parms[0]);
				var rows = int.Parse(parms[1]);

				for (var iRow = 0; iRow < rows; iRow++)
				{
					var thisRow = new List<char>();
					image.Add(thisRow);
					for (var iCol = 0; iCol < cols; iCol++)
					{
						thisRow.Add('O');
					}
				}
				return true;
			}

			private bool Execute(string cmd, StringBuilder sb)
			{
				var parts = cmd.Split(new[] { ' ' });
				var cmdLetter = parts[0][0];
				if (_lookup.ContainsKey(cmdLetter))
				{
					return _lookup[cmdLetter](parts.Skip(1).ToArray(), sb, _image);
				}
				return true;
			}

			public string RetrieveSampleInput()
			{
				return @"
I 5 6
L 2 3 A
S one.bmp
G 2 3 J
F 3 3 J
V 2 3 4 W
H 3 4 2 Z
S two.bmp
X
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
one.bmp
OOOOO
OOOOO
OAOOO
OOOOO
OOOOO
OOOOO
two.bmp
JJJJJ
JJZZJ
JWJJJ
JWJJJ
JJJJJ
JJJJJ
";
			}
		}
	}
}
