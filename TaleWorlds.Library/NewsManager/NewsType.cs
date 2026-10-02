using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TaleWorlds.Library.NewsManager
{
	// Token: 0x020000AC RID: 172
	public struct NewsType
	{
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x00016D8E File Offset: 0x00014F8E
		// (set) Token: 0x0600068E RID: 1678 RVA: 0x00016D96 File Offset: 0x00014F96
		[JsonConverter(typeof(StringEnumConverter))]
		public NewsItem.NewsTypes Type { get; set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600068F RID: 1679 RVA: 0x00016D9F File Offset: 0x00014F9F
		// (set) Token: 0x06000690 RID: 1680 RVA: 0x00016DA7 File Offset: 0x00014FA7
		public int Index { get; set; }
	}
}
