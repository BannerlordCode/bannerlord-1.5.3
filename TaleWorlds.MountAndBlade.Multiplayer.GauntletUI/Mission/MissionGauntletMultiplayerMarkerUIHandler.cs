using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.FlagMarker;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x02000015 RID: 21
	[OverrideView(typeof(MissionMultiplayerMarkerUIHandler))]
	public class MissionGauntletMultiplayerMarkerUIHandler : MissionView
	{
		// Token: 0x060000F4 RID: 244 RVA: 0x000065B4 File Offset: 0x000047B4
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._dataSource = new MultiplayerMissionMarkerVM(base.MissionScreen.CombatCamera);
			this._gauntletLayer = new GauntletLayer("MPMissionMarkers", 1, false);
			this._gauntletLayer.LoadMovie("MPMissionMarkers", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00006617 File Offset: 0x00004817
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000664C File Offset: 0x0000484C
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (GameNetwork.IsMultiplayer && MultiplayerSpectatorHelper.IsLocalPeerSpectator())
			{
				if (base.Input.IsGameKeyPressed(5))
				{
					this._spectatorMarkersToggled = !this._spectatorMarkersToggled;
				}
				this._dataSource.IsEnabled = this._spectatorMarkersToggled;
			}
			else
			{
				this._spectatorMarkersToggled = false;
				if (base.Input.IsGameKeyDown(5))
				{
					this._dataSource.IsEnabled = true;
				}
				else
				{
					this._dataSource.IsEnabled = false;
				}
			}
			this._dataSource.Tick(dt);
		}

		// Token: 0x04000067 RID: 103
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000068 RID: 104
		private MultiplayerMissionMarkerVM _dataSource;

		// Token: 0x04000069 RID: 105
		private bool _spectatorMarkersToggled;
	}
}
