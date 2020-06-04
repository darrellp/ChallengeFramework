using System;

namespace ChallengesNetStandard
{
    public class ChallengeInfoStd : IComparable
	{
		public string Name { get; }
        public string Contest { get; }
        public IChallenge Challenge { get; }
        public Uri Uri { get; private set; }

		public ChallengeInfoStd(string name, string contest, IChallenge challenge, Uri uri = null)
		{
			Name = name;
			Contest = contest;
			Challenge = challenge;
			Uri = uri;
		}

		public int CompareTo(object obj)
		{
			var challengeInfo = obj as ChallengeInfoStd;
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
