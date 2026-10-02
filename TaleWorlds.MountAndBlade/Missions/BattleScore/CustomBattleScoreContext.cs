using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.Missions.BattleScore
{
	// Token: 0x020003FA RID: 1018
	public class CustomBattleScoreContext : BattleScoreContext
	{
		// Token: 0x0600381B RID: 14363 RVA: 0x000E8FF6 File Offset: 0x000E71F6
		public CustomBattleScoreContext(Mission mission)
		{
			this._mission = mission;
		}

		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x0600381C RID: 14364 RVA: 0x000E9005 File Offset: 0x000E7205
		public override bool IsPowerComparisonRelevant
		{
			get
			{
				return this._mission.Mode != MissionMode.Deployment;
			}
		}

		// Token: 0x0600381D RID: 14365 RVA: 0x000E9018 File Offset: 0x000E7218
		public override Banner GetAttackerBanner()
		{
			return this.GetSideBannerInfo(BattleSideEnum.Attacker);
		}

		// Token: 0x0600381E RID: 14366 RVA: 0x000E9021 File Offset: 0x000E7221
		public override Banner GetDefenderBanner()
		{
			return this.GetSideBannerInfo(BattleSideEnum.Defender);
		}

		// Token: 0x0600381F RID: 14367 RVA: 0x000E902C File Offset: 0x000E722C
		private Banner GetSideBannerInfo(BattleSideEnum sideEnum)
		{
			MissionCombatantsLogic missionBehavior = this._mission.GetMissionBehavior<MissionCombatantsLogic>();
			if (missionBehavior == null)
			{
				return null;
			}
			return missionBehavior.GetBannerForSide(sideEnum);
		}

		// Token: 0x04001832 RID: 6194
		private readonly Mission _mission;
	}
}
