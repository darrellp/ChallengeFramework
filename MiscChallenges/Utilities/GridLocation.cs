using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace MiscChallenges.Challenges
{
	public struct GridLocation
	{
		public readonly int Row;
		public readonly int Col;
		private static readonly NeighborInfo DefaultNeighborInfo = new NeighborInfo();

		public bool IsNoLoc()
		{
			return Row < 0;
		}

		public GridLocation(int row, int col)
		{
			if (row < 0 || col < 0)
			{
				throw new ArgumentException();
			}
			Row = row;
			Col = col;
		}

// ReSharper disable once UnusedParameter.Local
		private GridLocation(bool dontCare)
		{
			Row = Col = -1;
		}

		public static GridLocation NoLoc()
		{
			return new GridLocation(true);
		}

		[Pure]
		public IEnumerable<GridLocation> Neighbors(NeighborInfo info = null)
		{
			if (info == null)
			{
				info = DefaultNeighborInfo;
			}

			for (var idRow = -1; idRow <= 1; idRow++)
			{
				for (var idCol = -1; idCol <= 1; idCol++)
				{
					if (idCol == 0 && idRow == 0)
					{
						if (info.IncludeOriginalCell)
						{
							yield return this;
						}
						continue;
					}
					if (idCol != 0 && idRow != 0 && info.F4Neighbors)
					{
						continue;
					}
					var curRow = Row + idRow;
					var curCol = Col + idCol;
					if (info.FWrap)
					{
						if (curRow < 0)
						{
							curRow += info.CRows;
						}
						else if (curRow >= info.CRows)
						{
							curRow -= info.CRows;
						}
						if (curCol < 0)
						{
							curCol += info.CCols;
						}
						else if (curCol >= info.CCols)
						{
							curCol -= info.CCols;
						}
					}
					else
					{
						if (curRow < 0 || curCol < 0 || curRow >= info.CRows || curCol >= info.CCols)
						{
							continue;
						}
					}
					yield return new GridLocation(curRow, curCol);
				}
			}
		}

		public override string ToString()
		{
			return string.Format("r:{0} c:{1}", Row, Col);
		}

		public class NeighborInfo
		{
			internal bool F4Neighbors { get; private set; }
			internal bool FWrap { get; private set; }
			internal int CRows { get; private set; }
			internal int CCols { get; private set; }
			internal bool IncludeOriginalCell { get; private set; }

			public NeighborInfo(
				int cRows = int.MaxValue,
				int cCols = int.MaxValue,
				bool f4Neighbors = true,
				bool fWrap = false,
				bool includeOriginalCell = false)
			{
				F4Neighbors = f4Neighbors;
				FWrap = fWrap;
				CRows = cRows;
				CCols = cCols;
				IncludeOriginalCell = includeOriginalCell;
			}
		}
	}
}
