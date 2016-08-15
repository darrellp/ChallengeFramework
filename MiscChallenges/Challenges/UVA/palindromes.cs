using System.Collections.Generic;
using static System.Console;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		/// <summary>
		/// </summary>
		[Challenge("UVA", "Palindromes",
			"https://uva.onlinejudge.org/index.php?option=com_onlinejudge&Itemid=8&category=6&page=show_problem&problem=342")]
		// ReSharper disable once InconsistentNaming
		public class Palindromes : IChallenge
		{
			private enum PalType
			{
				NotPal,
				Regular,
				Mirrored,
				MirroredPal
			};

			private readonly Dictionary<char, char> _letterMirrors = new Dictionary<char, char>
			{
				{'A', 'A'}, {'E', '3'}, {'H', 'H'}, {'I', 'I'},
				{'J', 'L'}, {'L', 'J'}, {'Z', '5'}, {'O', 'O'},
				{'S', '2'}, {'T', 'T'}, {'U', 'U'}, {'V', 'V'},
				{'W', 'W'}, {'X', 'X'}, {'Y', 'Y'}, {'1', '1'},
				{'2', 'S'}, {'3', 'E'}, {'5', 'Z'}, {'8', '8'}
			};

			public void Solve()
			{
				string nextString;
				while ((nextString = ReadLine()) != null)
				{
					var tag = string.Empty;
					switch (CheckPalType(nextString))
					{
						case PalType.NotPal:
							tag = "not a palindrome";
							break;

						case PalType.Regular:
							tag = "a regular palindrome";
							break;

						case PalType.Mirrored:
							tag = "a mirrored string";
							break;

						case PalType.MirroredPal:
							tag = "a mirrored palindrome";
							break;
					}
                    WriteLine(@"{0} -- is {1}.", nextString, tag);
                    // Because of the weird "after each output line, you must print an empty line"
                    // requirement.  I'm interepreting this literally - i.e., EACH output line,
                    // including the last one.
                    WriteLine();
				}
			}

			private PalType CheckPalType(string s)
			{
				bool fIsMirrored = true;
				bool fIsRegular = true;
				for (var ich = 0; ich < s.Length/2; ich++)
				{
					var ichBack = s.Length - ich - 1;
					if (s[ichBack] != s[ich])
					{
						fIsRegular = false;
					}
					if (!_letterMirrors.ContainsKey(s[ich]) || s[ichBack] != _letterMirrors[s[ich]])
					{
						fIsMirrored = false;
					}
					if (!fIsMirrored && !fIsRegular)
					{
						return PalType.NotPal;
					}
				}
				if (fIsMirrored && fIsRegular)
				{
					return PalType.MirroredPal;
				}
				return fIsMirrored ? PalType.Mirrored : PalType.Regular;
			}

			public string RetrieveSampleInput()
			{
				return @"
NOTAPALINDROME
ISAPALINILAPASI
2A3MEAS
ATOYOTA
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
NOTAPALINDROME -- is not a palindrome.

ISAPALINILAPASI -- is a regular palindrome.

2A3MEAS -- is a mirrored string.

ATOYOTA -- is a mirrored palindrome.

";
			}
		}
	}
}
