#include <iostream>

using namespace std;

void CodingTest01()
{
	// 자연수 N이 입력되면 1부터 N까지 수 중 M의 배수합을 툴력하는 프로그램을 작성

	int n = 0, m = 0, sum = 0;

	cin >> n >> m;

	for (int i = 0; i <= n; ++i)
	{
		if (i % m == 0)
		{
			sum += i;
		}
	}

	cout << sum << endl;
}


int main(void)
{

}