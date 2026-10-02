using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Missions.BattleScore;

namespace SandBox.Missions.BattleScore
{
	// Token: 0x020000A2 RID: 162
	public class SandboxSimulationBattleScoreContext : BattleScoreContext
	{
		// Token: 0x060006AE RID: 1710 RVA: 0x0002CF3F File Offset: 0x0002B13F
		public SandboxSimulationBattleScoreContext(BattleSimulation battleSimulation)
		{
			this._battleSimulation = battleSimulation;
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x0002CF4E File Offset: 0x0002B14E
		public override bool IsPowerComparisonRelevant
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0002CF51 File Offset: 0x0002B151
		public override Banner GetAttackerBanner()
		{
			return this._battleSimulation.MapEvent.AttackerSide.LeaderParty.Banner;
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x0002CF6D File Offset: 0x0002B16D
		public override Banner GetDefenderBanner()
		{
			return this._battleSimulation.MapEvent.DefenderSide.LeaderParty.Banner;
		}

		// Token: 0x04000393 RID: 915
		private readonly BattleSimulation _battleSimulation;
	}
}
