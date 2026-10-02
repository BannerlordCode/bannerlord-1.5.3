using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000265 RID: 613
	public class NpcIsMaleTag : ConversationTag
	{
		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06002400 RID: 9216 RVA: 0x0009E671 File Offset: 0x0009C871
		public override string StringId
		{
			get
			{
				return "NpcIsMaleTag";
			}
		}

		// Token: 0x06002401 RID: 9217 RVA: 0x0009E678 File Offset: 0x0009C878
		public override bool IsApplicableTo(CharacterObject character)
		{
			return !character.IsFemale;
		}

		// Token: 0x04000AB3 RID: 2739
		public const string Id = "NpcIsMaleTag";
	}
}
