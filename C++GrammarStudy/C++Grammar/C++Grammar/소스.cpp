#include <iostream>

using namespace std;

void CodingTest01()
{
	// �ڿ��� N�� �ԷµǸ� 1���� N���� �� �� M�� ������� �����ϴ� ���α׷��� �ۼ�

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