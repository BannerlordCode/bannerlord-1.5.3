using System;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000296 RID: 662
	public class PlayerIsAtSeaTag : ConversationTag
	{
		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06002493 RID: 9363 RVA: 0x0009F15B File Offset: 0x0009D35B
		public override string StringId
		{
			get
			{
				return "PlayerIsAtSeaTag";
			}
		}

		// Token: 0x06002494 RID: 9364 RVA: 0x0009F164 File Offset: 0x0009D364
		public override bool IsApplicableTo(CharacterObject character)
		{
			MobileParty mobileParty = (Hero.MainHero.IsPrisoner ? Hero.MainHero.PartyBelongedToAsPrisoner.MobileParty : Hero.MainHero.PartyBelongedTo);
			MobileParty mobileParty2 = (character.HeroObject.IsPrisoner ? character.HeroObject.PartyBelongedToAsPrisoner.MobileParty : character.HeroObject.PartyBelongedTo);
			return mobileParty.IsCurrentlyAtSea && mobileParty2 != mobileParty;
		}

		// Token: 0x04000AE5 RID: 2789
		public const string Id = "PlayerIsAtSeaTag";
	}
}
