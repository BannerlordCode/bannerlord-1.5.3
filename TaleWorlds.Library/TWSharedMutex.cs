using System;
using System.Threading;

namespace TaleWorlds.Library
{
	// Token: 0x0200009C RID: 156
	public class TWSharedMutex
	{
		// Token: 0x06000591 RID: 1425 RVA: 0x00013970 File Offset: 0x00011B70
		public void EnterReadLock()
		{
			for (;;)
			{
				if (Volatile.Read(ref this._writerFlag) != 1 && Volatile.Read(ref this._writeRequests) <= 0)
				{
					Interlocked.Increment(ref this._readerCount);
					if (Volatile.Read(ref this._writerFlag) == 0 && Volatile.Read(ref this._writeRequests) == 0)
					{
						break;
					}
					Interlocked.Decrement(ref this._readerCount);
				}
				else
				{
					Thread.SpinWait(4);
				}
			}
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x000139D8 File Offset: 0x00011BD8
		public void EnterWriteLock()
		{
			Interlocked.Increment(ref this._writeRequests);
			while (Interlocked.CompareExchange(ref this._writerFlag, 1, 0) != 0)
			{
				Thread.SpinWait(4);
			}
			while (Volatile.Read(ref this._readerCount) > 0)
			{
				Thread.SpinWait(4);
			}
			Interlocked.Decrement(ref this._writeRequests);
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x00013A2C File Offset: 0x00011C2C
		public void ExitReadLock()
		{
			Interlocked.Decrement(ref this._readerCount);
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00013A3A File Offset: 0x00011C3A
		public void ExitWriteLock()
		{
			Volatile.Write(ref this._writerFlag, 0);
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x00013A48 File Offset: 0x00011C48
		public bool IsReadLockHeld
		{
			get
			{
				return Volatile.Read(ref this._readerCount) > 0;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x00013A58 File Offset: 0x00011C58
		public bool IsWriteLockHeld
		{
			get
			{
				return Volatile.Read(ref this._writerFlag) > 0;
			}
		}

		// Token: 0x040001B4 RID: 436
		private int _readerCount;

		// Token: 0x040001B5 RID: 437
		private int _writerFlag;

		// Token: 0x040001B6 RID: 438
		private int _writeRequests;
	}
}
