using System;
using System.Collections.Generic;
using System.Linq;

namespace MiscChallenges.Challenges
{
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// This roughly follows the algorithm at
	/// http://csclab.murraystate.edu/bob.pilgrim/445/munkres.html and the steps mentioned in the
	/// comments correspond to the steps at that website.
	/// 
	/// The algorithm at the website is, IMHO, poorly worded and layed out and I've layed things out
	/// a bit differently here.  For instance, the algorithm says things like "find an uncovered
	/// zero...if there is no uncovered zero, find the smallest uncovered element in the matrix".
	/// Obviously, these both are subsumed by "Find the minimal uncovered element in the matrix.  If
	/// it's zero... If it's nonzero..." which is how I structured my stuff.  Also, step 4 in the web
	/// site is really several operations with different results jumping to different parts of the
	/// algorithm so I tried to smooth that out as much as possible.  Finally, the algorithm on the
	/// web site would have required some GoTo's which I also eliminated.  In the end I think mine is
	/// easier to understand, faster and more elegant but it has the disadvantage that I can't always
	/// unequivocally label parts of my code as "step X" to correspond to the web site's algorithm.
	/// </summary>
	///
	/// <remarks>	Darrell Plank, 5/27/2020. </remarks>
	////////////////////////////////////////////////////////////////////////////////////////////////////

	public class Hungarian
	{
		#region Private Variables
		private bool _fRotated;					// True if we had to rotate from original matrix
		private List<List<int>> _matrix;		// Properly rotated matrix with cols >= rows
		private int _rows;						// Rows in rotated matrix
		private int _cols;						// Columns in rotated matrix
		private readonly bool _fMax;			// True if we're to find max rather than min
		private List<bool> _coveredColumns;		// Columns that are covered
		private List<bool> _coveredRows;		// Rows that are covered
		private readonly List<int> _rowStars;	// For each row, -1 if there is no star in the row,
												//	otherwise the index of the starred zero in that row.
		private List<int> _rowPrimes;			// Identical to _rowStars only for primes
		#endregion

		#region Properties
		private int CoveredCount
		{
			get { return _coveredColumns.Count(f => f) + _coveredRows.Count(f => f); }
		}
		#endregion

		#region Constructor
		public Hungarian(List<List<int>> matrix, bool fMax)
		{
			_fMax = fMax;
			// Load matrix and implement step 0
			LoadFromMatrix(matrix);
			_coveredColumns = Enumerable.Repeat(false, _cols).ToList();
			_coveredRows = Enumerable.Repeat(false, _rows).ToList();
			_rowStars = Enumerable.Repeat(-1, _rows).ToList();
			_rowPrimes = Enumerable.Repeat(-1, _rows).ToList();
		}
		#endregion

		#region Solving
		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Minimize assignment in _matrix. </summary>
		///
		/// <remarks>	Darrell Plank, 5/27/2020. </remarks>
		///
		/// <returns>	An list of column assignments for each row. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public List<int> Solve()
		{
			// Step 1
			SubtractRowMins();

			// Step 2 and 3
			InitialStarring();

			while (CoveredCount != _rows)
			{
				// Preparation for step 4
				var smallestUncovered = GetSmallestUncovered();
				var iRowSmall = smallestUncovered.Row;
				var iColSmall = smallestUncovered.Col;
				var minVal = _matrix[iRowSmall][iColSmall];

				// Step 4
				if (minVal == 0)
				{
					if (_rowStars[iRowSmall] < 0)
					{
						// No starred zero on the row with the uncovered zero
						// Step 5
						HandlePrimeChain(smallestUncovered);

						// Step 3
						SetCoversAndPrimes();
					}
					else
					{
						// More of Step 4
						
						// Found a starred zero in the uncovered zero's row - uncover
						// it's column and cover this row as part of step 4. Prime the
						// uncovered zero.
						_coveredRows[iRowSmall] = true;
						_coveredColumns[_rowStars[iRowSmall]] = false;
						_rowPrimes[iRowSmall] = iColSmall;
					}
				}
				else
				{
					// Step 6
					HandleNonzeroMin(minVal);
				}
			}

			return _fRotated ? 
				_rowStars.Select((iCol, iRow) => iCol).ToList() :
				_rowStars;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Clears the covers and primes. </summary>
		///
		/// <remarks>	This is essentially step 3. </remarks>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		private void SetCoversAndPrimes()
		{
			_rowPrimes = Enumerable.Repeat(-1, _rows).ToList();
			_coveredColumns = Enumerable.Repeat(false, _cols).ToList();
			_coveredRows = Enumerable.Repeat(false, _rows).ToList();
			foreach (var iCol in _rowStars.Where(i => i >= 0))
			{
				_coveredColumns[iCol] = true;
			}
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Handles the case where the minimum uncovered element is non-zero. </summary>
		///
		/// <remarks>	This is step pretty directly step 6. </remarks>
		///
		/// <param name="minVal">	The minimum value. </param>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		private void HandleNonzeroMin(int minVal)
		{
			foreach(var iRow in Enumerable.Range(0, _rows).Where(i => _coveredRows[i]))
			{
				AddToRow(iRow, minVal);
			}
			foreach (var iCol in Enumerable.Range(0, _cols).Where(i => !_coveredColumns[i]))
			{
				AddToColumn(iCol, -minVal);
			}
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Handles the prime chain of step 5. </summary>
		///
		/// <remarks>	Darrell Plank, 5/27/2020. </remarks>
		///
		/// <param name="firstPrime">	The first prime to start the chain. </param>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		private void HandlePrimeChain(GridLocation firstPrime)
		{
			var curPrimeLoc = firstPrime;
			var curStarLoc = GetStarInColumn(curPrimeLoc.Col);

			StarZero(curPrimeLoc);

			while (!curStarLoc.IsNoLoc())
			{
				_rowStars[curStarLoc.Row] = -1;
				curPrimeLoc = new GridLocation(curStarLoc.Row, _rowPrimes[curStarLoc.Row]);
				curStarLoc = GetStarInColumn(curPrimeLoc.Col);
				StarZero(curPrimeLoc);
			}
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Finds a star in a given column, if any. </summary>
		///
		/// <remarks>	Darrell Plank, 5/27/2020. </remarks>
		///
		/// <param name="col">	The column to search for the star. </param>
		///
		/// <returns>	Location of the star if found, otherwise NoLoc. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		private GridLocation GetStarInColumn(int col)
		{
			for (int iRow = 0; iRow < _rows; iRow++)
			{
				if (_rowStars[iRow] == col)
				{
					return new GridLocation(iRow, col);
				}
			}
			return GridLocation.NoLoc();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Get the smallest uncovered value in _matrix. </summary>
		///
		/// <remarks>
		/// This is really in preparation for step 4 which is really several steps.  The first step is to
		/// find the smallest value in the matrix.  After that we decide what to do based on whether it
		/// is zero or non-zero.
		/// </remarks>
		///
		/// <returns>	GridLocation with the smallest non-covered value. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		private GridLocation GetSmallestUncovered()
		{
			var iRowMin = 0;
			var iColMin = 0;
			var min = int.MaxValue;

			for (var iCol = 0; iCol < _cols; iCol++)
			{
				if (_coveredColumns[iCol])
				{
					continue;
				}
				for (var iRow = 0; iRow < _rows; iRow++)
				{
					if (_coveredRows[iRow])
					{
						continue;
					}
					if (_matrix[iRow][iCol] < min)
					{
						min = _matrix[iRow][iCol];
						iRowMin = iRow;
						iColMin = iCol;
						if (min == 0)
						{
							return new GridLocation(iRowMin, iColMin);
						}
					}
				}
			}
			return new GridLocation(iRowMin, iColMin);
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Step 1 of the algorithm - subtract row min from each row. </summary>
		///
		/// <remarks>	Darrell Plank, 5/27/2020. </remarks>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		private void SubtractRowMins()
		{
			for (var iRow = 0; iRow < _rows; iRow++)
			{
				AddToRow(iRow, -_matrix[iRow].Min());
			}
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Adds a single value to a column. </summary>
		///
		/// <remarks>	Darrell Plank, 5/27/2020. </remarks>
		///
		/// <param name="iCol">	The column to add to. </param>
		/// <param name="val"> 	The value to add. </param>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		private void AddToColumn(int iCol, int val)
		{
			for (var iRow = 0; iRow < _rows; iRow++)
			{
				_matrix[iRow][iCol] += val;
			}
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Adds a single value to a row. </summary>
		///
		/// <remarks>	Darrell Plank, 5/27/2020. </remarks>
		///
		/// <param name="iRow">	The row to add to. </param>
		/// <param name="val"> 	The value to add. </param>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		private void AddToRow(int iRow, int val)
		{
			_matrix[iRow] = _matrix[iRow].Select(i => i + val).ToList();
		}
		#endregion

		#region Starring
		private void StarZero(GridLocation loc)
		{
			// Step 2
			_rowStars[loc.Row] = loc.Col;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Does the initial starring. </summary>
		///
		/// <remarks>	Darrell Plank, 5/27/2020. </remarks>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		private void InitialStarring()
		{
			foreach (var loc in GetInitialStars())
			{
				// Step 2
				StarZero(loc);

				// Step 3
				_coveredColumns[loc.Col] = true;
			}
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Find the initial zeroes to be starred in step 2. </summary>
		///
		/// <remarks>
		/// The algorithm speaks in terms of "finding" zeroes as though you are supposed to randomly pick
		/// a zero in the matrix.  I don't see the reason for this and instead systematically pick the
		/// first zero of each row.  This is step 2 and most of step 3 of the algorithm.
		/// </remarks>
		///
		/// <returns>
		/// An enumerator that allows foreach to be used to process the initial stars in this collection.
		/// </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		private IEnumerable<GridLocation> GetInitialStars()
		{
			for (var iRow = 0; iRow < _rows; iRow++)
			{
				// We have to use col + 1 in the enum so that zero is not an acceptable value since
				// FirstOrDefault will return zero if not value is found.  This allows us to distinguish
				// between not finding a zero from finding a zero in the 0'th column.  We then
				// subtract 1 at the very end so that if we didn't find a zero iCol will be -1,
				// otherwise the column of the zero.
				var iCol = _matrix[iRow].
					Select((val, col) => !_coveredColumns[col] && val == 0 ? col + 1 : -1).
					FirstOrDefault(i => i > 0) - 1;

				if (iCol >= 0)
				{
					yield return new GridLocation(iRow, iCol);
				}
			}
		}
		#endregion

		#region Loading
		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Gets properly rotated row from original (unrotated) matrix. </summary>
		///
		/// <remarks>	Darrell Plank, 5/27/2020. </remarks>
		///
		/// <param name="matrix">	Original matrix. </param>
		/// <param name="iRow">  	Index of the row we want. </param>
		///
		/// <returns>	List of the ints in the row. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		private List<int> GetRow(List<List<int>> matrix, int iRow)
		{
			var enumRow = _fRotated
				? Enumerable.Range(0, _cols).Select(i => matrix[i][iRow])
				: matrix[iRow];
			if (_fMax)
			{
				enumRow = enumRow.Select(i => int.MaxValue - i);
			}
			return enumRow.ToList();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Loads matrix into solver. </summary>
		///
		/// <remarks>
		/// Step 0 of the algorithm is implemented in this function - i.e., we rotate the matrix if
		/// necessary to ensure that there are at least as many columns as rows. We copy the matrix in
		/// the process to ensure that we don't destroy the original matrix.
		/// </remarks>
		///
		/// <exception cref="ArgumentException">	Thrown when one or more arguments have unsupported or
		/// 										illegal values. </exception>
		///
		/// <param name="matrix">	(unrotated) matrix to load. </param>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		private void LoadFromMatrix(List<List<int>> matrix)
		{
			var cCols = matrix[0].Count;
			var cRows = matrix.Count;

			if (matrix.Select(row => row.Count).Any(c => c != cCols))
			{
				// Can't be ragged - has to at least be rectangular
				throw new ArgumentException("Non-square matrix in MinSquare");
			}

			_fRotated = cRows > cCols;
			_rows = _fRotated ? cCols : cRows;
			_cols = _fRotated ? cRows : cCols;

			_matrix = Enumerable.
				Range(0, _rows).
				Select(i => GetRow(matrix, i)).
				ToList();
		}
		#endregion
	}
}
