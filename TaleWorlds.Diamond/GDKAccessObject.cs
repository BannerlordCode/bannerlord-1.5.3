using System;
using Newtonsoft.Json;

namespace TaleWorlds.Diamond
{
	// Token: 0x0200000B RID: 11
	[Serializable]
	public class GDKAccessObject : AccessObject
	{
		// Token: 0x0600003C RID: 60 RVA: 0x0000295B File Offset: 0x00000B5B
		public GDKAccessObject()
		{
			base.Type = "GDK";
		}

		// Token: 0x04000011 RID: 17
		[JsonProperty]
		public string Id;

		// Token: 0x04000012 RID: 18
		[JsonProperty]
		public string Token;
	}
}
