using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using static System.Console;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("ProgChallenges", "Erdos Numbers",
			"https://onlinejudge.org/index.php?option=com_onlinejudge&Itemid=8&page=show_problem&problem=985")]
		public class ErdosNumbers : IChallenge
		{
			public void Solve()
			{
				var ret = new StringBuilder();

				var cScenarios = GetVal();
				for (var iScenario = 0; iScenario < cScenarios; iScenario++)
				{
					var scenario = new Scenario();
					ret.Append("Scenario " + (iScenario + 1) + Environment.NewLine);
					ret.Append(scenario.Solve());
				}
				Write(ret.ToString());
			}

			public string RetrieveSampleInput()
			{
				return @"
1
4 3
Smith, M.N., Martin, G., Erdos, P.: Newtonian forms of prime factors
Erdos, P., Reisig, W.: Stuttering in petri nets
Smith, M.N., Chen, X.: First order derivates in structured programming
Jablonski, T., Hsueh, Z.: Selfstabilizing data structures
Smith, M.N.
Hsueh, Z.
Chen, X.
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
Scenario 1
Smith, M.N. 1
Hsueh, Z. infinity
Chen, X. 2
";
			}
		}
	}

	public class Scenario
	{
		readonly List<Paper> _papers = new List<Paper>();
		readonly List<string> _authors = new List<string>();
		readonly Dictionary<string, ErdosVertex> _authorDict = new Dictionary<string, ErdosVertex>();

		internal class ErdosVertex : AStarState
		{
			private readonly string _author;
			private readonly HashSet<ErdosVertex> _collaborators = new HashSet<ErdosVertex>();

			static ErdosVertex InstallAuthor(string author, Dictionary<string, ErdosVertex> authorDict)
			{
				if (authorDict.ContainsKey(author))
				{
					return authorDict[author];
				}
				var ret = new ErdosVertex(author);
				authorDict[author] = ret;
				return ret;
			}

			public ErdosVertex(string author)
			{
				_author = author;
			}

			public static void AddCollaborators(string author, IEnumerable<string> collaborators,
				Dictionary<string, ErdosVertex> authorDict)
			{
				InstallAuthor(author, authorDict)._collaborators.UnionWith(collaborators.Where(a => a != author).Select(s => InstallAuthor(s, authorDict)).ToList());
			}

			public override IEnumerable<IState> Successors()
			{
				return _collaborators;
			}

			public override bool IsEqual(IState istate)
			{
				var other = istate as ErdosVertex;
				if (other == null)
				{
					throw new InvalidOperationException("comparing wrong type in ErdosVertex");
				}
				return _author == other._author;
			}

			public override int GetHashCode()
			{
				return _author.GetHashCode();
			}
		}

		internal class Paper
		{
			internal readonly List<string> Authors = new List<string>();

			internal Paper()
			{
				var line = ReadLine();
				// ReSharper disable once PossibleNullReferenceException
				var split = line.IndexOf(':');
				var names = line.Substring(0, split).Split(',').ToList();
				for (var iName = 0; iName < names.Count / 2; iName++)
				{
					Authors.Add(names[iName * 2].Trim() + "," + names[iName * 2 + 1]);
				}
			}
		}

		public Scenario()
		{
			// ReSharper disable once PossibleNullReferenceException
			var counts = ReadLine().Split(' ').Select(int.Parse).ToArray();
			var cPapers = counts[0];
			var cAuthors = counts[1];

			for (var iPaper = 0; iPaper < cPapers; iPaper++)
			{
				var curPaper = new Paper();
				_papers.Add(curPaper);
				foreach (var author in curPaper.Authors)
				{
					ErdosVertex.AddCollaborators(author, curPaper.Authors, _authorDict);
				}
			}
			for (var iAuthor = 0; iAuthor < cAuthors; iAuthor++)
			{
				_authors.Add(ReadLine());
			}
		}

		public string Solve()
		{
			var sb = new StringBuilder();
			var erdos = _authorDict["Erdos, P."];

			foreach (var author in _authors)
			{
				var astar = new AStar<ErdosVertex>(erdos, _authorDict[author]);
				var solution = astar.Solve();
				sb.Append(String.Format("{0} {1}", author, solution == null ? "infinity" : (solution.Count - 1).ToString()) + Environment.NewLine);
			}
			return sb.ToString();
		}
	}
}
