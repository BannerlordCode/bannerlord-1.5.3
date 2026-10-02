using System;
using System.Drawing;
using System.Numerics;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension.Standalone.Native.Windows;

namespace TaleWorlds.TwoDimension.Standalone
{
	// Token: 0x02000009 RID: 9
	public class GraphicsForm : IMessageCommunicator
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000057 RID: 87 RVA: 0x000046CC File Offset: 0x000028CC
		// (set) Token: 0x06000058 RID: 88 RVA: 0x000046D4 File Offset: 0x000028D4
		public DirectXGraphicsContext GraphicsContext { get; private set; }

		// Token: 0x06000059 RID: 89 RVA: 0x000046E0 File Offset: 0x000028E0
		public GraphicsForm(int width, int height, ResourceDepot resourceDepot, bool borderlessWindow = false, bool enableWindowBlur = false, bool layeredWindow = false, string name = null)
		{
			DXGI.RECT rect = this.DecideWindowPosition();
			int num = rect.right - rect.left;
			int num2 = rect.bottom - rect.top;
			int num3 = rect.left + (num - width) / 2;
			int num4 = rect.top + (num2 - height) / 2;
			this._windowsForm = new WindowsForm(num3, num4, width, height, resourceDepot, borderlessWindow, enableWindowBlur, name);
			this.Initalize(layeredWindow);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00004764 File Offset: 0x00002964
		public GraphicsForm(int x, int y, int width, int height, ResourceDepot resourceDepot, bool borderlessWindow = false, bool enableWindowBlur = false, bool layeredWindow = false, string name = null)
		{
			this._windowsForm = new WindowsForm(x, y, width, height, resourceDepot, borderlessWindow, enableWindowBlur, name);
			this.Initalize(layeredWindow);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000047A9 File Offset: 0x000029A9
		public GraphicsForm(WindowsForm windowsForm)
		{
			this._windowsForm = windowsForm;
			this.Initalize(false);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000047D4 File Offset: 0x000029D4
		private void Initalize(bool layeredWindow)
		{
			this._currentInputData = new InputData();
			this._oldInputData = new InputData();
			this._messageLoopInputData = new InputData();
			this._windowsForm.AddMessageHandler(new WindowsFormMessageHandler(this.MessageHandler));
			this._windowsForm.Show();
			this.GraphicsContext = new DirectXGraphicsContext();
			this._layeredWindow = layeredWindow;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00004838 File Offset: 0x00002A38
		public DXGI.RECT DecideWindowPosition()
		{
			IntPtr intPtr = User32.MonitorFromWindow(User32.GetDesktopWindow(), 1U);
			Rectangle rectangle;
			User32.GetClientRect(User32.GetDesktopWindow(), out rectangle);
			DXGI.RECT rect = new DXGI.RECT
			{
				left = rectangle.Left,
				right = rectangle.Right,
				top = rectangle.Top,
				bottom = rectangle.Bottom
			};
			DXGI.RECT rect2 = rect;
			IntPtr zero = IntPtr.Zero;
			DXGI.CreateDXGIFactory(ref DXGI.IID_IDXGIFactory, out zero);
			if (zero == IntPtr.Zero)
			{
				return rect2;
			}
			MBList<Tuple<uint, ulong>> mblist = new MBList<Tuple<uint, ulong>>();
			uint num = 0U;
			IntPtr intPtr2;
			while (DXGIFactory.EnumAdapters(zero, num, out intPtr2) == 0)
			{
				DXGI.DXGI_ADAPTER_DESC dxgi_ADAPTER_DESC;
				DXGIAdapter.GetDesc(intPtr2, out dxgi_ADAPTER_DESC);
				ulong num2 = (ulong)dxgi_ADAPTER_DESC.DedicatedVideoMemory;
				if (num2 > 0UL)
				{
					mblist.Add(new Tuple<uint, ulong>(num, num2));
				}
				ComRelease.Release(intPtr2);
				num += 1U;
			}
			if (mblist.Count == 0)
			{
				ComRelease.Release(zero);
				return rect2;
			}
			mblist.Sort((Tuple<uint, ulong> x, Tuple<uint, ulong> y) => y.Item2.CompareTo(x.Item2));
			foreach (Tuple<uint, ulong> tuple in mblist)
			{
				IntPtr intPtr3;
				if (DXGIFactory.EnumAdapters(zero, tuple.Item1, out intPtr3) == 0)
				{
					uint num3 = 0U;
					IntPtr intPtr4;
					while (DXGIAdapter.EnumOutputs(intPtr3, num3, out intPtr4) == 0)
					{
						DXGI.DXGI_OUTPUT_DESC dxgi_OUTPUT_DESC;
						DXGIOutput.GetDesc(intPtr4, out dxgi_OUTPUT_DESC);
						ComRelease.Release(intPtr4);
						if (dxgi_OUTPUT_DESC.AttachedToDesktop && dxgi_OUTPUT_DESC.Monitor == intPtr)
						{
							ComRelease.Release(intPtr3);
							ComRelease.Release(zero);
							return dxgi_OUTPUT_DESC.DesktopCoordinates;
						}
						num3 += 1U;
					}
					ComRelease.Release(intPtr3);
				}
			}
			foreach (Tuple<uint, ulong> tuple2 in mblist)
			{
				IntPtr intPtr5;
				if (DXGIFactory.EnumAdapters(zero, tuple2.Item1, out intPtr5) == 0)
				{
					uint num4 = 0U;
					IntPtr intPtr6;
					while (DXGIAdapter.EnumOutputs(intPtr5, num4, out intPtr6) == 0)
					{
						DXGI.DXGI_OUTPUT_DESC dxgi_OUTPUT_DESC2;
						DXGIOutput.GetDesc(intPtr6, out dxgi_OUTPUT_DESC2);
						ComRelease.Release(intPtr6);
						if (dxgi_OUTPUT_DESC2.AttachedToDesktop)
						{
							ComRelease.Release(intPtr5);
							ComRelease.Release(zero);
							return dxgi_OUTPUT_DESC2.DesktopCoordinates;
						}
						num4 += 1U;
					}
					ComRelease.Release(intPtr5);
				}
			}
			ComRelease.Release(zero);
			return rect2;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00004AA8 File Offset: 0x00002CA8
		public void Destroy()
		{
			if (this._isFinalized)
			{
				return;
			}
			this._isFinalized = true;
			LayeredWindowController layeredWindowController = this._layeredWindowController;
			if (layeredWindowController != null)
			{
				layeredWindowController.OnFinalize();
			}
			this._windowsForm.Destroy();
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00004AD6 File Offset: 0x00002CD6
		public void MinimizeWindow()
		{
			User32.ShowWindow(this._windowsForm.Handle, WindowShowStyle.Minimize);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004AEC File Offset: 0x00002CEC
		public void InitializeGraphicsContext(ResourceDepot resourceDepot)
		{
			if (this._layeredWindow)
			{
				this.GraphicsContext.IsLayeredWindow = true;
			}
			this.GraphicsContext.CreateContext(this._windowsForm.Handle, resourceDepot);
			this.GraphicsContext.ProjectionMatrix = MatrixExtensions.CreateOrthographicOffCenter(0f, (float)this._windowsForm.Width, (float)this._windowsForm.Height, 0f, 0f, 2f);
			if (this._layeredWindow)
			{
				this._layeredWindowController = new LayeredWindowController(this._windowsForm.Handle, this._windowsForm.Width, this._windowsForm.Height, this.GraphicsContext);
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00004B9C File Offset: 0x00002D9C
		public void BeginFrame()
		{
			if (this.GraphicsContext != null)
			{
				this.GraphicsContext.BeginFrame(this._windowsForm.Width, this._windowsForm.Height);
				this.GraphicsContext.ProjectionMatrix = MatrixExtensions.CreateOrthographicOffCenter(0f, (float)this._windowsForm.Width, (float)this._windowsForm.Height, 0f, 0f, 2f);
				LayeredWindowController layeredWindowController = this._layeredWindowController;
				if (layeredWindowController == null)
				{
					return;
				}
				layeredWindowController.SetSize(this._windowsForm.Width, this._windowsForm.Height);
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00004C38 File Offset: 0x00002E38
		public void Update()
		{
			if (!this._isDragging && this._mouseOverDragArea && this._currentInputData.LeftMouse && !this._oldInputData.LeftMouse)
			{
				this._isDragging = true;
				this.MessageHandler(WindowMessage.LeftButtonUp, 0L, 0L);
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00004C88 File Offset: 0x00002E88
		public void MessageLoop()
		{
			if (this._isDragging)
			{
				User32.ReleaseCapture();
				User32.SendMessage(this._windowsForm.Handle, 161U, new IntPtr(2), IntPtr.Zero);
				this._isDragging = false;
				User32.SetCapture(this._windowsForm.Handle);
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00004CDC File Offset: 0x00002EDC
		public void UpdateInput(bool mouseOverDragArea = false)
		{
			this._mouseOverDragArea = mouseOverDragArea;
			InputData oldInputData = this._oldInputData;
			this._oldInputData = this._currentInputData;
			this._currentInputData = oldInputData;
			object inputDataLocker = this._inputDataLocker;
			lock (inputDataLocker)
			{
				this._currentInputData.FillFrom(this._messageLoopInputData);
				this._messageLoopInputData.Reset();
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00004D54 File Offset: 0x00002F54
		public void PostRender()
		{
			if (this._layeredWindowController != null)
			{
				this._layeredWindowController.PostRender();
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00004D6C File Offset: 0x00002F6C
		public bool GetKeyDown(InputKey keyCode)
		{
			if (keyCode == InputKey.LeftMouseButton)
			{
				return this.LeftMouseDown();
			}
			if (keyCode == InputKey.RightMouseButton)
			{
				return this.RightMouseDown();
			}
			return this._currentInputData.KeyData[(int)keyCode] && !this._oldInputData.KeyData[(int)keyCode];
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00004DB8 File Offset: 0x00002FB8
		public bool GetKey(InputKey keyCode)
		{
			if (keyCode == InputKey.LeftMouseButton)
			{
				return this.LeftMouse();
			}
			if (keyCode == InputKey.RightMouseButton)
			{
				return this.RightMouse();
			}
			return this._currentInputData.KeyData[(int)keyCode];
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00004DE5 File Offset: 0x00002FE5
		public bool GetKeyUp(InputKey keyCode)
		{
			if (keyCode == InputKey.LeftMouseButton)
			{
				return this.LeftMouseUp();
			}
			if (keyCode == InputKey.RightMouseButton)
			{
				return this.RightMouseUp();
			}
			return !this._currentInputData.KeyData[(int)keyCode] && this._oldInputData.KeyData[(int)keyCode];
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00004E23 File Offset: 0x00003023
		public float GetMouseDeltaZ()
		{
			return this._currentInputData.MouseScrollDelta;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00004E30 File Offset: 0x00003030
		public bool LeftMouse()
		{
			return this._currentInputData.LeftMouse;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00004E3D File Offset: 0x0000303D
		public bool LeftMouseDown()
		{
			return this._currentInputData.LeftMouse && !this._oldInputData.LeftMouse;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00004E5C File Offset: 0x0000305C
		public bool LeftMouseUp()
		{
			return !this._currentInputData.LeftMouse && this._oldInputData.LeftMouse;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00004E78 File Offset: 0x00003078
		public bool RightMouse()
		{
			return this._currentInputData.RightMouse;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004E85 File Offset: 0x00003085
		public bool RightMouseDown()
		{
			return this._currentInputData.RightMouse && !this._oldInputData.RightMouse;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00004EA4 File Offset: 0x000030A4
		public bool RightMouseUp()
		{
			return !this._currentInputData.RightMouse && this._oldInputData.RightMouse;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00004EC0 File Offset: 0x000030C0
		public Vector2 MousePosition()
		{
			return new Vector2((float)this._currentInputData.CursorX, (float)this._currentInputData.CursorY);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004EDF File Offset: 0x000030DF
		public bool MouseMove()
		{
			return this._currentInputData.MouseMove;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00004EEC File Offset: 0x000030EC
		public void FillInputDataFromCurrent(InputData inputData)
		{
			inputData.FillFrom(this._currentInputData);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004EFC File Offset: 0x000030FC
		private void MessageHandler(WindowMessage message, long wParam, long lParam)
		{
			object obj;
			checked
			{
				if (message <= WindowMessage.KeyDown)
				{
					if (message <= WindowMessage.Close)
					{
						switch (message)
						{
						case WindowMessage.Size:
						case (WindowMessage)6U:
							return;
						case WindowMessage.SetFocus:
							goto IL_0354;
						case WindowMessage.KillFocus:
							goto IL_02FC;
						default:
							if (message != WindowMessage.Close)
							{
								return;
							}
							this.Destroy();
							Environment.Exit(0);
							return;
						}
					}
					else
					{
						if (message == WindowMessage.DisplayChange)
						{
							return;
						}
						if (message != WindowMessage.KeyDown)
						{
							return;
						}
						obj = this._inputDataLocker;
						lock (obj)
						{
							this._messageLoopInputData.KeyData[(int)((IntPtr)wParam)] = true;
							return;
						}
					}
				}
				else if (message <= WindowMessage.MouseWheel)
				{
					if (message != WindowMessage.KeyUp)
					{
						switch (message)
						{
						case WindowMessage.MouseMove:
							goto IL_026F;
						case WindowMessage.LeftButtonDown:
							goto IL_0214;
						case WindowMessage.LeftButtonUp:
							goto IL_01B9;
						case (WindowMessage)515U:
						case (WindowMessage)518U:
						case (WindowMessage)519U:
						case (WindowMessage)520U:
						case (WindowMessage)521U:
							return;
						case WindowMessage.RightButtonDown:
							goto IL_015E;
						case WindowMessage.RightButtonUp:
							goto IL_0107;
						case WindowMessage.MouseWheel:
							goto IL_02CA;
						default:
							return;
						}
					}
				}
				else
				{
					if (message != WindowMessage.DeviceChange)
					{
						return;
					}
					return;
				}
				obj = this._inputDataLocker;
				lock (obj)
				{
					this._messageLoopInputData.KeyData[(int)((IntPtr)wParam)] = false;
					return;
				}
			}
			IL_0107:
			obj = this._inputDataLocker;
			lock (obj)
			{
				this._messageLoopInputData.RightMouse = false;
				int num = (int)lParam % 65536;
				int num2 = (int)(lParam / 65536L);
				this._messageLoopInputData.CursorX = num;
				this._messageLoopInputData.CursorY = num2;
				return;
			}
			IL_015E:
			obj = this._inputDataLocker;
			lock (obj)
			{
				this._messageLoopInputData.RightMouse = true;
				int num3 = (int)lParam % 65536;
				int num4 = (int)(lParam / 65536L);
				this._messageLoopInputData.CursorX = num3;
				this._messageLoopInputData.CursorY = num4;
				return;
			}
			IL_01B9:
			obj = this._inputDataLocker;
			lock (obj)
			{
				this._messageLoopInputData.LeftMouse = false;
				int num5 = (int)lParam % 65536;
				int num6 = (int)(lParam / 65536L);
				this._messageLoopInputData.CursorX = num5;
				this._messageLoopInputData.CursorY = num6;
				return;
			}
			IL_0214:
			obj = this._inputDataLocker;
			lock (obj)
			{
				this._messageLoopInputData.LeftMouse = true;
				int num7 = (int)lParam % 65536;
				int num8 = (int)(lParam / 65536L);
				this._messageLoopInputData.CursorX = num7;
				this._messageLoopInputData.CursorY = num8;
				return;
			}
			IL_026F:
			obj = this._inputDataLocker;
			lock (obj)
			{
				this._messageLoopInputData.MouseMove = true;
				int num9 = (int)lParam % 65536;
				int num10 = (int)(lParam / 65536L);
				this._messageLoopInputData.CursorX = num9;
				this._messageLoopInputData.CursorY = num10;
				return;
			}
			IL_02CA:
			obj = this._inputDataLocker;
			lock (obj)
			{
				short num11 = (short)(wParam >> 16);
				this._messageLoopInputData.MouseScrollDelta = (float)num11;
				return;
			}
			IL_02FC:
			obj = this._inputDataLocker;
			lock (obj)
			{
				for (int i = 0; i < 256; i++)
				{
					this._messageLoopInputData.KeyData[i] = false;
					this._messageLoopInputData.RightMouse = false;
					this._messageLoopInputData.LeftMouse = false;
				}
				return;
			}
			IL_0354:
			obj = this._inputDataLocker;
			lock (obj)
			{
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000074 RID: 116 RVA: 0x000052F8 File Offset: 0x000034F8
		public int Width
		{
			get
			{
				return this._windowsForm.Width;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00005305 File Offset: 0x00003505
		public int Height
		{
			get
			{
				return this._windowsForm.Height;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00005312 File Offset: 0x00003512
		public bool IsMinimized
		{
			get
			{
				return this._windowsForm.IsMinimized;
			}
		}

		// Token: 0x04000034 RID: 52
		public const int WM_NCLBUTTONDOWN = 161;

		// Token: 0x04000035 RID: 53
		public const int HT_CAPTION = 2;

		// Token: 0x04000036 RID: 54
		private WindowsForm _windowsForm;

		// Token: 0x04000038 RID: 56
		private InputData _currentInputData;

		// Token: 0x04000039 RID: 57
		private InputData _oldInputData;

		// Token: 0x0400003A RID: 58
		private InputData _messageLoopInputData;

		// Token: 0x0400003B RID: 59
		private object _inputDataLocker = new object();

		// Token: 0x0400003C RID: 60
		private bool _mouseOverDragArea = true;

		// Token: 0x0400003D RID: 61
		private bool _isDragging;

		// Token: 0x0400003E RID: 62
		private LayeredWindowController _layeredWindowController;

		// Token: 0x0400003F RID: 63
		private bool _layeredWindow;

		// Token: 0x04000040 RID: 64
		private bool _isFinalized;
	}
}
