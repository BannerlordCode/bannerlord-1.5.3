using System;
using System.Linq;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200027A RID: 634
	public class PlayerBesiegingTag : ConversationTag
	{
		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x0600243F RID: 9279 RVA: 0x0009EC0E File Offset: 0x0009CE0E
		public override string StringId
		{
			get
			{
				return "PlayerBesiegingTag";
			}
		}

		// Token: 0x06002440 RID: 9280 RVA: 0x0009EC18 File Offset: 0x0009CE18
		public override bool IsApplicableTo(CharacterObject character)
		{
			if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.SiegeEvent != null)
			{
				return Settlement.CurrentSettlement.SiegeEvent.BesiegerCamp.GetInvolvedPartiesForEventType(MapEvent.BattleTypes.Siege).Any<PartyBase>((PartyBase party) => party.MobileParty == Hero.MainHero.PartyBelongedTo);
			}
			return false;
		}

		// Token: 0x04000AC9 RID: 2761
		public const string Id = "PlayerBesiegingTag";
	}
}
