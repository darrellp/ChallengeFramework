using System.Linq;
using static System.Console;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("ProgChallenges", "Check the Check",
            "http://www.programming-challenges.com/pg.php?page=downloadproblem&probid=110107&format=html")]
        public class CheckTheCheck : IChallenge
        {
            private char[][] _board;
            int _wKingRow, _wKingCol, _bKingRow, _bKingCol;

            public void Solve()
            {
                var iGame = 1;
                while (true)
                {
                    GetBoard();
                    if (!EndFlag())
                    {
                        WriteLine(@"Game #" + iGame++ + @": " + GetCheck());
                    }
                    else
                    {
                        break;
                    }
                    ReadLine();
                }
            }

            private string GetCheck()
            {
                LocateKings();

                if (RookOrQueenAttack(true) ||
                    KnightAttack(true) ||
                    BishopOrQueenAttack(true) ||
                    PawnAttack(true))
                {
                    return "black king is in check.";
                }
                if (RookOrQueenAttack(false) ||
                    KnightAttack(false) ||
                    BishopOrQueenAttack(false) ||
                    PawnAttack(false))
                {
                    return "white king is in check.";
                }
                return "no king is in check.";
            }

            private bool OnTheBoard(int row, int col)
            {
                return row >= 0 && row < 8 && col >= 0 && col < 8;
            }

            private bool Search(int row, int col, int drow, int dcol, string search)
            {
                row += drow;
                col += dcol;
                while (OnTheBoard(row, col))
                {
                    var curPiece = _board[row][col];

                    if (curPiece == '.')
                    {
                        row += drow;
                        col += dcol;
                        continue;
                    }
                    return search.Contains(curPiece);
                }
                return false;
            }

            private bool RookOrQueenAttack(bool isBlack)
            {
                var kRow = isBlack ? _bKingRow : _wKingRow;
                var kCol = isBlack ? _bKingCol : _wKingCol;
                var search = isBlack ? "QR" : "qr";

                return Search(kRow, kCol, 1, 0, search) ||
                       Search(kRow, kCol, -1, 0, search) ||
                       Search(kRow, kCol, 0, 1, search) ||
                       Search(kRow, kCol, 0, -1, search);
            }

            private bool BishopOrQueenAttack(bool isBlack)
            {
                var kRow = isBlack ? _bKingRow : _wKingRow;
                var kCol = isBlack ? _bKingCol : _wKingCol;
                var search = isBlack ? "QB" : "qb";

                return Search(kRow, kCol, 1, 1, search) ||
                       Search(kRow, kCol, 1, -1, search) ||
                       Search(kRow, kCol, -1, 1, search) ||
                       Search(kRow, kCol, -1, -1, search);
            }

            private bool IsPieceAt(int row, int col, char piece)
            {
                return OnTheBoard(row, col) && _board[row][col] == piece;
            }

            private bool KnightAttack(bool isBlack)
            {
                var kRow = isBlack ? _bKingRow : _wKingRow;
                var kCol = isBlack ? _bKingCol : _wKingCol;
                var piece = isBlack ? 'N' : 'n';

                return IsPieceAt(kRow + 2, kCol - 1, piece) ||
                       IsPieceAt(kRow + 2, kCol + 1, piece) ||
                       IsPieceAt(kRow - 2, kCol - 1, piece) ||
                       IsPieceAt(kRow - 2, kCol + 1, piece) ||
                       IsPieceAt(kRow + 1, kCol - 2, piece) ||
                       IsPieceAt(kRow + 1, kCol + 2, piece) ||
                       IsPieceAt(kRow - 1, kCol - 2, piece) ||
                       IsPieceAt(kRow - 1, kCol + 2, piece);
            }

            private bool PawnAttack(bool isBlack)
            {
                var kRow = isBlack ? _bKingRow : _wKingRow;
                var kCol = isBlack ? _bKingCol : _wKingCol;
                var piece = isBlack ? 'P' : 'p';
                var dir = isBlack ? 1 : -1;

                return IsPieceAt(kRow + dir, kCol - 1, piece) ||
                       IsPieceAt(kRow + dir, kCol + 1, piece);
            }

            private void LocateKings()
            {
                for (var iRow = 0; iRow < 8; iRow++)
                {
                    for (var iCol = 0; iCol < 8; iCol++)
                    {
                        var piece = _board[iRow][iCol];
                        switch (piece)
                        {
                            case 'k':
                                _bKingCol = iCol;
                                _bKingRow = iRow;
                                break;
                            case 'K':
                                _wKingCol = iCol;
                                _wKingRow = iRow;
                                break;
                        }
                    }
                }
            }

            private bool EndFlag()
            {
                for (var iRow = 0; iRow < 8; iRow++)
                {
                    for (var iCol = 0; iCol < 8; iCol++)
                    {
                        if (_board[iRow][iCol] != '.')
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            private void GetBoard()
            {
                _board = new char[8][];

                // ReSharper disable AssignNullToNotNullAttribute
                for (var iRow = 0; iRow < 8; iRow++)
                {
                    _board[iRow] = ReadLine().ToArray();
                }
                // ReSharper restore AssignNullToNotNullAttribute
            }

 
            public string RetrieveSampleInput()
            {
                return @"
..k.....
ppp.pppp
........
.R...B..
........
........
PPPPPPPP
K.......

rnbqk.nr
ppp..ppp
....p...
...p....
.bPP....
.....N..
PP..PPPP
RNBQKB.R

........
........
........
........
........
........
........
........
";
            }

            public string RetrieveSampleOutput()
            {
                return @"
Game #1: black king is in check.
Game #2: white king is in check.
";
            }
        }
    }
}
