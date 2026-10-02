using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.PlayerServices;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI
{
	// Token: 0x02000008 RID: 8
	public class MultiplayerReportPlayerScreen : GlobalLayer
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00004293 File Offset: 0x00002493
		// (set) Token: 0x0600007C RID: 124 RVA: 0x0000429A File Offset: 0x0000249A
		public static MultiplayerReportPlayerScreen Current { get; private set; }

		// Token: 0x0600007D RID: 125 RVA: 0x000042A4 File Offset: 0x000024A4
		public MultiplayerReportPlayerScreen()
		{
			this._dataSource = new MultiplayerReportPlayerVM(new Action<string, PlayerId, string, PlayerReportType, string>(this.OnReportDone), new Action(this.OnClose));
			this._dataSource.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			this._dataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			GauntletLayer gauntletLayer = new GauntletLayer("MultiplayerReportPlayer", 15350, false);
			gauntletLayer.LoadMovie("MultiplayerReportPlayer", this._dataSource);
			gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			base.Layer = gauntletLayer;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004358 File Offset: 0x00002558
		protected override void OnTick(float dt)
		{
			if (this._isActive)
			{
				if (base.Layer.Input.IsHotKeyReleased("Confirm"))
				{
					UISoundsHelper.PlayUISound("event:/ui/default");
					this._dataSource.ExecuteDone();
					return;
				}
				if (base.Layer.Input.IsHotKeyReleased("Exit"))
				{
					UISoundsHelper.PlayUISound("event:/ui/default");
					this._dataSource.ExecuteCancel();
				}
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000043C8 File Offset: 0x000025C8
		private void OnClose()
		{
			if (!this._isActive)
			{
				return;
			}
			this._isActive = false;
			base.Layer.InputRestrictions.ResetInputRestrictions();
			ScreenManager.SetSuspendLayer(base.Layer, true);
			base.Layer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(base.Layer);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004418 File Offset: 0x00002618
		public static void OnInitialize()
		{
			if (MultiplayerReportPlayerScreen.Current == null)
			{
				MultiplayerReportPlayerScreen.Current = new MultiplayerReportPlayerScreen();
				ScreenManager.AddGlobalLayer(MultiplayerReportPlayerScreen.Current, false);
				MultiplayerReportPlayerManager.ReportHandlers += MultiplayerReportPlayerScreen.Current.OnReportRequest;
				MultiplayerReportPlayerScreen.Current._isActive = false;
				ScreenManager.SetSuspendLayer(MultiplayerReportPlayerScreen.Current.Layer, true);
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004474 File Offset: 0x00002674
		public static void OnFinalize()
		{
			if (MultiplayerReportPlayerScreen.Current != null)
			{
				ScreenManager.RemoveGlobalLayer(MultiplayerReportPlayerScreen.Current, true);
				MultiplayerReportPlayerManager.ReportHandlers -= MultiplayerReportPlayerScreen.Current.OnReportRequest;
				MultiplayerReportPlayerScreen.Current._dataSource.OnFinalize();
				MultiplayerReportPlayerScreen.Current._dataSource = null;
				MultiplayerReportPlayerScreen.Current = null;
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000044C8 File Offset: 0x000026C8
		private void OnReportRequest(string gameId, PlayerId playerId, string playerName, bool isRequestedFromMission)
		{
			if (this._isActive)
			{
				return;
			}
			this._isActive = true;
			ScreenManager.SetSuspendLayer(base.Layer, false);
			base.Layer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(base.Layer);
			base.Layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this._dataSource.OpenNewReportWithGamePlayerId(gameId, playerId, playerName, isRequestedFromMission);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x0000452A File Offset: 0x0000272A
		private void OnReportDone(string gameId, PlayerId playerId, string playerName, PlayerReportType reportReason, string reasonText)
		{
			if (!this._isActive)
			{
				return;
			}
			this.OnClose();
			NetworkMain.GameClient.ReportPlayer(gameId, playerId, playerName, reportReason, reasonText);
			MultiplayerReportPlayerManager.OnPlayerReported(playerId);
		}

		// Token: 0x04000023 RID: 35
		private MultiplayerReportPlayerVM _dataSource;

		// Token: 0x04000024 RID: 36
		private bool _isActive;
	}
}
