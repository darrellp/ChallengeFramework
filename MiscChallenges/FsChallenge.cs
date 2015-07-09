using System.IO;

namespace MiscChallenges
{
	class FsChallenge : IChallenge
	{
		private FS_Challenges.IChallenge _fsChallenge;
		public FsChallenge(FS_Challenges.IChallenge fsChallenge)
		{
			_fsChallenge = fsChallenge;
		}

		public void Solve()
		{
			_fsChallenge.Solve();
		}

		public string RetrieveSampleInput()
		{
			return _fsChallenge.RetrieveSampleInput();
		}

		public string RetrieveSampleOutput()
		{
			return _fsChallenge.RetrieveSampleOutput();
		}
	}
}
