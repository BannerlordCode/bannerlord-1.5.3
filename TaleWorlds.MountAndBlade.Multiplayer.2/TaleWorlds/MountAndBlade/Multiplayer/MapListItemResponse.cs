using System;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Multiplayer
{
	// Token: 0x02000058 RID: 88
	[Serializable]
	public class MapListItemResponse
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x0000C0E6 File Offset: 0x0000A2E6
		// (set) Token: 0x060002CA RID: 714 RVA: 0x0000C0EE File Offset: 0x0000A2EE
		public string Name { get; private set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0000C0F7 File Offset: 0x0000A2F7
		// (set) Token: 0x060002CC RID: 716 RVA: 0x0000C0FF File Offset: 0x0000A2FF
		public string UniqueToken { get; private set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0000C108 File Offset: 0x0000A308
		// (set) Token: 0x060002CE RID: 718 RVA: 0x0000C110 File Offset: 0x0000A310
		public string Revision { get; private set; }

		// Token: 0x060002CF RID: 719 RVA: 0x0000C119 File Offset: 0x0000A319
		[JsonConstructor]
		public MapListItemResponse(string name, string uniqueToken, string revision)
		{
			this.Name = name;
			this.UniqueToken = uniqueToken;
			this.Revision = revision;
		}
	}
}
