using System;
using System.Collections;

namespace System.Management
{
	// Token: 0x02000024 RID: 36
	public class MethodDataCollection : ICollection, IEnumerable
	{
		// Token: 0x0600018E RID: 398 RVA: 0x00003581 File Offset: 0x00001781
		internal MethodDataCollection()
		{
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00003589 File Offset: 0x00001789
		public int Count
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00003595 File Offset: 0x00001795
		public bool IsSynchronized
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x17000094 RID: 148
		public virtual MethodData this[string methodName]
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000192 RID: 402 RVA: 0x000035AD File Offset: 0x000017AD
		public object SyncRoot
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x06000193 RID: 403 RVA: 0x000035B9 File Offset: 0x000017B9
		public virtual void Add(string methodName)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x000035C5 File Offset: 0x000017C5
		public virtual void Add(string methodName, ManagementBaseObject inParameters, ManagementBaseObject outParameters)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x000035D1 File Offset: 0x000017D1
		public void CopyTo(Array array, int index)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x000035DD File Offset: 0x000017DD
		public void CopyTo(MethodData[] methodArray, int index)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x000035E9 File Offset: 0x000017E9
		public MethodDataCollection.MethodDataEnumerator GetEnumerator()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000035F5 File Offset: 0x000017F5
		public virtual void Remove(string methodName)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00003601 File Offset: 0x00001801
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0200003C RID: 60
		public class MethodDataEnumerator : IEnumerator
		{
			// Token: 0x06000259 RID: 601 RVA: 0x00003EDF File Offset: 0x000020DF
			internal MethodDataEnumerator()
			{
			}

			// Token: 0x170000D4 RID: 212
			// (get) Token: 0x0600025A RID: 602 RVA: 0x00003EE7 File Offset: 0x000020E7
			public MethodData Current
			{
				get
				{
					throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
				}
			}

			// Token: 0x170000D5 RID: 213
			// (get) Token: 0x0600025B RID: 603 RVA: 0x00003EF3 File Offset: 0x000020F3
			object IEnumerator.Current
			{
				get
				{
					throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
				}
			}

			// Token: 0x0600025C RID: 604 RVA: 0x00003EFF File Offset: 0x000020FF
			public bool MoveNext()
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}

			// Token: 0x0600025D RID: 605 RVA: 0x00003F0B File Offset: 0x0000210B
			public void Reset()
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}
	}
}
