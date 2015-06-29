using System;

namespace MiscChallenges
{
	public class ChallengeAttribute : Attribute
	{
		public string Contest { get; private set; }
		public string Name { get; private set; }
		public Uri URI { get; private set; }

		public ChallengeAttribute(string contest, string name, string uriString = null)
		{
			Contest = contest;
			Name = name;
			URI = uriString == null ? null : new Uri(uriString);
		}
	}
}