using System;
using StoryMode.Extensions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace StoryMode.GameComponents
{
	// Token: 0x0200003F RID: 63
	public class StoryModeCombatXpModel : CombatXpModel
	{
		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x000191DC File Offset: 0x000173DC
		public override float CaptainRadius
		{
			get
			{
				return base.BaseModel.CaptainRadius;
			}
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x000191E9 File Offset: 0x000173E9
		public override SkillObject GetSkillForWeapon(WeaponComponentData weapon, bool isSiegeEngineHit)
		{
			return base.BaseModel.GetSkillForWeapon(weapon, isSiegeEngineHit);
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x000191F8 File Offset: 0x000173F8
		public override ExplainedNumber GetXpFromHit(CharacterObject attackerTroop, CharacterObject captain, CharacterObject attackedTroop, PartyBase attackerParty, int damage, bool isFatal, CombatXpModel.MissionTypeEnum missionType)
		{
			if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.IsTrainingField())
			{
				return new ExplainedNumber(0f, false, null);
			}
			return base.BaseModel.GetXpFromHit(attackerTroop, captain, attackedTroop, attackerParty, damage, isFatal, missionType);
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00019230 File Offset: 0x00017430
		public override float GetXpMultiplierFromShotDifficulty(float shotDifficulty)
		{
			return base.BaseModel.GetXpMultiplierFromShotDifficulty(shotDifficulty);
		}
	}
}
