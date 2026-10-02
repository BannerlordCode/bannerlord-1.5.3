using System;
using SandBox.View;
using SandBox.ViewModelCollection.SaveLoad;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace SandBox.GauntletUI
{
	// Token: 0x02000010 RID: 16
	[OverrideView(typeof(SaveLoadScreen))]
	public class GauntletSaveLoadScreen : ScreenBase
	{
		// Token: 0x060000C0 RID: 192 RVA: 0x000077EF File Offset: 0x000059EF
		public GauntletSaveLoadScreen(bool isSaving)
		{
			this._isSaving = isSaving;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00007800 File Offset: 0x00005A00
		protected override void OnInitialize()
		{
			base.OnInitialize();
			bool flag = GameStateManager.Current.GameStates.FirstOrDefaultQ<GameState>((GameState s) => s is MapState) != null;
			this._dataSource = new SaveLoadVM(this._isSaving, flag);
			this._dataSource.SetDeleteInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Delete"));
			this._dataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this._dataSource.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			if (Game.Current != null)
			{
				Game.Current.GameStateManager.RegisterActiveStateDisableRequest(this);
			}
			this._spriteCategory = UIResourceManager.LoadSpriteCategory("ui_saveload");
			this._gauntletLayer = new GauntletLayer("SaveLoadScreen", 1, true);
			this._gauntletLayer.LoadMovie("SaveLoadScreen", this._dataSource);
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			base.AddLayer(this._gauntletLayer);
			if (BannerlordConfig.ForceVSyncInMenus)
			{
				Utilities.SetForceVsync(true);
			}
			InformationManager.HideAllMessages();
			this._dataSource.LoadSavesAsync();
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00007948 File Offset: 0x00005B48
		protected override void OnPostFrameTick(float dt)
		{
			base.OnPostFrameTick(dt);
			this.UpdateInputRestrictions();
			this._dataSource.OnTick(dt);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00007964 File Offset: 0x00005B64
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			this.UpdateInputRestrictions();
			if (this._gauntletLayer.Input.IsHotKeyReleased("Exit"))
			{
				this._dataSource.ExecuteDone();
				UISoundsHelper.PlayUISound("event:/ui/panels/next");
				return;
			}
			if (this._gauntletLayer.Input.IsHotKeyPressed("Confirm") && !this._gauntletLayer.IsFocusedOnInput())
			{
				this._dataSource.ExecuteLoadSave();
				UISoundsHelper.PlayUISound("event:/ui/panels/next");
				return;
			}
			if (this._gauntletLayer.Input.IsHotKeyPressed("Delete") && !this._gauntletLayer.IsFocusedOnInput())
			{
				this._dataSource.DeleteSelectedSave();
				UISoundsHelper.PlayUISound("event:/ui/panels/next");
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00007A20 File Offset: 0x00005C20
		protected override void OnFinalize()
		{
			base.OnFinalize();
			if (Game.Current != null)
			{
				Game.Current.GameStateManager.UnregisterActiveStateDisableRequest(this);
			}
			base.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(this._gauntletLayer);
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
			this._spriteCategory.Unload();
			Utilities.SetForceVsync(false);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00007A98 File Offset: 0x00005C98
		private void UpdateInputRestrictions()
		{
			if (this._dataSource.IsBusyWithAnAction)
			{
				this._gauntletLayer.InputRestrictions.ResetInputRestrictions();
				this._gauntletLayer.InputRestrictions.SetMouseVisibility(true);
				this._gauntletLayer.IsFocusLayer = false;
				ScreenManager.TryLoseFocus(this._gauntletLayer);
				return;
			}
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this._gauntletLayer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(this._gauntletLayer);
		}

		// Token: 0x04000053 RID: 83
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000054 RID: 84
		private SaveLoadVM _dataSource;

		// Token: 0x04000055 RID: 85
		private SpriteCategory _spriteCategory;

		// Token: 0x04000056 RID: 86
		private readonly bool _isSaving;
	}
}
