using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000109 RID: 265
	public class DefaultCharacterStatsModel : CharacterStatsModel
	{
		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06001779 RID: 6009 RVA: 0x0006D08D File Offset: 0x0006B28D
		public override int MaxCharacterTier
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x0006D090 File Offset: 0x0006B290
		public override int WoundedHitPointLimit(Hero hero)
		{
			return 20;
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x0006D094 File Offset: 0x0006B294
		public override int GetTier(CharacterObject character)
		{
			if (character.IsHero)
			{
				return 0;
			}
			return MathF.Min(MathF.Max(MathF.Ceiling(((float)character.Level - 5f) / 5f), 0), Campaign.Current.Models.CharacterStatsModel.MaxCharacterTier);
		}

		// Token: 0x0600177C RID: 6012 RVA: 0x0006D0E4 File Offset: 0x0006B2E4
		public override ExplainedNumber MaxHitpoints(CharacterObject character, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(100f, includeDescriptions, null);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.Trainer, BattleEnvironment.Any, character, true, ref explainedNumber);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.TwoHanded.ThickHides, BattleEnvironment.Any, character, true, ref explainedNumber);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Medicine.DoctorsOath, BattleEnvironment.Any, character, false, ref explainedNumber);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Medicine.FortitudeTonic, BattleEnvironment.Any, character, false, ref explainedNumber);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Athletics.WellBuilt, BattleEnvironment.Any, character, true, ref explainedNumber);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.UnwaveringDefense, BattleEnvironment.Any, character, true, ref explainedNumber);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Medicine.PreventiveMedicine, BattleEnvironment.Any, character, true, ref explainedNumber);
			if (character.IsHero && character.HeroObject.PartyBelongedTo != null && character.HeroObject.PartyBelongedTo.LeaderHero != character.HeroObject)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.FortitudeTonic, character.HeroObject.PartyBelongedTo, true, ref explainedNumber);
			}
			if (character.GetPerkValue(DefaultPerks.Athletics.MightyBlow))
			{
				int num = character.GetSkillValue(DefaultSkills.Athletics) - Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus;
				explainedNumber.Add((float)num, DefaultPerks.Athletics.MightyBlow.Name, null);
			}
			return explainedNumber;
		}
	}
}
