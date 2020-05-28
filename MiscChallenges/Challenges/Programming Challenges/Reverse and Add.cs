//using System;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.IO;

//namespace Programming_Challenges.Challenges
//{
//	public static partial class ChallengeClass
//	{
//		[Challenge("5.9.2", "Reverse and Add")]
//		public class ReverseAndAdd : IChallenge
//		{
//			public string Solve(StringReader str)
//			{
//				var ret = new StringBuilder();
//				var cCases = GetVal(str);

//				for (var i = 0; i < cCases; i++)
//				{
//					var n = GetVal(str);
//					DoRevAdd(n, ret);
//				}
//				return ret.ToString();
//			}

//			private static void DoRevAdd(int n, StringBuilder ret)
//			{
//				var cReverses = 0;
//				var rev = Reverse(n);

//				while (rev != n)
//				{
//					n = n + rev;
//					cReverses++;
//					rev = Reverse(n);
//				}
//				ret.Append(string.Format("{0} {1}" + Environment.NewLine, cReverses, n));
//			}

//			private static int Reverse(int n)
//			{
//				return int.Parse(new String(n.ToString(CultureInfo.InvariantCulture).Reverse().ToArray()));
//			}

//			public string RetrieveSampleInput()
//			{
//				return @"
//3
//195
//265
//750
//";
//			}

//			public string RetrieveSampleOutput()
//			{
//				return @"
//4 9339
//5 45254
//3 6666
//";
//			}
//		}
//	}
//}
