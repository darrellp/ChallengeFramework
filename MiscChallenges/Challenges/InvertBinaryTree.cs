using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Markup;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("TalentBuddy", "Invert Binary Tree", "https://www.talentbuddy.co/challenge/5580a44e379d4f5893b7397e")]
		// ReSharper disable once InconsistentNaming
		public class InvertBinaryTree : IChallenge
		{
			// ReSharper disable once UnusedParameter.Local
			public void invert_tree(int root, int[] leftChild, int[] rightChild)
			{
				PrintTree(root, leftChild, rightChild, false);
			}

			private void PrintTree(int root, int[] leftChild, int[] rightChild, bool precedingSpace = true)
			{
				if (root == 0)
				{
					return;
				}
				if (precedingSpace)
				{
					Console.Write(' ');
				}
				Console.Write(root);
				PrintTree(rightChild[root], leftChild, rightChild);
				PrintTree(leftChild[root], leftChild, rightChild);
			}

			public string Solve(StringReader stm)
			{
				using (var stream = new MemoryStream())
				{
					using (var output = new StreamWriter(stream, Encoding.UTF8))
					{
						Console.SetOut(output);
						var root = GetVal(stm);
						var leftChildren = GetVals(stm).ToArray();
						var rightChildren = GetVals(stm).ToArray();
						invert_tree(root, leftChildren, rightChildren);
						output.Flush();
						stream.Position = 0;
						using (var streamReader = new StreamReader(stream))
						{
							return streamReader.ReadToEnd() + Environment.NewLine;
						}
					}
				}
			}

			public string RetrieveSampleInput()
			{
				return @"
3
0 0 0 2 1 0
0 0 0 4 5 0
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
3 4 5 1 2
";
			}
		}
	}
}
