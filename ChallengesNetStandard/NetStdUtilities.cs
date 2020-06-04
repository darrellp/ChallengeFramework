using System;
using System.Collections.Generic;
using System.Linq;
using ChallengesNetStandard.Challenges;

namespace ChallengesNetStandard
{
	public static class NetStdUtilities
	{
        public static List<ChallengeInfoStd> RetrieveChallenges()
        {
            var challenges = (typeof(ChallengeClass)).GetNestedTypes().Select(MethodTest).Where(t => t != null)
                .ToList();
            return challenges;
        }

        private static ChallengeInfoStd MethodTest(Type member)
        {
            return member.
                GetCustomAttributes(true).
                OfType<ChallengeAttribute>().
                Select(t => new ChallengeInfoStd(
                    t.Name,
                    t.Contest,
                    (IChallenge)Activator.CreateInstance(member),
                    t.URI)).
                FirstOrDefault();
        }
    }
}
