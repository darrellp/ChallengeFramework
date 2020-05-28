//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.IO;

//namespace Programming_Challenges.Challenges
//{
//	public static partial class ChallengeClass
//	{
//		[Challenge("3.8.1", "WERTYU")]
//		// ReSharper disable once InconsistentNaming
//		public class WERTYU : IChallenge
//		{
//			private const string Qwerty =
//				@"`1234567890-=" +
//				@"QWERTYUIOP[]\" +
//				@"ASDFGHJKL;'" +
//				@"ZXCVBNM,./";

//			private readonly Dictionary<char, char> _decoder = new Dictionary<char, char>(); 

//			public string Solve(StringReader str)
//			{
//				var ret = new StringBuilder();

//				// For all possible input characters
//				for (var ich = 1; ich < Qwerty.Length; ich++)
//				{
//					// Map them to their output characters
//					_decoder[Qwerty[ich]] = Qwerty[ich - 1];
//				}

//				while (true)
//				{
//					var line = str.ReadLine();
//					if (line == null)
//					{
//						break;
//					}
					
//					// Decode the line and add it to our results
//					ret.Append(Decode(line) + Environment.NewLine);
//				}
//				return ret.ToString();
//			}

//			private string Decode(string line)
//			{
//				// Replace each character and return the result string
//				return new string(line.Select(ch => ch == ' ' ? ' ' : _decoder[ch]).ToArray());
//			}

//			public string RetrieveSampleInput()
//			{
//				return @"
//O S, GOMR YPFSU/
//";
//			}

//			public string RetrieveSampleOutput()
//			{
//				return @"
//I AM FINE TODAY.
//";
//			}
//		}
//	}
//}
