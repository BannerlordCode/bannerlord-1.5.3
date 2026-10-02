using System;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.MountAndBlade.ViewModelCollection.ProfileSelection;
using TaleWorlds.PlatformService;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI
{
	// Token: 0x02000017 RID: 23
	[GameStateScreen(typeof(ProfileSelectionState))]
	public class GauntletProfileSelectionScreen : MBProfileSelectionScreenBase
	{
		// Token: 0x060000DC RID: 220 RVA: 0x00007253 File Offset: 0x00005453
		public GauntletProfileSelectionScreen(ProfileSelectionState state)
			: base(state)
		{
			this._state = state;
			this._state.OnProfileSelection += this.OnProfileSelection;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000727A File Offset: 0x0000547A
		private void OnProfileSelection()
		{
			ProfileSelectionVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.OnActivate(this._state.IsDirectPlayPossible);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00007298 File Offset: 0x00005498
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this._gauntletLayer = new GauntletLayer("ProfileSelection", 1, false);
			this._dataSource = new ProfileSelectionVM(this._state.IsDirectPlayPossible);
			ProfileSelectionVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnActivate(this._state.IsDirectPlayPossible);
			}
			this._gauntletLayer.LoadMovie("ProfileSelectionScreen", this._dataSource);
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this._gauntletLayer.IsFocusLayer = true;
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			ScreenManager.TrySetFocus(this._gauntletLayer);
			base.AddLayer(this._gauntletLayer);
			MouseManager.ShowCursor(false);
			MouseManager.ShowCursor(true);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00007361 File Offset: 0x00005561
		protected override void OnActivate()
		{
			base.OnActivate();
			ProfileSelectionVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.OnActivate(this._state.IsDirectPlayPossible);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00007384 File Offset: 0x00005584
		protected override void OnFinalize()
		{
			base.OnFinalize();
			this._state.OnProfileSelection -= this.OnProfileSelection;
			this._gauntletLayer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(this._gauntletLayer);
			base.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000073EC File Offset: 0x000055EC
		protected override void OnProfileSelectionTick(float dt)
		{
			base.OnProfileSelectionTick(dt);
			if (!this._state.IsDirectPlayPossible || !this._gauntletLayer.Input.IsHotKeyReleased("Play"))
			{
				if (this._gauntletLayer.Input.IsHotKeyReleased("SelectProfile"))
				{
					base.OnActivateProfileSelection();
				}
				return;
			}
			if (PlatformServices.Instance.UserLoggedIn)
			{
				this._state.StartGame();
				return;
			}
			base.OnActivateProfileSelection();
		}

		// Token: 0x0400008B RID: 139
		private GauntletLayer _gauntletLayer;

		// Token: 0x0400008C RID: 140
		private ProfileSelectionVM _dataSource;

		// Token: 0x0400008D RID: 141
		private ProfileSelectionState _state;
	}
}
