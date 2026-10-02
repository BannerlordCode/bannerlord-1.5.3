using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200028A RID: 650
	public class AseraiTag : ConversationTag
	{
		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x0600246F RID: 9327 RVA: 0x0009EF43 File Offset: 0x0009D143
		public override string StringId
		{
			get
			{
				return "AseraiTag";
			}
		}

		// Token: 0x06002470 RID: 9328 RVA: 0x0009EF4A File Offset: 0x0009D14A
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.Culture.StringId == "aserai";
		}

		// Token: 0x04000AD9 RID: 2777
		public const string Id = "AseraiTag";
	}
}
