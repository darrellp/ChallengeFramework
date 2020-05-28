// ReSharper disable SpecifyACultureInStringConversionExplicitly
using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
	{
		[Challenge("ProgChallenges", "Interpreter",
			"https://onlinejudge.org/index.php?option=onlinejudge&Itemid=8&page=show_problem&problem=974")]
		public class Interpreter : IChallenge
		{
			private int[] _ram;
			private int[] _regs;
			private int _ip;


			public void Solve()
			{
				var ret = new StringBuilder();

				while (true)
				{
					// ReSharper disable once AssignNullToNotNullAttribute
					var cCases = int.Parse(ReadLine());
					_ip = 0;
					ReadLine();

					for (var i = 0; i < cCases; i++)
					{
						_ram = new int[1000];
						_regs = new int[10];
						FillRam();
						SolveCase(ret);
					}
					break;
				}
				Write(ret.ToString());
			}

			private void FillRam()
			{
				string cmd;
				var nextLocation = 0;

				while ((cmd = ReadLine()) != String.Empty && cmd != null)
				{
					_ram[nextLocation++] = int.Parse(cmd);
				}
			}

			private void SolveCase(StringBuilder ret)
			{
				var cInst = 0;

				while (true)
				{
					cInst++;
					var cmd = _ram[_ip];
					if (cmd == 100)
					{
						ret.Append(cInst + Environment.NewLine);
						return;
					}
					var cmdString = cmd.ToString();
					var zeroes = 3 - cmdString.Length;
					cmdString = new String('0', zeroes) + cmdString;

					_ip = _lookup[cmdString[0]](cmdString[1], cmdString[2], _ram, _regs, _ip);
				}
			}

			private readonly Dictionary<char, Func<char, char, int[], int[], int, int>> _lookup =
				new Dictionary<char, Func<char, char, int[], int[], int, int>>
				{
					{'2', SetRegDirect},
					{'3', AddToReg},
					{'4', MultiplyToReg},
					{'5', CopyReg},
					{'6', AddRegs},
					{'7', MultiplyRegs},
					{'8', SetRegRam},
					{'9', SetRam},
					{'0', GotoIf}
				};

			private static int SetRegDirect(char arg1, char arg2, int[] ram, int[] regs, int ip)
			{
				var regNo = arg1 - '0';
				regs[regNo] = arg2 - '0';
				regs[regNo] %= 1000;
				return ip + 1;
			}

			private static int AddToReg(char arg1, char arg2, int[] ram, int[] regs, int ip)
			{
				var regNo = arg1 - '0';
				regs[regNo] += arg2 - '0';
				regs[regNo] %= 1000;
				return ip + 1;
			}

			private static int MultiplyToReg(char arg1, char arg2, int[] ram, int[] regs, int ip)
			{
				var regNo = arg1 - '0';
				regs[regNo] *= arg2 - '0';
				regs[regNo] %= 1000;
				return ip + 1;
			}

			private static int CopyReg(char arg1, char arg2, int[] ram, int[] regs, int ip)
			{
				regs[arg1 - '0'] = regs[arg2 - '0'];
				return ip + 1;
			}

			private static int AddRegs(char arg1, char arg2, int[] ram, int[] regs, int ip)
			{
				var regNo = arg1 - '0';
				regs[regNo] += regs[arg2 - '0'];
				regs[regNo] %= 1000;
				return ip + 1;
			}

			private static int MultiplyRegs(char arg1, char arg2, int[] ram, int[] regs, int ip)
			{
				var regNo = arg1 - '0';
				regs[regNo] *= regs[arg2 - '0'];
				regs[regNo] %= 1000;
				return ip + 1;
			}

			private static int SetRegRam(char arg1, char arg2, int[] ram, int[] regs, int ip)
			{
				regs[arg1 - '0'] = ram[regs[arg2 - '0']];
				return ip + 1;
			}

			private static int SetRam(char arg1, char arg2, int[] ram, int[] regs, int ip)
			{
				ram[regs[arg2 - '0']] = regs[arg1 - '0'];
				return ip + 1;
			}

			private static int GotoIf(char arg1, char arg2, int[] ram, int[] regs, int ip)
			{
				if (regs[arg2 - '0'] != 0)
				{
					return regs[arg1 - '0'];
				}
				return ip + 1;
			}

			public string RetrieveSampleInput()
			{
				return @"
1

299
492
495
399
492
495
399
283
279
689
078
100
000
000
000
";
			}

			public string RetrieveSampleOutput()
			{
				return @"
16
";
			}
		}
	}
}
