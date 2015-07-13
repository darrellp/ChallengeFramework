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
#include <vector>
#include <memory>
#include <string>

prolog("UVA", Blocks, "Blocks - cpp", "https://uva.onlinejudge.org/index.php?option=com_onlinejudge&Itemid=8&category=3&page=show_problem&problem=37")

using namespace std;
typedef vector<vector<int>> blkType;

blkType *pblocks;
vector<int> *pinverseBlocks;

static void returnBlock(vector<int> *pstack)
{
	if (pstack->size() != 0)
	{
		int returnBlock = pstack->back();
		pstack->pop_back();
		(*pinverseBlocks)[returnBlock] = returnBlock;
		(*pblocks)[returnBlock].push_back(returnBlock);
	}
}

static void clear(int block)
{
	vector<int> *pstack = &(*pblocks)[(*pinverseBlocks)[block]];
	size_t iBlock;
	while ((iBlock = pstack->back()) != block)
	{
		returnBlock(pstack);
	}
}

static void move(int blockFrom, int blockTo)
{
	vector<int> tmpStack;
	int iFrom = (*pinverseBlocks)[blockFrom];
	int iTo = (*pinverseBlocks)[blockTo];
	vector<int> *stackFrom = &(*pblocks)[iFrom];
	vector<int> *stackTo = &(*pblocks)[iTo];
	size_t iBlock;

	while ((iBlock = stackFrom->back()) != blockFrom)
	{
		stackFrom->pop_back();
		tmpStack.push_back(iBlock);
	}
	// Pop blockFrom off the from stack
	stackFrom->pop_back();
	// ...and push it onto the to stack
	stackTo->push_back(blockFrom);
	(*pinverseBlocks)[blockFrom] = iTo;

	while (tmpStack.size() != 0)
	{
		iBlock = tmpStack.back();
		tmpStack.pop_back();
		stackTo->push_back(iBlock);
		(*pinverseBlocks)[iBlock] = iTo;
	}
}

static void perform(string verb, size_t blockFrom, string type, size_t blockTo)
{
	if ((*pinverseBlocks)[blockFrom] == (*pinverseBlocks)[blockTo])
	{
		return;
	}

	if (verb == "move")
	{
		clear(blockFrom);
	}
	if (type == "onto")
	{
		clear(blockTo);
	}
	move(blockFrom, blockTo);
}

void report()
{
	for (size_t iStack = 0; iStack < (*pblocks).size(); iStack++)
	{
		vector<int> *pStack = &(*pblocks)[iStack];

		cout << iStack << ":";
		for (size_t iBlock = 0; iBlock < (*pStack).size(); iBlock++)
		{
			cout << " " << (*pStack)[iBlock];
		}
		cout << endl;
	}
}

int main(void) {
	size_t cBlocks;
	cin >> cBlocks;
	auto pBlocksShared = shared_ptr<blkType>(new blkType(cBlocks));
	auto pInverseBlocksShared = shared_ptr<vector<int>>(new vector<int>(cBlocks));
	pblocks = pBlocksShared.get();
	pinverseBlocks = pInverseBlocksShared.get();

	for (size_t iStack = 0; iStack < cBlocks; iStack++)
	{
		(*pblocks)[iStack].push_back(iStack);
		(*pinverseBlocks)[iStack] = iStack;
	}
	string verb, type;
	size_t blockFrom, blockTo;

	cin >> verb;
	while (verb != "quit")
	{
		cin >> blockFrom >> type >> blockTo;
		perform(verb, blockFrom, type, blockTo);
		cin >> verb;
	}

	report();
	return 0;
}

sampleInput
R"(
10
move 9 onto 1
move 8 over 1
move 7 over 1
move 6 over 1
pile 8 over 6
pile 8 over 5
move 2 over 1
move 4 over 9
quit
)";

sampleOutput
R"(
0: 0
1: 1 9 2 4
2:
3: 3
4:
5: 5 8 7 6
6:
7:
8:
9:
)";

epilog
