using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using static System.Console;

namespace MiscChallenges.Challenges
{
	public static partial class ChallengeClass
    {
        private static List<int> _mpValToIndexRepeating5;
        private static int _nextCheck;
        private static List<byte[]> _md5Cache;

        // Warning: This one took almost 3 minutes the last time I ran it.
		[Challenge("Advent of Code 2016", "One Time Pad (14)", "https://adventofcode.com/2016/day/14")]
        // ReSharper disable once UnusedMember.Global
        public class OneTimePad : IChallenge
		{
			public void Solve()
            {
                var salt = ReadLine();
                void InitArrays()
                {
                    _mpValToIndexRepeating5 = Enumerable.Repeat(-1, 16).ToList();
                    _nextCheck = 0;
                    _md5Cache = new List<byte[]>(23000);
                }

                InitArrays();

				WriteLine(MoreEnumerable
                    .Generate(0, i => i + 1)
                    .Where(n => IsKey(salt, n, false))
                    .Take(64)
                    .Last());

				InitArrays();
				Write(MoreEnumerable
					.Generate(0, i => i + 1)
					.Where(n => IsKey(salt, n, true))
					.Take(64)
					.Last());

				_md5Cache = null;       // Free up this memory
            }

			public string RetrieveSampleInput()
			{
				return @"
zpqevtbw
";
			}

			public string RetrieveSampleOutput()
			{
				return null;
			}

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            /// <summary>	Calculates the next md5 value. </summary>
            ///
            /// <remarks>	Darrell Plank, 5/29/2020. </remarks>
            ///
            /// <param name="salt">	   	The salt. </param>
            /// <param name="index">   	Zero-based index of the. </param>
            /// <param name="fStretch">	True to stretch. </param>
            ///
            /// <returns>	The calculated md 5. </returns>
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            private static byte[] CalculateMd5(string salt, int index, bool fStretch)
            {
                if (_md5Cache.Count > index)
                {
                    return _md5Cache[index];
                }

                if (_md5Cache.Count != index)
                {
                    throw new ArgumentException("Index out of order in CalculateMd5");
                }
                var md5 = CalculateMd5Hash(salt + index);
                if (fStretch)
                {
                    for (var i = 0; i < 2016; i++)
                    {
                        md5 = CalculateMd5HashBytes(md5);
                    }
                }
                _md5Cache.Add(md5);
                return md5;
            }

            private static bool fDoubleNybble(byte b)
            {
                return (b >> 4) == (b & 0xf);

            }
            private static int FindFirstTriple(byte[] hash)
            {
                for (var i = 0; i < hash.Length; i++)
                {
                    var b = hash[i];
                    if (!fDoubleNybble(b))
                    {
                        continue;
                    }

                    var lowNybble = b & 0xf;
                    if (i > 0 && (hash[i - 1] & 0xf) == lowNybble)
                    {
                        return lowNybble;
                    }

                    if (i < hash.Length - 1 && (hash[i + 1] >> 4) == lowNybble)
                    {
                        return lowNybble;
                    }
                }

                return -1;
            }

            private static void CheckFiveNybbles(byte[] hash, int hashIndex)
            {
                for (var i = 0; i < hash.Length - 1; i++)
                {
                    var b = hash[i];
                    var bNext = hash[i + 1];
                    if (b != bNext || !fDoubleNybble(b))
                    {
                        continue;
                    }
                    var lowNybble = b & 0xf;

                    if (i > 0 && (hash[i - 1] & 0xf) == lowNybble)
                    {
                        _mpValToIndexRepeating5[lowNybble] = hashIndex;
                    }

                    if (i < hash.Length - 2 && (hash[i + 2] >> 4) == lowNybble)
                    {
                        _mpValToIndexRepeating5[lowNybble] = hashIndex;
                    }
				}
            }

			private static bool IsKey(string salt, int n, bool fStretch)
			{
                var md5 = CalculateMd5(salt, n, fStretch);
                var test = FindFirstTriple(md5);
                if (test < 0)
                {
                    return false;
                }

                for (; _nextCheck <= n + 1000; _nextCheck++)
                {
                    var md5Test = CalculateMd5(salt, _nextCheck, fStretch);
                    CheckFiveNybbles(md5Test, _nextCheck);
                }

				return _mpValToIndexRepeating5[test] > n;
            }

            private static byte[] _lookup;
            private static readonly byte[] strBuffer = new byte[32];

			public static byte[] CalculateMd5HashBytes(byte[] input)
            {
                if (_lookup == null)
                {
                    // Build lookup table
                    _lookup = new byte[512];

                    for (var i = 0; i < 256; i++)
                    {
                        var s = BitConverter.ToString(new[] { (byte)i }).ToLowerInvariant();
                        _lookup[i * 2] = (byte)s[0];
                        _lookup[i * 2 + 1] = (byte)s[1];
                    }
                }

                for (var b = 0; b < 16; b++)
                {
                    var n1 = b * 2;
                    var n2 = input[b] * 2;
                    strBuffer[n1] = _lookup[n2];
                    strBuffer[n1 + 1] = _lookup[n2 + 1];
                }
                return CalculateMd5Hash(strBuffer);
            }
		}
	}
}
