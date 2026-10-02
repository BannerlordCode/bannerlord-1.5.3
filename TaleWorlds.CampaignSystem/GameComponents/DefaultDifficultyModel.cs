using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000116 RID: 278
	public class DefaultDifficultyModel : DifficultyModel
	{
		// Token: 0x06001802 RID: 6146 RVA: 0x00071980 File Offset: 0x0006FB80
		public override float GetPlayerTroopsReceivedDamageMultiplier()
		{
			switch (CampaignOptions.PlayerTroopsReceivedDamage)
			{
			case CampaignOptions.Difficulty.VeryEasy:
				return 0.5f;
			case CampaignOptions.Difficulty.Easy:
				return 0.75f;
			case CampaignOptions.Difficulty.Realistic:
				return 1f;
			default:
				return 1f;
			}
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x000719C0 File Offset: 0x0006FBC0
		public override int GetPlayerRecruitSlotBonus()
		{
			switch (CampaignOptions.RecruitmentDifficulty)
			{
			case CampaignOptions.Difficulty.VeryEasy:
				return 2;
			case CampaignOptions.Difficulty.Easy:
				return 1;
			case CampaignOptions.Difficulty.Realistic:
				return 0;
			default:
				return 0;
			}
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x000719F0 File Offset: 0x0006FBF0
		public override float GetPlayerMapMovementSpeedBonusMultiplier()
		{
			switch (CampaignOptions.PlayerMapMovementSpeed)
			{
			case CampaignOptions.Difficulty.VeryEasy:
				return 0.1f;
			case CampaignOptions.Difficulty.Easy:
				return 0.05f;
			case CampaignOptions.Difficulty.Realistic:
				return 0f;
			default:
				return 0f;
			}
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x00071A30 File Offset: 0x0006FC30
		public override float GetStealthDifficultyMultiplier()
		{
			switch (CampaignOptions.StealthAndDisguiseDifficulty)
			{
			case CampaignOptions.Difficulty.VeryEasy:
				return 0.5f;
			case CampaignOptions.Difficulty.Easy:
				return 0.75f;
			case CampaignOptions.Difficulty.Realistic:
				return 1f;
			default:
				return 0f;
			}
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x00071A70 File Offset: 0x0006FC70
		public override float GetDisguiseDifficultyMultiplier()
		{
			switch (CampaignOptions.StealthAndDisguiseDifficulty)
			{
			case CampaignOptions.Difficulty.VeryEasy:
				return 0.4f;
			case CampaignOptions.Difficulty.Easy:
				return 1f;
			case CampaignOptions.Difficulty.Realistic:
				return 1.2f;
			default:
				return 0f;
			}
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x00071AB0 File Offset: 0x0006FCB0
		public override float GetCombatAIDifficultyMultiplier()
		{
			switch (CampaignOptions.CombatAIDifficulty)
			{
			case CampaignOptions.Difficulty.VeryEasy:
				return 0f;
			case CampaignOptions.Difficulty.Easy:
				return 0.5f;
			}
			return 1f;
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x00071AE8 File Offset: 0x0006FCE8
		public override float GetPersuasionBonusChance()
		{
			switch (CampaignOptions.PersuasionSuccessChance)
			{
			case CampaignOptions.Difficulty.VeryEasy:
				return 0.1f;
			case CampaignOptions.Difficulty.Easy:
				return 0.05f;
			case CampaignOptions.Difficulty.Realistic:
				return 0f;
			default:
				return 0f;
			}
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x00071B28 File Offset: 0x0006FD28
		public override float GetClanMemberDeathChanceMultiplier()
		{
			switch (CampaignOptions.ClanMemberDeathChance)
			{
			case CampaignOptions.Difficulty.VeryEasy:
				return -1f;
			case CampaignOptions.Difficulty.Easy:
				return -0.5f;
			case CampaignOptions.Difficulty.Realistic:
				return 0f;
			default:
				return 0f;
			}
		}
	}
}
