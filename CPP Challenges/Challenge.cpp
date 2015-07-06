#include "stdafx.h"
#include "Challenge.h"
#include <vector>
#include <list>
#include <sstream>
#include <cstdio>
#include <iostream>

std::vector<Challenge *> Challenge::challenges;

Challenge::Challenge(char *contest, char *name, char *uri, char *input, char *output, solverT solver)
{
	_contest = contest;
	_name = name;
	_uri = uri;
	_input = input;
	_output = output;
	_solver = solver;
	challenges.push_back(this);
}

Challenge::~Challenge()
{}

char* StringReturn(const char *in)
{
	ULONG ulSize = strlen(in) + sizeof(char);
	char* pszReturn = (char*)::CoTaskMemAlloc(ulSize);
	strcpy(pszReturn, in);
	return pszReturn;
}

char *Challenge::RunSolver(int index, char *input)
{
	// Set up our input stream
	std::stringstream stmInput;
	stmInput << input;

	// Redirect cout
	std::streambuf *backupOutput = std::cout.rdbuf();
	std::stringstream sout;
	std::streambuf *soutbuf = sout.rdbuf();
	std::cout.rdbuf(soutbuf);

	// Redirect cin
	std::streambuf *backupInput = std::cin.rdbuf();
	std::streambuf *psbuf = stmInput.rdbuf();
	std::cin.rdbuf(psbuf);

	solverT solver = challenges[index]->GetSolver();
	solver();

	// Restore cin, cout
	std::cin.rdbuf(backupInput);
	std::cout.rdbuf(backupOutput);

	return ::StringReturn(sout.str().c_str());
}

char *Challenge::GatherChallengeInfo()
{
	std::ostringstream stm;

	for (auto pChallenge = challenges.begin(); pChallenge != challenges.end(); ++pChallenge)
	{
		if (strchr((*pChallenge)->GetContest(), '$') ||
			strchr((*pChallenge)->GetName(), '<') ||
			strchr((*pChallenge)->GetUri(), '>') ||
			strchr((*pChallenge)->GetInput(), '$') ||
			strchr((*pChallenge)->GetOutput(), '$'))
		{
			return nullptr;
		}
		stm <<
			(*pChallenge)->GetContest() <<
			"$" <<
			(*pChallenge)->GetName() <<
			"<" <<
			(*pChallenge)->GetUri() <<
			">" <<
			(*pChallenge)->GetInput() <<
			"$" <<
			(*pChallenge)->GetOutput() <<
			"$";
	}
	return ::StringReturn(stm.str().c_str());
}

char *Challenge::GetContest() { return _contest; }
char *Challenge::GetName() { return _name; }
char *Challenge::GetUri() { return _uri; }
// Skip initial \n for input and output...
char *Challenge::GetInput() { return _input; }
char *Challenge::GetOutput() { return _output; }
solverT Challenge::GetSolver() { return _solver; }


extern "C"
{
	__declspec(dllexport) char *GatherChallengeInfo()
	{
		return Challenge::GatherChallengeInfo();
	}

	__declspec(dllexport) char *RunSolver(int index, char *input)
	{
		return Challenge::RunSolver(index, input);
	}
}
