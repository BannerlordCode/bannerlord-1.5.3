using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer;
using TaleWorlds.MountAndBlade.ViewModelCollection;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission.Singleplayer
{
	// Token: 0x0200003A RID: 58
	[OverrideView(typeof(MissionLeaveView))]
	public class MissionGauntletLeaveView : MissionView
	{
		// Token: 0x060002A6 RID: 678 RVA: 0x0000FA48 File Offset: 0x0000DC48
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._dataSource = new MissionLeaveVM(new Func<float>(base.Mission.GetMissionEndTimerValue), new Func<float>(base.Mission.GetMissionEndTimeInSeconds));
			this._gauntletLayer = new GauntletLayer("MissionLeave", 47, false);
			this._gauntletLayer.LoadMovie("LeaveUI", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000FAC3 File Offset: 0x0000DCC3
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000FAE4 File Offset: 0x0000DCE4
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			this._dataSource.Tick(dt);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000FAF9 File Offset: 0x0000DCF9
		private void OnEscapeMenuToggled(bool isOpened)
		{
			ScreenManager.SetSuspendLayer(this._gauntletLayer, !isOpened);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000FB0A File Offset: 0x0000DD0A
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 0f;
			}
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000FB2F File Offset: 0x0000DD2F
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 1f;
			}
		}

		// Token: 0x04000157 RID: 343
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000158 RID: 344
		private MissionLeaveVM _dataSource;
	}
}
