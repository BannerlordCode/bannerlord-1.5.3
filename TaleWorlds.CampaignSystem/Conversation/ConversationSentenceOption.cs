using System;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Conversation
{
	// Token: 0x02000247 RID: 583
	public struct ConversationSentenceOption
	{
		// Token: 0x04000A72 RID: 2674
		public int SentenceNo;

		// Token: 0x04000A73 RID: 2675
		public string Id;

		// Token: 0x04000A74 RID: 2676
		public object RepeatObject;

		// Token: 0x04000A75 RID: 2677
		public TextObject Text;

		// Token: 0x04000A76 RID: 2678
		public string DebugInfo;

		// Token: 0x04000A77 RID: 2679
		public bool IsClickable;

		// Token: 0x04000A78 RID: 2680
		public bool HasPersuasion;

		// Token: 0x04000A79 RID: 2681
		public string SkillName;

		// Token: 0x04000A7A RID: 2682
		public string TraitName;

		// Token: 0x04000A7B RID: 2683
		public bool IsSpecial;

		// Token: 0x04000A7C RID: 2684
		public bool IsUsedOnce;

		// Token: 0x04000A7D RID: 2685
		public TextObject HintText;

		// Token: 0x04000A7E RID: 2686
		public PersuasionOptionArgs PersuationOptionArgs;
	}
}
