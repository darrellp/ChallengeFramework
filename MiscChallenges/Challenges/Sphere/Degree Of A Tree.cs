using System.Collections.Generic;
using System.Diagnostics;
using static System.Console;
using static System.Math;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("Sphere", "Degree of a Tree", "http://www.spoj.com/problems/TREEDEGREE/")]
        // ReSharper disable once InconsistentNaming
        public class DegreeOfATree : IChallenge
        {
            public void Solve()
            {
                ReadLine();
                while (GetValue() != null)
                {
                    var treeVals = GetVals();

                    GetDegree(treeVals, out int treeDegree, out _);
                    WriteLine(treeDegree);
                }
            }

            private void GetDegree(List<int> treeVals, out int treeDegree, out int rootDegree)
            {
                Debug.Assert(treeVals[0] == treeVals[treeVals.Count - 1]);
                var nextVertexLocation = 1;
                treeDegree = rootDegree = 0;

                while (nextVertexLocation != treeVals.Count - 1)
                {
                    rootDegree++;

                    var lastVertexLocation =
                        treeVals.FindIndex(nextVertexLocation + 1, v => v == treeVals[nextVertexLocation]);

                    if (lastVertexLocation != nextVertexLocation + 1)
                    {
                        var subtree = treeVals.GetRange(nextVertexLocation, lastVertexLocation - nextVertexLocation + 1);
                        GetDegree(subtree, out int subTreeDegree, out int subRootDegree);
                        treeDegree = Max(treeDegree, subTreeDegree);
                        treeDegree = Max(treeDegree, subRootDegree + 1);
                    }

                    nextVertexLocation = lastVertexLocation + 1;
                }
                treeDegree = Max(treeDegree, rootDegree);
            }

            public string RetrieveSampleInput()
            {
                return @"
2
1
1 1
5
1 3 2 2 4 4 5 5 3 1
";
            }

            public string RetrieveSampleOutput()
            {
                return @"
0
4
";
            }
        }
    }
}
