//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;

//namespace Programming_Challenges.Challenges
//{
//	public static partial class ChallengeClass
//	{
//		[Challenge("11.6.4", "Unidirectional TSP")]
//		public class UnidirectionalTsp : IChallenge
//		{
//			public string Solve(StringReader stm)
//			{
//				LocalSolver solver;
//				var sbRet = new StringBuilder();
//				while ((solver = LocalSolver.ReadCase(stm)) != null)
//				{
//					solver.Solve(sbRet);
//				}
//				return sbRet.ToString();
//			}

//			public string RetrieveSampleInput()
//			{
//				return @"
//5 6
//3 4 1 2 8 6
//6 1 8 2 7 4
//5 9 3 9 9 5
//8 4 1 3 2 6
//3 7 2 8 6 4
//5 6
//3 4 1 2 8 6
//6 1 8 2 7 4
//5 9 3 9 9 5
//8 4 1 3 2 6
//3 7 2 1 2 3
//2 2
//9 10 9 10
//";
//			}

//			public string RetrieveSampleOutput()
//			{
//				return @"
//1 2 3 4 4 5
//16
//1 2 1 5 4 5
//11
//1 1
//19
//";
//			}

//			private class LocalSolver
//			{
//				private readonly int[,] _matrix;
//				private readonly int _cCols;
//				private readonly int _cRows;

//				private LocalSolver(int[,] matrix)
//				{
//					_matrix = matrix;
//					_cRows = matrix.GetLength(0);
//					_cCols = matrix.GetLength(1);
//				}

//				public static LocalSolver ReadCase(StringReader stm)
//				{
//					var header = stm.ReadLine();
//					if (header == null)
//					{
//						return null;
//					}
//					var stgs = header.Split(' ').Where(s => s != string.Empty).ToArray();
//					var rows = int.Parse(stgs[0]);
//					var cols = int.Parse(stgs[1]);
//					var matrix = new int[rows, cols];
//					var cVals = rows * cols;
//					var iRow = 0;
//					var iCol = 0;
//					while (cVals > 0)
//					{
//						// ReSharper disable once PossibleNullReferenceException
//						stgs = stm.ReadLine().Split(' ').Where(s => s != string.Empty).ToArray();
//						foreach (var stg in stgs)
//						{
//							matrix[iRow, iCol] = int.Parse(stg);
//							if (++iCol >= cols)
//							{
//								iCol = 0;
//								iRow++;
//							}
//						}
//						cVals -= stgs.Length;
//					}
//					return new LocalSolver(matrix);
//				}

//				private int Wrap(int index, bool up)
//				{
//					return (index + (up ? 1 : -1) + _cRows) % _cRows;
//				}

//				public void Solve(StringBuilder sbRet)
//				{
//					List<int>[] curPaths;
//					var curColumnMins = FindMinPath(out curPaths);
//					var min = int.MaxValue;
//					var iMin = -1;
//					for (var iRow = 0; iRow < _cRows; iRow++)
//					{
//						if (curColumnMins[iRow] < min)
//						{
//							min = curColumnMins[iRow];
//							iMin = iRow;
//						}
//					}
//					var pad = "";
//					for (var iCol = 0; iCol < _cCols; iCol++)
//					{
//						sbRet.Append(pad + (curPaths[iMin][iCol] + 1));
//						pad = " ";
//					}
//					sbRet.Append(Environment.NewLine);
//					sbRet.Append(curColumnMins[iMin].ToString() + Environment.NewLine);
//				}

//				private int[] FindMinPath(out List<int>[] curPaths)
//				{
//					var curColumnMins = Enumerable.Range(0, _cRows).Select(i => _matrix[i, 0]).ToArray();
//					curPaths = Enumerable.Range(0, _cRows).Select(i => new List<int> {i}).ToArray();
//					var newColumnMins = new int[_cRows];
//					var newPaths = new List<int>[_cRows];

//					for (var iCol = 1; iCol < _cCols; iCol++)
//					{
//						for (var iRow = 0; iRow < _cRows; iRow++)
//						{
//							var i0 = Wrap(iRow, false);
//							var i1 = iRow;
//							var i2 = Wrap(iRow, true);
//							var wt0 = curColumnMins[i0];
//							var wt1 = curColumnMins[i1];
//							var wt2 = curColumnMins[i2];
//							int minIndex;
//							if (wt0 <= wt1 && wt0 <= wt2)
//							{
//								minIndex = i0;
//								if (wt0 == wt1)
//								{
//									minIndex = Math.Min(minIndex, i1);
//								}
//								if (wt0 == wt2)
//								{
//									minIndex = Math.Min(minIndex, i2);
//								}
//							}
//							else if (wt1 <= wt2)
//							{
//								minIndex = i1;
//								if (wt1 == wt2)
//								{
//									minIndex = Math.Min(minIndex, i2);
//								}
//							}
//							else
//							{
//								minIndex = i2;
//							}
//							newColumnMins[iRow] = _matrix[iRow, iCol] + curColumnMins[minIndex];
//							newPaths[iRow] = new List<int>(curPaths[minIndex]) {iRow};
//						}
//						curColumnMins = newColumnMins;
//						curPaths = newPaths;
//						// Could swap old and new arrays for efficiency since the contents in both
//						// new arrays will be completely overwritten, but simpler code to just
//						// reallocate and efficiency's not a problem here.
//						newColumnMins = new int[_cRows];
//						newPaths = new List<int>[_cRows];
//					}
//					return curColumnMins;
//				}
//			}
//		}
//	}
//}
