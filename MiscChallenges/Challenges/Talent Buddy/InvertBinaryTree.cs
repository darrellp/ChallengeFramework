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

			public void Solve()
			{
				var root = GetVal();
				var leftChildren = GetVals().ToArray();
				var rightChildren = GetVals().ToArray();
				invert_tree(root, leftChildren, rightChildren);
				Console.WriteLine();
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
