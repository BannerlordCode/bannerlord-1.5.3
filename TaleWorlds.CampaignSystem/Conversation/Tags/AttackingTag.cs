using System;
using Helpers;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000278 RID: 632
	public class AttackingTag : ConversationTag
	{
		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x06002439 RID: 9273 RVA: 0x0009EBAD File Offset: 0x0009CDAD
		public override string StringId
		{
			get
			{
				return "AttackingTag";
			}
		}

		// Token: 0x0600243A RID: 9274 RVA: 0x0009EBB4 File Offset: 0x0009CDB4
		public override bool IsApplicableTo(CharacterObject character)
		{
			return HeroHelper.WillLordAttack() || (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.SiegeEvent != null && Settlement.CurrentSettlement.Parties.Contains(Hero.MainHero.PartyBelongedTo));
		}

		// Token: 0x04000AC7 RID: 2759
		public const string Id = "AttackingTag";
	}
}
