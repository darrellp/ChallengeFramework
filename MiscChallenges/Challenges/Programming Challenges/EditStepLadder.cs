using System;
using System.Collections.Generic;
using System.IO;
using static System.Console;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("ProgChallenges", "Edit Step Ladder",
            "https://onlinejudge.org/external/100/10029.pdf")]
		public class EditStepLadder : IChallenge
		{
			public void Solve()
			{
				var words = new List<StepLadderVertex>();
				// iLongest keeps track of where the longest ladder ends.
				// That, combined with the back pointers in StepLadderVertex
				// will allow us to reconstruct the ladder itself.  We
				// aren't asked to do that by the challenge but I keep this
				// info just so it's there if I ever decided to use this for
				// "real" work.
				// ReSharper disable once NotAccessedVariable
				var iLongest = -1;
				var longestPath = 0;

				while (true)
				{
					var curWord = ReadLine();
					if (string.IsNullOrEmpty(curWord))
					{
						break;
					}

					var wordVtx = new StepLadderVertex(curWord);
					wordVtx.FindLongestPathTo(curWord, words);
					if (wordVtx.LongestPath > longestPath)
					{
						longestPath = wordVtx.LongestPath;
						// ReSharper disable once RedundantAssignment
						iLongest = words.Count - 1;
					}
				}
				Write(longestPath.ToString() + Environment.NewLine);
			}

			public string RetrieveSampleInput()
			{
				return @"
cat
dig
dog
fig
fin
fine
fog
log
wine
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
5
";
			}

			internal class StepLadderVertex
			{
				private readonly string _word;

				public StepLadderVertex(string word)
				{
					PrevWordIndex = -1;
					LongestPath = 1;
					_word = word;
				}

				public int LongestPath { get; set; }

				// ReSharper disable once UnusedAutoPropertyAccessor.Global
				public int PrevWordIndex { get; set; }

				public void FindLongestPathTo(string word, List<StepLadderVertex> priorWords)
				{
					for (var iWord = 0; iWord < priorWords.Count; iWord++)
					{
						var wordVertex = priorWords[iWord];
						if (wordVertex._word == word)
						{
							return;
						}
						if (IsAdjacent(wordVertex._word, word) && wordVertex.LongestPath >= LongestPath)
						{
							LongestPath = wordVertex.LongestPath + 1;
							PrevWordIndex = iWord;
						}
					}
					priorWords.Add(this);
				}

				internal static bool IsAdjacent(string word1, string word2)
				{
					if (Math.Abs(word1.Length - word2.Length) > 1)
					{
						return false;
					}
					var iFirstDif = 0;
					for (; iFirstDif < Math.Min(word1.Length, word2.Length); iFirstDif++)
					{
						if (word1[iFirstDif] != word2[iFirstDif])
						{
							break;
						}
					}
					var iLastDif1 = word1.Length - 1;
					var iLastDif2 = word2.Length - 1;
					for (; iLastDif1 >= 0 && iLastDif2 >= 0; iLastDif1--, iLastDif2--)
					{
						if (word1[iLastDif1] != word2[iLastDif2])
						{
							break;
						}
					}
					return iFirstDif >= iLastDif1;
				}
			}
		}
	}
}
