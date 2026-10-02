using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.MissionLogics;
using TaleWorlds.MountAndBlade.Missions.Objectives;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Objective;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Missions
{
	// Token: 0x02000004 RID: 4
	[OverrideView(typeof(MissionObjectiveView))]
	public class MissionGauntletObjectiveView : MissionObjectiveView
	{
		// Token: 0x06000004 RID: 4 RVA: 0x00002060 File Offset: 0x00000260
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._objectiveLogic = base.Mission.GetMissionBehavior<MissionObjectiveLogic>();
			if (this._objectiveLogic == null)
			{
				Debug.FailedAssert("Mission objective view is enabled but there is no objective logic in mission", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\Mission\\MissionGauntletObjectiveView.cs", "OnMissionScreenInitialize", 34);
				return;
			}
			this._dataSource = new MissionObjectiveVM(this._objectiveLogic, base.MissionScreen.CombatCamera);
			this._gauntletLayer = new GauntletLayer("MissionObjective", 1, false);
			this._gauntletLayer.LoadMovie("MissionObjectives", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020FC File Offset: 0x000002FC
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			if (this._gauntletLayer != null)
			{
				base.MissionScreen.RemoveLayer(this._gauntletLayer);
				this._gauntletLayer = null;
			}
			if (this._dataSource != null)
			{
				this._dataSource.OnFinalize();
				this._dataSource = null;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000214C File Offset: 0x0000034C
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this._objectiveLogic == null || this._gauntletLayer == null || this._dataSource == null)
			{
				return;
			}
			this.UpdateContextAlpha(dt);
			MissionObjective currentObjective = this._objectiveLogic.CurrentObjective;
			if (this._latestObjective != currentObjective)
			{
				this._latestObjective = currentObjective;
				this._dataSource.UpdateObjective(this._latestObjective);
			}
			this._dataSource.Tick(dt);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000021BC File Offset: 0x000003BC
		private void UpdateContextAlpha(float dt)
		{
			float num = (this._dataSource.IsEnabled ? 1f : 0f);
			float num2 = MathF.Clamp(dt * 6f, 0f, 1f);
			float num3 = this._gauntletLayer.UIContext.ContextAlpha;
			num3 = MathF.Lerp(num3, num, num2, 1E-05f);
			this._gauntletLayer.UIContext.ContextAlpha = num3;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000222A File Offset: 0x0000042A
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 0f;
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000224F File Offset: 0x0000044F
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 1f;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002274 File Offset: 0x00000474
		protected override void OnResumeView()
		{
			base.OnResumeView();
			ScreenManager.SetSuspendLayer(this._gauntletLayer, false);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002288 File Offset: 0x00000488
		protected override void OnSuspendView()
		{
			base.OnSuspendView();
			ScreenManager.SetSuspendLayer(this._gauntletLayer, true);
		}

		// Token: 0x04000001 RID: 1
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000002 RID: 2
		private MissionObjectiveVM _dataSource;

		// Token: 0x04000003 RID: 3
		private MissionObjectiveLogic _objectiveLogic;

		// Token: 0x04000004 RID: 4
		private MissionObjective _latestObjective;
	}
}
