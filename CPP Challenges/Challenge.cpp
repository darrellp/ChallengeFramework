#include "stdafx.h"
#include "Challenge.h"
#include <vector>
#include <sstream>

std::vector<Challenge *> Challenge::challenges;

Challenge::Challenge(char *contest, char *name, char *uri)
{
	_contest = contest;
	_name = name;
	_uri = uri;
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

char *Challenge::GatherChallengeInfo()
{
	std::ostringstream stm;

	for (auto pChallenge = challenges.begin(); pChallenge != challenges.end(); ++pChallenge)
	{
		if (strchr((*pChallenge)->GetContest(), '/') ||
			strchr((*pChallenge)->GetName(), '<') ||
			strchr((*pChallenge)->GetUri(), '>'))
		{
			return nullptr;
		}
		stm <<
			(*pChallenge)->GetContest() <<
			"/" <<
			(*pChallenge)->GetName() <<
			"<" <<
			(*pChallenge)->GetUri() <<
			">";
	}
	return ::StringReturn(stm.str().c_str());
}

char *Challenge::GetContest() { return _contest; }
char *Challenge::GetName() { return _name; }
char *Challenge::GetUri() { return _uri; }


extern "C"
{
	__declspec(dllexport) char *GatherChallengeInfo()
	{
		return Challenge::GatherChallengeInfo();
	}
}
