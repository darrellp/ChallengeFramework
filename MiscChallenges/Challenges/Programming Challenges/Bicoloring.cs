using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using edge = System.Tuple<int, int>;
using static System.Console;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("ProgChallenges", "Bicoloring",
            "http://www.programming-challenges.com/pg.php?page=downloadproblem&probid=110901&format=html")]
        public class Bicoloring : IChallenge
        {
            public void Solve()
            {
                BicoloringCase nextCase;
                while ((nextCase = BicoloringCase.NextCase()) != null)
                {
                    nextCase.Solve();
                }
            }

            enum Color
            {
                Red,
                Black,
                Uncolored
            }
            public class BicoloringCase
            {
                private int _cVertices;
                private int _cEdges;
                private Color[] _vertexColors;
                private readonly List<edge> _edges = new List<edge>();
                private List<int>[] _vertexToAdjacentEdges; 

                public bool Load()
                {
                    _cVertices = GetVal();
                    if (_cVertices == 0)
                    {
                        return false;
                    }
                    _vertexToAdjacentEdges = new List<int>[_cVertices];
                    _vertexColors = Enumerable.Repeat(Color.Uncolored, _cVertices).ToArray();
                    _cEdges = GetVal();
                    for (int iEdge = 0; iEdge < _cEdges; iEdge++)
                    {
                        var edgeVertices = GetVals();
                        _edges.Add(new edge(edgeVertices[0], edgeVertices[1]));
                        AddEdgeAdjacencies(edgeVertices[0], edgeVertices[1], _edges.Count - 1);
                    }
                    return true;
                }

                private void AddEdgeAdjacencies(int vtx1, int vtx2, int edgeIndex)
                {
                    SetEdgeAdjacency(vtx1, edgeIndex);
                    SetEdgeAdjacency(vtx2, edgeIndex);
                }

                private void SetEdgeAdjacency(int vtx, int edgeIndex)
                {
                    if (_vertexToAdjacentEdges[vtx] == null)
                    {
                        _vertexToAdjacentEdges[vtx] = new List<int>();
                    }
                    _vertexToAdjacentEdges[vtx].Add(edgeIndex);
                }

                public static BicoloringCase NextCase()
                {
                    var newCase = new BicoloringCase();
                    return newCase.Load() ? newCase : null;
                }

                Color Opposite(Color clr)
                {
                    Debug.Assert(clr != Color.Uncolored, "Trying to find opposite color from Uncolored");
                    return clr == Color.Black ? Color.Red : Color.Black;
                }

                public void Solve()
                {
                    var edgeIndicesToBeChecked = new Queue<int>();
                    var edgesChecked = new bool[_cEdges];

                    while (!edgesChecked.All(f => f))
                    {
                        var iStartEdge = edgesChecked.TakeWhile(f => f).Count();

                        // Arbitrarily color the first vertex black.
                        _vertexColors[_edges[iStartEdge].Item1] = Color.Black;
                        edgeIndicesToBeChecked.Enqueue(iStartEdge);

                        while (edgeIndicesToBeChecked.Count != 0)
                        {
                            var edgeIndex = edgeIndicesToBeChecked.Dequeue();
                            if (edgesChecked[edgeIndex])
                            {
                                continue;
                            }
                            var edge = _edges[edgeIndex];
                            var vtx1 = edge.Item1;
                            var vtx2 = edge.Item2;
                            // One end or the other had ought to already be colored - that's how an edge gets into
                            // the queue - so if both ends are equal, they're equal to the same color.
                            Debug.Assert(_vertexColors[vtx1] != Color.Uncolored ||
                                         _vertexColors[vtx2] != Color.Uncolored);
                            if (_vertexColors[vtx1] == _vertexColors[vtx2])
                            {
                                WriteLine(@"NOT BICOLORABLE.");
                                return;
                            }
                            edgesChecked[edgeIndex] = true;
                            if (_vertexColors[vtx1] == Color.Uncolored)
                            {
                                _vertexColors[vtx1] = Opposite(_vertexColors[vtx2]);
                                foreach (var iEdge in _vertexToAdjacentEdges[vtx1])
                                {
                                    edgeIndicesToBeChecked.Enqueue(iEdge);
                                }
                            }
                            else if (_vertexColors[vtx2] == Color.Uncolored)
                            {
                                _vertexColors[vtx2] = Opposite(_vertexColors[vtx1]);
                                foreach (var iEdge in _vertexToAdjacentEdges[vtx2])
                                {
                                    edgeIndicesToBeChecked.Enqueue(iEdge);
                                }
                            }
                        }
                    }
                    WriteLine(@"BICOLORABLE.");
                }
            }

            public string RetrieveSampleInput()
            {
                return @"
3
3
0 1
1 2
2 0
9
8
0 1
0 2
0 3
0 4
0 5
0 6
0 7
0 8
0
";
            }

            public string RetrieveSampleOutput()
            {
                return @"
NOT BICOLORABLE.
BICOLORABLE.
";
            }
        }
    }
}
