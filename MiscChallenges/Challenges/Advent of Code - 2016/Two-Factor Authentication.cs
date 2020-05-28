using System;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Console;
using static RegexStringLibrary.Stex;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("Advent of Code 2016", "Two-Factor Authentication (8)", "https://adventofcode.com/2016/day/8")]
        public class TwoFactorAuthentication : IChallenge
        {
            public void Solve()
            {
                var input = ReadAll();

                var screen = new bool[Rows][];
                for (int i = 0; i < Rows; i++)
                {
                    screen[i] = new bool[Cols];
                }

                var instructions = input.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (var instruction in instructions)
                {
                    if (instruction.StartsWith("rect"))
                    {
                        var match = RegexRect.Match(instruction);
                        var rows = int.Parse(match.Groups["rows"].Value);
                        var cols = int.Parse(match.Groups["cols"].Value);
                        for (var iRow = 0; iRow < rows; iRow++)
                        {
                            for (var iCol = 0; iCol < cols; iCol++)
                            {
                                screen[iRow][iCol] = true;
                            }
                        }
                    }
                    else
                    {
                        var match = RegexRotate.Match(instruction);
                        var index = int.Parse(match.Groups["index"].Value);
                        var amount = int.Parse(match.Groups["amount"].Value);
                        var fRow = instruction[7] == 'r';
                        if (fRow)
                        {
                            Array.Copy(screen[index], Cols - amount, _shiftOut, 0, amount);
                            Array.ConstrainedCopy(screen[index], 0, screen[index], amount, Cols - amount);
                            Array.Copy(_shiftOut, screen[index], amount);
                        }
                        else
                        {
                            var shiftCol = Enumerable
                                .Range(0, Rows)
                                .Select(iRow => screen[(iRow + Rows - amount) % Rows][index])
                                .ToList();
                            for (int iRow = 0; iRow < Rows; iRow++)
                            {
                                screen[iRow][index] = shiftCol[iRow];
                            }
                        }
                    }
                }

                var output = screen.Sum(x => x.Sum(v => v ? 1 : 0));
                WriteLine(output);

                //new ScreenDisplay(screen).Dump("Problem 2");
            }

            //class ScreenDisplay
            //{
            //    bool[][] _screen;

            //    public ScreenDisplay(bool[][] screen)
            //    {
            //        _screen = screen;
            //    }

            //    object ToDump()
            //    {
            //        var bitmap = new System.Drawing.Bitmap(Cols, Rows);
            //        for (int iRow = 0; iRow < Rows; iRow++)
            //        {
            //            for (int iCol = 0; iCol < Cols; iCol++)
            //            {
            //                if (_screen[iRow][iCol])
            //                {
            //                    bitmap.SetPixel(iCol, iRow, System.Drawing.Color.White);
            //                }
            //            }
            //        }
            //        return new System.Drawing.Bitmap(bitmap, Cols * 3, Rows * 3);
            //    }
            //}

            private const int Rows = 6;
            private const int Cols = 50;
            readonly bool[] _shiftOut = new bool[Cols];

            static readonly string SearchRect = Cat(
                Integer("cols"),
                "x",
                Integer("rows"));
            static readonly Regex RegexRect = new Regex(SearchRect);

            static readonly string SearchRotate = Cat(
                Integer("index"),
                " by ",
                Integer("amount"));
            static readonly Regex RegexRotate = new Regex(SearchRotate);

            public string RetrieveSampleInput()
            {
                return @"
rect 1x1
rotate row y=0 by 5
rect 1x1
rotate row y=0 by 5
rect 1x1
rotate row y=0 by 3
rect 1x1
rotate row y=0 by 2
rect 1x1
rotate row y=0 by 3
rect 1x1
rotate row y=0 by 2
rect 1x1
rotate row y=0 by 5
rect 1x1
rotate row y=0 by 5
rect 1x1
rotate row y=0 by 3
rect 1x1
rotate row y=0 by 2
rect 1x1
rotate row y=0 by 3
rect 2x1
rotate row y=0 by 2
rect 1x2
rotate row y=1 by 5
rotate row y=0 by 3
rect 1x2
rotate column x=30 by 1
rotate column x=25 by 1
rotate column x=10 by 1
rotate row y=1 by 5
rotate row y=0 by 2
rect 1x2
rotate row y=0 by 5
rotate column x=0 by 1
rect 4x1
rotate row y=2 by 18
rotate row y=0 by 5
rotate column x=0 by 1
rect 3x1
rotate row y=2 by 12
rotate row y=0 by 5
rotate column x=0 by 1
rect 4x1
rotate column x=20 by 1
rotate row y=2 by 5
rotate row y=0 by 5
rotate column x=0 by 1
rect 4x1
rotate row y=2 by 15
rotate row y=0 by 15
rotate column x=10 by 1
rotate column x=5 by 1
rotate column x=0 by 1
rect 14x1
rotate column x=37 by 1
rotate column x=23 by 1
rotate column x=7 by 2
rotate row y=3 by 20
rotate row y=0 by 5
rotate column x=0 by 1
rect 4x1
rotate row y=3 by 5
rotate row y=2 by 2
rotate row y=1 by 4
rotate row y=0 by 4
rect 1x4
rotate column x=35 by 3
rotate column x=18 by 3
rotate column x=13 by 3
rotate row y=3 by 5
rotate row y=2 by 3
rotate row y=1 by 1
rotate row y=0 by 1
rect 1x5
rotate row y=4 by 20
rotate row y=3 by 10
rotate row y=2 by 13
rotate row y=0 by 10
rotate column x=5 by 1
rotate column x=3 by 3
rotate column x=2 by 1
rotate column x=1 by 1
rotate column x=0 by 1
rect 9x1
rotate row y=4 by 10
rotate row y=3 by 10
rotate row y=1 by 10
rotate row y=0 by 10
rotate column x=7 by 2
rotate column x=5 by 1
rotate column x=2 by 1
rotate column x=1 by 1
rotate column x=0 by 1
rect 9x1
rotate row y=4 by 20
rotate row y=3 by 12
rotate row y=1 by 15
rotate row y=0 by 10
rotate column x=8 by 2
rotate column x=7 by 1
rotate column x=6 by 2
rotate column x=5 by 1
rotate column x=3 by 1
rotate column x=2 by 1
rotate column x=1 by 1
rotate column x=0 by 1
rect 9x1
rotate column x=46 by 2
rotate column x=43 by 2
rotate column x=24 by 2
rotate column x=14 by 3
rotate row y=5 by 15
rotate row y=4 by 10
rotate row y=3 by 3
rotate row y=2 by 37
rotate row y=1 by 10
rotate row y=0 by 5
rotate column x=0 by 3
rect 3x3
rotate row y=5 by 15
rotate row y=3 by 10
rotate row y=2 by 10
rotate row y=0 by 10
rotate column x=7 by 3
rotate column x=6 by 3
rotate column x=5 by 1
rotate column x=3 by 1
rotate column x=2 by 1
rotate column x=1 by 1
rotate column x=0 by 1
rect 9x1
rotate column x=19 by 1
rotate column x=10 by 3
rotate column x=5 by 4
rotate row y=5 by 5
rotate row y=4 by 5
rotate row y=3 by 40
rotate row y=2 by 35
rotate row y=1 by 15
rotate row y=0 by 30
rotate column x=48 by 4
rotate column x=47 by 3
rotate column x=46 by 3
rotate column x=45 by 1
rotate column x=43 by 1
rotate column x=42 by 5
rotate column x=41 by 5
rotate column x=40 by 1
rotate column x=33 by 2
rotate column x=32 by 3
rotate column x=31 by 2
rotate column x=28 by 1
rotate column x=27 by 5
rotate column x=26 by 5
rotate column x=25 by 1
rotate column x=23 by 5
rotate column x=22 by 5
rotate column x=21 by 5
rotate column x=18 by 5
rotate column x=17 by 5
rotate column x=16 by 5
rotate column x=13 by 5
rotate column x=12 by 5
rotate column x=11 by 5
rotate column x=3 by 1
rotate column x=2 by 5
rotate column x=1 by 5
rotate column x=0 by 1"; 
            }

            public string RetrieveSampleOutput()
            {
                return null;
            }
        }
    }
}
