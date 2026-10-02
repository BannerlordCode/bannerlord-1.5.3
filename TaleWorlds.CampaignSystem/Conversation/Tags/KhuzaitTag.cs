using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000289 RID: 649
	public class KhuzaitTag : ConversationTag
	{
		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x0600246C RID: 9324 RVA: 0x0009EF1D File Offset: 0x0009D11D
		public override string StringId
		{
			get
			{
				return "KhuzaitTag";
			}
		}

		// Token: 0x0600246D RID: 9325 RVA: 0x0009EF24 File Offset: 0x0009D124
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.Culture.StringId == "khuzait";
		}

		// Token: 0x04000AD8 RID: 2776
		public const string Id = "KhuzaitTag";
	}
}
