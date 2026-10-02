using System;
using System.Collections.Specialized;

namespace System.Management
{
	// Token: 0x02000035 RID: 53
	public class SelectQuery : WqlObjectQuery
	{
		// Token: 0x0600021D RID: 541 RVA: 0x00003BF7 File Offset: 0x00001DF7
		public SelectQuery()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00003C09 File Offset: 0x00001E09
		public SelectQuery(bool isSchemaQuery, string condition)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00003C1B File Offset: 0x00001E1B
		public SelectQuery(string queryOrClassName)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00003C2D File Offset: 0x00001E2D
		public SelectQuery(string className, string condition)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00003C3F File Offset: 0x00001E3F
		public SelectQuery(string className, string condition, string[] selectedProperties)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000222 RID: 546 RVA: 0x00003C51 File Offset: 0x00001E51
		// (set) Token: 0x06000223 RID: 547 RVA: 0x00003C5D File Offset: 0x00001E5D
		public string ClassName
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
			set
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000224 RID: 548 RVA: 0x00003C69 File Offset: 0x00001E69
		// (set) Token: 0x06000225 RID: 549 RVA: 0x00003C75 File Offset: 0x00001E75
		public string Condition
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
			set
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000226 RID: 550 RVA: 0x00003C81 File Offset: 0x00001E81
		// (set) Token: 0x06000227 RID: 551 RVA: 0x00003C8D File Offset: 0x00001E8D
		public bool IsSchemaQuery
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
			set
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000228 RID: 552 RVA: 0x00003C99 File Offset: 0x00001E99
		// (set) Token: 0x06000229 RID: 553 RVA: 0x00003CA5 File Offset: 0x00001EA5
		public override string QueryString
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
			set
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600022A RID: 554 RVA: 0x00003CB1 File Offset: 0x00001EB1
		// (set) Token: 0x0600022B RID: 555 RVA: 0x00003CBD File Offset: 0x00001EBD
		public StringCollection SelectedProperties
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
			set
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00003CC9 File Offset: 0x00001EC9
		protected internal void BuildQuery()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00003CD5 File Offset: 0x00001ED5
		public override object Clone()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00003CE1 File Offset: 0x00001EE1
		protected internal override void ParseQuery(string query)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}
	}
}
