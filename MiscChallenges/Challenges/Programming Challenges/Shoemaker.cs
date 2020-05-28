//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.IO;

//namespace Programming_Challenges.Challenges
//{
//	public static partial class ChallengeClass
//	{
//		[Challenge("4.6.5", "Shoemaker's Problem")]
//		public class Shoemaker : IChallenge
//		{
//			public string Solve(StringReader str)
//			{
//				var ret = new StringBuilder();
//				var cCases = GetVal(str);

//				for (var iCase = 0; iCase < cCases; iCase++)
//				{
//					var caseCur = new ShoemakerCase(str);
//					caseCur.Solve(ret);
//				}

//				return ret.ToString();
//			}

//			public string RetrieveSampleInput()
//			{
//				return @"
//1

//4
//3 4
//1 1000
//2 2
//5 5
//";
//			}

//			public string RetrieveSampleOutput()
//			{
//				return @"
//2 1 3 4
//";
//			}

//			class ShoemakerCase
//			{
//				struct Job
//				{
//					private readonly int _fine;
//					private readonly int _days;
//					// ReSharper disable once InconsistentNaming
//					public int IJob { get; private set; }

//					public int Fine
//					{
//						get { return _fine; }
//					}

//					public int Days
//					{
//						get { return _days; }
//					}


//					public Job(int iJob, int fine, int days) : this()
//					{
//						_fine = fine;
//						_days = days;
//						IJob = iJob + 1;
//					}
//				}

//				private readonly List<Job> _jobs;

//				public ShoemakerCase(StringReader str)
//				{
//					str.ReadLine();
//					var cJobs = GetVal(str);
//					_jobs = new List<Job>(cJobs);

//					for (var iJob = 0; iJob < cJobs; iJob++)
//					{
//						var jobInfo = GetVals(str);
//						_jobs.Add(new Job(iJob, jobInfo[1], jobInfo[0]));
//					}
//				}

//				public void Solve(StringBuilder output)
//				{
//					var fFirst = true;

//					_jobs.Sort((j1,j2) => (j1.Days * j2.Fine).CompareTo(j2.Days * j1.Fine));

//					foreach (var t in _jobs)
//					{
//						output.Append((fFirst ? "" : " ") + t.IJob);
//						fFirst = false;
//					}
//					output.Append(Environment.NewLine);
//				}
//			}
//		}
//	}
//}
