using System;

namespace TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer
{
	// Token: 0x0200008C RID: 140
	public class DeploymentView : MissionView
	{
		// Token: 0x06000553 RID: 1363 RVA: 0x00026FAC File Offset: 0x000251AC
		public override void AfterStart()
		{
			base.AfterStart();
			this._deploymentHandler = base.Mission.GetMissionBehavior<DeploymentHandler>();
			this.CreateWidgets();
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00026FCB File Offset: 0x000251CB
		public override void OnRemoveBehavior()
		{
			this.RemoveWidgets();
			base.OnRemoveBehavior();
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00026FD9 File Offset: 0x000251D9
		protected virtual void CreateWidgets()
		{
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00026FDB File Offset: 0x000251DB
		protected virtual void RemoveWidgets()
		{
		}

		// Token: 0x040002FB RID: 763
		private DeploymentHandler _deploymentHandler;
	}
}
