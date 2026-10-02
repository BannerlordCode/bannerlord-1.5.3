using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000261 RID: 609
	public class PlayerIsKnownButNotFamousTag : ConversationTag
	{
		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x060023F4 RID: 9204 RVA: 0x0009E55F File Offset: 0x0009C75F
		public override string StringId
		{
			get
			{
				return "PlayerIsKnownButNotFamousTag";
			}
		}

		// Token: 0x060023F5 RID: 9205 RVA: 0x0009E568 File Offset: 0x0009C768
		public override bool IsApplicableTo(CharacterObject character)
		{
			int num = Campaign.Current.Models.DiplomacyModel.GetBaseRelation(Hero.MainHero, Hero.OneToOneConversationHero);
			if (Hero.OneToOneConversationHero.Clan != null && num == 0)
			{
				num = Campaign.Current.Models.DiplomacyModel.GetBaseRelation(Hero.MainHero, Hero.OneToOneConversationHero.Clan.Leader);
			}
			return num != 0 && Clan.PlayerClan.Renown < 50f && Campaign.Current.ConversationManager.CurrentConversationIsFirst;
		}

		// Token: 0x04000AAF RID: 2735
		public const string Id = "PlayerIsKnownButNotFamousTag";
	}
}
