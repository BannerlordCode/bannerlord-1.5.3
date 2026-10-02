using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Helpers
{
	// Token: 0x02000010 RID: 16
	public static class TraitEffectHelper
	{
		// Token: 0x06000080 RID: 128 RVA: 0x00007D44 File Offset: 0x00005F44
		public static void ApplyTraitEffect(Hero hero, TraitEffectObject effect, ref ExplainedNumber result)
		{
			float traitEffectBonus = TraitEffectHelper.GetTraitEffectBonus(hero, effect);
			if (traitEffectBonus == 0f)
			{
				return;
			}
			if (effect.IncrementType == EffectIncrementType.Add)
			{
				result.Add(traitEffectBonus, new TextObject("{=ENta0wCu}Personality", null), null);
				return;
			}
			if (effect.IncrementType == EffectIncrementType.AddFactor)
			{
				result.AddFactor(traitEffectBonus, new TextObject("{=ENta0wCu}Personality", null));
				return;
			}
			Debug.FailedAssert("effect.IncrementType is out of range!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Helpers.cs", "ApplyTraitEffect", 3532);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00007DB4 File Offset: 0x00005FB4
		public static float GetTraitEffectBonus(Hero hero, TraitEffectObject effect)
		{
			int traitLevel = hero.GetTraitLevel(effect.Trait);
			if (traitLevel == 0)
			{
				return 0f;
			}
			float bonus = effect.GetBonus(traitLevel);
			if (bonus == 0f)
			{
				return 0f;
			}
			return bonus;
		}
	}
}
