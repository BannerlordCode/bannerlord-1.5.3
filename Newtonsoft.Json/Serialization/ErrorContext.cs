using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000078 RID: 120
	[NullableContext(1)]
	[Nullable(0)]
	public class ErrorContext
	{
		// Token: 0x06000659 RID: 1625 RVA: 0x0001B538 File Offset: 0x00019738
		internal ErrorContext([Nullable(2)] object originalObject, [Nullable(2)] object member, string path, Exception error)
		{
			this.OriginalObject = originalObject;
			this.Member = member;
			this.Error = error;
			this.Path = path;
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x0001B55D File Offset: 0x0001975D
		// (set) Token: 0x0600065B RID: 1627 RVA: 0x0001B565 File Offset: 0x00019765
		internal bool Traced { get; set; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x0001B56E File Offset: 0x0001976E
		public Exception Error { get; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x0001B576 File Offset: 0x00019776
		[Nullable(2)]
		public object OriginalObject
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600065E RID: 1630 RVA: 0x0001B57E File Offset: 0x0001977E
		[Nullable(2)]
		public object Member
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x0001B586 File Offset: 0x00019786
		public string Path { get; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x0001B58E File Offset: 0x0001978E
		// (set) Token: 0x06000661 RID: 1633 RVA: 0x0001B596 File Offset: 0x00019796
		public bool Handled { get; set; }
	}
}
