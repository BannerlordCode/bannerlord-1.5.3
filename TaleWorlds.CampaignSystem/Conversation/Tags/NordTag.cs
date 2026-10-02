using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000297 RID: 663
	public class NordTag : ConversationTag
	{
		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06002496 RID: 9366 RVA: 0x0009F1DD File Offset: 0x0009D3DD
		public override string StringId
		{
			get
			{
				return "NordTag";
			}
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x0009F1E4 File Offset: 0x0009D3E4
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.Culture.StringId == "nord";
		}

		// Token: 0x04000AE6 RID: 2790
		public const string Id = "NordTag";
	}
}
