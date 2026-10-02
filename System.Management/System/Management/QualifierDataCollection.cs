using System;
using System.Collections;

namespace System.Management
{
	// Token: 0x02000032 RID: 50
	public class QualifierDataCollection : ICollection, IEnumerable
	{
		// Token: 0x060001E3 RID: 483 RVA: 0x00003907 File Offset: 0x00001B07
		internal QualifierDataCollection()
		{
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000390F File Offset: 0x00001B0F
		public int Count
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x0000391B File Offset: 0x00001B1B
		public bool IsSynchronized
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x170000B2 RID: 178
		public virtual QualifierData this[string qualifierName]
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x00003933 File Offset: 0x00001B33
		public object SyncRoot
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000393F File Offset: 0x00001B3F
		public virtual void Add(string qualifierName, object qualifierValue)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000394B File Offset: 0x00001B4B
		public virtual void Add(string qualifierName, object qualifierValue, bool isAmended, bool propagatesToInstance, bool propagatesToSubclass, bool isOverridable)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00003957 File Offset: 0x00001B57
		public void CopyTo(Array array, int index)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00003963 File Offset: 0x00001B63
		public void CopyTo(QualifierData[] qualifierArray, int index)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000396F File Offset: 0x00001B6F
		public QualifierDataCollection.QualifierDataEnumerator GetEnumerator()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000397B File Offset: 0x00001B7B
		public virtual void Remove(string qualifierName)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00003987 File Offset: 0x00001B87
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0200003E RID: 62
		public class QualifierDataEnumerator : IEnumerator
		{
			// Token: 0x06000263 RID: 611 RVA: 0x00003F4F File Offset: 0x0000214F
			internal QualifierDataEnumerator()
			{
			}

			// Token: 0x170000D8 RID: 216
			// (get) Token: 0x06000264 RID: 612 RVA: 0x00003F57 File Offset: 0x00002157
			public QualifierData Current
			{
				get
				{
					throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
				}
			}

			// Token: 0x170000D9 RID: 217
			// (get) Token: 0x06000265 RID: 613 RVA: 0x00003F63 File Offset: 0x00002163
			object IEnumerator.Current
			{
				get
				{
					throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
				}
			}

			// Token: 0x06000266 RID: 614 RVA: 0x00003F6F File Offset: 0x0000216F
			public bool MoveNext()
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}

			// Token: 0x06000267 RID: 615 RVA: 0x00003F7B File Offset: 0x0000217B
			public void Reset()
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}
	}
}
