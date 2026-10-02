using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000C0 RID: 192
	public class JsonCloneSettings
	{
		// Token: 0x06000AA5 RID: 2725 RVA: 0x0002A6E9 File Offset: 0x000288E9
		public JsonCloneSettings()
		{
			this.CopyAnnotations = true;
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000AA6 RID: 2726 RVA: 0x0002A6F8 File Offset: 0x000288F8
		// (set) Token: 0x06000AA7 RID: 2727 RVA: 0x0002A700 File Offset: 0x00028900
		public bool CopyAnnotations { get; set; }

		// Token: 0x04000386 RID: 902
		[Nullable(1)]
		internal static readonly JsonCloneSettings SkipCopyAnnotations = new JsonCloneSettings
		{
			CopyAnnotations = false
		};
	}
}
