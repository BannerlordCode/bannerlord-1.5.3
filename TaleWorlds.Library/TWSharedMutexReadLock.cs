using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200009D RID: 157
	public struct TWSharedMutexReadLock : IDisposable
	{
		// Token: 0x06000597 RID: 1431 RVA: 0x00013A68 File Offset: 0x00011C68
		public TWSharedMutexReadLock(TWSharedMutex mtx)
		{
			mtx.EnterReadLock();
			this._mtx = mtx;
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x00013A77 File Offset: 0x00011C77
		public void Dispose()
		{
			this._mtx.ExitReadLock();
		}

		// Token: 0x040001B7 RID: 439
		private readonly TWSharedMutex _mtx;
	}
}
