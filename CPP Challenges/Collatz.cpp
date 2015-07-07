#ifdef CHALLENGE_RUNNER
#include "stdafx.h"
#include "Challenge.h"
#else
#define prolog(a,b,c,d)
#define epilog
#define sampleInput char *_dummy1_ =
#define sampleOutput char *_dummy2_ =
#endif

#include<iostream>
#include<math.h>

prolog("ProgChallenges", Collatz, "The 3n+1 Problem - cpp", "http://www.programming-challenges.com/pg.php?page=downloadproblem&probid=110101&format=html")

int CollatzCount(int a)
{
	long long v = a;
	auto count = 1;

	while (v != 1)
	{
		count++;
		if ((v & 1) == 0)
		{
			v = v / 2;
		}
		else
		{
			v = 3 * v + 1;
		}
	}
	return count;
}

using namespace std;
int main()
{
	long n1, n2;
	int maxSteps;

	while (true)
	{
		cin >> n1 >> n2;
		if (cin.eof())
		{
			break;
		}
		maxSteps = -1;
		int low = min(n1, n2);
		auto high = max(n1, n2);
		for (auto i = low; i <= high; i++)
		{
			auto cycleLength = CollatzCount(i);
			maxSteps = max(cycleLength, maxSteps);
		}
		cout << n1 << ' ' << n2 << ' ' << maxSteps << endl;
	}

	return 0;
}

sampleInput
R"(
1 10
100 200
201 210
900 1000
1 1
10 1
210 201
113383 113383
999999 1
)";

sampleOutput
R"(
1 10 20
100 200 125
201 210 89
900 1000 174
1 1 1
10 1 20
210 201 89
113383 113383 248
999999 1 525
)";

epilog
