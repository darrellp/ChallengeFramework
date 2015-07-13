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
#include <queue>
#include <functional>
#include <vector>

prolog("Code Chef", Test, "Test - cpp", "http://www.codechef.com/problems/TEST")

using namespace std;
int main(void) {
	int x;
	cin >> x;
	do
	{
		cout << x << endl;
		cin >> x;
	} while (x != 42);
	return 0;
}
sampleInput
R"(
1
2
88
42
99
)";

sampleOutput
R"(
1
2
88
)";

epilog
