using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("ProgChallenges", "Australian Voting",
            "http://www.programming-challenges.com/pg.php?page=downloadproblem&probid=110108&format=html")]
        public class AustralianVoting : IChallenge
        {
            public void Solve()
            {
                var cCases = GetVal();
                ReadLine();
                var firstTime = true;

                for (var iElection = 0; iElection < cCases; iElection++)
                {
                    var votes = new Votes();
                    var winners = votes.Solve();

                    Write((firstTime ? string.Empty : Environment.NewLine));
                    firstTime = false;

                    foreach (var winner in winners)
                    {
                        WriteLine(winner);
                    }
                }
            }

            public class Votes
            {
                private readonly List<string> _candidates;
                private readonly List<List<int>> _ballots = new List<List<int>>();
                private readonly List<bool> _eliminated = new List<bool>();

                public Votes()
                {
                    var cCandidates = GetVal();
                    _candidates = new List<string>();
                    for (var i = 0; i < cCandidates; i++)
                    {
                        _candidates.Add(ReadLine());
                        _eliminated.Add(false);
                    }

                    string ballots;
                    while ((ballots = ReadLine()) != string.Empty && ballots != null)
                    {
                        _ballots.Add(
                            ballots.Split(' ').
                            Select(int.Parse).
                            ToList());
                    }
                }

                // ReSharper disable once ReturnTypeCanBeEnumerable.Global
                public List<string> Solve()
                {
                    while (true)
                    {
                        var tallies = GetTallies();

                        if (FoundWinners(tallies))
                        {
                            break;
                        }
                        RemoveLosers(tallies);
                    }

                    return _eliminated.
                        IndexWhere(f => !f).
                        Select(i => _candidates[i]).
                        ToList();
                }

                private bool FoundWinners(List<int> tallies)
                {
                    var max = tallies.Max();
                    var winners = tallies.IndexWhere(t => t == max).ToList();
                    var oneWinner = winners.Count == 1 && tallies[winners[0]] > _ballots.Count / 2;

                    if (oneWinner)
                    {
                        for (var i = 0; i < _candidates.Count; i++)
                        {
                            if (i != winners[0])
                            {
                                _eliminated[i] = true;
                            }
                        }
                    }

                    return oneWinner || tallies.All(c => c == max);
                }

                private void RemoveLosers(IReadOnlyList<int> tallies)
                {
                    var min = tallies.Where(c => c != 0).Min();
                    for (var i = 0; i < tallies.Count; i++)
                    {
                        if (tallies[i] == min)
                        {
                            _eliminated[i] = true;
                        }
                    }
                }

                private List<int> GetTallies()
                {
                    var tallies = new List<int>(_candidates.Count);

                    for (var i = 0; i < _candidates.Count; i++)
                    {
                        tallies.Add(0);
                    }

                    foreach (var ballot in _ballots)
                    {
                        tallies[ballot.First(c => !_eliminated[c - 1]) - 1]++;
                    }
                    return tallies;
                }
            }

            public string RetrieveSampleInput()
            {
                // We eliminate this first newline in the caller so that the uninterrupted input
                // can go at the left hand column.
                return @"
2

3
John Doe
Jane Smith
Jane Austen
1 2 3
2 1 3
2 3 1
1 2 3
3 1 2

3
John Doe
Jane Smith
Jane Austen
1 2 3
2 3 1
3 1 2
";
            }

            public string RetrieveSampleOutput()
            {
                // Caller will eliminate first newline...
                return @"
John Doe

John Doe
Jane Smith
Jane Austen
";
            }
        }
    }
}