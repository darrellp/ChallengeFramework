//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using Programming_Challenges.Utilities;

//namespace Programming_Challenges.Challenges
//{
//	public static partial class ChallengeClass
//	{
//		[Challenge("12.6.2", "The Monocycle")]
//		public class Monocycle : IChallenge
//		{
//			public string Solve(StringReader stm)
//			{
//				LocalSolver solver;
//				var sbRet = new StringBuilder();
//				var iCase = 1;
//				var fFirst = true;
//				while ((solver = LocalSolver.ReadCase(stm)) != null)
//				{
//					if (!fFirst)
//					{
//						sbRet.Append(Environment.NewLine);
//					}
//					fFirst = false;
//					sbRet.Append(string.Format("Case #{0}", iCase++));
//					sbRet.Append(Environment.NewLine);
//					solver.Solve(sbRet);
//				}
//				return sbRet.ToString();
//			}

//			public string RetrieveSampleInput()
//			{
//				return @"
//1 3
//S#T
//10 10
//#S.......#
//#..#.##.##
//#.##.##.##
//.#....##.#
//##.##..#.#
//#..#.##...
//#......##.
//..##.##...
//#.###...#.
//#.....###T
//0 0
//";
//			}

//			public string RetrieveSampleOutput()
//			{
//				return @"
//Case #1
//destination not reachable

//Case #2
//minimum time = 49 sec
//";
//			}

//			private class LocalSolver
//			{
//				private bool[][] _isBlocked;
//				private int _cRows;
//				private int _cCols;
//				private GridLocation _start;
//				private GridLocation _end;
//				private const int Colors = 5;

//				public static LocalSolver ReadCase(StringReader stm)
//				{
//					var ret = new LocalSolver();

//					// ReSharper disable once PossibleNullReferenceException
//					var stgs = stm.ReadLine().Split(' ').Where(s => s != string.Empty).ToArray();
//					ret._cRows = int.Parse(stgs[0]);
//					ret._cCols = int.Parse(stgs[1]);
//					if (ret._cRows == 0 && ret._cCols == 0)
//					{
//						return null;
//					}

//					ret._isBlocked = new bool[ret._cRows][];

//					// We go down here so that index 0 is the bottom row
//					for (var iRow = 0; iRow < ret._cRows; iRow++)
//					{
//						var line = stm.ReadLine();
//						// ReSharper disable once PossibleNullReferenceException
//						var iColStart = line.IndexOf('S');
//						var iColEnd = line.IndexOf('T');

//						if (iColStart >= 0)
//						{
//							ret._start = new GridLocation(iRow, iColStart);
//						}
//						if (iColEnd >= 0)
//						{
//							ret._end = new GridLocation(iRow, iColEnd);
//						}
//						ret._isBlocked[iRow] = line.Select(c => c == '#').ToArray();
//					}

//					return ret;
//				}

//				private static int CalculateTime(List<MonocycleState> sln)
//				{
//					if (sln == null)
//					{
//						return int.MaxValue;
//					}
//					var ret = 0;

//					for (var iCell = 1; iCell < sln.Count; iCell++)
//					{
//						ret += (int)sln[iCell - 1].SuccessorDistance(sln[iCell]);
//					}
//					return ret;
//				}

//				public void Solve(StringBuilder sbRet)
//				{
//					var targets = new HashSet<MonocycleState>
//					{
//						new MonocycleState(0, _end, 0, this),
//						new MonocycleState(0, _end, 1, this),
//						new MonocycleState(0, _end, 2, this),
//						new MonocycleState(0, _end, 3, this),
//					};
//					var start = new MonocycleState(0, _start, 0, this);
//					var sln = new AStar<MonocycleState>(start, targets).Solve();
//					sbRet.Append(sln == null ? "destination not reachable" : string.Format("minimum time = {0} sec", CalculateTime(sln)));
//					sbRet.Append(Environment.NewLine);
//				}

//				private class MonocycleState : AStarState
//				{
//					private readonly int _color;
//					private readonly int _direction;
//					private readonly GridLocation _location;
//					private readonly GridLocation.NeighborInfo _nInfo;
//					private readonly LocalSolver _solver;

//					public MonocycleState(int color, GridLocation location, int direction, LocalSolver solver)
//					{
//						_color = color;
//						_location = location;
//						_solver = solver;
//						_direction = direction;
//						_nInfo = new GridLocation.NeighborInfo(solver._cRows, solver._cCols);
//					}

//					// 0 = north, 1 = east, 2 = south, 3 = west
//					private static int NewDirection(GridLocation locOld, GridLocation locNew)
//					{
//						if (locOld.Row == locNew.Row)
//						{
//							return locOld.Col == locNew.Col - 1 ? 1 : 3;
//						}
//						return locOld.Row == locNew.Row - 1 ? 0 : 2;
//					}

//					public override IEnumerable<IState> Successors()
//					{
//						var newColor = (_color + 1) % Colors;
//						// ReSharper disable once ImpureMethodCallOnReadonlyValueField
//						return _location.Neighbors(_nInfo).
//							Where(loc => !_solver._isBlocked[loc.Row][loc.Col]).
//							Select(loc => new MonocycleState(newColor, loc, NewDirection(_location, loc), _solver));
//					}

//					public override double EstDistance(IState target)
//					{
//						var monocycleState = target as MonocycleState;
//						if (monocycleState == null)
//						{
//							return double.MaxValue;
//						}

//						// We're gonna have to change at least this many rows and columns plus make
//						// this many turns at one second each...
//						return Math.Abs(_location.Row - monocycleState._location.Row) +
//						       Math.Abs(_location.Col - monocycleState._location.Col) +
//							   DirectionDistance(_direction, monocycleState._direction);
//					}

//					private double DirectionDistance(int dir0, int dir1)
//					{
//						var d0 = Math.Min(dir0, dir1);
//						var d1 = Math.Max(dir0, dir1);
//						if (d0 == 0 && d1 == 3)
//						{
//							return 1;
//						}
//						return d1 - d0;
//					}

//					public override double SuccessorDistance(IState successor)
//					{
//						var monocycleState = successor as MonocycleState;
//						if (monocycleState == null)
//						{
//							return double.MaxValue;
//						}
//						return 1 + DirectionDistance(_direction, monocycleState._direction);
//					}

//					public override bool IsEqual(IState istate)
//					{
//						var monocycleState = istate as MonocycleState;
//						if (monocycleState == null)
//						{
//							return false;
//						}
//						return monocycleState._color == _color &&
//							monocycleState._location.Equals(_location) &&
//							monocycleState._direction == _direction;
//					}

//					public override int GetHashCode()
//					{
//						return (_color * 31769) ^
//							(_direction * 15013) ^
//						    (_location.Row * 10007) ^
//							(_location.Col * 21001);
//					}

//					public override string ToString()
//					{
//						return string.Format("{0} clr:{1} d:{2}", _location, _color, _direction);
//					}
//				}
//			}
//		}
//	}
//}
