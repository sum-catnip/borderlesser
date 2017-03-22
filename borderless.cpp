#include <windows.h>
#include <cstdio>

HWND getWindow() {
	printf("Press CRLT + H to make the currently focussed window borderless\n");
	MSG msg;
	while (GetMessage(&msg, NULL, 0, 0) != 0) {
		if (msg.message == WM_HOTKEY) {
			return GetForegroundWindow();
		}
	}
}

int main() {
	RegisterHotKey(NULL, 1, MOD_CONTROL, 0x48);
	int screenWidth = GetSystemMetrics(SM_CXSCREEN);
	int screenHeigth = GetSystemMetrics(SM_CYSCREEN);
	HWND window;
	char windowTitle[100];
	while (true) {
		window = getWindow();
		SetWindowLong(window, GWL_STYLE, 0x00000000);
		SetWindowLong(window, GWL_STYLE, 0x94000000);
		SetWindowPos(window, HWND_TOPMOST, 0, 0, screenWidth, screenHeigth, NULL);
		GetWindowText(window, windowTitle, 100);
		printf("Set window: %s to borderless\n\n", windowTitle);
	}
}