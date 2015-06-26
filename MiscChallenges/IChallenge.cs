using System.IO;

namespace MiscChallenges
{
	public interface IChallenge
	{
		string Solve(StringReader data);
		string RetrieveSampleInput();
		string RetrieveSampleOutput();
	}
}