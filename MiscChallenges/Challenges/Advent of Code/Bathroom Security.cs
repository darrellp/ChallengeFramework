using System;
using System.Text;
using static System.Console;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("Advent of Code", "Bathroom Security", "https://adventofcode.com/2016/day/2")]
        public class BathroomSecurity : IChallenge
        {
            public void Solve()
            {
                var input = ReadAll();
                // ReSharper disable once PossibleNullReferenceException
                var instructions = input.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                var codeBuilder = new StringBuilder();
                var pointCur = new Point(1, 1);
                foreach (var t in instructions)
                {
                    pointCur.Move(t);
                    codeBuilder.Append(pointCur.ToSecurityButton().ToString());
                }
                WriteLine(codeBuilder.ToString());

                codeBuilder = new StringBuilder();
                pointCur = new Point(0, 2);
                foreach (var t in instructions)
                {
                    pointCur.Move2(t);
                    codeBuilder.Append(pointCur.ToSecurityButton2().ToString());
                }
                WriteLine(codeBuilder.ToString());
            }

            static readonly char[][] Keypad = new char[][] {
                "  1  ".ToCharArray(),
                " 234 ".ToCharArray(),
                "56789".ToCharArray(),
                " ABC ".ToCharArray(),
                "  D  ".ToCharArray(),
            };

            private struct Point
            {
                private int _x;
                private int _y;

                public Point(int x, int y)
                {
                    _x = x;
                    _y = y;
                }

                private void Move2(char dir)
                {
                    int y2, x2;

                    switch (dir)
                    {
                        case 'U':
                            y2 = Math.Max(0, _y - 1);
                            if (Keypad[y2][_x] != ' ')
                            {
                                _y = y2;
                            }
                            break;
                        case 'D':
                            y2 = Math.Min(4, _y + 1);
                            if (Keypad[y2][_x] != ' ')
                            {
                                _y = y2;
                            }
                            break;
                        case 'L':
                            x2 = Math.Max(0, _x - 1);
                            if (Keypad[_y][x2] != ' ')
                            {
                                _x = x2;
                            }
                            break;
                        case 'R':
                            x2 = Math.Min(4, _x + 1);
                            if (Keypad[_y][x2] != ' ')
                            {
                                _x = x2;
                            }
                            break;
                    }
                }

                private void Move(char dir)
                {
                    switch (dir)
                    {
                        case 'U':
                            _y = Math.Max(0, _y - 1);
                            break;
                        case 'D':
                            _y = Math.Min(2, _y + 1);
                            break;
                        case 'L':
                            _x = Math.Max(0, _x - 1);
                            break;
                        case 'R':
                            _x = Math.Min(2, _x + 1);
                            break;
                    }
                }

                public void Move2(string instructions)
                {
                    foreach (var dir in instructions)
                    {
                        Move2(dir);
                    }
                }

                public void Move(string instructions)
                {
                    foreach (var dir in instructions)
                    {
                        Move(dir);
                    }
                }

                public int ToSecurityButton()
                {
                    return _y * 3 + _x + 1;
                }

                public char ToSecurityButton2()
                {
                    return Keypad[_y][_x];
                }

                public static Point operator +(Point p1, Point p2)
                {
                    return new Point(p1._x + p2._x, p1._y + p2._y);
                }

                public static Point operator *(int s, Point p)
                {
                    return new Point(s * p._x, s * p._y);
                }

                public override string ToString()
                {
                    return $"({_x}, {_y})";
                }
            }

            public string RetrieveSampleInput()
            {
                return @"
ULUULLUULUUUUDURUUULLDLDDRDRDULULRULLRLULRUDRRLDDLRULLLDRDRRDDLLLLDURUURDUDUUURDRLRLLURUDRDULURRUDLRDRRLLRDULLDURURLLLULLRLUDDLRRURRLDULRDDULDLRLURDUDRLLRUDDRLRDLLDDUURLRUDDURRLRURLDDDURRDLLDUUDLLLDUDURLUDURLRDLURURRLRLDDRURRLRRDURLURURRRULRRDLDDDDLLRDLDDDRDDRLUUDDLDUURUULDLUULUURRDRLDDDULRRRRULULLRLLDDUDRLRRLLLLLDRULURLLDULULLUULDDRURUDULDRDRRURLDRDDLULRDDRDLRLUDLLLDUDULUUUUDRDRURDDULLRDRLRRURLRDLRRRRUDDLRDDUDLDLUUDLDDRRRDRLLRLUURUDRUUULUDDDLDUULULLRUDULULLLDRLDDLLUUDRDDDDRUDURDRRUUDDLRRRRURLURLD
LDLUDDLLDDRLLDLDRDDDDDUURUDDDUURLRLRLDULLLDLUDDDULLDUDLRUUDDLUULLDRLDDUDLUDDLURRRLDUURDDRULLURLLRLLUUDRLDDDLDLDRDUDLRDURULDLDRRDRLDLUURRRRLUDDULDULUUUDULDDRLLDDRRUULURRUURRLDUUUDDDDRUURUDRRRDDDDLRLURRRRUUDDDULRRURRDLULRURDDRDRLUDLURDDRDURRUURDUDUDRRDDURRRDURDLUUUURRUDULLDDRLLLURLDUDRRLDDLULUDUDDDDUDLUUULUURUDRURUUDUUURRLDUUDRDRURLLDLLLLLRLLUDURDRRLULRRDDDRLDRDDURLRDULULLDDURURLRRDRULDULUUUURLDURUDUDUDDLUDRRDURULRDULLLDRRDLDLUDURDULULLDDURDDUDRUUUDUDRLDUURDUUUDUURURUDRULRURLDLRDDURDLUU
DDLDRLLDRRDRRLLUUURDDULRDUDRDRUDULURLLDDLRRRUDRDLDLURRRULUDRDLULLULLDUUDRLRUDDLRRURRUULRLDLLLDLRLLLURLLLURLLRDDLULLDUURLURDLLDLDUDLDRUUUDDLLDRRRRRUDRURUURRRDRUURDRDDRLDUUULUDUDRUDLLLLDRDRURRRDUUURLDLRLRDDDRLUDULDRLLULRDLDURDLDURUUDDULLULRDDRLRUURLLLURDRUURUUDUUULRDUDDRDURRRDUUDRRRUDRDLRURDLLDDDURLLRRDDDDLDULULDRLDRULDDLRRRLUDLLLLUDURRRUURUUULRRLDUURDLURRLRLLRDLRDDRDDLRDLULRUUUDDDUDRRURDDURURDDUDLURUUURUUUUDURDDLDRDULDRLDRLLRLRRRLDRLLDDRDLDLUDDLUDLULDLLDRDLLRDULDUDDULRRRUUDULDULRRURLRDRUDLDUDLURRRDDULRDDRULDLUUDDLRDUURDRDR
URDURRRRUURULDLRUUDURDLLDUULULDURUDULLUDULRUDUUURLDRRULRRLLRDUURDDDLRDDRULUUURRRRDLLDLRLRULDLRRRRUDULDDURDLDUUULDURLLUDLURULLURRRDRLLDRRDULUDDURLDULLDURLUDUULRRLLURURLDLLLURDUDRLDDDRDULLUDDRLDDRRRLDULLLLDUURULUDDDURUULUUUDURUDURDURULLLDRULULDRRLDRLDLRLRUDUDURRLURLRUUDRRDULULDLLDRDRRRDUDUURLDULLLURRDLUDDLDDRDDUDLDDRRRUDRULLURDDULRLDUDDDRULURLLUDLLRLRRDRDRRURUUUURDLUURRDULLRDLDLRDDRDRLLLRRDDLDDDDLUDLRLULRRDDRDLDLUUUDLDURURLULLLDDDULURLRRURLDDRDDLD
UDUULLRLUDLLUULRURRUUDDLLLDUURRURURDDRDLRRURLLRURLDDDRRDDUDRLLDRRUDRDRDDRURLULDDLDLRRUDDULLRLDDLRURLUURUURURDLDUDRLUUURRRLUURUDUDUUDDLDULUULRLDLLURLDRUDRLLRULURDLDDLLULLDRRUUDDLRRRUDDLRDRRRULDRDDRRULLLUDRUULURDUDRDLRRLDLRLRLDDULRRLULUUDDULDUDDULRRURLRDRDURUDDDLLRLDRDRULDDLLRLLRDUDDDDDDRLRLLDURUULDUUUDRURRLLRLDDDDRDRDUURRURDRDLLLUDDRDRRRDLUDLUUDRULURDLLLLLRDUDLLRULUULRLULRURULRLRRULUURLUDLDLLUURDLLULLLDDLRUDDRULRDLULRUURLDRULRRLULRLRULRDLURLLRURULRDRDLRRLRRDRUUURURULLLDDUURLDUDLLRRLRLRULLDUUUULDDUUU";
            }

            public string RetrieveSampleOutput()
            {
                return null;
            }
        }
    }
}
