using System;
using TaleWorlds.MountAndBlade.View.MissionViews.Order;

namespace TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer
{
	// Token: 0x0200008A RID: 138
	public class DeploymentMissionView : MissionView
	{
		// Token: 0x0600054B RID: 1355 RVA: 0x00026ECE File Offset: 0x000250CE
		public override void AfterStart()
		{
			this._orderTroopPlacer = base.Mission.GetMissionBehavior<OrderTroopPlacer>();
			this._deploymentBoundaryMarkerHandler = base.Mission.GetMissionBehavior<MissionDeploymentBoundaryMarker>();
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00026EF2 File Offset: 0x000250F2
		public override void OnDeploymentPlanMade(Team team, bool isFirstPlan)
		{
			if (team == base.Mission.PlayerTeam && base.Mission.DeploymentPlan.HasDeploymentBoundaries(base.Mission.PlayerTeam))
			{
				OrderTroopPlacer orderTroopPlacer = this._orderTroopPlacer;
				if (orderTroopPlacer == null)
				{
					return;
				}
				orderTroopPlacer.RestrictOrdersToDeploymentBoundaries(true);
			}
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00026F30 File Offset: 0x00025130
		public override void OnDeploymentFinished()
		{
			if (this._deploymentBoundaryMarkerHandler != null)
			{
				if (base.Mission.DeploymentPlan.HasDeploymentBoundaries(base.Mission.PlayerTeam))
				{
					OrderTroopPlacer orderTroopPlacer = this._orderTroopPlacer;
					if (orderTroopPlacer != null)
					{
						orderTroopPlacer.RestrictOrdersToDeploymentBoundaries(false);
					}
				}
				base.Mission.RemoveMissionBehavior(this._deploymentBoundaryMarkerHandler);
			}
			if (!base.Mission.HasMissionBehavior<MissionBoundaryWallView>())
			{
				MissionBoundaryWallView missionBoundaryWallView = new MissionBoundaryWallView();
				base.MissionScreen.AddMissionView(missionBoundaryWallView);
			}
		}

		// Token: 0x040002F9 RID: 761
		protected OrderTroopPlacer _orderTroopPlacer;

		// Token: 0x040002FA RID: 762
		protected MissionDeploymentBoundaryMarker _deploymentBoundaryMarkerHandler;
	}
}
