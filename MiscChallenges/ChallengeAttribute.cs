using System;

namespace MiscChallenges
{
	public class ChallengeAttribute : Attribute
	{
		public string Contest { get; private set; }
		public string Name { get; private set; }

		public ChallengeAttribute(string contest, string name)
		{
			Contest = contest;
			Name = name;
		}
	}
}