using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000192 RID: 402
	public abstract class CharacterDevelopmentModel : MBGameModel<CharacterDevelopmentModel>
	{
		// Token: 0x06001CAA RID: 7338
		public abstract int SkillsRequiredForLevel(int level);

		// Token: 0x06001CAB RID: 7339
		public abstract int GetMaxSkillPoint();

		// Token: 0x06001CAC RID: 7340
		public abstract int GetXpRequiredForSkillLevel(int skillLevel);

		// Token: 0x06001CAD RID: 7341
		public abstract int GetSkillLevelChange(Hero hero, SkillObject skill, float skillXp);

		// Token: 0x06001CAE RID: 7342
		public abstract int GetXpAmountForSkillLevelChange(Hero hero, SkillObject skill, int skillLevelChange);

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06001CAF RID: 7343
		public abstract int MaxAttribute { get; }

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06001CB0 RID: 7344
		public abstract int MaxFocusPerSkill { get; }

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06001CB1 RID: 7345
		public abstract int MaxSkillRequiredForEpicPerkBonus { get; }

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06001CB2 RID: 7346
		public abstract int MinSkillRequiredForEpicPerkBonus { get; }

		// Token: 0x06001CB3 RID: 7347
		public abstract void GetTraitLevelForTraitXp(Hero hero, TraitObject trait, int newValue, out int traitLevel, out int traitXp);

		// Token: 0x06001CB4 RID: 7348
		public abstract int GetTraitXpRequiredForTraitLevel(TraitObject trait, int traitLevel);

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06001CB5 RID: 7349
		public abstract int FocusPointsPerLevel { get; }

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06001CB6 RID: 7350
		public abstract int FocusPointsAtStart { get; }

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06001CB7 RID: 7351
		public abstract int AttributePointsAtStart { get; }

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06001CB8 RID: 7352
		public abstract int LevelsPerAttributePoint { get; }

		// Token: 0x06001CB9 RID: 7353
		public abstract ExplainedNumber CalculateLearningLimit(IReadOnlyPropertyOwner<CharacterAttribute> characterAttributes, int focusValue, SkillObject skill, bool includeDescriptions = false);

		// Token: 0x06001CBA RID: 7354
		public abstract ExplainedNumber CalculateLearningRate(IReadOnlyPropertyOwner<CharacterAttribute> characterAttributes, int focusValue, int skillValue, SkillObject skill, bool includeDescriptions = false);

		// Token: 0x06001CBB RID: 7355
		public abstract SkillObject GetNextSkillToAddFocus(Hero hero);

		// Token: 0x06001CBC RID: 7356
		public abstract CharacterAttribute GetNextAttributeToUpgrade(Hero hero);

		// Token: 0x06001CBD RID: 7357
		public abstract PerkObject GetNextPerkToChoose(Hero hero, PerkObject perk);
	}
}
