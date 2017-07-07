using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using static System.Console;
using static System.Math;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("Advent of Code", "No Time for a Taxicab", "https://adventofcode.com/2016/day/1")]
        public class NoTimeForATaxicab : IChallenge
        {
            public void Solve()
            {
                var directions = new[]
                {
                    new Point(0, 1),
                    new Point(1, 0),
                    new Point(0, -1),
                    new Point(-1, 0),
                };

                var input = ReadLine();
                // ReSharper disable once PossibleNullReferenceException
                var output = input
                    .Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                    .Scan(new { di = 0, pt = new Point(0, 0) }, (cp, s) =>
                    {
                        var newdi = (cp.di + (s[0] == 'R' ? 5 : 3)) % 4;
                        return new { di = newdi, pt = cp.pt + int.Parse(s.Substring(1)) * directions[newdi] };
                    })
                    .Last().pt
                    .ManhattanNorm();
                WriteLine(output);

                var visited = new HashSet<Point>();
                var ptCur = new Point(0, 0);
                var directionIndex = 0;
                visited.Add(ptCur);
                bool fFound = false;

                var instructions = input.Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (var instruction in instructions)
                {
                    directionIndex = (directionIndex + (instruction[0] == 'R' ? 3 : 5)) % 4;
                    var direction = directions[directionIndex];
                    var length = int.Parse(instruction.Substring(1));
                    for (int iStep = 0; iStep < length && !fFound; iStep++)
                    {
                        var newPt = ptCur + direction;
                        if (visited.Contains(newPt))
                        {
                            WriteLine(newPt.ManhattanNorm());
                            fFound = true;
                        }
                        visited.Add(newPt);
                        ptCur = newPt;
                    }
                    if (fFound)
                    {
                        break;
                    }
                }
            }

            struct Point
        {
            private readonly int _x;
            private readonly int _y;

            public Point(int x, int y)
            {
                _x = x;
                _y = y;
            }

            public static Point operator +(Point p1, Point p2)
            {
                return new Point(p1._x + p2._x, p1._y + p2._y);
            }

            public static Point operator *(int s, Point p)
            {
                return new Point(s * p._x, s * p._y);
            }

            public double ManhattanNorm()
            {
                return Abs(_x) + Abs(_y);
            }

            public override string ToString()
            {
                return $"({_x}, {_y})";
            }
        }
            public string RetrieveSampleInput()
            {
                return @"
                R2, L1, R2, R1, R1, L3, R3, L5, L5, L2, L1, R4, R1, R3, L5, L5, R3, L4, L4, R5, R4, R3, L1, L2, R5, R4, L2, R1, R4, R4, L2, L1, L1, R190, R3, L4, R52, R5, R3, L5, R3, R2, R1, L5, L5, L4, R2, L3, R3, L1, L3, R5, L3, L4, R3, R77, R3, L2, R189, R4, R2, L2, R2, L1, R5, R4, R4, R2, L2, L2, L5, L1, R1, R2, L3, L4, L5, R1, L1, L2, L2, R2, L3, R3, L4, L1, L5, L4, L4, R3, R5, L2, R4, R5, R3, L2, L2, L4, L2, R2, L5, L4, R3, R1, L2, R2, R4, L1, L4, L4, L2, R2, L4, L1, L1, R4, L1, L3, L2, L2, L5, R5, R2, R5, L1, L5, R2, R4, R4, L2, R5, L5, R5, R5, L4, R2, R1, R1, R3, L3, L3, L4, L3, L2, L2, L2, R2, L1, L3, R2, R5, R5, L4, R3, L3, L4, R2, L5, R5";
            }

            public string RetrieveSampleOutput()
            {
                return null;
            }
        }
    }
}
