using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace MiscChallenges.Challenges
{
    public static partial class ChallengeClass
    {
        [Challenge("ProgChallenges", "Crypt Kicker",
            "http://www.programming-challenges.com/pg.php?page=downloadproblem&probid=110204&format=html")]
        public class CryptKicker : IChallenge
        {
            private List<string> _arrangedWords;
            private readonly List<string> _words = new List<string>();
            private const char DummyChar = '*';

            public void Solve()
            {
                var cWords = GetVal();

                for (var iWord = 0; iWord < cWords; iWord++)
                {
                    _words.Add(ReadLine());
                }

                ArrangeWords();

                while (true)
                {
                    var cryptLine = ReadLine();
                    if (cryptLine == null)
                    {
                        break;
                    }
                    WriteLine(Decrypt(cryptLine));
                }
            }

            /// <summary>
            /// Overoptimization for this small case but interesting to do anyway.
            /// First order words in order of increasing rarity of their length and
            /// then, within lengths, sort according to the words which have least
            /// letters with the words that have gone before.
            /// </summary>
            private void ArrangeWords()
            {
                var orderedLengths = OrderLengths();
                _arrangedWords = OrderWords(orderedLengths);
            }

            /// <summary>
            /// Order lengths by frequency in the dictionary
            /// </summary>
            /// <returns>Enumerable of lengths from rarest to most common</returns>
            private IEnumerable<int> OrderLengths()
            {
                var mapLengthToCount = new Dictionary<int, int>();
                TallyLengths(mapLengthToCount);
                return OrderLengthPairs(mapLengthToCount);
            }

            /// <summary>
            /// Order by length frequency in the dictionary with length itself as secondary key
            /// </summary>
            /// <param name="mapLengthToCount"></param>
            /// <returns></returns>
            private static IEnumerable<int> OrderLengthPairs(Dictionary<int, int> mapLengthToCount)
            {
                var pairs = mapLengthToCount.ToList();
                pairs.Sort((kp1, kp2) =>
                {
                    var compVals = kp1.Value.CompareTo(kp2.Value);
                    if (compVals == 0)
                    {
                        return -kp1.Key.CompareTo(kp2.Key);
                    }
                    return compVals;
                });
                var orderedLengths = pairs.Select(p => p.Key);
                return orderedLengths;
            }

            private void TallyLengths(Dictionary<int, int> mapLengthToCount)
            {
                foreach (var length in _words.Select(word => word.Length))
                {
                    if (!mapLengthToCount.ContainsKey(length))
                    {
                        mapLengthToCount[length] = 0;
                    }
                    mapLengthToCount[length]++;
                }
            }

            private List<string> OrderWords(IEnumerable<int> orderedLengths)
            {
                var coveredChars = new HashSet<char>();
                var newWords = new List<string>();
                foreach (var lengthVar in orderedLengths)
                {
                    // Otherwise, compiler weirdness for using "lengthVar" in LINQ...
                    var length = lengthVar;
                    var wordSet = _words.Where(w => w.Length == length).ToList();

                    AddWordsOfLength(wordSet, coveredChars, newWords);
                }
                return newWords;
            }

            private void AddWordsOfLength(List<string> wordSet, HashSet<char> coveredChars, List<string> newWords)
            {
                var length = wordSet[0].Length;
                while (wordSet.Count != 0)
                {
                    var maxUncovered = 0;
                    var nextWord = string.Empty;
                    var newlyCoveredWords = new List<string>();
                    foreach (var word in wordSet)
                    {
                        var chars = new HashSet<char>(word);
                        chars.IntersectWith(coveredChars);
                        if (chars.Count == 0)
                        {
                            nextWord = word;
                            break;
                        }
                        if (length - chars.Count > maxUncovered)
                        {
                            maxUncovered = length - chars.Count;
                            nextWord = word;
                        }
                        else if (chars.Count == length)
                        {
                            newlyCoveredWords.Add(word);
                        }
                    }
                    if (nextWord != string.Empty)
                    {
                        coveredChars.UnionWith(nextWord);
                        newWords.Add(nextWord);
                        wordSet.Remove(nextWord);

                        var chars = new HashSet<char>(nextWord);
                        chars.ExceptWith(coveredChars);
                    }
                    foreach (var word in newlyCoveredWords)
                    {
                        wordSet.Remove(word);
                    }
                }
            }

            private string Decrypt(string cryptLine)
            {
                var cryptWords = cryptLine.Split(' ');
                var decrypter = new Dictionary<char, char>();

                for (var ch = 'a'; ch <= 'z'; ch++)
                {
                    decrypter[ch] = DummyChar;
                }
                FitWord(0, cryptWords, decrypter);
                return new string(cryptLine.Select(ch => ch == ' ' ? ' ' : decrypter[ch]).ToArray());
            }

            bool FitWord(int iWord, string[] cryptWords, Dictionary<char, char> decrypter)
            {
                if (iWord == _arrangedWords.Count)
                {
                    return CheckSolution(cryptWords, decrypter);
                }
                var wordToFit = _arrangedWords[iWord];

                foreach (var nextFit in cryptWords.Where(w => Fits(wordToFit, w, decrypter)))
                {
                    var backOut = AddDecryptionFromWord(wordToFit, nextFit, decrypter);
                    if (FitWord(iWord + 1, cryptWords, decrypter))
                    {
                        return true;
                    }
                    foreach (var ch in backOut)
                    {
                        decrypter[ch] = DummyChar;
                    }
                }
                return FitWord(iWord + 1, cryptWords, decrypter);
            }

            private bool CheckSolution(IEnumerable<string> cryptWords, Dictionary<char, char> decrypter)
            {
                return cryptWords.
                    Select(cryptedWord => new string(cryptedWord.Select(ch => ch == ' ' ? ' ' : decrypter[ch]).ToArray())).
                    All(decrypted => !decrypted.Contains('*') && _words.Contains(decrypted));
            }

            private IEnumerable<char> AddDecryptionFromWord(string wordToFit, string trialWord, Dictionary<char, char> decrypter)
            {
                var ret = new List<char>();
                for (var i = 0; i < wordToFit.Length; i++)
                {
                    if (decrypter[trialWord[i]] == DummyChar)
                    {
                        decrypter[trialWord[i]] = wordToFit[i];
                        ret.Add(trialWord[i]);
                    }
                }
                return ret;
            }

            private static bool Fits(string wordToFit, string wordToFitTo, Dictionary<char, char> decrypter)
            {
                if (wordToFit.Length != wordToFitTo.Length)
                {
                    return false;
                }

                var wordDecoder = new Dictionary<char, char>();
                var wordEncoder = new Dictionary<char, char>();

                for (var ichar = 0; ichar < wordToFit.Length; ichar++)
                {
                    var chPlain = wordToFit[ichar];
                    var chCrypted = wordToFitTo[ichar];

                    if ((wordDecoder.ContainsKey(chPlain) && wordDecoder[chPlain] != chCrypted) ||
                        (wordEncoder.ContainsKey(chCrypted) && wordEncoder[chCrypted] != chPlain))
                    {
                        return false;
                    }
                    if (decrypter[chCrypted] == DummyChar)
                    {
                        wordDecoder[chPlain] = chCrypted;
                        wordEncoder[chCrypted] = chPlain;
                    }
                    else if (chPlain != decrypter[chCrypted])
                    {
                        return false;
                    }
                }
                return true;
            }

            public string RetrieveSampleInput()
            {
                return @"
6
and
dick
jane
puff
spot
yertle
abcc
abcd
bjvg xsb hxsn xsb qymm xsb rqat xsb pnetfn
xxxx yyy zzzz www yyyy aaa bbbb ccc dddddd
bjvg xsb hxsr xsb qymm xsb rqat xsb pnetfn
";
            }

            public string RetrieveSampleOutput()
            {
                return @"
puff
dick
dick and jane and puff and spot and yertle
**** *** **** *** **** *** **** *** ******
**** *** **** *** **** *** **** *** ******
";
            }
        }
    }
}
