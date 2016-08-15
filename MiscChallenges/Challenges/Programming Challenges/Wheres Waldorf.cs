using System.Collections.Generic;
using static System.Console;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("ProgChallenges", "Where's Waldorf",
            "http://www.programming-challenges.com/pg.php?page=downloadproblem&probid=110302&format=html")]
        public class WheresWaldorf : IChallenge
        {
            public class WaldorfCase
            {
                private readonly int _rows;
                private readonly int _columns;
                private readonly List<string> _grid;
                private readonly List<string> _searchWords;

                public WaldorfCase()
                {
                    ReadLine();
                    var dims = GetVals();
                    _rows = dims[0];
                    _columns = dims[1];
                    _grid = new List<string>(_rows);
                    // ReSharper disable PossibleNullReferenceException
                    for (var iRow = 0; iRow < _rows; iRow++)
                    {
                        _grid.Add(ReadLine().ToUpper());
                    }
                    var cWords = GetVal();
                    _searchWords = new List<string>(cWords);
                    for (var iWord = 0; iWord < cWords; iWord++)
                    {
                        _searchWords.Add(ReadLine().ToUpper());
                    }
                    // ReSharper restore PossibleNullReferenceException
                }

                public void Solve()
                {
                    foreach (var word in _searchWords)
                    {
                        GetPosition(word);
                    }
                }

                private void GetPosition(string word)
                {
                    for (var iRow = 0; iRow < _rows; iRow++)
                    {
                        for (var iCol = 0; iCol < _columns; iCol++)
                        {
                            if (_grid[iRow][iCol] == word[0] && CheckLocation(iRow, iCol, word))
                            {
                                WriteLine($"{iRow + 1} {iCol + 1}");
                            }
                        }
                    }
                }

                private bool CheckLocation(int iRow, int iCol, string word)
                {
                    for (var dx = -1; dx <= 1; dx++)
                    {
                        for (var dy = -1; dy <= 1; dy++)
                        {
                            if (dx == 0 && dy == 0)
                            {
                                continue;
                            }
                            if (CheckDirection(iRow, iCol, dx, dy, word))
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                }

                private bool CheckDirection(int iRow, int iCol, int dx, int dy, string word)
                {
                    var lastRow = iRow + (word.Length - 1) * dy;
                    var lastCol = iCol + (word.Length - 1) * dx;
                    if (lastRow < 0 || lastRow >= _rows || lastCol < 0 || lastCol >= _columns)
                    {
                        return false;
                    }
                    var curRow = iRow;
                    var curCol = iCol;
                    foreach (var t in word)
                    {
                        if (_grid[curRow][curCol] != t)
                        {
                            return false;
                        }
                        curRow += dy;
                        curCol += dx;
                    }
                    return true;
                }
            }

            public void Solve()
            {
                var cCases = GetVal();

                for (var iElection = 0; iElection < cCases; iElection++)
                {
                    var caseCur = new WaldorfCase();
                    caseCur.Solve();
                }
            }

            public string RetrieveSampleInput()
            {
                return @"
1

8 11
abcDEFGhigg
hEbkWalDork
FtyAwaldORm
FtsimrLqsrc
byoArBeDeyv
Klcbqwikomk
strEBGadhrb
yUiqlxcnBjf
4
Waldorf
Bambi
Betty
Dagbert
";
            }

            public string RetrieveSampleOutput()
            {
                return @"
2 5
2 3
1 2
7 8
";
            }
        }
    }
}
