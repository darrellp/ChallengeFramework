using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;
using static System.Math;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
        [Challenge("Advent of Code 2016", "A Maze of Twisty Little Cubicles (13)", "https://adventofcode.com/2016/day/13")]
		public class TwistyCubicles : IChallenge
        {
			public void Solve()
			{
                var mapClear = new bool?[1000][];
                var start = new Position(1, 1);
				Position.InitClass( GetVal(), mapClear);
				var astar = new AStar<Position>(start, new Position(39, 31));
                // Subtract one because they don't want the first location included in their answer
				WriteLine(astar.Solve().Count - 1);

                // Part 2
                WriteLine(start.CountPaths(50));
            }

			public string RetrieveSampleInput()
			{
				return @"
1364
";
			}

			public string RetrieveSampleOutput()
            {
                return @"
86
127
";
            }
		}

        public class Position : IState
        {
            private static bool?[][] _mapClear;
            private static int _favorite;
            private static int[,] _mapVisited;
            private int cmaxSteps;


			private readonly GridLocation _location;

            internal Position(int row, int col) : this(new GridLocation(row, col)) { }
            internal Position(GridLocation location)
            {
                _location = location;
            }

            internal int CountPaths(int cSteps)
            {
                _mapVisited = new int[_location.Col + cSteps, _location.Row + cSteps];
                cmaxSteps = cSteps;
                for (var ix = 0; ix < _location.Col + cSteps; ix++)
                {
                    for (var iy = 0; iy < _location.Row + cSteps; iy++)
                    {
                        _mapVisited[ix, iy] = int.MaxValue;
                    }
                }

				CpWalker(cSteps);

                var ret = 0;
                for (var ix = 0; ix < _location.Col + cSteps; ix++)
                {
                    for (var iy = 0; iy < _location.Row + cSteps; iy++)
                    {
                        if (_mapVisited[ix, iy] < int.MaxValue)
                        {
                            ret++;
                        }
                    }
                }

                return ret;
            }

			public void CpWalker(int cSteps)
            {
                if (cSteps == 0)
                {
                    _mapVisited[_location.Col, _location.Row] = Min(_mapVisited[_location.Col, _location.Row], cmaxSteps);
                    return;
                }

                var cstepsTaken = cmaxSteps - cSteps;

                var probes = Successors()
                    .Cast<Position>()
                    .Where(p => _mapVisited[p._location.Col, p._location.Row] > cstepsTaken);

                foreach (var p in probes)
                {
                    _mapVisited[p._location.Col, p._location.Row] = cstepsTaken;
                    p.CpWalker(cSteps - 1);
                }
            }

			internal static bool IsClear(GridLocation pos)
            {
                var x = pos.Col;
                var y = pos.Row;
                if (_mapClear[x] == null)
                {
                    _mapClear[x] = new bool?[1000];
                }

                if (!_mapClear[x][y].HasValue)
                {
                    var val = (ulong)(x * x + 3 * x + 2 * x * y + y + y * y + _favorite);
                    _mapClear[x][y] = (Bitops.BitCount(val) & 1) == 0;

                }
				return _mapClear[x][y].Value;
            }

            internal static void InitClass(int favorite, bool?[][] mapClear)
            {
                _favorite = favorite;
                _mapClear = mapClear;
            }

            public IEnumerable<IState> Successors()
            {
                return _location.Neighbors()
                    .Where(IsClear)
                    .Select(l => new Position(l));
            }

            public double SuccessorDistance(IState successor)
            {
                return 1;
            }

            public double EstDistance(IState target)
            {
                if (!(target is Position pos))
                {
                    return double.MaxValue;
                }

                return Abs(pos._location.Col - _location.Col) + Abs(pos._location.Row - _location.Row);
            }

            public override bool Equals(object obj)
            {
                var ret = obj is Position pos && pos._location.Row == _location.Row && pos._location.Col == _location.Col;
                return ret;

            }

            public bool IsEqual(IState state)
            {
                throw new NotImplementedException();
            }

			public override int GetHashCode()
            {
                return _location.Col + _location.Row;
            }

            public override string ToString()
            {
                return $"({_location.Row}, {_location.Col})";
            }
        }
    }
}
