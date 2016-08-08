using System;
using System.Collections.Generic;
using System.Linq;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("ProgChallenges", "Stern-Brocot",
            "http://www.programming-challenges.com/pg.php?page=downloadproblem&probid=110507&format=html")]
        public class SternBrocot : IChallenge
        {
            public void Solve()
            {
                while (true)
                {
                    var vals = GetVals();
                    if (vals[0] == 1 && vals[1] == 1)
                    {
                        break;
                    }
                    if (vals[0] == vals[1])
                    {
                        Console.WriteLine("I");
                        continue;
                    }
                    GetSequence(vals[0], vals[1]);
                }
            }

            private void GetSequence(int num, int den)
            {
                var curDen = 1;
                var curNum = 1;
                var lNum = 0;
                var lDen = 1;
                var rNum = 1;
                var rDen = 0;

                while (true)
                {
                    var ncd = num * curDen;
                    var dcn = den * curNum;

                    if (ncd == dcn)
                    {
                        break;
                    }
                    if (ncd < dcn)
                    {
                        Console.Write("L");
                        rDen = curDen;
                        rNum = curNum;
                    }
                    else
                    {
                        Console.Write("R");
                        lDen = curDen;
                        lNum = curNum;
                    }
                    curDen = lDen + rDen;
                    curNum = lNum + rNum;
                }
                Console.WriteLine();
            }


            public string RetrieveSampleInput()
            {
                return @"
5 7
878 323
1 1
";
            }

            public string RetrieveSampleOutput()
            {
                return @"
LRRL
RRLRRLRLLLLRLRRR
";
            }
        }
    }
}