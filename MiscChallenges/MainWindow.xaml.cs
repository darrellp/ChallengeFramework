using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MiscChallenges.Challenges;

namespace MiscChallenges
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		bool _originalInput = true;
		private bool _changingSelection;
		private Thread _challengeThread;

		public MainWindow()
		{
			InitializeComponent();

			var challenges = (typeof(ChallengeClass)).
				GetNestedTypes().
				Select(MethodTest).
				Where(t => t != null)
				.ToList();

			var contests = new Dictionary<string, List<ChallengeInfo>>();

			foreach (var test in challenges)
			{
				if (contests.ContainsKey(test.Contest))
				{
					contests[test.Contest].Add(test);
				}
				else
				{
					contests[test.Contest] = new List<ChallengeInfo> { test };
				}
			}
			foreach (var info in contests)
			{
				var infoList = info.Value;
				var contest = info.Key;
				infoList.Sort((info1, info2) => String.Compare(info1.Name, info2.Name, StringComparison.Ordinal));
				var item = new TreeViewItem
				{
					Header = contest,
					ItemsSource = infoList
				};
				tvChallenges.Items.Add(item);
			}
		}

		private static ChallengeInfo MethodTest(Type member)
		{
			return member.
				GetCustomAttributes(true).
				OfType<ChallengeAttribute>().
				Select(t => new ChallengeInfo(
					t.Name,
					t.Contest,
					(IChallenge)Activator.CreateInstance(member))).
				FirstOrDefault();
		}

		private string GetChallengeData(IChallenge challenge)
		{
			var challengeDataString = challenge.RetrieveSampleInput();
			string challengeData = null;

			if (challengeDataString != null)
			{
				challengeData = challengeDataString.Substring(Environment.NewLine.Count());
			}
			return challengeData;
		}

		private void SolveChallengeAsync(IChallenge challenge, string challengeData, Stopwatch sw)
		{
			var challengeTask = Task.Factory.StartNew(delegate
			{
				string strRet;
				var isError = false;

				_challengeThread = Thread.CurrentThread;
				using (var str = challengeData == null ? null : new StringReader(challengeData))
				{
					try
					{
						sw.Start();
						strRet = challenge.Solve(str);
						sw.Stop();
					}
					catch (Exception ex)
					{
						sw.Stop();
						isError = true;
						strRet = "Error: " + ex.Message;
					}
				}
				return new Tuple<string, bool>(strRet, isError);
			});
			challengeTask.ContinueWith(_ =>
			{
				Dispatcher.Invoke(
					delegate
					{
						try
						{
							ChallengeSolved(challengeTask.Result.Item1, challengeTask.Result.Item2, challenge, sw);
						}
						catch (AggregateException)
						{
							sw.Stop();
							ChallengeSolved("Challenge Terminated", true, challenge, sw);
						}
					});
			});
		}

		private void RunChallenges(object sender, RoutedEventArgs e)
		{
			// Check to see if we're located on a valid challenge...
			var challengeInfo = tvChallenges.SelectedItem as ChallengeInfo;
			if (challengeInfo == null)
			{
				return;
			}

			// Yes - get the challenge and the input data if any
			var challenge = challengeInfo.Challenge;
			var challengeData = tbxInput.Text;
			var sw = new Stopwatch();

			// Disable all the UI other than Cancel until this is done...
			DisableUI();
			SolveChallengeAsync(challenge, challengeData, sw);
		}

		private void ChallengeSolved(string result, bool isError, IChallenge challenge, Stopwatch sw)
		{
			var strResultString = challenge.RetrieveSampleOutput();
			var isResult = strResultString != null && _originalInput;

			var isSuccess = isResult && result == strResultString.Substring(Environment.NewLine.Count());
			var colorOutput = Colors.Black;
			svOutput.ScrollToTop();

			// If there was an error or if there was a result we were supposed to meet and we didn't meet it
			if (isError || (isResult && !isSuccess))
			{
				colorOutput = Colors.Red;
			}
			// If there was a result we were supposed to meet and we nailed it
			else if (isResult)
			{
				colorOutput = Colors.Green;
			}
			tbOutput.Foreground = new SolidColorBrush(colorOutput);
			tbOutput.Text = result + Environment.NewLine + sw.ElapsedMilliseconds + " ms.";
			EnableUI();
		}

		private void EnableUI()
		{
			btnCancel.IsEnabled = false;
			tvChallenges.IsEnabled = true;
			btnRun.IsEnabled = true;
			btnUrl.IsEnabled = true;
			_challengeThread = null;
		}

		private void DisableUI()
		{
			btnCancel.IsEnabled = true;
			tvChallenges.IsEnabled = false;
			btnRun.IsEnabled = false;
			btnUrl.IsEnabled = false;
		}

		private void CancelChallenge(object sender, RoutedEventArgs e)
		{
			if (_challengeThread != null)
			{
				_challengeThread.Abort();
				EnableUI();
			}
		}

		private void tbxInput_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (!_changingSelection)
			{
				_originalInput = false;
			}
		}

		private void tvChallenges_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			_originalInput = true;
			_changingSelection = true;
			var challengeInfo = tvChallenges.SelectedItem as ChallengeInfo;
			if (challengeInfo == null)
			{
				return;
			}
			var challengeData = GetChallengeData(challengeInfo.Challenge);
			tbxInput.Text = challengeData;
			_changingSelection = false;
		}

		private void VisitURL(object sender, RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
		}
	}
}
