using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200009E RID: 158
	public struct TWSharedMutexWriteLock : IDisposable
	{
		// Token: 0x06000599 RID: 1433 RVA: 0x00013A84 File Offset: 0x00011C84
		public TWSharedMutexWriteLock(TWSharedMutex mtx)
		{
			mtx.EnterWriteLock();
			this._mtx = mtx;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x00013A93 File Offset: 0x00011C93
		public void Dispose()
		{
			this._mtx.ExitWriteLock();
		}

		// Token: 0x040001B8 RID: 440
		private readonly TWSharedMutex _mtx;
	}
}
