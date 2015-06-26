using System;
// ReSharper disable AssignNullToNotNullAttribute
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		public static ulong Fact(int n)
		{
			var ret = 1UL;

			for (var i = 2L; i <= n; i++)
			{
				ret *= (ulong)i;
			}
			return ret;
		}

		public static ulong Comb(int n, int k)
		{
			k = Math.Min(k, n - k);
			ulong den = 1;
			for (var i = (ulong)(n - k + 1); i <= (ulong)n; i++)
			{
				den *= i;
			}
			return den / Fact(k);
		}

		public static int GetVal(StringReader str)
		{
			return int.Parse(str.ReadLine());
		}

		public static List<int> GetVals(StringReader str)
		{
			var line = str.ReadLine();
			if (line == null)
			{
				return null;
			}
			return line.Split(' ').
				Select(int.Parse).
				ToList();
		}

		public static List<double> GetDblVals(StringReader str)
		{
			var line = str.ReadLine();
			if (line == null)
			{
				return null;
			}
			return line.Split(' ').
				Select(double.Parse).
				ToList();
		}

		public static IEnumerable<T> Iterate<T>(T start, Func<T, T> next, Func<T, bool> terminate = null)
		{
			var cur = start;
			while (terminate == null || !terminate(cur))
			{
				yield return cur;
				cur = next(cur);
			}
			yield return cur;
		}

		public static IEnumerable<int> IndexWhere<T>(this IEnumerable<T> e, Func<T, bool> f)
		{
			var i = 0;
			foreach (var var in e)
			{
				if (f(var))
				{
					yield return i;
				}
				i++;
			}
		}
	}
}
