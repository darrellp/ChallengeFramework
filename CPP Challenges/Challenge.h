#include <vector>

#define prolog(contest, nameVar, name, uri) namespace nameVar {auto dummy = new Challenge(contest,name,uri); extern "C" {
#define epilog }}
#define main() __declspec(dllexport) main()

class Challenge
{
	friend char *GatherChallengeInfoCore();
public:
	Challenge(char *contest, char *name, char *uri);
	~Challenge();
	static char *GatherChallengeInfo();

	char *GetContest();
	char *GetName();
	char *GetUri();

private:
	static std::vector<Challenge *> challenges;
	char *_contest;
	char *_name;
	char *_uri;
};

