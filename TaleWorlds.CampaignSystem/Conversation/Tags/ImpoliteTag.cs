using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000271 RID: 625
	public class ImpoliteTag : ConversationTag
	{
		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x06002424 RID: 9252 RVA: 0x0009E9C2 File Offset: 0x0009CBC2
		public override string StringId
		{
			get
			{
				return "ImpoliteTag";
			}
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x0009E9CC File Offset: 0x0009CBCC
		public override bool IsApplicableTo(CharacterObject character)
		{
			if (!character.IsHero)
			{
				return false;
			}
			int heroRelation = CharacterRelationManager.GetHeroRelation(character.HeroObject, Hero.MainHero);
			return (character.HeroObject.IsLord || character.HeroObject.IsMerchant || character.HeroObject.IsGangLeader) && Clan.PlayerClan.Renown < 100f && heroRelation < 1 && character.GetTraitLevel(DefaultTraits.Mercy) + character.GetTraitLevel(DefaultTraits.Generosity) < 0;
		}

		// Token: 0x04000ABF RID: 2751
		public const string Id = "ImpoliteTag";
	}
}
