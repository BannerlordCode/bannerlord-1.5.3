using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaleWorlds.Library
{
	// Token: 0x02000093 RID: 147
	public abstract class TestCommonBase
	{
		// Token: 0x06000541 RID: 1345
		public abstract void Tick();

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000542 RID: 1346 RVA: 0x00012B3F File Offset: 0x00010D3F
		public static TestCommonBase BaseInstance
		{
			get
			{
				return TestCommonBase._baseInstance;
			}
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00012B46 File Offset: 0x00010D46
		public void StartTimeoutTimer()
		{
			this.timeoutTimerStart = DateTime.Now;
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00012B53 File Offset: 0x00010D53
		public void ToggleTimeoutTimer()
		{
			this.timeoutTimerEnabled = !this.timeoutTimerEnabled;
			if (!this.timeoutTimerEnabled)
			{
				Action onDumpCreationStartedCallback = this.OnDumpCreationStartedCallback;
				if (onDumpCreationStartedCallback == null)
				{
					return;
				}
				onDumpCreationStartedCallback();
				return;
			}
			else
			{
				Action onDumpCreatedCallback = this.OnDumpCreatedCallback;
				if (onDumpCreatedCallback == null)
				{
					return;
				}
				onDumpCreatedCallback();
				return;
			}
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00012B90 File Offset: 0x00010D90
		public bool CheckTimeoutTimer()
		{
			return this.timeoutTimerEnabled && DateTime.Now.Subtract(this.timeoutTimerStart).TotalSeconds > (double)this.commonWaitTimeoutLimits;
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00012BCE File Offset: 0x00010DCE
		protected TestCommonBase()
		{
			TestCommonBase._baseInstance = this;
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00012C04 File Offset: 0x00010E04
		public virtual string GetGameStatus()
		{
			return "";
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00012C0C File Offset: 0x00010E0C
		public void WaitFor(double seconds)
		{
			if (!this.isParallelThread)
			{
				DateTime now = DateTime.Now;
				while ((DateTime.Now - now).TotalSeconds < seconds)
				{
					Monitor.Pulse(this.TestLock);
					Monitor.Wait(this.TestLock);
				}
			}
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00012C58 File Offset: 0x00010E58
		public virtual async Task WaitUntil(Func<bool> func)
		{
			while (!func())
			{
				await this.WaitForAsync(0.1);
			}
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00012CA5 File Offset: 0x00010EA5
		public Task WaitForAsync(double seconds, Random random)
		{
			return Task.Delay((int)(seconds * 1000.0 * random.NextDouble()));
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00012CBF File Offset: 0x00010EBF
		public Task WaitForAsync(double seconds)
		{
			return Task.Delay((int)(seconds * 1000.0));
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00012CD2 File Offset: 0x00010ED2
		public static string GetAttachmentsFolderPath()
		{
			return "..\\..\\..\\Tools\\TestAutomation\\Attachments\\";
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00012CD9 File Offset: 0x00010ED9
		public virtual void OnFinalize()
		{
			TestCommonBase._baseInstance = null;
		}

		// Token: 0x0400019D RID: 413
		public int TestRandomSeed;

		// Token: 0x0400019E RID: 414
		public bool IsTestEnabled;

		// Token: 0x0400019F RID: 415
		public bool isParallelThread;

		// Token: 0x040001A0 RID: 416
		public string SceneNameToOpenOnStartup;

		// Token: 0x040001A1 RID: 417
		public object TestLock = new object();

		// Token: 0x040001A2 RID: 418
		private static TestCommonBase _baseInstance;

		// Token: 0x040001A3 RID: 419
		private DateTime timeoutTimerStart = DateTime.Now;

		// Token: 0x040001A4 RID: 420
		private bool timeoutTimerEnabled = true;

		// Token: 0x040001A5 RID: 421
		private int commonWaitTimeoutLimits = 1500;

		// Token: 0x040001A6 RID: 422
		public Action OnDumpCreationStartedCallback;

		// Token: 0x040001A7 RID: 423
		public Action OnDumpCreatedCallback;
	}
}
