using System.Linq;
using static System.Console;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("ProgChallenges", "LCD Display",
            "http://www.programming-challenges.com/pg.php?page=downloadproblem&probid=110104&format=html")]
        public class LCDDisplay : IChallenge
        {
            public void Solve()
            {
                var firstTime = true;
                while (true)
                {
                    // ReSharper disable PossibleNullReferenceException
                    var vals = ReadLine().Split(' ');
                    // ReSharper restore PossibleNullReferenceException
                    var size = int.Parse(vals[0]);
                    var digitString = vals[1];
                    if (size == 0 && digitString == "0")
                    {
                        break;
                    }
                    if (!firstTime)
                    {
                        WriteLine();
                    }
                    var lcdMaker = new LCDMaker(digitString, size);
                    lcdMaker.AppendLCD();
                    firstTime = false;
                }
            }

            class LCDMaker
            {
                private readonly bool[] _topBars = { true, false, true, true, false, true, true, true, true, true };
                private readonly bool[] _midBars = { false, false, true, true, true, true, true, false, true, true };
                private readonly bool[] _botBars = { true, false, true, true, false, true, true, false, true, true };
                private readonly bool[] _tlBars = { true, false, false, false, true, true, true, false, true, true };
                private readonly bool[] _trBars = { true, true, true, true, true, false, false, true, true, true };
                private readonly bool[] _blBars = { true, false, true, false, false, false, true, false, true, false };
                private readonly bool[] _brBars = { true, true, false, true, true, true, true, true, true, true };
                private const string Space = " ";

                enum LCDState
                {
                    Starting,	// Starting
                    Ended,		// Done - ready to return
                    Blank,		// Blanks internal to the character
                    Between,	// Blank between characters
                    TL,			// Ready to print a bar of TL
                    TR,			// Ready to print a bar of TR
                    BL,			// Ready to print a bar of BL
                    BR,			// Ready to print a bar of BR
                    Top,		// Ready to print top crossbar
                    Middle,		// Ready to print middle crossbar
                    Bottom,		// Ready to print bottom crossbar
                    EOL			// End of an output line
                }

                private readonly int _size;
                readonly int[] _digits;
                LCDState _state = LCDState.Starting;
                int _charIndex;
                int _row;
                readonly string _crossBar;
                readonly string _crossBarBlank;
                private readonly string _internalBlank;
                LCDState _nextState = LCDState.Between;

                internal LCDMaker(string digitString, int size)
                {
                    _digits = digitString.Select(ch => ch - '0').ToArray();
                    _size = size;
                    _crossBar = Space + new string('-', size) + Space;
                    _crossBarBlank = new string(Space[0], size + 2);
                    _internalBlank = new string(Space[0], size);
                }

                internal void AppendLCD()
                {
                    while (_state != LCDState.Ended)
                    {
                        switch (_state)
                        {
                            case LCDState.Starting:
                                _state = LCDState.Top;
                                break;

                            case LCDState.Top:
                                DrawCrossBar(_topBars, LCDState.Top);
                                break;

                            case LCDState.Middle:
                                DrawCrossBar(_midBars, LCDState.Middle);
                                break;

                            case LCDState.Bottom:
                                DrawCrossBar(_botBars, LCDState.Bottom);
                                break;

                            case LCDState.Between:
                                Write(Space);
                                _state = _nextState;
                                break;

                            case LCDState.Blank:
                                Write(_internalBlank);
                                _state = _nextState;
                                break;

                            case LCDState.TL:
                                DrawVBar(_tlBars, LCDState.TR);
                                break;

                            case LCDState.TR:
                                DrawVBar(_trBars, LCDState.TL);
                                break;

                            case LCDState.BL:
                                DrawVBar(_blBars, LCDState.BR);
                                break;

                            case LCDState.BR:
                                DrawVBar(_brBars, LCDState.BL);
                                break;

                            case LCDState.EOL:
                                WriteLine();
                                if (_row == 0)
                                {
                                    _state = LCDState.TL;
                                }
                                else if (_row == _size)
                                {
                                    _state = LCDState.Middle;
                                }
                                else if (_row == _size + 1)
                                {
                                    _state = LCDState.BL;
                                }
                                else if (_row == 2 * _size + 1)
                                {
                                    _state = LCDState.Bottom;
                                }
                                else if (_row == 2 * _size + 2)
                                {
                                    _state = LCDState.Ended;
                                }
                                else
                                {
                                    _state = _nextState;
                                }
                                _row++;
                                _charIndex = 0;
                                break;
                        }
                    }
                }

                private void DrawVBar(bool[] digitToBarPresence, LCDState nextState)
                {
                    var onLeft = (_state == LCDState.BL || _state == LCDState.TL);

                    Write(digitToBarPresence[_digits[_charIndex]] ? "|" : Space);

                    _state = onLeft
                        ? LCDState.Blank
                        : (++_charIndex == _digits.Length ? LCDState.EOL : LCDState.Between);
                    _nextState = nextState;
                }

                private void DrawCrossBar(bool[] digitToBarPresence, LCDState nextState)
                {
                    Write(digitToBarPresence[_digits[_charIndex]] ? _crossBar : _crossBarBlank);
                    _charIndex++;
                    _state = _charIndex == _digits.Length ? LCDState.EOL : LCDState.Between;
                    _nextState = nextState;
                }
            }

            public string RetrieveSampleInput()
            {
                return @"
2 12345
3 67890
0 0
";
            }

            public string RetrieveSampleOutput()
            {
                return @"
      --   --        -- 
   |    |    | |  | |   
   |    |    | |  | |   
      --   --   --   -- 
   | |       |    |    |
   | |       |    |    |
      --   --        -- 

 ---   ---   ---   ---   --- 
|         | |   | |   | |   |
|         | |   | |   | |   |
|         | |   | |   | |   |
 ---         ---   ---       
|   |     | |   |     | |   |
|   |     | |   |     | |   |
|   |     | |   |     | |   |
 ---         ---   ---   --- 
";
            }
        }
    }
}
