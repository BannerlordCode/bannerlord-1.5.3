using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000205 RID: 517
	public abstract class HeroCreationModel : MBGameModel<HeroCreationModel>
	{
		// Token: 0x0600202C RID: 8236
		[return: TupleElementNames(new string[] { "birthDay", "deathDay" })]
		public abstract ValueTuple<CampaignTime, CampaignTime> GetBirthAndDeathDay(CharacterObject character, bool createAlive, int age);

		// Token: 0x0600202D RID: 8237
		public abstract Settlement GetBornSettlement(Hero character);

		// Token: 0x0600202E RID: 8238
		public abstract StaticBodyProperties GetStaticBodyProperties(Hero character, bool isOffspring, float variationAmount = 0.2f);

		// Token: 0x0600202F RID: 8239
		public abstract FormationClass GetPreferredUpgradeFormation(Hero character);

		// Token: 0x06002030 RID: 8240
		public abstract Clan GetClan(Hero character);

		// Token: 0x06002031 RID: 8241
		public abstract CultureObject GetCulture(Hero hero, Settlement bornSettlement, Clan clan);

		// Token: 0x06002032 RID: 8242
		public abstract CharacterObject GetRandomTemplateByOccupation(Occupation occupation, Settlement settlement = null);

		// Token: 0x06002033 RID: 8243
		[return: TupleElementNames(new string[] { "trait", "level" })]
		public abstract List<ValueTuple<TraitObject, int>> GetTraitsForHero(Hero hero);

		// Token: 0x06002034 RID: 8244
		public abstract Equipment GetCivilianEquipment(Hero hero);

		// Token: 0x06002035 RID: 8245
		public abstract Equipment GetBattleEquipment(Hero hero);

		// Token: 0x06002036 RID: 8246
		public abstract CharacterObject GetCharacterTemplateForOffspring(Hero mother, Hero father, bool isOffspringFemale);

		// Token: 0x06002037 RID: 8247
		[return: TupleElementNames(new string[] { "firstName", "name" })]
		public abstract ValueTuple<TextObject, TextObject> GenerateFirstAndFullName(Hero hero);

		// Token: 0x06002038 RID: 8248
		public abstract List<ValueTuple<SkillObject, int>> GetDefaultSkillsForHero(Hero hero);

		// Token: 0x06002039 RID: 8249
		public abstract List<ValueTuple<SkillObject, int>> GetInheritedSkillsForHero(Hero hero);

		// Token: 0x0600203A RID: 8250
		public abstract bool IsHeroCombatant(Hero hero);
	}
}
