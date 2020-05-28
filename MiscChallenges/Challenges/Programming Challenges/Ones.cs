//using System;
//using System.Linq;
//using System.Text;
//using System.IO;

//namespace Programming_Challenges.Challenges
//{
//	public static partial class ChallengeClass
//	{
//		[Challenge("5.9.4", "Ones")]
//		public class Ones : IChallenge
//		{
//			public string Solve(StringReader str)
//			{
//				var ret = new StringBuilder();

//				while (true)
//				{
//					var n = GetVal(str);
//					FindOnes(n, ret);
//					if (str.Peek() == -1)
//					{
//						break;
//					}
//				}
//				return ret.ToString();
//			}

//			private static void FindOnes(int n, StringBuilder ret)
//			{
//				var cOnes = Iterate(1, i => (i * 10 + 1) % n, i => i == 0).Count();
//				ret.Append(string.Format("{0}" + Environment.NewLine, cOnes));
//			}

//			public string RetrieveSampleInput()
//			{
//				return @"
//3
//7
//9901
//";
//			}

//			public string RetrieveSampleOutput()
//			{
//				return @"
//3
//6
//12
//";
//			}
//		}
//	}
//}
