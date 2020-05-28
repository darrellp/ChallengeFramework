//using System;
//using System.Linq;
//using System.Text;
//using System.IO;
//using System.Collections.Generic;

//namespace Programming_Challenges.Challenges
//{
//	public static partial class ChallengeClass
//	{
//		[Challenge("1.6.3", "The Trip")]
//		public class Trip : IChallenge
//		{
//			public string Solve(StringReader stm)
//			{
//				var ret = new StringBuilder();
//				while (true)
//				{
//					var nextCase = GetCase(stm);
//					if (nextCase == null)
//					{
//						break;
//					}
//					var remainder = nextCase.Sum() % nextCase.Count();
//					var baseval = nextCase.Sum() / nextCase.Count();
//					var countMore = nextCase.Count;
//					var amt = 0;

//					foreach (var t in nextCase.Where(t => t <= baseval))
//					{
//						countMore--;
//						amt += baseval - t;
//					}
//					var res = (amt + Math.Max(0, remainder - countMore)).ToString();
//					var newval = "$" + res.Substring(0, res.Length - 2) + "." + res.Substring(res.Length - 2);
//					ret.Append(newval + Environment.NewLine);
//				}
//				return ret.ToString();
//			}

//			private List<int> GetCase(StringReader stm)
//			{
//// ReSharper disable AssignNullToNotNullAttribute
//				var cStudents = int.Parse(stm.ReadLine());
//				if (cStudents == 0)
//				{
//					return null;
//				}
//				var ret = new List<int>();
//				for (var iStudent = 0; iStudent < cStudents; iStudent++)
//				{
//					ret.Add(int.Parse(new string(stm.ReadLine().Where(Char.IsDigit).ToArray())));
//				}
//				return ret;
//// ReSharper restore AssignNullToNotNullAttribute
//			}

//			public string RetrieveSampleInput()
//			{
//				return @"
//3
//10.00
//20.00
//30.00
//4
//15.00
//15.01
//3.00
//3.01
//0";
//			}

//			public string RetrieveSampleOutput()
//			{
//				return @"
//$10.00
//$11.99
//";
//			}
//		}
//	}
//}
