using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static System.Console;

namespace MiscChallenges.Challenges
{ 
	public static partial class ChallengeClass
	{
        [Challenge("Euler Project", "Prob 43",
            "https://projecteuler.net/problem=43")]
        public class Euler43 : IChallenge
        {
            public void Solve()
            {
                WriteLine(Check17().
                    SelectMany(s => Check(s, 13)).
                    SelectMany(s => Check(s, 11)).
                    SelectMany(s => Check(s, 7)).
                    SelectMany(s => Check(s, 5)).
                    SelectMany(s => Check(s, 3)).
                    SelectMany(s => Check(s, 2)).
                    SelectMany(s => Check(s, 1)).
                    Select(long.Parse).
                    Sum());
            }

		    static IEnumerable<string> Check17()
		    {
			    for (var i = ((100 / 17) + 1) * 17; i < 1000; i += 17 )
			    {
				    var stringValue = i.ToString(CultureInfo.InvariantCulture);
				    if ((new HashSet<char>(stringValue)).Count != 3)
				    {
					    continue;
				    }
				    yield return i.ToString(CultureInfo.InvariantCulture);
			    }
		    }

		    static IEnumerable<string> Check(string invalue, int checkVal)
		    {
			    var test = int.Parse(invalue.Substring(0, 2));
			    var digitValue = -100;
			    for (var digit = '0'; digit <= '9'; digit++)
			    {
				    digitValue += 100;
				    if (invalue.Contains(digit))
				    {
					    continue;
				    }
				    var newTest = digitValue + test;
				    if (newTest % checkVal == 0)
				    {
					    yield return digit + invalue;
				    }
			    }
		    }

            public string RetrieveSampleInput() { return null; }
            public string RetrieveSampleOutput()
            {
                return @"
16695334890
";
            }

		}
	}
}
