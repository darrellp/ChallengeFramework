//using System;
//using System.Linq;
//using System.Text;
//using System.IO;

//namespace Programming_Challenges.Challenges
//{
//	public static partial class ChallengeClass
//	{
//		[Challenge("2.8.1", "Jolly Jumpers")]
//		public class JollyJumper : IChallenge
//		{
//			public string Solve(StringReader stm)
//			{
//				var ret = new StringBuilder();
//				while (true)
//				{
//					var curCase = stm.ReadLine();
//					if (string.IsNullOrEmpty(curCase))
//					{
//						break;
//					}
//					var vals = curCase.Split(new[] {' '}).Select(int.Parse).ToArray();
//					var cvals = vals[0];
//					var sequence = vals.Skip(1).ToArray();
//					var found = new bool[cvals - 1];
//					var isJolly = true;

//					for (var i = 0; i < cvals - 1; i++)
//					{
//						var diff = Math.Abs(sequence[i + 1] - sequence[i]);
//						if (diff == 0 || diff >= cvals || found[diff - 1])
//						{
//							ret.Append("Not jolly" + Environment.NewLine);
//							isJolly = false;
//							break;
//						}
//						found[diff - 1] = true;
//					}
//					if (isJolly)
//					{
//						ret.Append("Jolly" + Environment.NewLine);
//					}
//				}
//				return ret.ToString();
//			}

//			public string RetrieveSampleInput()
//			{
//				return @"
//4 1 4 2 3
//5 1 4 2 -1 6
//";
//			}

//			public string RetrieveSampleOutput()
//			{
//				return @"
//Jolly
//Not jolly
//";
//			}
//		}
//	}
//}
