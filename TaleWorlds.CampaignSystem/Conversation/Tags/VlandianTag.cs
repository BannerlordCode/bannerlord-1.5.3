using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000288 RID: 648
	public class VlandianTag : ConversationTag
	{
		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x06002469 RID: 9321 RVA: 0x0009EEF7 File Offset: 0x0009D0F7
		public override string StringId
		{
			get
			{
				return "VlandianTag";
			}
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x0009EEFE File Offset: 0x0009D0FE
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.Culture.StringId == "vlandia";
		}

		// Token: 0x04000AD7 RID: 2775
		public const string Id = "VlandianTag";
	}
}
