using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Priority_Queue;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("UVA", "Black Box",
			"https://uva.onlinejudge.org/index.php?option=com_onlinejudge&Itemid=8&category=7&page=show_problem&problem=442")]
		// ReSharper disable once InconsistentNaming
		public class BlackBox : IChallenge
		{
			public string Solve(StringReader stm)
			{
				var ret = new StringBuilder();
				var nCases = GetVal(stm);
				bool firstCase = true;
				for (var iCase = 0; iCase < nCases; iCase++)
				{
					// Skip the blank line between cases in the input
					stm.ReadLine();

					// Write the blank line between output cases if necessary
					if (!firstCase)
					{
						ret.WriteLine("");
					}
					firstCase = false;

					// Retrieve the next case
					var caseCur = new BlackBoxCase(stm);

					// Write each value in the solution on a separate line
					foreach (var getValue in caseCur.Solve())
					{
						ret.WriteLine("{0}", getValue);
					}
				}
				return ret.ToString();
			}

			public class BlackBoxCase
			{
				private readonly List<int> _added;
				private readonly List<int> _getCounts;
 
				public BlackBoxCase(StringReader stm)
				{
					stm.ReadLine();		// We don't really need to know these numbers - they're implicit
					_added = GetVals(stm);
					_getCounts = GetVals(stm);
				}

				public IEnumerable<int> Solve()
				{
					var pq = new NthPriorityQueue<int>();
					var iAdd = 0;

					foreach (int t in _getCounts)
					{
						for (; iAdd < t; iAdd++)
						{
							pq.Add(_added[iAdd]);
						}
						yield return pq.Peek();
						pq.N++;
					}
				}
			}

			/// <summary>
			/// Keep a priority queue where we pull the n'th smallest rather than the smallest
			/// </summary>
			public class NthPriorityQueue<T> where T : IComparable
			{
				readonly BinaryPriorityQueue<T> _smallest = new BinaryPriorityQueue<T>((n1, n2) => n2.CompareTo(n1));
				readonly BinaryPriorityQueue<T> _largest = new BinaryPriorityQueue<T>();
				private int _n;

				public NthPriorityQueue()
				{
					_n = 0;
				}

				public NthPriorityQueue(int n)
				{
					_n = n;
				}

				public int N
				{
					get { return _n; }
					set
					{
						if (value < 0)
						{
							throw new ArgumentException("Invalid N in NthPriorityQueue");
						}
						if (value == _n)
						{
							return;
						}

						var oldN = _n;
						_n = value;
						if (oldN < _n)
						{
							while (oldN < _n && _largest.Count > 0)
							{
								_smallest.Add(_largest.Pop());
								oldN++;
							}
						}
						else
						{
							while (oldN > _n)
							{
								_largest.Add(_smallest.Pop());
								oldN--;
							}
						}
					}
				}

				public void Add(T val)
				{
					if (_smallest.Count < N)
					{
						_smallest.Add(val);
						return;
					}
					if (N != 0 && val.CompareTo(_smallest.Peek()) < 0)
					{
						_smallest.Add(val);
						val = _smallest.Pop();
					}
					_largest.Add(val);
				}

				public T Peek()
				{
					return _largest.Peek();
				}

				public T Pop()
				{
					return _largest.Pop();
				}
			}

			public string RetrieveSampleInput()
			{
				return @"
1

7 4
3 1 -4 2 8 -1000 2
1 2 6 6
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
3
3
1
2
";
			}
		}
	}
}
