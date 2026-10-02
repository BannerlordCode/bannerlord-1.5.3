using System;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.TeamSelection;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x0200000C RID: 12
	[OverrideView(typeof(MultiplayerCultureSelectUIHandler))]
	public class MissionGauntletCultureSelection : MissionView
	{
		// Token: 0x060000A8 RID: 168 RVA: 0x00004EE0 File Offset: 0x000030E0
		public MissionGauntletCultureSelection()
		{
			this.ViewOrderPriority = 22;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004EF0 File Offset: 0x000030F0
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._missionLobbyComponent = base.Mission.GetMissionBehavior<MissionLobbyComponent>();
			this._missionLobbyComponent.OnCultureSelectionRequested += this.OnCultureSelectionRequested;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00004F20 File Offset: 0x00003120
		public override void OnMissionScreenFinalize()
		{
			this._missionLobbyComponent.OnCultureSelectionRequested -= this.OnCultureSelectionRequested;
			base.OnMissionScreenFinalize();
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004F3F File Offset: 0x0000313F
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this._toOpen && base.MissionScreen.SetDisplayDialog(true))
			{
				this._toOpen = false;
				this.OnOpen();
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004F6C File Offset: 0x0000316C
		private void OnOpen()
		{
			this._dataSource = new MultiplayerCultureSelectVM(new Action<BasicCultureObject>(this.OnCultureSelected), new Action(this.OnClose));
			this._gauntletLayer = new GauntletLayer("MultiplayerCultureSelection", this.ViewOrderPriority, false);
			this._gauntletLayer.LoadMovie("MultiplayerCultureSelection", this._dataSource);
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			base.MissionScreen.AddLayer(this._gauntletLayer);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00004FF0 File Offset: 0x000031F0
		private void OnClose()
		{
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			base.MissionScreen.SetDisplayDialog(false);
			this._gauntletLayer.InputRestrictions.ResetInputRestrictions();
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00005044 File Offset: 0x00003244
		private void OnCultureSelectionRequested()
		{
			this._toOpen = true;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000504D File Offset: 0x0000324D
		private void OnCultureSelected(BasicCultureObject culture)
		{
			this._missionLobbyComponent.OnCultureSelected(culture);
			this.OnClose();
		}

		// Token: 0x0400003B RID: 59
		private GauntletLayer _gauntletLayer;

		// Token: 0x0400003C RID: 60
		private MultiplayerCultureSelectVM _dataSource;

		// Token: 0x0400003D RID: 61
		private MissionLobbyComponent _missionLobbyComponent;

		// Token: 0x0400003E RID: 62
		private bool _toOpen;
	}
}
