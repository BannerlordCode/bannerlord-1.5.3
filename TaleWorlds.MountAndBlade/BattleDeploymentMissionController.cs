using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Missions.Handlers;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200027C RID: 636
	public class BattleDeploymentMissionController : DeploymentMissionController
	{
		// Token: 0x060023C4 RID: 9156 RVA: 0x0007F4CB File Offset: 0x0007D6CB
		public BattleDeploymentMissionController(bool isPlayerAttacker)
			: base(isPlayerAttacker)
		{
		}

		// Token: 0x060023C5 RID: 9157 RVA: 0x0007F4D4 File Offset: 0x0007D6D4
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._battleDeploymentHandler = base.Mission.GetMissionBehavior<BattleDeploymentHandler>();
			this.MissionAgentSpawnLogic = base.Mission.GetMissionBehavior<DefaultBattleMissionAgentSpawnLogic>();
		}

		// Token: 0x060023C6 RID: 9158 RVA: 0x0007F500 File Offset: 0x0007D700
		protected override void OnAfterStart()
		{
			for (int i = 0; i < 2; i++)
			{
				this.MissionAgentSpawnLogic.SetSpawnTroops((BattleSideEnum)i, false, false);
			}
			this.MissionAgentSpawnLogic.SetReinforcementsSpawnEnabled(false, true);
		}

		// Token: 0x060023C7 RID: 9159 RVA: 0x0007F534 File Offset: 0x0007D734
		protected override void OnSetupTeamsOfSide(BattleSideEnum battleSide)
		{
			this.MissionAgentSpawnLogic.SetSpawnTroops(battleSide, true, true);
			base.SetupAgentAIStatesForSide(battleSide);
			this.MissionAgentSpawnLogic.OnSideDeploymentOver(battleSide);
		}

		// Token: 0x060023C8 RID: 9160 RVA: 0x0007F557 File Offset: 0x0007D757
		protected override void OnSetupTeamsFinished()
		{
			this._battleDeploymentHandler.HandleGeneralsDeploymentFrames();
			base.Mission.IsTeleportingAgents = true;
		}

		// Token: 0x060023C9 RID: 9161 RVA: 0x0007F570 File Offset: 0x0007D770
		protected override void BeforeDeploymentFinished()
		{
			base.Mission.IsTeleportingAgents = false;
		}

		// Token: 0x060023CA RID: 9162 RVA: 0x0007F57E File Offset: 0x0007D77E
		protected override void AfterDeploymentFinished()
		{
			this.MissionAgentSpawnLogic.SetReinforcementsSpawnEnabled(true, true);
			base.Mission.RemoveMissionBehavior(this._battleDeploymentHandler);
		}

		// Token: 0x04000DB8 RID: 3512
		protected DefaultBattleMissionAgentSpawnLogic MissionAgentSpawnLogic;

		// Token: 0x04000DB9 RID: 3513
		private BattleDeploymentHandler _battleDeploymentHandler;
	}
}
