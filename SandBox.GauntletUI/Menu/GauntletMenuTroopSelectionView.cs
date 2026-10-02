using System;
using SandBox.View.Map;
using SandBox.View.Menu;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TroopSelection;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Menu
{
	// Token: 0x0200002B RID: 43
	[OverrideView(typeof(MenuTroopSelectionView))]
	public class GauntletMenuTroopSelectionView : MenuView
	{
		// Token: 0x0600021A RID: 538 RVA: 0x0000D77F File Offset: 0x0000B97F
		public GauntletMenuTroopSelectionView(TroopRoster fullRoster, TroopRoster initialSelections, Func<CharacterObject, bool> canChangeStatusOfTroop, Action<TroopRoster> onDone, int maxSelectableTroopCount, int minSelectableTroopCount)
		{
			this._onDone = onDone;
			this._fullRoster = fullRoster;
			this._initialSelections = initialSelections;
			this._changeChangeStatusOfTroop = canChangeStatusOfTroop;
			this._maxSelectableTroopCount = maxSelectableTroopCount;
			this._minSelectableTroopCount = minSelectableTroopCount;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000D7B4 File Offset: 0x0000B9B4
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this._dataSource = new GameMenuTroopSelectionVM(this._fullRoster, this._initialSelections, this._changeChangeStatusOfTroop, new Action<TroopRoster>(this.OnDone), this._maxSelectableTroopCount, this._minSelectableTroopCount)
			{
				IsEnabled = true
			};
			this._dataSource.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			this._dataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this._dataSource.SetResetInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Reset"));
			base.Layer = new GauntletLayer("MapTroopSelection", 206, false);
			this._layerAsGauntletLayer = base.Layer as GauntletLayer;
			base.Layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			base.Layer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			base.Layer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory"));
			this._movie = this._layerAsGauntletLayer.LoadMovie("GameMenuTroopSelection", this._dataSource);
			base.Layer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(this._layerAsGauntletLayer);
			base.MenuViewContext.AddLayer(base.Layer);
			MapScreen mapScreen;
			if ((mapScreen = ScreenManager.TopScreen as MapScreen) != null)
			{
				mapScreen.SetIsInHideoutTroopManage(true);
			}
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000D926 File Offset: 0x0000BB26
		private void OnDone(TroopRoster obj)
		{
			MapScreen.Instance.SetIsInHideoutTroopManage(false);
			base.MenuViewContext.CloseTroopSelection();
			Action<TroopRoster> onDone = this._onDone;
			if (onDone == null)
			{
				return;
			}
			onDone.DynamicInvokeWithLog(new object[] { obj });
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000D95C File Offset: 0x0000BB5C
		protected override void OnFinalize()
		{
			base.Layer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(base.Layer);
			this._dataSource.OnFinalize();
			this._dataSource = null;
			this._layerAsGauntletLayer.ReleaseMovie(this._movie);
			base.MenuViewContext.RemoveLayer(base.Layer);
			this._movie = null;
			base.Layer = null;
			this._layerAsGauntletLayer = null;
			MapScreen.Instance.SetIsInHideoutTroopManage(false);
			base.OnFinalize();
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000D9DC File Offset: 0x0000BBDC
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			if (this._dataSource != null)
			{
				this._dataSource.IsFiveStackModifierActive = base.Layer.Input.IsHotKeyDown("FiveStackModifier");
				this._dataSource.IsEntireStackModifierActive = base.Layer.Input.IsHotKeyDown("EntireStackModifier");
			}
			ScreenLayer layer = base.Layer;
			if (layer != null && layer.Input.IsHotKeyPressed("Exit"))
			{
				UISoundsHelper.PlayUISound("event:/ui/default");
				this._dataSource.ExecuteCancel();
			}
			else
			{
				ScreenLayer layer2 = base.Layer;
				if (layer2 != null && layer2.Input.IsHotKeyPressed("Confirm") && this._dataSource.IsDoneEnabled)
				{
					UISoundsHelper.PlayUISound("event:/ui/default");
					this._dataSource.ExecuteDone();
				}
				else
				{
					ScreenLayer layer3 = base.Layer;
					if (layer3 != null && layer3.Input.IsHotKeyPressed("Reset"))
					{
						UISoundsHelper.PlayUISound("event:/ui/default");
						this._dataSource.ExecuteReset();
					}
				}
			}
			GameMenuTroopSelectionVM dataSource = this._dataSource;
			if (dataSource != null && !dataSource.IsEnabled)
			{
				base.MenuViewContext.CloseTroopSelection();
			}
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000DB04 File Offset: 0x0000BD04
		protected override void OnMapConversationActivated()
		{
			base.OnMapConversationActivated();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000DB20 File Offset: 0x0000BD20
		protected override void OnMapConversationDeactivated()
		{
			base.OnMapConversationDeactivated();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}

		// Token: 0x040000B2 RID: 178
		private readonly Action<TroopRoster> _onDone;

		// Token: 0x040000B3 RID: 179
		private readonly TroopRoster _fullRoster;

		// Token: 0x040000B4 RID: 180
		private readonly TroopRoster _initialSelections;

		// Token: 0x040000B5 RID: 181
		private readonly Func<CharacterObject, bool> _changeChangeStatusOfTroop;

		// Token: 0x040000B6 RID: 182
		private readonly int _maxSelectableTroopCount;

		// Token: 0x040000B7 RID: 183
		private readonly int _minSelectableTroopCount;

		// Token: 0x040000B8 RID: 184
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x040000B9 RID: 185
		private GameMenuTroopSelectionVM _dataSource;

		// Token: 0x040000BA RID: 186
		private GauntletMovieIdentifier _movie;
	}
}
