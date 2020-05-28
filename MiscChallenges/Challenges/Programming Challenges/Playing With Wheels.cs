//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.IO;
//using Programming_Challenges.Utilities;

//namespace Programming_Challenges.Challenges
//{
//// ReSharper disable AssignNullToNotNullAttribute
//	public static partial class ChallengeClass
//	{
//		[Challenge("9.6.2", "Playing With Wheels")]
//		public class PlayingWithWheels : IChallenge
//		{
//			public string Solve(StringReader stm)
//			{
//				var ret = new StringBuilder();
//				var cCases = int.Parse(stm.ReadLine());

//				for (var i = 0; i < cCases; i++)
//				{
//					var solver = new Solver(stm);

//					ret.Append(solver.Solve() + Environment.NewLine);
//				}
//				return ret.ToString();
//			}


//			public string RetrieveSampleInput()
//			{
//				return @"
//2

//8 0 5 6
//6 5 0 8
//5
//8 0 5 7
//8 0 4 7
//5 5 0 8
//7 5 0 8
//6 4 0 8

//0 0 0 0
//5 3 1 7
//8
//0 0 0 1
//0 0 0 9
//0 0 1 0
//0 0 9 0
//0 1 0 0
//0 9 0 0
//1 0 0 0
//9 0 0 0
//";
//			}

//			public string RetrieveSampleOutput()
//			{
//				return @"
//14
//-1
//";
//			}
//		}

//		private class Solver
//		{
//			private readonly Config _start;
//			private readonly Config _target;
//			private readonly HashSet<Config> _forbidden = new HashSet<Config>();

//			private Config GetConfig(StringReader stm)
//			{
//// ReSharper disable once PossibleNullReferenceException
//				return new Config(stm.ReadLine().Split(' ').Select(int.Parse).ToList(), this);
//			}

//			public Solver(StringReader stm)
//			{
//				stm.ReadLine();				// Blank line
//				_start = GetConfig(stm);
//				_target = GetConfig(stm);
//				var cForbidden = int.Parse(stm.ReadLine());
//				for (var i = 0; i < cForbidden; i++)
//				{
//					_forbidden.Add(GetConfig(stm));
//				}
//			}

//			public int Solve()
//			{
//				var astar = new AStar<Config>(_start, _target);
//				var path = astar.Solve();
//				if (path == null)
//				{
//					return -1;
//				}
//				return path.Count - 1;
//			}

//			private class Config : AStarState
//			{
//				private readonly List<int> _wheels;
//				private readonly Solver _solver;

//				public Config(IEnumerable<int> wheels, Solver solver)
//				{
//					_wheels = new List<int>(wheels);
//					_solver = solver;
//				}

//				public override IEnumerable<IState> Successors()
//				{
//					// For each wheel
//					for (var i = 0; i < 4; i++)
//					{
//						// For each possible new position
//						for (var d = 9; d <= 11; d += 2)
//						{
//							// Set up a new configuration
//							var wheels = new List<int>(_wheels);
//							wheels[i] = (wheels[i] + d) % 10;
//							var newConfig = new Config(wheels, _solver);
							
//							// Is the configuration forbidden?
//							if (_solver._forbidden.Contains(newConfig))
//							{
//								// Find another
//								continue;
//							}

//							// Return the new configuration
//							yield return newConfig;
//						}
//					}
//				}

//				private static int DigitDistance(int d1, int d2)
//				{
//					var min = Math.Min(d1, d2);
//					var max = Math.Max(d1, d2);
//					var d = max - min;
//					if (d <= 5)
//					{
//						return d;
//					}
//					return 10 - d;
//				}

//				public override double EstDistance(IState target)
//				{
//					return _wheels.Zip(((Config)target)._wheels, DigitDistance).Sum();
//				}

//				public override bool IsEqual(IState istate)
//				{
//					var other = (Config) istate;

//					for (var i = 0; i < 4; i++)
//					{
//						if (other._wheels[i] != _wheels[i])
//						{
//							return false;
//						}
//					}
//					return true;
//				}

//				private readonly static int[] Powers = {1,10,100,1000};
//				public override int GetHashCode()
//				{
//					return _wheels.Select((v, i) => v * Powers[i]).Sum();
//				}

//				public override string ToString()
//				{
//					return string.Format("{0}{1}{2}{3}", _wheels[0], _wheels[1], _wheels[2], _wheels[3]);
//				}
//			}
//		}
//	}
//}
