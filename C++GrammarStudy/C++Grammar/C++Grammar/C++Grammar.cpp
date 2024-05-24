#include <iostream>

using namespace std;

void VariableAndCalculation()
{
	// [타입][이름]

	// ** 정수형
	// char					:1byte
	// short				:2byte
	// int					:4byte
	// __int64(long long)	:8byte

	// 문자
	char chNum;
	char chChar;

	// ** 실수형
	// float	: 4byte
	// double   : 8byte
	float speed = 0.5f;

	// ** 불리언
	// bool 
	bool isAttack = false;

	// 0b 0000	-> 2진법 표현 가능
	// 0x F		-> 16진법 표현 가능

	chChar = 'a';
	cout << chChar << endl;
	chNum = 97;
	cout << chNum << endl;
	// 아스키 코드상 65~90 영어 대문자/97~122 영어 소문자

	isAttack = true;

	// **산술연산
	// + - * / % 
	int a, b;
	a = 10;
	b = 10;

	cout << (a + b) << endl;
	cout << (a - b) << endl;
	cout << (a * b) << endl;
	cout << (a / b) << endl;
	cout << (a % b) << endl;

	// **쉬프트 연산
	// << >> : 비트를 한칸씩 밈
	// ※ Warning!!!! unsigned로 하지 않았을때 내가 생각한 연산과 다르게 나올수 있으니 주의

	// **비교연산
	// == : 같은지 
	// != : 다른지
	// <
	// <=
	// >
	// >=

	// **논리연산
	// !  : not;
	// && : and;
	// || : or;
	bool isRich;
	bool isTall;

	isRich = true;
	isTall = true;
	cout << !isRich << endl;
}
void ConditionalStatement()
{
	// **조건문
	// if(조건){내용}
	// if- else
	// if- else if
	int choice;

	cin >> choice;

	if (choice == 0)	// 가위
	{
		cout << "가위" << endl;
	}
	else if(choice == 1)	//바위
	{
		cout << "바위" << endl;
	}
	else if (choice == 2)	// 보
	{
		cout << "보" << endl;
	}
	else 
	{
		cout << "잘못냄" << endl;
	}

	// ↓ 가독성 측면에서 더 효율적

	switch (choice)
	{
		case 0:
			cout << "가위" << endl;
			break;
		case 1:
			cout << "바위" << endl;
			break;
		case 2:
			cout << "보" << endl;
			break;
		default:
			cout << "잘못냄" << endl;
			break;
	}
}
void Loop()
{
	// **반복문 
	// while 
	// for 
	// do-while 

	int count;

	while (count <= 5)
	{
		cout << "ㅋ" << endl;
		count++;
	}

	// 위와 아래는 결국 같은 코드라는 것

	// for(초기화문; 조건문; 제어문){}
	for (int cnt = 0; cnt <= 5; cnt++)
	{
		cout << "ㅋ" << endl;
		cnt++;

		if (cnt > 4)
			break;
	}

	// 빠져나오기	=> break;
	// 지나치기		=> continue; 
	// ※ Warning!!!! while의 경우는 처음으로 그냥 다시 돌아가버리지만 for문의 경우는 제어문 갓다가 조건문 다시 검사하기 때문에
	//                한번 지나치기만 하지만 while은 문장이 끝나버리는 경우가 발생 할 수 있다.
}

int main()
{
	ConditionalStatement();
	return 0;
}

