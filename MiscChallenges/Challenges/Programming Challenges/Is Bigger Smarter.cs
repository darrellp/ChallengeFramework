using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("ProgChallenges", "Is Bigger Smarter?",
            "http://www.programming-challenges.com/pg.php?page=downloadproblem&probid=111101&format=html")]
        public class IsBiggerSmarter : IChallenge
        {
            public void Solve()
            {
                var solver = LocalSolver.ReadCase();
                solver.Solve();
            }

            public string RetrieveSampleInput()
            {
                return @"
6008 1300
6000 2100
500 2000
1000 4000
1100 3000
6000 2000
8000 1400
6000 1200
2000 1900
";
            }

            public string RetrieveSampleOutput()
            {
                return @"
4
4
5
2
7
";
            }

            private class LocalSolver
            {
                private readonly List<ElephantDatum> _data = new List<ElephantDatum>();

                public static LocalSolver ReadCase()
                {
                    string line;
                    var ret = new LocalSolver();
                    var iData = 1;

                    while ((line = ReadLine()) != null)
                    {
                        var stgs = line.Split(' ').Where(s => s != string.Empty).ToArray();
                        ret._data.Add(new ElephantDatum(int.Parse(stgs[0]), int.Parse(stgs[1]), iData++));
                    }
                    return ret;
                }

                // Amounts to finding the longest path in a DAG...
                public void Solve()
                {
                    var sortedList = _data.OrderBy(ed => ed.Weight * 10000 - ed.Iq);
                    var nodes = new List<ElephantNode>(_data.Count);
                    nodes.AddRange(sortedList.Select(datum => new ElephantNode(datum)));
                    nodes = nodes.OrderByDescending(en => en.Iq).ToList();

                    var maxPathLength = -1;
                    var inodeMax = -1;
                    for (var iNode = 0; iNode < nodes.Count; iNode++)
                    {
                        FindPathLength(iNode, nodes);
                        if (nodes[iNode].PathLength > maxPathLength)
                        {
                            maxPathLength = nodes[iNode].PathLength;
                            inodeMax = iNode;
                        }
                    }

                    var solution = new List<ElephantNode>();

                    for (; inodeMax >= 0; inodeMax = nodes[inodeMax].PrevIndex)
                    {
                        solution.Add(nodes[inodeMax]);
                    }

                    solution.Reverse();

                    WriteLine(solution.Count);
                    foreach (var node in solution)
                    {
                        WriteLine(node.OriginalIndex);
                    }
                }

                private void FindPathLength(int iNode, List<ElephantNode> nodes)
                {
                    var node = nodes[iNode];

                    for (var iNodeProbe = 0; iNodeProbe < iNode; iNodeProbe++)
                    {
                        var curNode = nodes[iNodeProbe];
                        if (curNode.Weight < node.Weight &&
                            curNode.Iq != node.Iq &&
                            curNode.PathLength + 1 > node.PathLength)
                        {
                            node.PathLength = curNode.PathLength + 1;
                            node.PrevIndex = iNodeProbe;
                        }
                    }
                }

                private struct ElephantDatum
                {
                    internal readonly int Weight;
                    internal readonly int OriginalIndex;
                    internal readonly int Iq;

                    public ElephantDatum(int weight, int iq, int originalIndex)
                    {
                        Weight = weight;
                        OriginalIndex = originalIndex;
                        Iq = iq;
                    }
                }

                private class ElephantNode
                {
                    private ElephantDatum _datum;
                    internal int Iq { get { return _datum.Iq; } }
                    internal int Weight { get { return _datum.Weight; } }
                    internal int OriginalIndex { get { return _datum.OriginalIndex; } }
                    internal int PathLength { get; set; }
                    internal int PrevIndex { get; set; }

                    public ElephantNode(ElephantDatum datum)
                    {
                        _datum = datum;
                        PathLength = 0;
                        PrevIndex = -1;
                    }
                }
            }
        }
    }
}
