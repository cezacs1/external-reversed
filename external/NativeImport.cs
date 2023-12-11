using System;
using System.Runtime.InteropServices;
using System.Text;

// Token: 0x02000012 RID: 18
internal class NativeImport
{
	// Token: 0x06000040 RID: 64
	[DllImport("kernel32.dll")]
	public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

	// Token: 0x06000041 RID: 65
	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, uint dwSize, out int lpNumberOfBytesRead);

	// Token: 0x06000042 RID: 66
	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool CloseHandle(IntPtr hObject);

	// Token: 0x06000045 RID: 69
	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
	public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

	// Token: 0x06000046 RID: 70
	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool SetCursorPos(int x, int y);

	// Token: 0x06000047 RID: 71
	[DllImport("user32.dll")]
	public static extern IntPtr GetForegroundWindow();

	// Token: 0x06000048 RID: 72
	[DllImport("user32.dll")]
	public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

	// Token: 0x06000049 RID: 73
	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool BlockInput([MarshalAs(UnmanagedType.Bool)] bool fBlockIt);

	// Token: 0x0600004A RID: 74 RVA: 0x00002FB4 File Offset: 0x000011B4
	public static string GetActiveWindowTitle()
	{
		StringBuilder stringBuilder = new StringBuilder(256);
		if (NativeImport.GetWindowText(NativeImport.GetForegroundWindow(), stringBuilder, 256) > 0)
		{
			return stringBuilder.ToString();
		}
		return null;
	}

	// Token: 0x0600004B RID: 75
	[DllImport("gdi32.dll")]
	public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

	// Token: 0x0600004C RID: 76
	[DllImport("kernel32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool AllocConsole();

	// Token: 0x0600004D RID: 77
	[DllImport("kernel32.dll")]
	public static extern IntPtr GetConsoleWindow();

	// Token: 0x0600004E RID: 78
	[DllImport("user32.dll")]
	public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

	// Token: 0x0600004F RID: 79
	[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
	public static extern bool FreeConsole();

	// Token: 0x06000050 RID: 80
	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, uint lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, uint hTemplateFile);

	// Token: 0x06000051 RID: 81
	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

	// Token: 0x06000052 RID: 82
	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

	// Token: 0x06000053 RID: 83
	[DllImport("kernel32.dll")]
	public static extern bool SetStdHandle(int nStdHandle, IntPtr hHandle);

	// Token: 0x06000054 RID: 84
	[DllImport("user32.dll")]
	public static extern ushort GetAsyncKeyState(int vKey);

	// Token: 0x06000055 RID: 85
	[DllImport("user32.dll")]
	public static extern short GetKeyState(int nVirtKey);

	// Token: 0x06000056 RID: 86
	[DllImport("kernel32.dll")]
	public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);

	// Token: 0x06000057 RID: 87
	[DllImport("kernel32.dll")]
	public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

	// Token: 0x06000058 RID: 88 RVA: 0x00002FF0 File Offset: 0x000011F0
	public static ulong ReadUInt64(IntPtr hProcess, ulong addr)
	{
		byte[] array = new byte[8];
		int num = 0;
		NativeImport.ReadProcessMemory(hProcess, (IntPtr)((long)addr), array, array.Length, out num);
		return BitConverter.ToUInt64(array, 0);
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00003024 File Offset: 0x00001224
	public static void Read(IntPtr hProcess, ulong addr, byte[] buffer, int size)
	{
		int num = 0;
		NativeImport.ReadProcessMemory(hProcess, (IntPtr)((long)addr), buffer, size, out num);
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00003048 File Offset: 0x00001248
	public static ulong ReadUInt64FromBuffer(byte[] buffer, int position)
	{
		return BitConverter.ToUInt64(buffer, position);
	}

	// Token: 0x0600005B RID: 91
	[DllImport("user32.dll")]
	public static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

	// Token: 0x0400004F RID: 79
	public const int PROCESS_WM_READ = 16;

	// Token: 0x04000050 RID: 80
	public const int SW_HIDE = 0;

	// Token: 0x04000051 RID: 81
	public const int SW_SHOW = 5;

	// Token: 0x04000052 RID: 82
	public const int MY_CODE_PAGE = 437;

	// Token: 0x04000053 RID: 83
	public const uint GENERIC_WRITE = 1073741824U;

	// Token: 0x04000054 RID: 84
	public const uint GENERIC_READ = 2147483648U;

	// Token: 0x04000055 RID: 85
	public const uint FILE_SHARE_WRITE = 2U;

	// Token: 0x04000056 RID: 86
	public const uint OPEN_EXISTING = 3U;

	// Token: 0x04000057 RID: 87
	public const int WM_NCLBUTTONDOWN = 161;

	// Token: 0x04000058 RID: 88
	public const int HTCAPTION = 2;
}
