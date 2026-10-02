using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x020002AA RID: 682
	public static class ConversationTagHelper
	{
		// Token: 0x060024CF RID: 9423 RVA: 0x0009F4F2 File Offset: 0x0009D6F2
		public static bool UsesHighRegister(CharacterObject character)
		{
			return ConversationTagHelper.EducatedClass(character) && !ConversationTagHelper.TribalVoiceGroup(character);
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x0009F507 File Offset: 0x0009D707
		public static bool UsesLowRegister(CharacterObject character)
		{
			return !ConversationTagHelper.EducatedClass(character) && !ConversationTagHelper.TribalVoiceGroup(character);
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x0009F51C File Offset: 0x0009D71C
		public static bool TribalVoiceGroup(CharacterObject character)
		{
			return character.Culture.StringId == "sturgia" || character.Culture.StringId == "aserai" || character.Culture.StringId == "khuzait" || character.Culture.StringId == "battania" || character.Culture.StringId == "vlandia" || character.Culture.StringId == "nord" || character.Culture.StringId == "vakken";
		}

		// Token: 0x060024D2 RID: 9426 RVA: 0x0009F5D0 File Offset: 0x0009D7D0
		public static bool EducatedClass(CharacterObject character)
		{
			bool flag = false;
			if (character.HeroObject != null)
			{
				Clan clan = character.HeroObject.Clan;
				if (clan != null && clan.IsNoble)
				{
					flag = true;
				}
				if (character.HeroObject.IsMerchant)
				{
					flag = true;
				}
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Siegecraft) >= 5 || character.HeroObject.GetTraitLevel(DefaultTraits.Surgery) >= 5)
				{
					flag = true;
				}
				if (character.HeroObject.IsGangLeader)
				{
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x060024D3 RID: 9427 RVA: 0x0009F64C File Offset: 0x0009D84C
		public static int TraitCompatibility(Hero hero1, Hero hero2, TraitObject trait)
		{
			int traitLevel = hero1.GetTraitLevel(trait);
			int traitLevel2 = hero2.GetTraitLevel(trait);
			if (traitLevel > 0 && traitLevel2 > 0)
			{
				return 1;
			}
			if (traitLevel < 0 || traitLevel2 < 0)
			{
				return MathF.Abs(traitLevel - traitLevel2) * -1;
			}
			return 0;
		}
	}
}
