//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;

//namespace Programming_Challenges.Challenges
//{
//	public static partial class ChallengeClass
//	{
//		[Challenge("13.6.3", "Knights of the Round Table")]
//		public class KinghtsOfTheRoundTable : IChallenge
//		{
//			public string Solve(StringReader stm)
//			{
//				var sbRet = new StringBuilder();
//				List<double> vals;

//				while ((vals = GetDblVals(stm)) != null)
//				{
//					sbRet.Append(SolveCase(vals) + Environment.NewLine);
//				}
//				return sbRet.ToString();
//			}

//			private string SolveCase(List<double> vals)
//			{
//				var sp = vals.Sum() / 2.0;
//				var triangleArea = Math.Sqrt(
//					sp *
//					(sp - vals[0]) *
//					(sp - vals[1]) *
//					(sp - vals[2]));
//				return "The radius of the round table is: " + (triangleArea / sp).ToString("F3");
//			}

//			public string RetrieveSampleInput()
//			{
//				return @"
//12.0 12.0 8.0
//";
//			}

//			public string RetrieveSampleOutput()
//			{
//				return @"
//The radius of the round table is: 2.828
//";
//			}
//		}
//	}
//}
