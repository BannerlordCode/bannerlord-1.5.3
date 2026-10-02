using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000042 RID: 66
	public static class User32
	{
		// Token: 0x06000151 RID: 337
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern short GetAsyncKeyState(int vkey);

		// Token: 0x06000152 RID: 338
		[DllImport("user32.dll")]
		public static extern bool DestroyWindow(IntPtr hWnd);

		// Token: 0x06000153 RID: 339
		[DllImport("user32.dll")]
		public static extern IntPtr GetDC(IntPtr hWnd);

		// Token: 0x06000154 RID: 340
		[DllImport("user32.dll")]
		public static extern IntPtr SetParent(IntPtr child, IntPtr newParent);

		// Token: 0x06000155 RID: 341
		[DllImport("user32.dll")]
		public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

		// Token: 0x06000156 RID: 342
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint);

		// Token: 0x06000157 RID: 343
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetCursorPos(out Point lpPoint);

		// Token: 0x06000158 RID: 344
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ReleaseCapture();

		// Token: 0x06000159 RID: 345
		[DllImport("user32.dll")]
		public static extern IntPtr SetCapture(IntPtr hWnd);

		// Token: 0x0600015A RID: 346
		[DllImport("user32.dll")]
		public static extern IntPtr SetActiveWindow(IntPtr hWnd);

		// Token: 0x0600015B RID: 347
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		// Token: 0x0600015C RID: 348
		[DllImport("user32.dll")]
		public static extern IntPtr CreateWindowEx(int dwExStyle, [MarshalAs(UnmanagedType.LPTStr)] string lpClassName, string lpWindowName, WindowStyle dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

		// Token: 0x0600015D RID: 349
		[DllImport("user32.dll")]
		public static extern bool ShowWindow(IntPtr hWnd, WindowShowStyle nCmdShow);

		// Token: 0x0600015E RID: 350
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsIconic(IntPtr hWnd);

		// Token: 0x0600015F RID: 351
		[DllImport("user32.dll")]
		public static extern bool CloseWindow(IntPtr hWnd);

		// Token: 0x06000160 RID: 352
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool PeekMessage(out NativeMessage lpMsg, [In] IntPtr hWnd, [In] uint wMsgFilterMin, [In] uint wMsgFilterMax, [In] uint wRemoveMsg);

		// Token: 0x06000161 RID: 353
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool TranslateMessage([In] ref NativeMessage lpMsg);

		// Token: 0x06000162 RID: 354
		[DllImport("user32.dll")]
		public static extern IntPtr DispatchMessage([In] ref NativeMessage lpMsg);

		// Token: 0x06000163 RID: 355
		[DllImport("user32.dll")]
		public static extern ushort RegisterClass([In] ref WindowClass lpWndClass);

		// Token: 0x06000164 RID: 356
		[DllImport("user32.dll")]
		public static extern bool UnregisterClass([MarshalAs(UnmanagedType.LPTStr)] string lpClassName, IntPtr hInstance);

		// Token: 0x06000165 RID: 357
		[DllImport("user32.dll")]
		public static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000166 RID: 358
		[DllImport("user32.dll")]
		public static extern IntPtr LoadCursorFromFile(string lpFileName);

		// Token: 0x06000167 RID: 359
		[DllImport("user32.dll")]
		public static extern IntPtr GetDesktopWindow();

		// Token: 0x06000168 RID: 360
		[DllImport("user32.dll")]
		public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

		// Token: 0x06000169 RID: 361
		[DllImport("user32.dll")]
		public static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

		// Token: 0x0600016A RID: 362
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetWindowRect(IntPtr hWnd, out Rectangle lpRect);

		// Token: 0x0600016B RID: 363
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

		// Token: 0x0600016C RID: 364
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

		// Token: 0x0600016D RID: 365
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UpdateWindow(IntPtr hWnd);

		// Token: 0x0600016E RID: 366
		[DllImport("user32.dll")]
		public static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

		// Token: 0x0600016F RID: 367
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UpdateLayeredWindow(IntPtr hWnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc, int crKey, ref BlendFunction pblend, int dwFlags);

		// Token: 0x06000170 RID: 368
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetMessage(out NativeMessage lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

		// Token: 0x06000171 RID: 369
		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000172 RID: 370
		[DllImport("user32.dll")]
		public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

		// Token: 0x06000173 RID: 371
		[DllImport("user32.dll")]
		public static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, User32.MonitorEnumDelegate lpfnEnum, IntPtr dwData);

		// Token: 0x06000174 RID: 372
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetMonitorInfo(IntPtr hMonitor, ref User32.MONITORINFOEX lpmi);

		// Token: 0x04000193 RID: 403
		public const uint MONITOR_DEFAULTTONULL = 0U;

		// Token: 0x04000194 RID: 404
		public const uint MONITOR_DEFAULTTOPRIMARY = 1U;

		// Token: 0x04000195 RID: 405
		public const uint MONITOR_DEFAULTTONEAREST = 2U;

		// Token: 0x02000085 RID: 133
		public struct RECT
		{
			// Token: 0x04000217 RID: 535
			public int left;

			// Token: 0x04000218 RID: 536
			public int top;

			// Token: 0x04000219 RID: 537
			public int right;

			// Token: 0x0400021A RID: 538
			public int bottom;
		}

		// Token: 0x02000086 RID: 134
		// (Invoke) Token: 0x0600025C RID: 604
		public delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref User32.RECT lprcMonitor, IntPtr lParam);

		// Token: 0x02000087 RID: 135
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct MONITORINFOEX
		{
			// Token: 0x0400021B RID: 539
			public int cbSize;

			// Token: 0x0400021C RID: 540
			public User32.RECT rcMonitor;

			// Token: 0x0400021D RID: 541
			public User32.RECT rcWork;

			// Token: 0x0400021E RID: 542
			public uint dwFlags;

			// Token: 0x0400021F RID: 543
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string szDevice;
		}
	}
}
