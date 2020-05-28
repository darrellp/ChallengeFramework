//using System;
//using System.IO;
//using System.Linq;
//using System.Text;

//namespace Programming_Challenges.Challenges
//{
//	public static partial class ChallengeClass
//	{
//		[Challenge("4.6.1", "Vito's Family")]
//		public class VitosFamily : IChallenge
//		{
//			public string Solve(StringReader stm)
//			{
//				var sbRet = new StringBuilder();
//				var cCases = GetVal(stm);

//				for (var iCase = 0; iCase < cCases; iCase++)
//				{
//					// Best position for vito is position with half the houses to the left and
//					// half to the right.
//					var vals = GetVals(stm);
//					var vitoIndex = vals[0] / 2;
//					vals = vals.Skip(1).ToList();
//					vals.Sort();
//					var vitoPos = vals[vitoIndex];
//					var minimalDistance = vals.Select(hn => Math.Abs(hn - vitoPos)).Sum();
//					sbRet.Append(minimalDistance + Environment.NewLine);
//				}
//				return sbRet.ToString();
//			}

//			public string RetrieveSampleInput()
//			{
//				return @"
//2
//2 2 4
//3 2 4 6
//";
//			}

//			public string RetrieveSampleOutput()
//			{
//				return @"
//2
//4
//";
//			}
//		}
//	}
//}
