using System.IO;

namespace MiscChallenges
{
	public interface IChallenge
	{
		void Solve();
		string RetrieveSampleInput();
		string RetrieveSampleOutput();
	}
}