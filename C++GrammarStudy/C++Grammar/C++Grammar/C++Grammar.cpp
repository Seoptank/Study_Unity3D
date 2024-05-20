#include <iostream>

using namespace std;

// [타입][이름]

// char					:1byte
// short				:2byte
// int					:4byte
// __int64(long long)	:8byte

int	hp;
int	maxHp;


int main()
{
	maxHp = 100;

	hp = maxHp;

	cout << hp;

	return 0;
}

