using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Armory
{
	// Token: 0x020000B8 RID: 184
	public class MultiplayerLobbyArmoryCosmeticItemButtonWidget : ButtonWidget
	{
		// Token: 0x060009BE RID: 2494 RVA: 0x0001B8A3 File Offset: 0x00019AA3
		public MultiplayerLobbyArmoryCosmeticItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0001B8AC File Offset: 0x00019AAC
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (base.EventManager.HoveredWidget == this && Input.IsKeyPressed(InputKey.ControllerRUp))
			{
				this.OnMouseAlternatePressed();
				return;
			}
			if (base.EventManager.HoveredWidget == this && Input.IsKeyReleased(InputKey.ControllerRUp))
			{
				this.OnMouseAlternateReleased(true);
			}
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x0001B904 File Offset: 0x00019B04
		private void UpdateSelectableState()
		{
			this._selectableTimer = 0f;
			base.IsDisabled = !this.IsSelectable;
			this._animationStartAlpha = (this.IsSelectable ? this.NonSelectableStateAlpha : this.SelectableStateAlpha);
			this._animationTargetAlpha = (this.IsSelectable ? this.SelectableStateAlpha : this.NonSelectableStateAlpha);
			base.EventManager.AddLateUpdateAction(this, new Action<float>(this.AnimateSelectableState), 1);
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x0001B97C File Offset: 0x00019B7C
		private void AnimateSelectableState(float dt)
		{
			this._selectableTimer += dt;
			float num;
			if (this._selectableTimer < this.SelectableStateAnimationDuration)
			{
				num = this._selectableTimer / this.SelectableStateAnimationDuration;
				base.EventManager.AddLateUpdateAction(this, new Action<float>(this.AnimateSelectableState), 1);
			}
			else
			{
				num = 1f;
			}
			float num2 = MathF.Lerp(this._animationStartAlpha, this._animationTargetAlpha, num, 1E-05f);
			base.IsVisible = num2 != 0f;
			this.SetGlobalAlphaRecursively(num2);
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0001BA04 File Offset: 0x00019C04
		protected override void HandleClick()
		{
			base.HandleClick();
			if (this.IsUnlocked)
			{
				this.HandleSoundEvent();
				return;
			}
			base.EventFired("Obtain", Array.Empty<object>());
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x0001BA2B File Offset: 0x00019C2B
		protected override void HandleAlternateClick()
		{
			base.HandleAlternateClick();
			this.HandleSoundEvent();
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0001BA3C File Offset: 0x00019C3C
		private void HandleSoundEvent()
		{
			int itemType = this.ItemType;
			switch (itemType)
			{
			case 12:
				base.EventFired("WearHelmet", Array.Empty<object>());
				return;
			case 13:
				base.EventFired("WearArmorBig", Array.Empty<object>());
				return;
			case 14:
			case 15:
				break;
			default:
				if (itemType != 22)
				{
					base.EventFired("WearGeneric", Array.Empty<object>());
					return;
				}
				break;
			}
			base.EventFired("WearArmorSmall", Array.Empty<object>());
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x060009C5 RID: 2501 RVA: 0x0001BAB3 File Offset: 0x00019CB3
		// (set) Token: 0x060009C6 RID: 2502 RVA: 0x0001BABB File Offset: 0x00019CBB
		public int ItemType
		{
			get
			{
				return this._itemType;
			}
			set
			{
				if (value != this._itemType)
				{
					this._itemType = value;
					base.OnPropertyChanged(value, "ItemType");
				}
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x060009C7 RID: 2503 RVA: 0x0001BAD9 File Offset: 0x00019CD9
		// (set) Token: 0x060009C8 RID: 2504 RVA: 0x0001BAE1 File Offset: 0x00019CE1
		public bool IsUnlocked
		{
			get
			{
				return this._isUnlocked;
			}
			set
			{
				if (value != this._isUnlocked)
				{
					this._isUnlocked = value;
					base.OnPropertyChanged(value, "IsUnlocked");
				}
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x060009C9 RID: 2505 RVA: 0x0001BAFF File Offset: 0x00019CFF
		// (set) Token: 0x060009CA RID: 2506 RVA: 0x0001BB07 File Offset: 0x00019D07
		public float SelectableStateAnimationDuration
		{
			get
			{
				return this._selectableStateAnimationDuration;
			}
			set
			{
				if (value != this._selectableStateAnimationDuration)
				{
					this._selectableStateAnimationDuration = value;
					base.OnPropertyChanged(value, "SelectableStateAnimationDuration");
				}
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x060009CB RID: 2507 RVA: 0x0001BB25 File Offset: 0x00019D25
		// (set) Token: 0x060009CC RID: 2508 RVA: 0x0001BB2D File Offset: 0x00019D2D
		public float SelectableStateAlpha
		{
			get
			{
				return this._selectableStateAlpha;
			}
			set
			{
				if (value != this._selectableStateAlpha)
				{
					this._selectableStateAlpha = value;
					base.OnPropertyChanged(value, "SelectableStateAlpha");
				}
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x060009CD RID: 2509 RVA: 0x0001BB4B File Offset: 0x00019D4B
		// (set) Token: 0x060009CE RID: 2510 RVA: 0x0001BB53 File Offset: 0x00019D53
		public float NonSelectableStateAlpha
		{
			get
			{
				return this._nonSelectableStateAlpha;
			}
			set
			{
				if (value != this._nonSelectableStateAlpha)
				{
					this._nonSelectableStateAlpha = value;
					base.OnPropertyChanged(value, "NonSelectableStateAlpha");
				}
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x060009CF RID: 2511 RVA: 0x0001BB71 File Offset: 0x00019D71
		// (set) Token: 0x060009D0 RID: 2512 RVA: 0x0001BB79 File Offset: 0x00019D79
		public bool IsSelectable
		{
			get
			{
				return this._isSelectable;
			}
			set
			{
				if (value != this._isSelectable)
				{
					this._isSelectable = value;
					base.OnPropertyChanged(value, "IsSelectable");
					this.UpdateSelectableState();
				}
			}
		}

		// Token: 0x04000465 RID: 1125
		private float _selectableTimer;

		// Token: 0x04000466 RID: 1126
		private float _animationTargetAlpha;

		// Token: 0x04000467 RID: 1127
		private float _animationStartAlpha;

		// Token: 0x04000468 RID: 1128
		private int _itemType;

		// Token: 0x04000469 RID: 1129
		private bool _isUnlocked;

		// Token: 0x0400046A RID: 1130
		private float _selectableStateAnimationDuration;

		// Token: 0x0400046B RID: 1131
		private float _selectableStateAlpha;

		// Token: 0x0400046C RID: 1132
		private float _nonSelectableStateAlpha;

		// Token: 0x0400046D RID: 1133
		private bool _isSelectable;
	}
}
