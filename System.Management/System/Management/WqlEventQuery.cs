using System;
using System.Collections.Specialized;

namespace System.Management
{
	// Token: 0x02000039 RID: 57
	public class WqlEventQuery : EventQuery
	{
		// Token: 0x06000235 RID: 565 RVA: 0x00003D01 File Offset: 0x00001F01
		public WqlEventQuery()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00003D13 File Offset: 0x00001F13
		public WqlEventQuery(string queryOrEventClassName)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00003D25 File Offset: 0x00001F25
		public WqlEventQuery(string eventClassName, string condition)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00003D37 File Offset: 0x00001F37
		public WqlEventQuery(string eventClassName, string condition, TimeSpan groupWithinInterval)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00003D49 File Offset: 0x00001F49
		public WqlEventQuery(string eventClassName, string condition, TimeSpan groupWithinInterval, string[] groupByPropertyList)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00003D5B File Offset: 0x00001F5B
		public WqlEventQuery(string eventClassName, TimeSpan withinInterval)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00003D6D File Offset: 0x00001F6D
		public WqlEventQuery(string eventClassName, TimeSpan withinInterval, string condition)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00003D7F File Offset: 0x00001F7F
		public WqlEventQuery(string eventClassName, TimeSpan withinInterval, string condition, TimeSpan groupWithinInterval, string[] groupByPropertyList, string havingCondition)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600023D RID: 573 RVA: 0x00003D91 File Offset: 0x00001F91
		// (set) Token: 0x0600023E RID: 574 RVA: 0x00003D9D File Offset: 0x00001F9D
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

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600023F RID: 575 RVA: 0x00003DA9 File Offset: 0x00001FA9
		// (set) Token: 0x06000240 RID: 576 RVA: 0x00003DB5 File Offset: 0x00001FB5
		public string EventClassName
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

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000241 RID: 577 RVA: 0x00003DC1 File Offset: 0x00001FC1
		// (set) Token: 0x06000242 RID: 578 RVA: 0x00003DCD File Offset: 0x00001FCD
		public StringCollection GroupByPropertyList
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

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000243 RID: 579 RVA: 0x00003DD9 File Offset: 0x00001FD9
		// (set) Token: 0x06000244 RID: 580 RVA: 0x00003DE5 File Offset: 0x00001FE5
		public TimeSpan GroupWithinInterval
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

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000245 RID: 581 RVA: 0x00003DF1 File Offset: 0x00001FF1
		// (set) Token: 0x06000246 RID: 582 RVA: 0x00003DFD File Offset: 0x00001FFD
		public string HavingCondition
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

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000247 RID: 583 RVA: 0x00003E09 File Offset: 0x00002009
		public override string QueryLanguage
		{
			get
			{
				throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000248 RID: 584 RVA: 0x00003E15 File Offset: 0x00002015
		// (set) Token: 0x06000249 RID: 585 RVA: 0x00003E21 File Offset: 0x00002021
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

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600024A RID: 586 RVA: 0x00003E2D File Offset: 0x0000202D
		// (set) Token: 0x0600024B RID: 587 RVA: 0x00003E39 File Offset: 0x00002039
		public TimeSpan WithinInterval
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

		// Token: 0x0600024C RID: 588 RVA: 0x00003E45 File Offset: 0x00002045
		protected internal void BuildQuery()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00003E51 File Offset: 0x00002051
		public override object Clone()
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00003E5D File Offset: 0x0000205D
		protected internal override void ParseQuery(string query)
		{
			throw new PlatformNotSupportedException(SR.PlatformNotSupported_SystemManagement);
		}
	}
}
