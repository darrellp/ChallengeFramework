using System;
using System.Collections.Generic;
using System.Linq;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("ProgChallenges", "File Fragmentation",
            "http://www.programming-challenges.com/pg.php?page=downloadproblem&probid=110306&format=html")]
        public class FileFragmentation : IChallenge
        {
            public void Solve()
            {
                var cCases = GetVal();
                Console.ReadLine();

                for (var iCase = 0; iCase < cCases; iCase++)
                {
                    var fragments = new List<string>();
                    string curFragment;
                    while ((curFragment = Console.ReadLine()) != null)
                    {
                        if (curFragment == String.Empty)
                        {
                            break;
                        }
                        fragments.Add(curFragment);
                    }
                    SolvePuzzle(fragments);
                }
            }

            private void SolvePuzzle(List<string> fragments)
            {
                var totalChars = fragments.Select(f => f.Length).Sum();
                var fileLength = totalChars * 2 / fragments.Count;
                // Find any potential "other halves" for the first fragment
                var otherHalfLength = fileLength - fragments[0].Length;
                foreach (var otherHalf in fragments.Where(f => f.Length == otherHalfLength))
                {
                    // Hypothesize first frag is start and other frag is end
                    var testFile = fragments[0] + otherHalf;
                    if (Check(testFile, fragments, fileLength))
                    {
                        Console.WriteLine(testFile);
                        return;
                    }
                    // Okay, try the other way around
                    testFile = otherHalf + fragments[0];
                    if (Check(testFile, fragments, fileLength))
                    {
                        Console.WriteLine(testFile);
                        return;
                    }
                }
            }

            private bool Check(string testFile, List<string> fragments, int fileLength)
            {
                var checkedOut = new bool[fragments.Count];

                for (var iThis = 0; iThis < fragments.Count; iThis++)
                {
                    // If this one has been checked out then continue on to the next
                    if (checkedOut[iThis])
                    {
                        continue;
                    }

                    var thisHalf = fragments[iThis];
                    var otherHalfLength = fileLength - fragments[iThis].Length;

                    for(var iOther = iThis + 1; iOther < fragments.Count; iOther++)
                    {
                        if (checkedOut[iOther])
                        {
                            continue;
                        }
                        var otherHalf = fragments[iOther];
                        if (otherHalf.Length != otherHalfLength)
                        {
                            continue;
                        }
                        // Check if our other half fits with this half.
                        if (thisHalf + otherHalf == testFile || otherHalf + thisHalf == testFile)
                        {
                            checkedOut[iThis] = checkedOut[iOther] = true;
                            break;
                        }
                    }
                    // If we couldn't find a match then return false
                    if (!checkedOut[iThis])
                    {
                        return false;
                    }
                }
                return true;
            }

            public string RetrieveSampleInput()
            {
                return @"
1

011
0111
01110
111
0111
10111
";
            }

            public string RetrieveSampleOutput()
            {
                return @"
01110111
";
            }
        }
    }
}
