using System.IO;
using System.Runtime.InteropServices;
using MiscChallenges.Challenges;

namespace MiscChallenges
{
	class CppChallenge : IChallenge
	{
		#region DllImports
		[DllImport("CPP Challenges.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.LPStr)]
		public static extern string RunSolver(int index, [MarshalAs(UnmanagedType.LPStr)] string input);
		#endregion

		#region Private variables
		private readonly int _index;
		private readonly string _input;
		private readonly string _output;
		#endregion

		#region IChallenge members
		public CppChallenge(int index, string input, string output)
		{
			_index = index;
			_input = input;
			_output = output;
		}

		public string Solve(StringReader data)
		{
			var input = ChallengeClass.CsStringToCpp(data.ReadToEnd());
			return ChallengeClass.CppStringToCs(RunSolver(_index, input));
		}

		public string RetrieveSampleInput()
		{
			return _input;
		}

		public string RetrieveSampleOutput()
		{
			return _output;
		}
		#endregion
	}
}
