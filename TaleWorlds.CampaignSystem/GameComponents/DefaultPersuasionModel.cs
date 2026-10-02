using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000146 RID: 326
	public class DefaultPersuasionModel : PersuasionModel
	{
		// Token: 0x06001A27 RID: 6695 RVA: 0x00083C1D File Offset: 0x00081E1D
		public override int GetSkillXpFromPersuasion(PersuasionDifficulty difficulty, int argumentDifficultyBonusCoefficient)
		{
			return (difficulty - PersuasionDifficulty.VeryEasy) / 1 * 5 * argumentDifficultyBonusCoefficient;
		}

		// Token: 0x06001A28 RID: 6696 RVA: 0x00083C28 File Offset: 0x00081E28
		public override void GetChances(PersuasionOptionArgs optionArgs, out float successChance, out float critSuccessChance, out float critFailChance, out float failChance, float difficultyMultiplier)
		{
			float defaultSuccessChance = this.GetDefaultSuccessChance(optionArgs, difficultyMultiplier);
			switch (optionArgs.ArgumentStrength)
			{
			case PersuasionArgumentStrength.ExtremelyHard:
				successChance = defaultSuccessChance * 0.05f;
				critFailChance = defaultSuccessChance * 0.15f;
				goto IL_00C3;
			case PersuasionArgumentStrength.VeryHard:
				successChance = defaultSuccessChance * 0.15f;
				critFailChance = defaultSuccessChance * 0.2f;
				goto IL_00C3;
			case PersuasionArgumentStrength.Hard:
				successChance = defaultSuccessChance * 0.35f;
				critFailChance = defaultSuccessChance * 0.15f;
				goto IL_00C3;
			case PersuasionArgumentStrength.Easy:
				successChance = defaultSuccessChance * 0.7f;
				critFailChance = 0f;
				goto IL_00C3;
			case PersuasionArgumentStrength.VeryEasy:
				successChance = defaultSuccessChance * 0.8f;
				critFailChance = 0f;
				goto IL_00C3;
			case PersuasionArgumentStrength.ExtremelyEasy:
				successChance = defaultSuccessChance * 0.9f;
				critFailChance = 0f;
				goto IL_00C3;
			}
			successChance = defaultSuccessChance * 0.55f;
			critFailChance = defaultSuccessChance * 0.15f;
			IL_00C3:
			float bonusSuccessChance = this.GetBonusSuccessChance(optionArgs);
			successChance = MathF.Clamp(successChance * bonusSuccessChance, 0f, 1f);
			critFailChance = MathF.Clamp(critFailChance * (2f - bonusSuccessChance), 0f, 1f);
			critSuccessChance = 0f;
			if (optionArgs.GivesCriticalSuccess)
			{
				critSuccessChance = successChance;
				successChance = 0f;
			}
			if (critSuccessChance > 0f && Hero.MainHero.GetPerkValue(DefaultPerks.Charm.MeaningfulFavors))
			{
				critSuccessChance = MathF.Clamp(critSuccessChance + critSuccessChance * DefaultPerks.Charm.MeaningfulFavors.PrimaryBonus, 0f, 1f);
			}
			failChance = 1f - critSuccessChance - successChance - critFailChance;
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x00083D9C File Offset: 0x00081F9C
		private float GetDefaultSuccessChance(PersuasionOptionArgs optionArgs, float difficultyMultiplier)
		{
			int skillValue = Hero.MainHero.GetSkillValue(optionArgs.SkillUsed);
			float num = (float)((optionArgs.TraitEffect == TraitEffect.Positive) ? Hero.MainHero.GetTraitLevel(optionArgs.TraitUsed) : (-(float)Hero.MainHero.GetTraitLevel(optionArgs.TraitUsed)));
			num = MBMath.ClampFloat((optionArgs.TraitUsed != null) ? num : 0f, -1f, 2f);
			float num2;
			if (num <= 0f)
			{
				num2 = 0.2f + (num - -1f) * 0.4f;
			}
			else if (num <= 1f)
			{
				num2 = 0.6f + (num - 0f) * 0.6f;
			}
			else
			{
				num2 = 1.2f + (num - 1f) * 0.4f;
			}
			return MathF.Clamp((100f - difficultyMultiplier / (0.01f * (100f + (float)skillValue * num2))) / 100f, 0.1f, 1f);
		}

		// Token: 0x06001A2A RID: 6698 RVA: 0x00083E8C File Offset: 0x0008208C
		private float GetBonusSuccessChance(PersuasionOptionArgs optionArgs)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
			explainedNumber.AddFactor(MathF.Clamp((float)(Hero.MainHero.GetSkillValue(optionArgs.SkillUsed) / Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus) * 0.2f, 0f, 0.2f), optionArgs.SkillUsed.Name);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Athletics.ImposingStature, BattleEnvironment.Any, Hero.MainHero.CharacterObject, true, ref explainedNumber);
			float persuasionBonusChance = Campaign.Current.Models.DifficultyModel.GetPersuasionBonusChance();
			if (persuasionBonusChance > 0f)
			{
				explainedNumber.AddFactor(persuasionBonusChance, GameTexts.FindText("str_game_difficulty", null));
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x06001A2B RID: 6699 RVA: 0x00083F44 File Offset: 0x00082144
		public override void GetEffectChances(PersuasionOptionArgs option, out float moveToNextStageChance, out float blockRandomOptionChance, float difficultyMultiplier)
		{
			moveToNextStageChance = 0f;
			blockRandomOptionChance = 0f;
			if (option.CanMoveToTheNextReservation || option.CanBlockOtherOption)
			{
				moveToNextStageChance = (option.CanMoveToTheNextReservation ? 1f : 0f);
				blockRandomOptionChance = (option.CanBlockOtherOption ? 1f : 0f);
			}
		}

		// Token: 0x06001A2C RID: 6700 RVA: 0x00083F9C File Offset: 0x0008219C
		public override PersuasionArgumentStrength GetArgumentStrengthBasedOnTargetTraits(CharacterObject character, Tuple<TraitObject, int>[] traitCorrelations)
		{
			float num = 0f;
			float num2 = 1f;
			for (int i = 0; i < traitCorrelations.Length; i++)
			{
				num2 += (float)(character.GetTraitLevel(traitCorrelations[i].Item1) * traitCorrelations[i].Item2);
				num += (float)MathF.Abs(traitCorrelations[i].Item2);
				num2 += (float)(CharacterObject.PlayerCharacter.GetTraitLevel(traitCorrelations[i].Item1) * traitCorrelations[i].Item2);
				num += (float)MathF.Abs(traitCorrelations[i].Item2);
			}
			if (num2 > num / 6f)
			{
				return PersuasionArgumentStrength.VeryEasy;
			}
			if (num2 > 0f)
			{
				return PersuasionArgumentStrength.ExtremelyEasy;
			}
			if (num2 < num / -6f)
			{
				return PersuasionArgumentStrength.VeryHard;
			}
			if (num2 < 0f)
			{
				return PersuasionArgumentStrength.Hard;
			}
			return PersuasionArgumentStrength.Normal;
		}

		// Token: 0x06001A2D RID: 6701 RVA: 0x00084050 File Offset: 0x00082250
		public override float CalculateInitialPersuasionProgress(CharacterObject character, float goalValue, float successValue)
		{
			float num;
			if (!character.IsHero)
			{
				num = 0f;
			}
			else
			{
				num = MathF.Max(0f, character.HeroObject.GetRelationWithPlayer() * successValue) / (float)Campaign.Current.Models.DiplomacyModel.MaxRelationLimit;
			}
			return num;
		}

		// Token: 0x06001A2E RID: 6702 RVA: 0x0008409C File Offset: 0x0008229C
		public override float CalculatePersuasionGoalValue(CharacterObject oneToOneConversationCharacter, float successValue)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(successValue, false, null);
			if (CharacterObject.OneToOneConversationCharacter != null && CharacterObject.OneToOneConversationCharacter.IsHero)
			{
				if (CharacterObject.OneToOneConversationCharacter.HeroObject.Culture == Hero.MainHero.Culture)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Charm.MoralLeader, BattleEnvironment.Any, Hero.MainHero.CharacterObject, true, ref explainedNumber);
				}
				else if (CharacterObject.OneToOneConversationCharacter.HeroObject.Culture != Hero.MainHero.Culture)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Charm.NaturalLeader, BattleEnvironment.Any, Hero.MainHero.CharacterObject, true, ref explainedNumber);
				}
			}
			explainedNumber.LimitMin(1f);
			return explainedNumber.ResultNumber;
		}

		// Token: 0x06001A2F RID: 6703 RVA: 0x00084144 File Offset: 0x00082344
		public override float GetDifficulty(PersuasionDifficulty difficulty)
		{
			switch (difficulty)
			{
			case PersuasionDifficulty.VeryEasy:
				return 0.9f;
			case PersuasionDifficulty.Easy:
				return 0.8f;
			case PersuasionDifficulty.EasyMedium:
				return 0.7f;
			case PersuasionDifficulty.Medium:
				return 0.6f;
			case PersuasionDifficulty.MediumHard:
				return 0.5f;
			case PersuasionDifficulty.Hard:
				return 0.4f;
			case PersuasionDifficulty.VeryHard:
				return 0.3f;
			case PersuasionDifficulty.UltraHard:
				return 0.2f;
			case PersuasionDifficulty.Impossible:
				return 0.1f;
			default:
				return 1f;
			}
		}
	}
}
