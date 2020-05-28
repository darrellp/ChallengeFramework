using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RegexStringLibrary;
using static RegexStringLibrary.Stex;
using static System.Console;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("Advent of Code 2016", "Leonardo's Monorail (12)", "https://adventofcode.com/2016/day/12")]
        public class LeonardosMonorail : IChallenge
        {
            public void Solve()
            {
                var input = ReadAll();
                var instructionCmds = input.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                List<Instruction> instructions = new List<Instruction>();
                foreach (var instructionCmd in instructionCmds)
                {
                    instructions.Add(Instruction.Parse(instructionCmd));
                }
                Machine machine = new Machine();

                while (machine.Ip >= 0 && machine.Ip < instructions.Count)
                {
                    machine.Execute(instructions[machine.Ip]);
                }
                WriteLine(machine.Regs[0]);

                machine = new Machine();
                machine.Regs[2] = 1;

                while (machine.Ip >= 0 && machine.Ip < instructions.Count)
                {
                    machine.Execute(instructions[machine.Ip]);
                }
                WriteLine(machine.Regs[0]);
            }

            private class Machine
            {
                public readonly int[] Regs = new int[4];
                public int Ip;

                public void Execute(Instruction instruction)
                {
                    var incIp = 1;

                    switch (instruction.IType)
                    {
                        case InstructionType.Cpy:
                            var val = instruction.Arg1;
                            if (instruction.IsRegCpy)
                            {
                                val = Regs[val];
                            }
                            Regs[instruction.Arg2] = val;
                            break;

                        case InstructionType.Dec:
                            Regs[instruction.Arg1]--;
                            break;

                        case InstructionType.Inc:
                            Regs[instruction.Arg1]++;
                            break;

                        case InstructionType.Jnz:
                            if (Regs[instruction.Arg1] != 0)
                            {
                                incIp = instruction.Arg2;
                            }
                            break;
                    }
                    Ip += incIp;
                }
            }

            private enum InstructionType
            {
                Cpy, Dec, Inc, Jnz
            }

            private struct Instruction
            {
                // ReSharper disable once InconsistentNaming
                public readonly InstructionType IType;
                public readonly bool IsRegCpy;
                public readonly int Arg1;
                public readonly int Arg2;

                private Instruction(InstructionType iType, int arg1, int arg2, bool isRegCpy = false)
                {
                    IType = iType;
                    Arg1 = arg1;
                    Arg2 = arg2;
                    IsRegCpy = isRegCpy;
                }

                private static readonly string Search =
                    "cpy".OrAnyOf("dec", "inc", "jnz").Named("type")
                    + " "
                    + NonWhite.Rep(1).Named("arg1")
                    + (" " + NonWhite.Rep(1).Named("arg2")).Optional();
                private static readonly Regex Rgx = new Regex(Search);
                private static readonly Dictionary<string, InstructionType> MapStringToType = new Dictionary<string, InstructionType>()
                {
                    ["cpy"] = InstructionType.Cpy,
                    ["dec"] = InstructionType.Dec,
                    ["inc"] = InstructionType.Inc,
                    ["jnz"] = InstructionType.Jnz
                };

                public static Instruction Parse(string s)
                {
                    var match = Rgx.Match(s);
                    var iType = MapStringToType[match.Groups["type"].Value];
                    var arg1 = ArgToValue(match.Groups["arg1"].Value);
                    var arg2 = 0;
                    if (match.Groups["arg2"].Success)
                    {
                        arg2 = ArgToValue(match.Groups["arg2"].Value);
                    }
                    var isRegCpy = iType == InstructionType.Cpy && !char.IsDigit(match.Groups["arg1"].Value[0]);
                    return new Instruction(iType, arg1, arg2, isRegCpy);
                }

                private static int ArgToValue(string arg)
                {
                    if ('a' <= arg[0] && arg[0] <= 'd')
                    {
                        return arg[0] - 'a';
                    }
                    return int.Parse(arg);
                }
            }

            public string RetrieveSampleInput()
            {
                return @"
cpy 1 a
cpy 1 b
cpy 26 d
jnz c 2
jnz 1 5
cpy 7 c
inc d
dec c
jnz c -2
cpy a c
inc a
dec b
jnz b -2
cpy c b
dec d
jnz d -6
cpy 16 c
cpy 17 d
inc a
dec d
jnz d -2
dec c
jnz c -5
";
            }

            public string RetrieveSampleOutput()
            {
                return null;
            }
        }
    }
}
