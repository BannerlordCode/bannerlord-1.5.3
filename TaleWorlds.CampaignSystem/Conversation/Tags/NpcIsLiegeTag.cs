using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200024C RID: 588
	public class NpcIsLiegeTag : ConversationTag
	{
		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x060023B5 RID: 9141 RVA: 0x0009E066 File Offset: 0x0009C266
		public override string StringId
		{
			get
			{
				return "NpcIsLiegeTag";
			}
		}

		// Token: 0x060023B6 RID: 9142 RVA: 0x0009E06D File Offset: 0x0009C26D
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.IsKingdomLeader;
		}

		// Token: 0x04000A9A RID: 2714
		public const string Id = "NpcIsLiegeTag";
	}
}
