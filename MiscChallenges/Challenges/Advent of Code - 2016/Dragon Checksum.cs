using System.Linq;
using static System.Console;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("Advent of Code 2016", "Dragon Checksum (16)", "https://adventofcode.com/2016/day/16")]
        public class DragonChecksum : IChallenge
        {
            public void Solve()
            {
                const int bufferLength = 272;
                var curData = ReadLine();
                while (curData.Length < bufferLength)
                {
                    curData = curData + "0" + new string(curData.Select(c => c == '1' ? '0' : '1').Reverse().ToArray());
                }

                curData = curData.Substring(0, bufferLength);
                while ((curData.Length & 1) == 0)
                {
                    var newLength = curData.Length / 2;
                    var checkSum = new char[newLength];
                    for (var ich = 0; ich < newLength; ich++)
                    {
                        checkSum[ich] = (curData[2 * ich] == curData[2 * ich + 1]) ? '1' : '0';
                    }

                    curData = new string(checkSum);
                }
                WriteLine(curData);

				// Part 2 - disk length = 35651584
				// TODO: Part 2
				// What I can say: There are 17 bits in the "salt".  The process of producing the bits (sticking a 0 on the end of the
				// current pattern and then reversing and not'ing the pattern after the 0) means that the Disk is filled with a pattern
				// with blocks of 18 bits plus one trailing part.  The 18 bits are the salt alternately forward and reversed with a
				// trailing bit of 1 or 0.  That bit comes from starting with 0 and repeating the reverse and copy mapping.  Thus, we
				// really have a bunch of 0's and 1's that look like this:
				//      s B0 s' B1 s B2 s'...Bn remainder
				// where s is the salt, s' is the reversed salt and B0, B1, ...Bn are bits produced from the reverse and copy mapping
				// starting with "0".  So the successive applications yields:
				//      0
				//      001
				//      0010011
				//      ...
				// Thus, the first part of the disk contents looks like:
				//      s 0 s' 0 s 1 s' 0 s 0 s' 1 s 1 ...
				// so that the salts are alternatively reversed or not and the bits are from the derived sequence above.  For the moment
				// I'm going to ignore the trailing bytes.  Handling them is just a distraction for the moment. Thus, the
				// vast majority of the final disk contents can be just represented by the Bi bits - one per 18 bit block.  The 18
				// bit blocks come in 4 varieties: s 0, s 1, s' 0, and s' 1.  Each of these 18 bit blocks will be reduced down independently
				// in the checksum since it deals with even length strings.  If we represent the reductions as s0, s1, sp0 and sp1 then the
				// reduced string can be represented as 2 bits per 9 bit block.  I think if we do similar actions enough we'll get our answer
				// without blowing out of resources.
            }

			public string RetrieveSampleInput()
            {
				return @"
10111011111001111
";
            }

            public string RetrieveSampleOutput()
            {
                return null;
            }
        }
    }
}