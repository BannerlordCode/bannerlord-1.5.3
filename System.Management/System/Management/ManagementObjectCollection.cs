using System;
using System.Collections;

namespace System.Management
{
	// Token: 0x0200001B RID: 27
	public class ManagementObjectCollection : ICollection, IEnumerable, IDisposable
	{
		// Token: 0x06000136 RID: 310 RVA: 0x00003189 File Offset: 0x00001389
		internal ManagementObjectCollection()
		{
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00003191 File Offset: 0x00001391
		public int Count
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000138 RID: 312 RVA: 0x0000319D File Offset: 0x0000139D
		public bool IsSynchronized
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000139 RID: 313 RVA: 0x000031A9 File Offset: 0x000013A9
		public object SyncRoot
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000031B5 File Offset: 0x000013B5
		public void CopyTo(Array array, int index)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000031C1 File Offset: 0x000013C1
		public void CopyTo(ManagementBaseObject[] objectCollection, int index)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000031CD File Offset: 0x000013CD
		public void Dispose()
		{
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000031CF File Offset: 0x000013CF
		public ManagementObjectCollection.ManagementObjectEnumerator GetEnumerator()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000031DB File Offset: 0x000013DB
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0200003B RID: 59
		public class ManagementObjectEnumerator : IEnumerator, IDisposable
		{
			// Token: 0x06000253 RID: 595 RVA: 0x00003EA5 File Offset: 0x000020A5
			internal ManagementObjectEnumerator()
			{
			}

			// Token: 0x170000D2 RID: 210
			// (get) Token: 0x06000254 RID: 596 RVA: 0x00003EAD File Offset: 0x000020AD
			public ManagementBaseObject Current
			{
				get
				{
					throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
				}
			}

			// Token: 0x170000D3 RID: 211
			// (get) Token: 0x06000255 RID: 597 RVA: 0x00003EB9 File Offset: 0x000020B9
			object IEnumerator.Current
			{
				get
				{
					throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
				}
			}

			// Token: 0x06000256 RID: 598 RVA: 0x00003EC5 File Offset: 0x000020C5
			public void Dispose()
			{
			}

			// Token: 0x06000257 RID: 599 RVA: 0x00003EC7 File Offset: 0x000020C7
			public bool MoveNext()
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}

			// Token: 0x06000258 RID: 600 RVA: 0x00003ED3 File Offset: 0x000020D3
			public void Reset()
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}
	}
}
