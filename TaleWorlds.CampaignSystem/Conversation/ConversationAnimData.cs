using System;
using System.Collections.Generic;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Conversation
{
	// Token: 0x02000240 RID: 576
	public class ConversationAnimData
	{
		// Token: 0x06002302 RID: 8962 RVA: 0x0009AB9B File Offset: 0x00098D9B
		public ConversationAnimData()
		{
			this.Reactions = new Dictionary<string, string>();
		}

		// Token: 0x04000A31 RID: 2609
		[SaveableField(0)]
		public string IdleAnimStart;

		// Token: 0x04000A32 RID: 2610
		[SaveableField(1)]
		public string IdleAnimLoop;

		// Token: 0x04000A33 RID: 2611
		[SaveableField(2)]
		public int FamilyType;

		// Token: 0x04000A34 RID: 2612
		[SaveableField(3)]
		public int MountFamilyType;

		// Token: 0x04000A35 RID: 2613
		[SaveableField(4)]
		public Dictionary<string, string> Reactions;
	}
}
