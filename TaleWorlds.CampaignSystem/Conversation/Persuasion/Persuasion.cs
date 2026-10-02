using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Conversation.Persuasion
{
	// Token: 0x020002B1 RID: 689
	public class Persuasion
	{
		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x060024ED RID: 9453 RVA: 0x0009F8BB File Offset: 0x0009DABB
		public float DifficultyMultiplier
		{
			get
			{
				return this._difficultyMultiplier;
			}
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x060024EE RID: 9454 RVA: 0x0009F8C3 File Offset: 0x0009DAC3
		// (set) Token: 0x060024EF RID: 9455 RVA: 0x0009F8CB File Offset: 0x0009DACB
		public float Progress { get; private set; }

		// Token: 0x060024F0 RID: 9456 RVA: 0x0009F8D4 File Offset: 0x0009DAD4
		public Persuasion(float goalValue, float successValue, float failValue, float criticalSuccessValue, float criticalFailValue, float initialProgress, PersuasionDifficulty difficulty)
		{
			this._chosenOptions = new List<Tuple<PersuasionOptionArgs, PersuasionOptionResult>>();
			this.GoalValue = Campaign.Current.Models.PersuasionModel.CalculatePersuasionGoalValue(CharacterObject.OneToOneConversationCharacter, goalValue);
			this.SuccessValue = successValue;
			this.FailValue = failValue;
			this.CriticalSuccessValue = criticalSuccessValue;
			this.CriticalFailValue = criticalFailValue;
			this._difficulty = difficulty;
			if (initialProgress < 0f)
			{
				this.Progress = Campaign.Current.Models.PersuasionModel.CalculateInitialPersuasionProgress(CharacterObject.OneToOneConversationCharacter, this.GoalValue, this.SuccessValue);
			}
			else
			{
				this.Progress = initialProgress;
			}
			this._difficultyMultiplier = Campaign.Current.Models.PersuasionModel.GetDifficulty(difficulty);
		}

		// Token: 0x060024F1 RID: 9457 RVA: 0x0009F994 File Offset: 0x0009DB94
		public void CommitProgress(PersuasionOptionArgs persuasionOptionArgs)
		{
			PersuasionOptionResult persuasionOptionResult = this.GetResult(persuasionOptionArgs);
			persuasionOptionResult = this.CheckPerkEffectOnResult(persuasionOptionResult);
			Tuple<PersuasionOptionArgs, PersuasionOptionResult> tuple = new Tuple<PersuasionOptionArgs, PersuasionOptionResult>(persuasionOptionArgs, persuasionOptionResult);
			persuasionOptionArgs.BlockTheOption(true);
			this._chosenOptions.Add(tuple);
			this.Progress = MathF.Clamp(this.Progress + this.GetPersuasionOptionResultValue(persuasionOptionResult), 0f, this.GoalValue);
			CampaignEventDispatcher.Instance.OnPersuasionProgressCommitted(tuple);
		}

		// Token: 0x060024F2 RID: 9458 RVA: 0x0009F9FC File Offset: 0x0009DBFC
		private PersuasionOptionResult CheckPerkEffectOnResult(PersuasionOptionResult result)
		{
			PersuasionOptionResult persuasionOptionResult = result;
			if (result == PersuasionOptionResult.CriticalFailure && Hero.MainHero.GetPerkValue(DefaultPerks.Charm.ForgivableGrievances) && MBRandom.RandomFloat <= DefaultPerks.Charm.ForgivableGrievances.PrimaryBonus)
			{
				TextObject textObject = new TextObject("{=5IQriov5}You avoided critical failure because of {PERK_NAME}.", null);
				textObject.SetTextVariable("PERK_NAME", DefaultPerks.Charm.ForgivableGrievances.Name);
				InformationManager.DisplayMessage(new InformationMessage(textObject.ToString(), Color.White));
				persuasionOptionResult = PersuasionOptionResult.Failure;
			}
			return persuasionOptionResult;
		}

		// Token: 0x060024F3 RID: 9459 RVA: 0x0009FA68 File Offset: 0x0009DC68
		private float GetPersuasionOptionResultValue(PersuasionOptionResult result)
		{
			switch (result)
			{
			case PersuasionOptionResult.CriticalFailure:
				return -this.CriticalFailValue;
			case PersuasionOptionResult.Failure:
				return 0f;
			case PersuasionOptionResult.Success:
				return this.SuccessValue;
			case PersuasionOptionResult.CriticalSuccess:
				return this.CriticalSuccessValue;
			case PersuasionOptionResult.Miss:
				return 0f;
			default:
				return 0f;
			}
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x0009FAB8 File Offset: 0x0009DCB8
		private PersuasionOptionResult GetResult(PersuasionOptionArgs optionArgs)
		{
			float num;
			float num2;
			float num3;
			float num4;
			Campaign.Current.Models.PersuasionModel.GetChances(optionArgs, out num, out num2, out num3, out num4, this._difficultyMultiplier);
			float num5 = MBRandom.RandomFloat;
			if (num5 < num2)
			{
				return PersuasionOptionResult.CriticalSuccess;
			}
			num5 -= num2;
			if (num5 < num)
			{
				return PersuasionOptionResult.Success;
			}
			num5 -= num;
			if (num5 < num4)
			{
				return PersuasionOptionResult.Failure;
			}
			num5 -= num4;
			if (num5 < num3)
			{
				return PersuasionOptionResult.CriticalFailure;
			}
			return PersuasionOptionResult.Miss;
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x0009FB1E File Offset: 0x0009DD1E
		public IEnumerable<Tuple<PersuasionOptionArgs, PersuasionOptionResult>> GetChosenOptions()
		{
			return this._chosenOptions.AsReadOnly();
		}

		// Token: 0x04000B23 RID: 2851
		public readonly float SuccessValue;

		// Token: 0x04000B24 RID: 2852
		public readonly float FailValue;

		// Token: 0x04000B25 RID: 2853
		public readonly float CriticalSuccessValue;

		// Token: 0x04000B26 RID: 2854
		public readonly float CriticalFailValue;

		// Token: 0x04000B27 RID: 2855
		private readonly float _difficultyMultiplier;

		// Token: 0x04000B28 RID: 2856
		private readonly PersuasionDifficulty _difficulty;

		// Token: 0x04000B29 RID: 2857
		private readonly List<Tuple<PersuasionOptionArgs, PersuasionOptionResult>> _chosenOptions;

		// Token: 0x04000B2A RID: 2858
		public readonly float GoalValue;
	}
}
