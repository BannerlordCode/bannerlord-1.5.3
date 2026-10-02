using System;
using System.Collections.Generic;
using System.Threading;

namespace TaleWorlds.Library
{
	// Token: 0x0200008E RID: 142
	public sealed class SingleThreadedSynchronizationContext : SynchronizationContext
	{
		// Token: 0x06000510 RID: 1296 RVA: 0x0001254E File Offset: 0x0001074E
		public SingleThreadedSynchronizationContext()
		{
			this._worksLock = new object();
			this._futureWorks = new List<SingleThreadedSynchronizationContext.WorkRequest>(100);
			this._currentWorks = new List<SingleThreadedSynchronizationContext.WorkRequest>(100);
			this._mainThreadId = Thread.CurrentThread.ManagedThreadId;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0001258C File Offset: 0x0001078C
		public override void Send(SendOrPostCallback callback, object state)
		{
			if (this._mainThreadId == Thread.CurrentThread.ManagedThreadId)
			{
				callback.DynamicInvokeWithLog(new object[] { state });
				return;
			}
			using (ManualResetEvent manualResetEvent = new ManualResetEvent(false))
			{
				object worksLock = this._worksLock;
				lock (worksLock)
				{
					this._futureWorks.Add(new SingleThreadedSynchronizationContext.WorkRequest(callback, state, manualResetEvent));
				}
				manualResetEvent.WaitOne();
			}
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00012624 File Offset: 0x00010824
		public override void Post(SendOrPostCallback callback, object state)
		{
			SingleThreadedSynchronizationContext.WorkRequest workRequest = new SingleThreadedSynchronizationContext.WorkRequest(callback, state, null);
			object worksLock = this._worksLock;
			lock (worksLock)
			{
				this._futureWorks.Add(workRequest);
			}
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00012674 File Offset: 0x00010874
		public void Tick()
		{
			object worksLock = this._worksLock;
			lock (worksLock)
			{
				List<SingleThreadedSynchronizationContext.WorkRequest> currentWorks = this._currentWorks;
				this._currentWorks = this._futureWorks;
				this._futureWorks = currentWorks;
			}
			foreach (SingleThreadedSynchronizationContext.WorkRequest workRequest in this._currentWorks)
			{
				workRequest.Invoke();
			}
			this._currentWorks.Clear();
		}

		// Token: 0x04000194 RID: 404
		private List<SingleThreadedSynchronizationContext.WorkRequest> _futureWorks;

		// Token: 0x04000195 RID: 405
		private List<SingleThreadedSynchronizationContext.WorkRequest> _currentWorks;

		// Token: 0x04000196 RID: 406
		private readonly object _worksLock;

		// Token: 0x04000197 RID: 407
		private readonly int _mainThreadId;

		// Token: 0x020000E4 RID: 228
		private struct WorkRequest
		{
			// Token: 0x060007A9 RID: 1961 RVA: 0x0001952B File Offset: 0x0001772B
			public WorkRequest(SendOrPostCallback callback, object state, ManualResetEvent waitHandle = null)
			{
				this._callback = callback;
				this._state = state;
				this._waitHandle = waitHandle;
			}

			// Token: 0x060007AA RID: 1962 RVA: 0x00019542 File Offset: 0x00017742
			public void Invoke()
			{
				this._callback.DynamicInvoke(new object[] { this._state });
				ManualResetEvent waitHandle = this._waitHandle;
				if (waitHandle == null)
				{
					return;
				}
				waitHandle.Set();
			}

			// Token: 0x040002F4 RID: 756
			private readonly SendOrPostCallback _callback;

			// Token: 0x040002F5 RID: 757
			private readonly object _state;

			// Token: 0x040002F6 RID: 758
			private readonly ManualResetEvent _waitHandle;
		}
	}
}
