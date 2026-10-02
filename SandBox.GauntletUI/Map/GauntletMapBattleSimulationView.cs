using System;
using SandBox.View.Map;
using SandBox.ViewModelCollection;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.ViewModelCollection.Scoreboard;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x02000030 RID: 48
	[OverrideView(typeof(BattleSimulationMapView))]
	public class GauntletMapBattleSimulationView : MapView
	{
		// Token: 0x0600024D RID: 589 RVA: 0x0000E943 File Offset: 0x0000CB43
		public GauntletMapBattleSimulationView(SPScoreboardVM dataSource)
		{
			this._dataSource = dataSource;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000E952 File Offset: 0x0000CB52
		protected override void OnMapConversationStart()
		{
			base.OnMapConversationStart();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000E96E File Offset: 0x0000CB6E
		protected override void OnMapConversationOver()
		{
			base.OnMapConversationOver();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000E98C File Offset: 0x0000CB8C
		protected override void CreateLayout()
		{
			base.CreateLayout();
			this._dataSource.Initialize(null, null, new Action(base.MapState.EndBattleSimulation), null);
			this._dataSource.SetShortcuts(new ScoreboardHotkeys
			{
				ShowMouseHotkey = null,
				ShowScoreboardHotkey = null,
				DoneInputKey = HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"),
				FastForwardKey = HotKeyManager.GetCategory("ScoreboardHotKeyCategory").GetHotKey("ToggleFastForward"),
				PauseInputKey = HotKeyManager.GetCategory("ScoreboardHotKeyCategory").GetHotKey("TogglePause")
			});
			base.Layer = new GauntletLayer("MapBattleSimulation", 101, false);
			this._layerAsGauntletLayer = base.Layer as GauntletLayer;
			base.Layer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			base.Layer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("ScoreboardHotKeyCategory"));
			this._layerAsGauntletLayer.LoadMovie("SPScoreboard", this._dataSource);
			this._dataSource.ExecutePlayAction();
			base.Layer.IsFocusLayer = true;
			base.Layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			base.MapScreen.AddLayer(base.Layer);
			ScreenManager.TrySetFocus(base.Layer);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000EAE8 File Offset: 0x0000CCE8
		protected override void OnFinalize()
		{
			this._dataSource.OnFinalize();
			base.MapScreen.RemoveLayer(base.Layer);
			base.Layer.IsFocusLayer = false;
			base.Layer.InputRestrictions.ResetInputRestrictions();
			ScreenManager.TryLoseFocus(base.Layer);
			this._layerAsGauntletLayer = null;
			base.Layer = null;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000EB48 File Offset: 0x0000CD48
		protected override void OnMapScreenUpdate(float dt)
		{
			base.OnMapScreenUpdate(dt);
			if (this._dataSource != null && base.Layer != null)
			{
				this._dataSource.Tick(dt);
				if (!this._dataSource.IsOver && base.Layer.Input.IsHotKeyReleased("ToggleFastForward"))
				{
					this._dataSource.IsFastForwarding = !this._dataSource.IsFastForwarding;
					this._dataSource.ExecuteFastForwardAction();
					return;
				}
				if (!this._dataSource.IsOver && this._dataSource.IsSimulation && this._dataSource.ShowScoreboard && base.Layer.Input.IsHotKeyReleased("TogglePause"))
				{
					this._dataSource.IsPaused = !this._dataSource.IsPaused;
					this._dataSource.ExecutePauseSimulationAction();
					return;
				}
				if (this._dataSource.IsOver && this._dataSource.ShowScoreboard && base.Layer.Input.IsHotKeyPressed("Confirm"))
				{
					this._dataSource.ExecuteQuitAction();
				}
			}
		}

		// Token: 0x040000CF RID: 207
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x040000D0 RID: 208
		private readonly SPScoreboardVM _dataSource;
	}
}
