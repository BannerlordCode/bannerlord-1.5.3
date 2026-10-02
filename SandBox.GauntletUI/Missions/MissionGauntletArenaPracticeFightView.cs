using System;
using SandBox.Missions.MissionLogics.Arena;
using SandBox.View.Missions;
using SandBox.ViewModelCollection.Missions;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace SandBox.GauntletUI.Missions
{
	// Token: 0x0200001A RID: 26
	[OverrideView(typeof(MissionArenaPracticeFightView))]
	public class MissionGauntletArenaPracticeFightView : MissionView
	{
		// Token: 0x0600017A RID: 378 RVA: 0x0000A7AC File Offset: 0x000089AC
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			ArenaPracticeFightMissionController missionBehavior = base.Mission.GetMissionBehavior<ArenaPracticeFightMissionController>();
			this._dataSource = new MissionArenaPracticeFightVM(missionBehavior);
			this._gauntletLayer = new GauntletLayer("MissionArenaPracticeFight", this.ViewOrderPriority, false);
			this._movie = this._gauntletLayer.LoadMovie("ArenaPracticeFight", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000A81B File Offset: 0x00008A1B
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			this._dataSource.Tick();
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000A82F File Offset: 0x00008A2F
		public override void OnMissionScreenFinalize()
		{
			this._dataSource.OnFinalize();
			this._gauntletLayer.ReleaseMovie(this._movie);
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			base.OnMissionScreenFinalize();
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000A864 File Offset: 0x00008A64
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 0f;
			}
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000A889 File Offset: 0x00008A89
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 1f;
			}
		}

		// Token: 0x04000073 RID: 115
		private MissionArenaPracticeFightVM _dataSource;

		// Token: 0x04000074 RID: 116
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000075 RID: 117
		private GauntletMovieIdentifier _movie;
	}
}
