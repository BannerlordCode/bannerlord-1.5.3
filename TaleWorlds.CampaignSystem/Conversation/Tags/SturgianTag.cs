using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200028B RID: 651
	public class SturgianTag : ConversationTag
	{
		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06002472 RID: 9330 RVA: 0x0009EF69 File Offset: 0x0009D169
		public override string StringId
		{
			get
			{
				return "SturgianTag";
			}
		}

		// Token: 0x06002473 RID: 9331 RVA: 0x0009EF70 File Offset: 0x0009D170
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.Culture.StringId == "sturgia";
		}

		// Token: 0x04000ADA RID: 2778
		public const string Id = "SturgianTag";
	}
}
