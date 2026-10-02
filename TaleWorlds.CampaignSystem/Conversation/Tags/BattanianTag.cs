using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000287 RID: 647
	public class BattanianTag : ConversationTag
	{
		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06002466 RID: 9318 RVA: 0x0009EED1 File Offset: 0x0009D0D1
		public override string StringId
		{
			get
			{
				return "BattanianTag";
			}
		}

		// Token: 0x06002467 RID: 9319 RVA: 0x0009EED8 File Offset: 0x0009D0D8
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.Culture.StringId == "battania";
		}

		// Token: 0x04000AD6 RID: 2774
		public const string Id = "BattanianTag";
	}
}
