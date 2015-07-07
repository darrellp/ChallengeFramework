#include <vector>

#define prolog(c, nv, n, u) namespace nv {char *_v1_ = c, *_v2_ = n, *_v3_ = u;
#define sampleInput char *input = 
#define sampleOutput char *output = 
#define epilog auto d = new Challenge(_v1_,_v2_,_v3_,input,output,&main);}
typedef int (*solverT)();

class Challenge
{
	friend char *GatherChallengeInfoCore();
public:
	Challenge(char *contest, char *name, char *uri, char *input, char *output, solverT solver);
	~Challenge();
	static char *GatherChallengeInfo();
	static char *RunSolver(int index, char *input);

	char *GetContest();
	char *GetName();
	char *GetUri();
	char *GetInput();
	char *GetOutput();
	solverT GetSolver();

private:
	static std::vector<Challenge *> challenges;
	char *_contest;
	char *_name;
	char *_uri;
	char *_input;
	char *_output;
	solverT _solver;
};

