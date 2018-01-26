using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RegexStringLibrary;
using static System.Console;
using static RegexStringLibrary.Stex;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("Advent of Code", "Radioisotope Thermoelectric Generators", "https://adventofcode.com/2016/day/11")]
        public class RadioisotopeThermoelectricGenerators : IChallenge
        {
            public void Solve() 
            {
                var input = ReadAll();
                var specs = input.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                var search = (Word.Rep(1).Named("generator") + " generator").Capture()
                    .OrAnyOf(Word.Rep(1).Named("chip") + "-compatible");
                var regex = new Regex(search);

                for (var iFloor = 0; iFloor < 4; iFloor++)
                {
                    WriteLine($"Floor {iFloor + 1}");
                    var matches = regex.Matches(specs[iFloor]);

                    foreach (Match match in matches)
                    {
                        if (match.Groups["generator"].Success)
                        {
                            WriteLine($"generator: {match.Groups["generator"].Value}");
                        }
                        else
                        {
                            WriteLine($"chip: {match.Groups["chip"].Value}");
                        }
                    }
                }
            }

            public class State
            {
                private bool FloorContainsChip(string name, int floor = -1)
                {
                    return _chipsOnFloor[floor > 0 ? floor : _elevatorFloor].Contains(name);
                }

                private bool FloorContainsRtg(string name, int floor = -1)
                {
                    return _rgtsOnFloor[floor > 0 ? floor : _elevatorFloor].Contains(name);
                }

                private HashSet<string>[] _chipsOnFloor;
                private HashSet<string>[] _rgtsOnFloor;
                private int _elevatorFloor;

                bool CheckMove(bool fUp, bool fChip1, string item1, bool fChip2, string item2)
                {
                    var retValue = true;

                    // Check to see if we've got both items on this floor
                    if (fChip1)
                    {
                        retValue &= !FloorContainsChip(item1);
                    }
                    else
                    {
                        retValue &= !FloorContainsRtg(item1);
                    }
                    if (retValue && item2 != null)
                    {
                        if (fChip2)
                        {
                            retValue &= !FloorContainsChip(item2);
                        }
                        else
                        {
                            retValue &= !FloorContainsRtg(item2);
                        }
                    }

                    // Check to see if we're stranding a microchip with an incompatible RTG
                    // Can't happen if our RTG was the only one on the floor...
                    if (retValue && _rgtsOnFloor[_elevatorFloor].Count > 1)
                    {
                        if (!fChip1 &&                      // First item is a generator
                            FloorContainsChip(item1))       // It's chip is being left behind
                        {
                            return false;
                        }

                        if (item2 != null && !fChip2 &&
                            FloorContainsChip(item2))
                        {
                            return false;
                        }
                    }

                    return retValue;
                }
            }

            public string RetrieveSampleInput()
            {
                return @"
The first floor contains a strontium generator, a strontium-compatible microchip, a plutonium generator, and a plutonium-compatible microchip.
The second floor contains a thulium generator, a ruthenium generator, a ruthenium-compatible microchip, a curium generator, and a curium-compatible microchip.
The third floor contains a thulium-compatible microchip.
The fourth floor contains nothing relevant.
";
            }

            public string RetrieveSampleOutput()
            {
                return null;
            }
        }
    }
}
