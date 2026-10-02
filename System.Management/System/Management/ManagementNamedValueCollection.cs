using System;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace System.Management
{
	// Token: 0x02000019 RID: 25
	public class ManagementNamedValueCollection : NameObjectCollectionBase
	{
		// Token: 0x060000F9 RID: 249 RVA: 0x00002DCA File Offset: 0x00000FCA
		public ManagementNamedValueCollection()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00002DDC File Offset: 0x00000FDC
		protected ManagementNamedValueCollection(SerializationInfo info, StreamingContext context)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x17000072 RID: 114
		public object this[string name]
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00002DFA File Offset: 0x00000FFA
		public void Add(string name, object value)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00002E06 File Offset: 0x00001006
		public ManagementNamedValueCollection Clone()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00002E12 File Offset: 0x00001012
		public void Remove(string name)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00002E1E File Offset: 0x0000101E
		public void RemoveAll()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}
	}
}
