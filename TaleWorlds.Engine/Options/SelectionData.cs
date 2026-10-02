using System;

namespace TaleWorlds.Engine.Options
{
	// Token: 0x020000A9 RID: 169
	public struct SelectionData
	{
		// Token: 0x06000F74 RID: 3956 RVA: 0x000123B6 File Offset: 0x000105B6
		public SelectionData(bool isLocalizationId, string data)
		{
			this.IsLocalizationId = isLocalizationId;
			this.Data = data;
		}

		// Token: 0x0400021D RID: 541
		public bool IsLocalizationId;

		// Token: 0x0400021E RID: 542
		public string Data;
	}
}
