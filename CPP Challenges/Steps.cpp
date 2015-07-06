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

prolog("UVA", Steps, "Steps", "https://uva.onlinejudge.org/index.php?option=com_onlinejudge&Itemid=8&category=10&page=show_problem&problem=787")

using namespace std;
int main()
{
	int x, y;
	int testCases;
	int min_steps = 0;
	cin >> testCases;

	for (int i = 0; i<testCases; i++)
	{
		cin >> x >> y;
		int difference = y - x;
		min_steps = 0;
		if (difference != 0)
		{
			int sumOfSteps = 0;
			int z = 2;
			while (difference > sumOfSteps)
			{
				sumOfSteps += (z / 2); //next step
				min_steps++;
				z++;
			}
		}

		cout << min_steps << endl;
	}
	return 0;
}

sampleInput
R"(
3
45 48
45 49
45 50
)";
sampleOutput
R"(
3
3
4
)";
epilog
