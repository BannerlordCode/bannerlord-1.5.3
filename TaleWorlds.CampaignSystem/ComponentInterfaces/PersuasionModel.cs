using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A5 RID: 421
	public abstract class PersuasionModel : MBGameModel<PersuasionModel>
	{
		// Token: 0x06001D26 RID: 7462
		public abstract int GetSkillXpFromPersuasion(PersuasionDifficulty difficulty, int argumentDifficultyBonusCoefficient);

		// Token: 0x06001D27 RID: 7463
		public abstract void GetChances(PersuasionOptionArgs optionArgs, out float successChance, out float critSuccessChance, out float critFailChance, out float failChance, float difficultyMultiplier);

		// Token: 0x06001D28 RID: 7464
		public abstract void GetEffectChances(PersuasionOptionArgs option, out float moveToNextStageChance, out float blockRandomOptionChance, float difficultyMultiplier);

		// Token: 0x06001D29 RID: 7465
		public abstract PersuasionArgumentStrength GetArgumentStrengthBasedOnTargetTraits(CharacterObject character, Tuple<TraitObject, int>[] traitCorrelation);

		// Token: 0x06001D2A RID: 7466
		public abstract float GetDifficulty(PersuasionDifficulty difficulty);

		// Token: 0x06001D2B RID: 7467
		public abstract float CalculateInitialPersuasionProgress(CharacterObject character, float goalValue, float successValue);

		// Token: 0x06001D2C RID: 7468
		public abstract float CalculatePersuasionGoalValue(CharacterObject oneToOneConversationCharacter, float successValue);
	}
}
