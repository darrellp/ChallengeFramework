using System;
using System.Collections.Generic;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("ProgChallenges", "Bigger Square Please",
			"http://www.programming-challenges.com/pg.php?page=downloadproblem&probid=110808&format=html")]
		public class BiggerSquare : IChallenge
		{
			public void Solve()
			{
				// ReSharper disable AssignNullToNotNullAttribute
				var cPuzzles = int.Parse(Console.ReadLine());
				for (var iPuzzle = 0; iPuzzle < cPuzzles; iPuzzle++)
				{
					var puzzle = int.Parse(Console.ReadLine());
					SolvePuzzle(puzzle);
				}
				// ReSharper restore AssignNullToNotNullAttribute
			}

			private void SolvePuzzle(int puzzle)
			{
				var initboard = new Board868(puzzle, new Board868(puzzle), 0);
				var res = initboard.Solve(puzzle * puzzle);
				Console.WriteLine(res.Count);
				foreach (var board in res)
				{
					Console.WriteLine(@"{0} {1} {2}", board.Row, board.Col, board.SquareSize);
				}
			}

			public string RetrieveSampleInput()
			{
				return @"
3
4
3
7
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
4
2 2 2
2 0 2
0 2 2
0 0 2
6
2 2 1
2 1 1
2 0 1
1 2 1
0 2 1
0 0 2
9
5 5 2
5 3 2
4 6 1
4 3 1
4 0 3
3 6 1
3 4 2
0 4 3
0 0 4
";
			}
		}
	}

	internal class Board868
	{
		private readonly int _boardSize;
		private readonly int[,] _board;
		private readonly int _placeRow = -1;
		private readonly int _placeCol = -1;
		private int _bestSquareSize;

		/// <summary>
		/// Rows go from 0 at the top to _boardSize - 1 at the bottom.  We fill in squares scanning
		/// rowwise from the top left.
		/// 
		/// We avoid checking symmetric cases by ensuring that no corner square exceeds
		/// the upper left corner square in size and the lower left square does not exceed
		/// the upper right.  We therefore have to maintain the size of the squares in the
		/// upper left and upper right corners.
		/// </summary>
		private int _ulSize;
		private int _urSize;

		public int BoardSize
		{
			get { return _boardSize; }
		}

		public int Row
		{
			get { return _placeRow; }
		}

		public int Col
		{
			get { return _placeCol; }
		}

		public int SquareSize
		{
			get { return _bestSquareSize; }
		}

		/// <summary>
		/// Check that the lower right border of a square is empty
		/// </summary>
		/// <remarks>
		/// We use this to ascertain whether we can construct squares of successively larger sizes
		/// as we search for solutions without checking for an entire empty square each time we
		/// propose enlarging the trial square
		/// </remarks>
		/// <param name="row">Row of the square</param>
		/// <param name="col">Column of the square</param>
		/// <param name="size">Size of the square</param>
		/// <returns>True if the bottom row and rightmost column of the square is clear</returns>
		private bool CheckLowerRightBorderIsEmpty(int row, int col, int size)
		{
			// Bounds check
			if (row + size > BoardSize || col + size > BoardSize)
			{
				return false;
			}

			// Verify that the right border is empty
			for (var iRow = 0; iRow < size; iRow++)
			{
				if (_board[row + iRow, col + size - 1] != 0)
				{
					return false;
				}
			}

			// Verify that the bottom border is empty
			for (var iCol = 0; iCol < size - 1; iCol++)
			{
				if (_board[row + size - 1, col + iCol] != 0)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Clear the lower right border of a square.  This effectively reduces the size
		/// of the square by 1.
		/// </summary>
		/// <param name="row">Row of the square</param>
		/// <param name="col">Column of the square</param>
		/// <param name="size">Size of the square</param>
		private void ClearLowerRightBorder(int row, int col, int size)
		{
			for (var iRow = 0; iRow < size; iRow++)
			{
				_board[row + iRow, col + size - 1] = 0;
			}
			for (var iCol = 0; iCol < size - 1; iCol++)
			{
				_board[row + size - 1, col + iCol] = 0;
			}
		}

		/// <summary>
		/// Find the biggest square that will fit to the right and below a location
		/// </summary>
		/// <param name="row">Location row</param>
		/// <param name="col">Location column</param>
		/// <returns>Size of the biggest square that will fit at the location</returns>
		public int FindBiggestFit(int row, int col)
		{
			// This is the only case where we have to disallow a size of _boardSize...
			if (col == 0 && row == 0)
			{
				return BoardSize - 1;
			}

			// Grow the square below and to the right checking the lower and right border
			// at each expansion
			for (var size = 1; size < BoardSize - Math.Min(col, row) + 1; size++)
			{
				if (!CheckLowerRightBorderIsEmpty(row, col, size))
				{
					// Found a filled spot
					return size - 1;
				}
			}

			return BoardSize - Math.Min(col, row);
		}

		/// <summary>
		/// Fill in a square on the board
		/// </summary>
		/// <param name="row">Row of the square</param>
		/// <param name="col">Column of the square</param>
		/// <param name="size">Size of the square</param>
		public void FillSquare(int row, int col, int size)
		{
			for (var iRow = 0; iRow < size; iRow++)
			{
				for (var iCol = 0; iCol < size; iCol++)
				{
					_board[row + iRow, col + iCol] = 1;
				}
			}
		}

		/// <summary>
		/// Solve the board using maxAllowed or fewer squares
		/// </summary>
		/// <remarks>
		/// This board will already be filled in completely above _curRow
		/// </remarks>
		/// <param name="maxAllowed"></param>
		/// <returns></returns>
		public List<Board868> Solve(int maxAllowed)
		{
			// List of child boards leading to a solution - no such solution yet
			List<Board868> curBest = null;

			// Are there no more squares available?
			if (maxAllowed == 0)
			{
				// Not solvable
				return null;
			}

			// Have we filled up the board?
			if (_placeRow < 0)
			{
				// return the solution
				// ...which happens to be empty for this board - i.e., there are no
				// steps to solve this board - it's already solved - so return
				// the empty list.
				return new List<Board868>();
			}

			// Still searching for a solution

			// Find biggest size we can fit below and to the right at the current placement location
			var biggestSize = FindBiggestFit(_placeRow, _placeCol);
			// ...and fill it up
			FillSquare(_placeRow, _placeCol, biggestSize);

			// For all squares which will fit here
			for (var iSize = biggestSize; iSize > 0; iSize--)
			{
				// Is this an acceptable size for symmetry breaking?
				if (!BreaksSymmetry(iSize))
				{
					// Spawn a child board with the square set in place
					var newBoard = new Board868(BoardSize, this, _placeRow);
					// Recursively solve the new board with the minimal squares
					var trialSolution = newBoard.Solve(maxAllowed - 1);

					// Did we find a solution?
					if (trialSolution != null)
					{
						// This is our best current solution
						curBest = trialSolution;
						// Ensure that future solutions do better than this
						maxAllowed = trialSolution.Count + 1;
						// Record the currently best size to place at this location
						_bestSquareSize = iSize;
					}
				}

				// Try the next smaller sized square
				// We do this by clearing the lower right border
				ClearLowerRightBorder(_placeRow, _placeCol, iSize);
			}

			// Did we find a solution among our children?
			if (curBest != null)
			{
				// Add our board to it
				curBest.Add(this);
			}

			// Return best solution found
			return curBest;
		}

		private bool BreaksSymmetry(int iSize)
		{
			if (_placeRow == 0)
			{
				// Are we at the upper left side?
				if (_placeCol == 0)
				{
					// Set our Upper Left size - no other corner square can exceed this
					_ulSize = iSize;
				}
				// Are we at the upper right side?
				if (_placeCol + iSize == BoardSize)
				{
					// Are we larger than the upper left square?
					if (iSize > _ulSize)
					{
						// Skip it
						// We can't be larger than the upper left to avoid symmetric solutions
						return true;
					}
					// If it's acceptable, set _urSize.  The Lower Left square cannot exceed this
					_urSize = iSize;
				}
			}

			// Are we at the bottom border?
			if (_placeRow + iSize == BoardSize)
			{
				// Are we at the bottom left?
				if (_placeCol == 0)
				{
					// Are we larger than the upper right square?
					if (iSize > _urSize)
					{
						// Skip it
						// For symmetry breaking
						return true;
					}
				}
				// Are we at the bottom right?
				if (_placeCol + iSize == BoardSize)
				{
					// Are we larger than the upper left square?
					if (iSize > _ulSize)
					{
						// Skip it
						// For symmetry breaking
						return true;
					}
				}
			}
			// This is an acceptable size
			return false;
		}

		/// <summary>
		/// Constructor for a brand new board with nothing in it.
		/// </summary>
		/// <param name="boardSize">Size of the board</param>
		public Board868(int boardSize)
		{
			_boardSize = boardSize;
			_board = new int[boardSize, boardSize];
		}

		/// <summary>
		/// Constructor for a child board in the search process
		/// </summary>
		/// <remarks>
		/// When created, this board will have the next cell in the board that needs filling in
		/// _placeRow, _placeCol.  We scan from the top down so everything above _placeRow is
		/// guaranteed to be filled in the resulting board.
		/// </remarks>
		/// <param name="boardSize">Size of the board</param>
		/// <param name="parent">Parent in the search process</param>
		/// <param name="searchRow">Where we should begin the search</param>
		public Board868(int boardSize, Board868 parent, int searchRow)
		{
			// Inherit stuff from the parent
			_boardSize = boardSize;
			_board = (int[,])parent._board.Clone();
			_ulSize = parent._ulSize;
			_urSize = parent._urSize;

			// Search out the next cell that needs filling
			for (var iRow = searchRow; iRow < BoardSize; iRow++)
			{
				for (var iCol = 0; iCol < BoardSize; iCol++)
				{
					// Searching for an empty space
					if (_board[iRow, iCol] == 0)
					{
						// If we find one then we'll start there
						_placeCol = iCol;
						_placeRow = iRow;
						return;
					}
				}
			}
		}
	}
}
