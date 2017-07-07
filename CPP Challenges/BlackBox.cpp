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

prolog("UVA", BlackBox, "Black Box - cpp", "https://uva.onlinejudge.org/index.php?option=com_onlinejudge&Itemid=8&category=7&page=show_problem&problem=442")

using namespace std;
template <class T> class NthPriorityQueue
{
private:
	std::priority_queue<int> _smallest;
	std::priority_queue<int, std::vector<int>, std::greater<int> > _largest;
	unsigned int _n;

public:
	NthPriorityQueue() { _n = 0; }

	int size()
	{
		return _smallest.size() + _largest.size();
	}

	void SetN(unsigned int n)
	{
		if (n == _smallest.size())
		{
			return;
		}

		_n = n;

		if (_smallest.size() < n)
		{
			while (_smallest.size() < n && _largest.size() > 0)
			{
				_smallest.push(_largest.top());
				_largest.pop();
			}
		}
		else
		{
			while (_smallest.size() > n)
			{
				_largest.push(_smallest.top());
				_smallest.pop();
			}
		}
	}

	void Add(T val)
	{
		if (_smallest.size() < _n)
		{
			_smallest.push(val);
			return;
		}
		if (_n != 0 && val < _smallest.top())
		{
			_smallest.push(val);
			val = _smallest.top();
			_smallest.pop();
		}
		_largest.push(val);
	}

	T Peek()
	{
		return _largest.top();
	}

	T Pop()
	{
		T top = _largest.top();
		_largest.pop();
		return top;
	}
};

int main()
{
	int cCases;
	bool fFirstCase = true;

	cin >> cCases;

	for (auto iCase = 0; iCase < cCases; iCase++)
	{
		NthPriorityQueue<int> pq;
		int cAdds, cGets;
		int iAdd = 0;

		if (!fFirstCase)
		{
			cout << endl;
		}
		fFirstCase = false;

		cin >> cAdds >> cGets;
		int *adds = new int[cAdds];

		for (auto iAdd = 0; iAdd < cAdds; iAdd++)
		{
			cin >> adds[iAdd];
		}

		for (auto iGet = 0; iGet < cGets; iGet++)
		{
			int get;
			cin >> get;

			for (; iAdd < get; iAdd++)
			{
				pq.Add(adds[iAdd]);
			}
			cout << pq.Peek() << endl;
			pq.SetN(iGet + 1);
		}
		delete[] adds;
	}

	return 0;
}

sampleInput
R"(
1

7 4
3 1 -4 2 8 -1000 2
1 2 6 6
)";

sampleOutput
R"(
3
3
1
2
)";

epilog
