using System;
using System.Collections;

namespace System.Management
{
	// Token: 0x0200002E RID: 46
	public class PropertyDataCollection : ICollection, IEnumerable
	{
		// Token: 0x060001C1 RID: 449 RVA: 0x00003765 File Offset: 0x00001965
		internal PropertyDataCollection()
		{
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x0000376D File Offset: 0x0000196D
		public int Count
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00003779 File Offset: 0x00001979
		public bool IsSynchronized
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x170000A5 RID: 165
		public virtual PropertyData this[string propertyName]
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00003791 File Offset: 0x00001991
		public object SyncRoot
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000379D File Offset: 0x0000199D
		public void Add(string propertyName, CimType propertyType, bool isArray)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000037A9 File Offset: 0x000019A9
		public virtual void Add(string propertyName, object propertyValue)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000037B5 File Offset: 0x000019B5
		public void Add(string propertyName, object propertyValue, CimType propertyType)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x000037C1 File Offset: 0x000019C1
		public void CopyTo(Array array, int index)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x000037CD File Offset: 0x000019CD
		public void CopyTo(PropertyData[] propertyArray, int index)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x000037D9 File Offset: 0x000019D9
		public PropertyDataCollection.PropertyDataEnumerator GetEnumerator()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x000037E5 File Offset: 0x000019E5
		public virtual void Remove(string propertyName)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x000037F1 File Offset: 0x000019F1
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0200003D RID: 61
		public class PropertyDataEnumerator : IEnumerator
		{
			// Token: 0x0600025E RID: 606 RVA: 0x00003F17 File Offset: 0x00002117
			internal PropertyDataEnumerator()
			{
			}

			// Token: 0x170000D6 RID: 214
			// (get) Token: 0x0600025F RID: 607 RVA: 0x00003F1F File Offset: 0x0000211F
			public PropertyData Current
			{
				get
				{
					throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
				}
			}

			// Token: 0x170000D7 RID: 215
			// (get) Token: 0x06000260 RID: 608 RVA: 0x00003F2B File Offset: 0x0000212B
			object IEnumerator.Current
			{
				get
				{
					throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
				}
			}

			// Token: 0x06000261 RID: 609 RVA: 0x00003F37 File Offset: 0x00002137
			public bool MoveNext()
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}

			// Token: 0x06000262 RID: 610 RVA: 0x00003F43 File Offset: 0x00002143
			public void Reset()
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}
	}
}
