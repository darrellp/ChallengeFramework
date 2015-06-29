using System;

namespace MiscChallenges
{
	class ChallengeInfo : IComparable
	{
		internal string Name { get; private set; }
		internal string Contest { get; private set; }
		internal IChallenge Challenge { get; private set; }
		internal Uri Uri { get; private set; }

		internal ChallengeInfo(string name, string contest, IChallenge challenge, Uri uri = null)
		{
			Name = name;
			Contest = contest;
			Challenge = challenge;
			Uri = uri;
		}

		public int CompareTo(object obj)
		{
			var challengeInfo = obj as ChallengeInfo;
			if (challengeInfo == null)
			{
				return 0;
			}

			return string.Compare((Contest + Challenge), challengeInfo.Contest + challengeInfo.Challenge, StringComparison.Ordinal);
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
