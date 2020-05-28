//using System;
//using System.IO;
//using System.Linq;
//using System.Text;
//using NumberTheoryLong;

//namespace Programming_Challenges.Challenges
//{
//	public static partial class ChallengeClass
//	{
//		[Challenge("7.6.6", "Smith Numbers")]
//		public class SmithNumbers : IChallenge
//		{
//			public string Solve(StringReader stm)
//			{
//				var sbRet = new StringBuilder();
//				// ReSharper disable once AssignNullToNotNullAttribute
//				var cCases = int.Parse(stm.ReadLine());
//				for (var iCase = 0; iCase < cCases; iCase++)
//				{
//					LocalSolver.ReadCase(stm).Solve(sbRet);
//				}
//				return sbRet.ToString();
//			}

//			public string RetrieveSampleInput()
//			{
//				return @"
//1
//4937774
//";
//			}

//			public string RetrieveSampleOutput()
//			{
//				return @"
//4937775
//";
//			}

//			private class LocalSolver
//			{
//				private long _lbound;

//				private LocalSolver(long lbound)
//				{
//					_lbound = lbound;
//				}

//				public static LocalSolver ReadCase(StringReader stm)
//				{
//					// ReSharper disable once AssignNullToNotNullAttribute
//					return new LocalSolver(long.Parse(stm.ReadLine()));
//				}

//				public void Solve(StringBuilder sbRet)
//				{
//					for(;;_lbound++)
//					{
//						if (IsSmith(_lbound))
//						{
//							sbRet.Append(_lbound + Environment.NewLine);
//							return;
//						}
//					}
//				}

//				private static bool IsSmith(long l)
//				{
//					if (l.IsPrime())
//					{
//						return false;
//					}
//					var digitSum = l.ToString().Select(c => c - '0').Sum();
//					var factorSum = Factoring.Factor(l).Sum(
//						factor => factor.Exp * factor.Prime.ToString().Select(c => c - '0').Sum());
//					return factorSum == digitSum;
//				}
//			}
//		}
//	}
//}
