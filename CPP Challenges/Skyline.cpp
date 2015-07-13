#ifdef CHALLENGE_RUNNER
#define NOMINMAX
#include "stdafx.h"
#include "Challenge.h"
#else
#define prolog(a,b,c,d)
#define epilog
#define sampleInput char *_dummy1_ =
#define sampleOutput char *_dummy2_ =
#endif
#include <limits>
#include<iostream>
#include <queue>
#include <functional>
#include <vector>

prolog("UVA", Skyline, "Skyline - cpp", "https://uva.onlinejudge.org/index.php?option=com_onlinejudge&Itemid=8&category=3&page=show_problem&problem=41")

using namespace std;

class Skyline
{
private:
	class CurrentHeight
	{
	public:
		bool SetHeight(int height)
		{
			bool fRet = height != _currentHeight;
			_currentHeight = height;
			return fRet;
		}

		int GetHeight()
		{
			return _currentHeight;
		}

	private:
		int _currentHeight = -1;
	};

	class PqHeight
	{
	public:
		int Height;
		bool IsValid;
		bool IsGround;

		PqHeight(int height, bool isGround = false)
		{
			Height = height;
			IsValid = true;
			IsGround = isGround;
		}

		struct LessThanByHeight
		{
			bool operator()(const PqHeight *lhs, const PqHeight *rhs) const
			{
				return lhs->Height < rhs->Height;
			}
		};

		void Remove()
		{
			IsValid = false;
		}

		static PqHeight* GetNextHeight(priority_queue<PqHeight *, vector<PqHeight *>, PqHeight::LessThanByHeight> *pHeights)
		{
			while (pHeights->size() > 0 && !pHeights->top()->IsValid)
			{
				PqHeight *pHeight = pHeights->top();
				pHeights->pop();
				delete pHeight;
			}
			
			PqHeight *pHeight = NULL;
			if (pHeights->size() > 0)
			{
				pHeight = pHeights->top();
			}
			return pHeight;
		}
	};

	class PqEvent
	{
	public:
		struct GreaterThanByPos
		{
			bool operator()(const PqEvent *lhs, const PqEvent *rhs) const
			{
				return lhs->_pos > rhs->_pos;
			}
		};

		PqEvent(PqHeight *pHeight, int pos, bool addEvent)
		{
			_pHeight = pHeight;
			_pos = pos;
			_addEvent = addEvent;
		}

		void Handle(priority_queue<PqHeight *, vector<PqHeight *>, PqHeight::LessThanByHeight> *pHeights)
		{
			if (_addEvent)
			{
				pHeights->push(_pHeight);
			}
			else
			{
				_pHeight->Remove();
			}
		}

		static void HandleNextPositionEvent(
			priority_queue<PqHeight *, vector<PqHeight *>, PqHeight::LessThanByHeight> *pHeights,
			priority_queue<PqEvent *, vector<PqEvent *>, PqEvent::GreaterThanByPos> *pEvents,
			CurrentHeight *pCurrentHeight,
			bool firstTime)
		{
			int curPos = pEvents->top()->_pos;

			while (pEvents->size() > 0 && pEvents->top()->_pos == curPos)
			{
				auto pEvent = pEvents->top();
				pEvent->Handle(pHeights);
				pEvents->pop();
				delete pEvent;
			}

			if (pHeights->size() > 0)
			{
				PqHeight *pHeightCur = PqHeight::GetNextHeight(pHeights);
				if (pCurrentHeight->SetHeight(pHeightCur->Height))
				{
					char const *leftPad = (firstTime ? "" : " ");
					cout << leftPad << curPos << " " << pHeights->top()->Height;
				}
			}
		}

	private:
		bool _firstTime = true;
		bool _addEvent;
		PqHeight *_pHeight;
		int _pos;
	};

public:
	void Solve()
	{
		bool firstTime = true;
		PqHeight *heightZero = new PqHeight(0, true);

		_heights.push(heightZero);
		LoadBuildings();

		while (_events.size() > 0)
		{
			PqEvent::HandleNextPositionEvent(&_heights, &_events, &_currentHeight, firstTime);
			firstTime = false;
		}
		delete heightZero;
		cout << endl;
	}

private:
	priority_queue<PqHeight *, vector<PqHeight *>, PqHeight::LessThanByHeight> _heights;
	priority_queue<PqEvent *, vector<PqEvent *>, PqEvent::GreaterThanByPos> _events;
	CurrentHeight _currentHeight;

	void LoadBuildings()
	{
		while (true)
		{
			int left, height, right;
			cin >> left >> height >> right;

			// zero height buildings can safely be ignored
			if (height == 0)
			{
				continue;
			}

			auto pHeight = new PqHeight(height);
			_events.push(new PqEvent(pHeight, left, true));
			_events.push(new PqEvent(pHeight, right, false));
			if (cin.eof())
			{
				break;
			}
		}
	}
};

int main(void) {
	Skyline sl;
	sl.Solve();
	return 0;
}

sampleInput
R"(
1 11 5
2 12 5
2 6 7
3 13 9
12 7 16
14 3 25
19 18 22
23 13 29
24 4 28
30 2 31
31 3 33
)";

sampleOutput
R"(
1 11 2 12 3 13 9 0 12 7 16 3 19 18 22 3 23 13 29 0 30 2 31 3 33 0
)";

epilog
