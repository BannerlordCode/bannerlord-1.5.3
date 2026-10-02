using System;
using System.Collections.Generic;
using System.Diagnostics;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension.Standalone.Native.Windows;

namespace TaleWorlds.TwoDimension.Standalone
{
	// Token: 0x02000011 RID: 17
	public class WindowsForm
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x0000611D File Offset: 0x0000431D
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x00006125 File Offset: 0x00004325
		public int Width { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000DA RID: 218 RVA: 0x0000612E File Offset: 0x0000432E
		// (set) Token: 0x060000DB RID: 219 RVA: 0x00006136 File Offset: 0x00004336
		public int Height { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000DC RID: 220 RVA: 0x0000613F File Offset: 0x0000433F
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00006147 File Offset: 0x00004347
		public string Text { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00006150 File Offset: 0x00004350
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00006158 File Offset: 0x00004358
		public IntPtr Handle { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00006161 File Offset: 0x00004361
		public bool IsMinimized
		{
			get
			{
				return User32.IsIconic(this.Handle);
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00006170 File Offset: 0x00004370
		public WindowsForm(int x, int y, int width, int height, ResourceDepot resourceDepot, bool borderlessWindow = false, bool enableWindowBlur = false, string name = null)
			: this(x, y, width, height, resourceDepot, IntPtr.Zero, borderlessWindow, enableWindowBlur, name)
		{
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00006198 File Offset: 0x00004398
		public WindowsForm(int x, int y, int width, int height, ResourceDepot resourceDepot, IntPtr parent, bool borderlessWindow = false, bool enableWindowBlur = false, string name = null)
		{
			this.Handle = IntPtr.Zero;
			WindowsForm.classNameCount++;
			this.Width = width;
			this.Height = height;
			this.Text = "Form";
			this.windowClassName = "Form" + WindowsForm.classNameCount;
			this.wc = default(WindowClass);
			this._windowProcedure = new WndProc(this.WndProc);
			this.wc.style = 0U;
			this.wc.lpfnWndProc = this._windowProcedure;
			this.wc.cbClsExtra = 0;
			this.wc.cbWndExtra = 0;
			this.wc.hCursor = User32.LoadCursorFromFile(resourceDepot.GetFilePath("mb_cursor.cur"));
			this.wc.hInstance = Kernel32.GetModuleHandle(null);
			this.wc.lpszMenuName = null;
			this.wc.lpszClassName = this.windowClassName;
			this.wc.hbrBackground = Gdi32.CreateSolidBrush(IntPtr.Zero);
			User32.RegisterClass(ref this.wc);
			if (string.IsNullOrEmpty(name))
			{
				name = "Gauntlet UI: " + Process.GetCurrentProcess().Id;
			}
			WindowStyle windowStyle;
			if (parent != IntPtr.Zero)
			{
				windowStyle = WindowStyle.WS_CHILD | WindowStyle.WS_VISIBLE;
			}
			else if (!borderlessWindow)
			{
				windowStyle = WindowStyle.OverlappedWindow;
			}
			else
			{
				windowStyle = (WindowStyle)2416443392U;
			}
			this.Handle = User32.CreateWindowEx(0, this.windowClassName, name, windowStyle, x, y, width, height, parent, IntPtr.Zero, Kernel32.GetModuleHandle(null), IntPtr.Zero);
			if (enableWindowBlur)
			{
				DwmBlurBehind dwmBlurBehind = default(DwmBlurBehind);
				dwmBlurBehind.dwFlags = BlurBehindConstraints.Enable | BlurBehindConstraints.BlurRegion;
				dwmBlurBehind.hRgnBlur = Gdi32.CreateRectRgn(0, 0, -1, -1);
				dwmBlurBehind.fEnable = true;
				Dwmapi.DwmEnableBlurBehindWindow(this.Handle, ref dwmBlurBehind);
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00006374 File Offset: 0x00004574
		public WindowsForm(int width, int height, ResourceDepot resourceDepot)
			: this(100, 100, width, height, resourceDepot, false, false, null)
		{
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00006391 File Offset: 0x00004591
		public void SetParent(IntPtr parentHandle)
		{
			User32.SetParent(this.Handle, parentHandle);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000063A0 File Offset: 0x000045A0
		public void Show()
		{
			User32.ShowWindow(this.Handle, WindowShowStyle.Show);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x000063AF File Offset: 0x000045AF
		public void Hide()
		{
			User32.ShowWindow(this.Handle, WindowShowStyle.Hide);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000063BE File Offset: 0x000045BE
		public void Destroy()
		{
			this.Hide();
			User32.DestroyWindow(this.Handle);
			User32.UnregisterClass(this.windowClassName, IntPtr.Zero);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000063E3 File Offset: 0x000045E3
		public void AddMessageHandler(WindowsFormMessageHandler messageHandler)
		{
			this._messageHandlers.Add(messageHandler);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000063F4 File Offset: 0x000045F4
		private IntPtr WndProc(IntPtr hWnd, uint message, IntPtr wParam, IntPtr lParam)
		{
			long num = wParam.ToInt64();
			long num2 = lParam.ToInt64();
			if (message == 5U)
			{
				int num3 = (int)num2 % 65536;
				int num4 = (int)(num2 / 65536L);
				this.Width = num3;
				this.Height = num4;
			}
			foreach (WindowsFormMessageHandler windowsFormMessageHandler in this._messageHandlers)
			{
				windowsFormMessageHandler((WindowMessage)message, num, num2);
			}
			return User32.DefWindowProc(hWnd, message, wParam, lParam);
		}

		// Token: 0x0400005B RID: 91
		private static int classNameCount;

		// Token: 0x0400005C RID: 92
		private WindowClass wc;

		// Token: 0x0400005D RID: 93
		private string windowClassName;

		// Token: 0x0400005E RID: 94
		private WndProc _windowProcedure;

		// Token: 0x04000062 RID: 98
		private List<WindowsFormMessageHandler> _messageHandlers = new List<WindowsFormMessageHandler>();
	}
}
