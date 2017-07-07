using System;
using System.Text;
using System.Text.RegularExpressions;
using static System.Console;
// ReSharper disable AssignNullToNotNullAttribute
using System.Collections.Generic;
using System.Linq;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		public static string CppStringToCs(string cppString)
		{
			return Regex.Replace(cppString, "\n", Environment.NewLine);
		}

		public static string CsStringToCpp(string cppString)
		{
			return Regex.Replace(cppString, Environment.NewLine, "\n");
		}

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

	    private static string ReadAll()
	    {
	        string nextLine;
	        StringBuilder sb = new StringBuilder();

	        while ((nextLine = ReadLine()) != null)
	        {
	            sb.Append(nextLine + "\r\n");
	        }
	        return sb.ToString();
	    }

		public static int GetVal()
		{
			return int.Parse(ReadLine());
		}

		public static List<int> GetVals()
		{
			var line = ReadLine();
		    return line?.Split(' ').
			    Select(int.Parse).
			    ToList();
		}

		public static List<double> GetDblVals()
		{
			var line = ReadLine();
		    return line?.Split(' ').
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
