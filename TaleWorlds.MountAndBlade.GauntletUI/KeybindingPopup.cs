using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI
{
	// Token: 0x0200001B RID: 27
	public class KeybindingPopup
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000106 RID: 262 RVA: 0x0000849C File Offset: 0x0000669C
		// (set) Token: 0x06000107 RID: 263 RVA: 0x000084A4 File Offset: 0x000066A4
		public bool IsActive { get; private set; }

		// Token: 0x06000108 RID: 264 RVA: 0x000084AD File Offset: 0x000066AD
		public KeybindingPopup(Action<Key> onDone, ScreenBase targetScreen)
		{
			this._onDone = onDone;
			this._targetScreen = targetScreen;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000084C4 File Offset: 0x000066C4
		public void Tick()
		{
			if (!this.IsActive)
			{
				return;
			}
			if (Input.IsGamepadActive)
			{
				this.OnToggle(false);
				return;
			}
			if (!this._isActiveFirstFrame)
			{
				InputKey firstKeyReleasedInRange = (InputKey)Input.GetFirstKeyReleasedInRange(0);
				if (firstKeyReleasedInRange != InputKey.Invalid)
				{
					this._onDone(new Key(firstKeyReleasedInRange));
					return;
				}
			}
			else
			{
				this._isActiveFirstFrame = false;
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00008518 File Offset: 0x00006718
		public void OnToggle(bool isActive)
		{
			if (this.IsActive != isActive)
			{
				this.IsActive = isActive;
				if (this.IsActive)
				{
					this._gauntletLayer = new GauntletLayer("KeyBindingPopup", 4005, false);
					ScreenManager.TrySetFocus(this._gauntletLayer);
					this._gauntletLayer.IsFocusLayer = true;
					this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
					ScreenManager.SetSuspendLayer(this._gauntletLayer, false);
					this._dataSource = new KeybindingPopupVM(delegate
					{
						this.OnToggle(false);
					});
					this._movie = this._gauntletLayer.LoadMovie("KeybindingPopup", this._dataSource);
					this._targetScreen.AddLayer(this._gauntletLayer);
					this._isActiveFirstFrame = true;
					return;
				}
				ScreenManager.TryLoseFocus(this._gauntletLayer);
				this._gauntletLayer.IsFocusLayer = false;
				this._gauntletLayer.InputRestrictions.ResetInputRestrictions();
				ScreenManager.SetSuspendLayer(this._gauntletLayer, true);
				if (this._movie != null)
				{
					this._gauntletLayer.ReleaseMovie(this._movie);
					this._movie = null;
				}
				this._targetScreen.RemoveLayer(this._gauntletLayer);
				this._gauntletLayer = null;
				this._dataSource = null;
			}
		}

		// Token: 0x040000A5 RID: 165
		private bool _isActiveFirstFrame;

		// Token: 0x040000A6 RID: 166
		private GauntletLayer _gauntletLayer;

		// Token: 0x040000A7 RID: 167
		private GauntletMovieIdentifier _movie;

		// Token: 0x040000A8 RID: 168
		private ScreenBase _targetScreen;

		// Token: 0x040000A9 RID: 169
		private Action<Key> _onDone;

		// Token: 0x040000AA RID: 170
		private KeybindingPopupVM _dataSource;
	}
}
