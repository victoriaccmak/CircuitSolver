#include <iostream>
#include <windows.h>
#include <tchar.h>
#include <stdlib.h>
#include <string.h>

static TCHAR szWindowClass[] = _T("Hello World");
static TCHAR szTitle[] = _T("First C++ Windows App");
HINSTANCE h_inst;
LRESULT CALLBACK WndProc(HWND hWnd, UINT msg, WPARAM  wParam, LPARAM lParam);	
bool running{ true };
BITMAPINFO bmi{};

struct Render_State
{
	int width;
	int height;
	void* memory;
};

Render_State rs;

int CALLBACK WinMain(_In_ HINSTANCE hInstance, _In_opt_ HINSTANCE hPrevInstance, _In_ LPSTR lpCmdLine, _In_ int nCmdShow)
{
	//Window class
	WNDCLASSEX wcex{};

	wcex.cbSize = sizeof(WNDCLASSEX);
	wcex.style = CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc = WndProc;
	wcex.cbClsExtra = 0;
	wcex.cbWndExtra = 0;
	wcex.hInstance = hInstance;
	wcex.hIcon = LoadIcon(wcex.hInstance, IDI_APPLICATION);
	wcex.hCursor = LoadCursor(NULL, IDC_ARROW);
	wcex.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);
	wcex.lpszMenuName = NULL;
	wcex.lpszClassName = szWindowClass;
	wcex.hIconSm = LoadIcon(wcex.hInstance, IDI_APPLICATION);

	//Register class
	if (!RegisterClassEx(&wcex))
	{
		MessageBox(NULL, _T("Call to RegisterClassEx Failed!"), _T("Tutorial"), NULL);
		return 1;
	}

	h_inst = hInstance;

	//Create window
	HWND hWnd{};

	hWnd = CreateWindow(szWindowClass, 
						szTitle, 
						WS_OVERLAPPEDWINDOW, 
						CW_USEDEFAULT, 
						CW_USEDEFAULT, 
						700, 400, 
						NULL, NULL, hInstance, NULL);

	HDC hdc{ GetDC(hWnd) };

	if (!hWnd)
	{
		MessageBox(NULL, _T("Call to CreateWindow Failed!"), _T("Tutorial"), MB_YESNO | MB_ICONEXCLAMATION);
	}

	ShowWindow(hWnd, nCmdShow);
	UpdateWindow(hWnd);

	MSG msg{};

	char* a = new char[] {'H', 'i', '\n'};

	while (running)
	{
		//Input
		while (PeekMessage(&msg, hWnd, 0, 0, PM_REMOVE))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
		//Simulate
		unsigned int* pixel = (unsigned int*)rs.memory;
		for (int y{}; y < rs.height; ++y)
		{
			for (int x{}; x < rs.width; ++x)
			{
				*pixel = x * y;
				pixel++;
			}
		}

		//Render
		StretchDIBits(hdc, 0, 0, rs.width, rs.height, 0, 0, rs.width, rs.height, rs.memory, &bmi, DIB_RGB_COLORS, SRCCOPY);
	}

	return (int)msg.wParam;
}

LRESULT CALLBACK WndProc(HWND hWnd, UINT msg, WPARAM  wParam, LPARAM lParam)
{
	try
	{
		PAINTSTRUCT ps;
		HDC hdc{};
		TCHAR greeting[] = _T("Hello World!!!!");
		//OutputDebugString(" " + msg);
		switch (msg)
		{
			/*case WM_PAINT:
			{
				hdc = BeginPaint(hWnd, &ps);

				TextOut(hdc, 5, 5, greeting, _tcslen(greeting));

				EndPaint(hWnd, &ps);
				return 1;
			}
			break;*/
			case WM_CLOSE:
			case WM_DESTROY:
			{
				PostQuitMessage(0);
				running = false;
				return 0;
			}
			break;
			case WM_SIZE:
			{
				RECT rect{};
				GetClientRect(hWnd, &rect);
				rs.width = rect.right - rect.left;
				rs.height = rect.bottom - rect.top;
				int bufferSize = rs.width * rs.height * sizeof(unsigned int);

				if (rs.memory)
				{
					VirtualFree(rs.memory, 0, MEM_RELEASE);
				}

				rs.memory = VirtualAlloc(0, bufferSize, MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
				bmi.bmiHeader.biSize = sizeof(bmi.bmiHeader);
				bmi.bmiHeader.biWidth = rs.width;
				bmi.bmiHeader.biHeight = rs.height;
				bmi.bmiHeader.biPlanes = 1;
				bmi.bmiHeader.biBitCount = 32;
				bmi.bmiHeader.biCompression = BI_RGB;
			}
			break;
			default:
			{
				return DefWindowProc(hWnd, msg, wParam, lParam);
			}
			break;
		}
	}
	catch (std::exception& e)
	{
		OutputDebugStringA(e.what());
	}

	return 0;
}
