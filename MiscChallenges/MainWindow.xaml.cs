using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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


		private void RunChallenges(object sender, RoutedEventArgs e)
		{
			var challengeInfo = tvChallenges.SelectedItem as ChallengeInfo;
			if (challengeInfo == null)
			{
				return;
			}
			var challenge = challengeInfo.Challenge;
			var challengeDataString = challenge.RetrieveSampleInput();
			var isChallengeData = challengeDataString != null;
			string challengeData = null;

			if (isChallengeData)
			{
				challengeData = challengeDataString.Substring(Environment.NewLine.Count());
			}
			string strResult;
			var sw = new Stopwatch();
			using (var str = challengeData == null ? null : new StringReader(challengeData))
			{
				sw.Start();
				strResult = challenge.Solve(str);
				sw.Stop();
			}
			var strResultString = challenge.RetrieveSampleOutput();
			var isResult = strResultString != null;

			var success = isResult && strResult == strResultString.Substring(Environment.NewLine.Count());
			svText.ScrollToTop();
			tbOutput.Foreground = new SolidColorBrush(isResult ? (success ? Colors.Green : Colors.Red) : Colors.Black);
			tbOutput.Text = strResult + Environment.NewLine + sw.ElapsedMilliseconds + " ms.";
		}
	}
}
