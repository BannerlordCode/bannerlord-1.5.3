using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200027B RID: 635
	public class PlayerIsLiegeTag : ConversationTag
	{
		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06002442 RID: 9282 RVA: 0x0009EC7B File Offset: 0x0009CE7B
		public override string StringId
		{
			get
			{
				return "PlayerIsLiegeTag";
			}
		}

		// Token: 0x06002443 RID: 9283 RVA: 0x0009EC84 File Offset: 0x0009CE84
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.MapFaction.IsKingdomFaction && character.HeroObject.MapFaction == Hero.MainHero.MapFaction && Hero.MainHero.MapFaction.Leader == Hero.MainHero;
		}

		// Token: 0x04000ACA RID: 2762
		public const string Id = "PlayerIsLiegeTag";
	}
}
