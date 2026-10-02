using System;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000284 RID: 644
	public class InHomeSettlementTag : ConversationTag
	{
		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x0600245D RID: 9309 RVA: 0x0009EE07 File Offset: 0x0009D007
		public override string StringId
		{
			get
			{
				return "InHomeSettlementTag";
			}
		}

		// Token: 0x0600245E RID: 9310 RVA: 0x0009EE10 File Offset: 0x0009D010
		public override bool IsApplicableTo(CharacterObject character)
		{
			return (character.IsHero && Settlement.CurrentSettlement != null && character.HeroObject.HomeSettlement == Settlement.CurrentSettlement) || (character.IsHero && Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.OwnerClan.Leader == character.HeroObject);
		}

		// Token: 0x04000AD3 RID: 2771
		public const string Id = "InHomeSettlementTag";
	}
}
