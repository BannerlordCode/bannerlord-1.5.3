using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission.Singleplayer
{
	// Token: 0x02000036 RID: 54
	[OverrideView(typeof(MissionAgentLockVisualizerView))]
	public class MissionGauntletAgentLockVisualizerView : MissionBattleUIBaseView
	{
		// Token: 0x0600026E RID: 622 RVA: 0x0000E5A0 File Offset: 0x0000C7A0
		protected override void OnCreateView()
		{
			this._missionMainAgentController = base.Mission.GetMissionBehavior<MissionMainAgentController>();
			this._missionMainAgentController.OnLockedAgentChanged += this.OnLockedAgentChanged;
			this._missionMainAgentController.OnPotentialLockedAgentChanged += this.OnPotentialLockedAgentChanged;
			this._dataSource = new MissionAgentLockVisualizerVM();
			this._layer = new GauntletLayer("MissionAgentLockVisualizer", 10, false);
			this._layer.LoadMovie("AgentLockTargets", this._dataSource);
			base.MissionScreen.AddLayer(this._layer);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000E632 File Offset: 0x0000C832
		protected override void OnDestroyView()
		{
			base.MissionScreen.RemoveLayer(this._layer);
			this._dataSource.OnFinalize();
			this._dataSource = null;
			this._layer = null;
			this._missionMainAgentController = null;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000E665 File Offset: 0x0000C865
		protected override void OnSuspendView()
		{
			if (this._layer != null)
			{
				ScreenManager.SetSuspendLayer(this._layer, true);
			}
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000E67B File Offset: 0x0000C87B
		protected override void OnResumeView()
		{
			if (this._layer != null)
			{
				ScreenManager.SetSuspendLayer(this._layer, false);
			}
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000E691 File Offset: 0x0000C891
		private void OnPotentialLockedAgentChanged(Agent newPotentialAgent)
		{
			MissionAgentLockVisualizerVM dataSource = this._dataSource;
			if (dataSource != null && dataSource.IsEnabled)
			{
				this._dataSource.OnPossibleLockAgentChange(this._latestPotentialLockedAgent, newPotentialAgent);
				this._latestPotentialLockedAgent = newPotentialAgent;
			}
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000E6C0 File Offset: 0x0000C8C0
		private void OnLockedAgentChanged(Agent newAgent)
		{
			MissionAgentLockVisualizerVM dataSource = this._dataSource;
			if (dataSource != null && dataSource.IsEnabled)
			{
				this._dataSource.OnActiveLockAgentChange(this._latestLockedAgent, newAgent);
				this._latestLockedAgent = newAgent;
			}
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000E6F0 File Offset: 0x0000C8F0
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (base.IsViewCreated && this._dataSource != null)
			{
				this._dataSource.IsEnabled = this.IsMainAgentAvailable();
				if (this._dataSource.IsEnabled)
				{
					for (int i = 0; i < this._dataSource.AllTrackedAgents.Count; i++)
					{
						MissionAgentLockItemVM missionAgentLockItemVM = this._dataSource.AllTrackedAgents[i];
						float num = 0f;
						float num2 = 0f;
						float num3 = 0f;
						MBWindowManager.WorldToScreenInsideUsableArea(base.MissionScreen.CombatCamera, missionAgentLockItemVM.TrackedAgent.GetChestGlobalPosition(), ref num, ref num2, ref num3);
						missionAgentLockItemVM.Position = new Vec2(num, num2);
					}
				}
			}
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000E7A7 File Offset: 0x0000C9A7
		private bool IsMainAgentAvailable()
		{
			Agent main = Agent.Main;
			return main != null && main.IsActive();
		}

		// Token: 0x04000142 RID: 322
		private GauntletLayer _layer;

		// Token: 0x04000143 RID: 323
		private MissionAgentLockVisualizerVM _dataSource;

		// Token: 0x04000144 RID: 324
		private MissionMainAgentController _missionMainAgentController;

		// Token: 0x04000145 RID: 325
		private Agent _latestLockedAgent;

		// Token: 0x04000146 RID: 326
		private Agent _latestPotentialLockedAgent;
	}
}
