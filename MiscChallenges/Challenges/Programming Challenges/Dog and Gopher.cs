using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("ProgChallenges", "Dog and Gopher",
            "https://onlinejudge.org/index.php?option=onlinejudge&Itemid=8&page=show_problem&problem=1251")]
		public class DogAndGopher : IChallenge
		{
			public void Solve()
			{
				var sbRet = new StringBuilder();

				DgSet dgSet;
				bool firstCase = true;

				while ((dgSet = DgSet.ReadSet(firstCase)) != null)
				{
					firstCase = false;
					sbRet.Append(dgSet.Solve() + Environment.NewLine);
				}
				Write(sbRet.ToString());
			}

			public class DgSet
			{
                private PointDbl Dog { get; }
                private PointDbl Gopher { get; }
                private List<PointDbl> Holes { get; }

                private DgSet(PointDbl dog, PointDbl gopher, List<PointDbl> holes)
				{
					Holes = holes;
					Gopher = gopher;
					Dog = dog;
				}

				public static DgSet ReadSet(bool firstCase)
				{
					if (!firstCase)
					{
						ReadLine();
					}
					var vals = GetDblVals();
					if (vals == null)
					{
						return null;
					}
					var cHoles = (int) (vals[0] + 0.5);
					var gopherPoint = new PointDbl(vals[1], vals[2]);
					var dogPoint = new PointDbl(vals[3], vals[4]);
					var holes = new List<PointDbl>(cHoles);
					for (var iHole = 0; iHole < cHoles; iHole++)
					{
						var holeVals = GetDblVals();
						holes.Add(new PointDbl(holeVals[0], holeVals[1]));
					}
					return new DgSet(dogPoint, gopherPoint, holes);
				}

				public string Solve()
				{
					foreach (var hole in Holes)
					{
						if (hole.DistanceTo(Dog) >= 2 * hole.DistanceTo(Gopher))
						{
							return "The gopher can escape through the hole at " + hole + ".";
						}
					}
					return "The gopher cannot escape.";
				}
			}

			public string RetrieveSampleInput()
			{
				return @"
1 1.000 1.000 2.000 2.000
1.500 1.500

2 2.000 2.000 1.000 1.000
1.500 1.500
2.500 2.500
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
The gopher cannot escape.
The gopher can escape through the hole at (2.500,2.500).
";
			}
		}
	}
}
