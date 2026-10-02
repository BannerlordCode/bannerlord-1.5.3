using System;
using Newtonsoft.Json;

namespace TaleWorlds.Diamond
{
	// Token: 0x02000008 RID: 8
	[Serializable]
	public class EpicAccessObject : AccessObject
	{
		// Token: 0x06000033 RID: 51 RVA: 0x000027BA File Offset: 0x000009BA
		public EpicAccessObject()
		{
			base.Type = "Epic";
		}

		// Token: 0x0400000E RID: 14
		[JsonProperty]
		public string AccessToken;

		// Token: 0x0400000F RID: 15
		[JsonProperty]
		public string EpicId;
	}
}
