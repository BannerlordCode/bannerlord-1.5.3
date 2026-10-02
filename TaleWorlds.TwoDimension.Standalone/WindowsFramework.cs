using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension.Standalone.Native.Windows;

namespace TaleWorlds.TwoDimension.Standalone
{
	// Token: 0x02000014 RID: 20
	public class WindowsFramework
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000EF RID: 239 RVA: 0x0000648A File Offset: 0x0000468A
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x00006492 File Offset: 0x00004692
		public WindowsFrameworkThreadConfig ThreadConfig { get; set; }

		// Token: 0x060000F1 RID: 241 RVA: 0x0000649B File Offset: 0x0000469B
		public WindowsFramework()
		{
			this._timer = new Stopwatch();
			this._messageCommunicators = new List<IMessageCommunicator>();
			this.IsActive = false;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000064C0 File Offset: 0x000046C0
		public void Initialize(FrameworkDomain[] frameworkDomains)
		{
			this._frameworkDomains = frameworkDomains;
			this.IsActive = true;
			if (this.ThreadConfig == WindowsFrameworkThreadConfig.SingleThread)
			{
				this._frameworkDomainThreads = new Thread[1];
				this.CreateThread(0);
				return;
			}
			if (this.ThreadConfig == WindowsFrameworkThreadConfig.MultiThread)
			{
				this._frameworkDomainThreads = new Thread[frameworkDomains.Length];
				for (int i = 0; i < frameworkDomains.Length; i++)
				{
					this.CreateThread(i);
				}
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00006524 File Offset: 0x00004724
		private void CreateThread(int index)
		{
			Common.SetInvariantCulture();
			this._frameworkDomainThreads[index] = new Thread(new ParameterizedThreadStart(this.MainLoop));
			this._frameworkDomainThreads[index].SetApartmentState(ApartmentState.STA);
			this._frameworkDomainThreads[index].Name = this._frameworkDomains[index].ToString() + " Thread";
			this._frameworkDomainThreads[index].CurrentCulture = CultureInfo.InvariantCulture;
			this._frameworkDomainThreads[index].CurrentUICulture = CultureInfo.InvariantCulture;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000065A5 File Offset: 0x000047A5
		public void RegisterMessageCommunicator(IMessageCommunicator communicator)
		{
			this._messageCommunicators.Add(communicator);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000065B3 File Offset: 0x000047B3
		public void UnRegisterMessageCommunicator(IMessageCommunicator communicator)
		{
			this._messageCommunicators.Remove(communicator);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x000065C4 File Offset: 0x000047C4
		private void MessageLoop()
		{
			try
			{
				if (this.ThreadConfig == WindowsFrameworkThreadConfig.NoThread)
				{
					int num = 0;
					while (this._frameworkDomains != null && num < this._frameworkDomains.Length)
					{
						this._frameworkDomains[num].Update();
						num++;
					}
				}
				for (int i = 0; i < this._messageCommunicators.Count; i++)
				{
					this._messageCommunicators[i].MessageLoop();
				}
			}
			catch (Exception ex)
			{
				Debug.Print(ex.Message, 0, Debug.DebugColor.White, 17592186044416UL);
				Debug.Print(ex.StackTrace, 0, Debug.DebugColor.White, 17592186044416UL);
				throw;
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000666C File Offset: 0x0000486C
		private void MainLoop(object parameter)
		{
			try
			{
				if (this.ThreadConfig == WindowsFrameworkThreadConfig.SingleThread)
				{
					while (this.IsActive)
					{
						for (int i = 0; i < this._frameworkDomains.Length; i++)
						{
							this._frameworkDomains[i].Update();
						}
					}
				}
				else if (this.ThreadConfig == WindowsFrameworkThreadConfig.MultiThread)
				{
					FrameworkDomain frameworkDomain = parameter as FrameworkDomain;
					while (this.IsActive)
					{
						frameworkDomain.Update();
					}
				}
				Interlocked.Increment(ref this._abortedThreadCount);
				this.OnFinalize();
			}
			catch (Exception ex)
			{
				Debug.Print(ex.Message, 0, Debug.DebugColor.White, 17592186044416UL);
				Debug.Print(ex.StackTrace, 0, Debug.DebugColor.White, 17592186044416UL);
				throw;
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00006724 File Offset: 0x00004924
		public void Stop()
		{
			this.IsActive = false;
			this.OnFinalize();
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00006734 File Offset: 0x00004934
		public void OnFinalize()
		{
			if (this._frameworkDomainThreads != null && this._abortedThreadCount != this._frameworkDomainThreads.Length)
			{
				return;
			}
			this._frameworkDomainThreads = null;
			FrameworkDomain[] frameworkDomains = this._frameworkDomains;
			for (int i = 0; i < frameworkDomains.Length; i++)
			{
				frameworkDomains[i].Destroy();
			}
			this._frameworkDomains = null;
			this.IsFinalized = true;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000678C File Offset: 0x0000498C
		public void Start()
		{
			this._timer.Start();
			this.IsActive = true;
			if (this.ThreadConfig == WindowsFrameworkThreadConfig.SingleThread)
			{
				this._frameworkDomainThreads[0].Start();
			}
			else if (this.ThreadConfig == WindowsFrameworkThreadConfig.MultiThread)
			{
				for (int i = 0; i < this._frameworkDomains.Length; i++)
				{
					this._frameworkDomainThreads[i].Start(this._frameworkDomains[i]);
				}
			}
			NativeMessage nativeMessage = default(NativeMessage);
			if (this.ThreadConfig == WindowsFrameworkThreadConfig.NoThread)
			{
				while (this.IsActive)
				{
					if (User32.PeekMessage(out nativeMessage, IntPtr.Zero, 0U, 0U, 1U))
					{
						User32.TranslateMessage(ref nativeMessage);
						User32.DispatchMessage(ref nativeMessage);
					}
					this.MessageLoop();
				}
				return;
			}
			while (this.IsActive)
			{
				if (User32.PeekMessage(out nativeMessage, IntPtr.Zero, 0U, 0U, 1U))
				{
					if (nativeMessage.msg == WindowMessage.Quit)
					{
						break;
					}
					User32.TranslateMessage(ref nativeMessage);
					User32.DispatchMessage(ref nativeMessage);
				}
				this.MessageLoop();
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000FB RID: 251 RVA: 0x0000686F File Offset: 0x00004A6F
		public long ElapsedTicks
		{
			get
			{
				return this._timer.ElapsedTicks;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000FC RID: 252 RVA: 0x0000687C File Offset: 0x00004A7C
		public long TicksPerSecond
		{
			get
			{
				return Stopwatch.Frequency;
			}
		}

		// Token: 0x04000068 RID: 104
		public bool IsActive;

		// Token: 0x04000069 RID: 105
		private FrameworkDomain[] _frameworkDomains;

		// Token: 0x0400006A RID: 106
		private Thread[] _frameworkDomainThreads;

		// Token: 0x0400006B RID: 107
		private Stopwatch _timer;

		// Token: 0x0400006D RID: 109
		private List<IMessageCommunicator> _messageCommunicators;

		// Token: 0x0400006E RID: 110
		public bool IsFinalized;

		// Token: 0x0400006F RID: 111
		private int _abortedThreadCount;
	}
}
