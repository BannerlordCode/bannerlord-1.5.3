using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000257 RID: 599
	public class PlayerIsAffiliatedTag : ConversationTag
	{
		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x060023D6 RID: 9174 RVA: 0x0009E30A File Offset: 0x0009C50A
		public override string StringId
		{
			get
			{
				return "PlayerIsAffiliatedTag";
			}
		}

		// Token: 0x060023D7 RID: 9175 RVA: 0x0009E311 File Offset: 0x0009C511
		public override bool IsApplicableTo(CharacterObject character)
		{
			return Hero.MainHero.MapFaction.IsKingdomFaction;
		}

		// Token: 0x04000AA5 RID: 2725
		public const string Id = "PlayerIsAffiliatedTag";
	}
}
