using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200026A RID: 618
	public class HostileRelationshipTag : ConversationTag
	{
		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x0600240F RID: 9231 RVA: 0x0009E7B0 File Offset: 0x0009C9B0
		public override string StringId
		{
			get
			{
				return "HostileRelationshipTag";
			}
		}

		// Token: 0x06002410 RID: 9232 RVA: 0x0009E7B8 File Offset: 0x0009C9B8
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
			return (num + num2 + num3 < -1 && unmodifiedClanLeaderRelationshipWithPlayer <= -5f) || unmodifiedClanLeaderRelationshipWithPlayer <= -20f;
		}

		// Token: 0x04000AB8 RID: 2744
		public const string Id = "HostileRelationshipTag";
	}
}
