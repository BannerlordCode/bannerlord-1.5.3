using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000269 RID: 617
	public class FriendlyRelationshipTag : ConversationTag
	{
		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x0600240C RID: 9228 RVA: 0x0009E720 File Offset: 0x0009C920
		public override string StringId
		{
			get
			{
				return "FriendlyRelationshipTag";
			}
		}

		// Token: 0x0600240D RID: 9229 RVA: 0x0009E728 File Offset: 0x0009C928
		public override bool IsApplicableTo(CharacterObject character)
		{
			if (!character.IsHero)
			{
				return false;
			}
			float unmodifiedClanLeaderRelationshipWithPlayer = character.HeroObject.GetUnmodifiedClanLeaderRelationshipWithPlayer();
			int num = ConversationTagHelper.TraitCompatibility(character.HeroObject, Hero.MainHero, DefaultTraits.Mercy);
			int num2 = ConversationTagHelper.TraitCompatibility(character.HeroObject, Hero.MainHero, DefaultTraits.Honor);
			int num3 = ConversationTagHelper.TraitCompatibility(character.HeroObject, Hero.MainHero, DefaultTraits.Valor);
			return (num + num2 + num3 > 0 && unmodifiedClanLeaderRelationshipWithPlayer >= 5f) || unmodifiedClanLeaderRelationshipWithPlayer >= 20f;
		}

		// Token: 0x04000AB7 RID: 2743
		public const string Id = "FriendlyRelationshipTag";
	}
}
