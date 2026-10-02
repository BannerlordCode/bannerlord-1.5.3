using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000065 RID: 101
	[NullableContext(2)]
	[Nullable(0)]
	internal class ReflectionMember
	{
		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x00017446 File Offset: 0x00015646
		// (set) Token: 0x0600057A RID: 1402 RVA: 0x0001744E File Offset: 0x0001564E
		public Type MemberType { get; set; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x00017457 File Offset: 0x00015657
		// (set) Token: 0x0600057C RID: 1404 RVA: 0x0001745F File Offset: 0x0001565F
		[Nullable(new byte[] { 2, 1, 2 })]
		public Func<object, object> Getter
		{
			[return: Nullable(new byte[] { 2, 1, 2 })]
			get;
			[param: Nullable(new byte[] { 2, 1, 2 })]
			set;
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x00017468 File Offset: 0x00015668
		// (set) Token: 0x0600057E RID: 1406 RVA: 0x00017470 File Offset: 0x00015670
		[Nullable(new byte[] { 2, 1, 2 })]
		public Action<object, object> Setter
		{
			[return: Nullable(new byte[] { 2, 1, 2 })]
			get;
			[param: Nullable(new byte[] { 2, 1, 2 })]
			set;
		}
	}
}
