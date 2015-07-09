using System;
using System.Text;
using System.Text.RegularExpressions;
// ReSharper disable AssignNullToNotNullAttribute
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		public static void WriteLine(this StringBuilder sb, string format, params object[] vals)
		{
			sb.Append(string.Format(format, vals) + Environment.NewLine);
		}

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

		public static int GetVal()
		{
			return int.Parse(Console.ReadLine());
		}

		public static List<int> GetVals()
		{
			var line = Console.ReadLine();
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

		public static string SolveInOut(StringReader stm, Action<string[]> main)
		{
			Console.SetIn(stm);
			using (var stream = new MemoryStream())
			{
				using (var output = new StreamWriter(stream, Encoding.UTF8))
				{
					Console.SetOut(output);
					main(null);
					output.Flush();
					stream.Position = 0;
					using (var streamReader = new StreamReader(stream))
					{
						return streamReader.ReadToEnd();
					}
				}
			}
		}
	}
}
