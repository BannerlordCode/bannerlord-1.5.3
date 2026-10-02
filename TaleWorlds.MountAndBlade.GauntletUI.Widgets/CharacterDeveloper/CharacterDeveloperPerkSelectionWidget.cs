using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterDeveloper
{
	// Token: 0x02000182 RID: 386
	public class CharacterDeveloperPerkSelectionWidget : Widget
	{
		// Token: 0x0600142E RID: 5166 RVA: 0x000372CC File Offset: 0x000354CC
		public CharacterDeveloperPerkSelectionWidget(UIContext context)
			: base(context)
		{
			base.IsVisible = false;
		}

		// Token: 0x0600142F RID: 5167 RVA: 0x000372E8 File Offset: 0x000354E8
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (base.IsVisible && this._latestMouseUpWidgetWhenActivated != base.EventManager.LatestMouseUpWidget && !base.CheckIsMyChildRecursive(base.EventManager.LatestMouseUpWidget))
			{
				this.Deactivate();
			}
			this.UpdatePosition();
		}

		// Token: 0x06001430 RID: 5168 RVA: 0x00037338 File Offset: 0x00035538
		private void UpdatePosition()
		{
			if (base.IsVisible && this._latestMouseUpWidgetWhenActivated != null)
			{
				float num = this._latestMouseUpWidgetWhenActivated.GlobalPosition.X + this._latestMouseUpWidgetWhenActivated.Size.X + this._distBetweenPerkItemsMultiplier * 2f * base._scaleToUse;
				float num2 = 0f;
				if (base.GetChild(0).ChildCount > 1)
				{
					PerkItemButtonWidget perkItemButtonWidget;
					if ((perkItemButtonWidget = this._latestMouseUpWidgetWhenActivated as PerkItemButtonWidget) != null)
					{
						if (perkItemButtonWidget.AlternativeType == 1)
						{
							num2 = this._latestMouseUpWidgetWhenActivated.GlobalPosition.Y + (this._latestMouseUpWidgetWhenActivated.Size.Y - 4f * base._scaleToUse) - base.Size.Y / 2f;
						}
						else if (perkItemButtonWidget.AlternativeType == 2)
						{
							num2 = this._latestMouseUpWidgetWhenActivated.GlobalPosition.Y - base.Size.Y / 2f;
						}
					}
				}
				else
				{
					num2 = this._latestMouseUpWidgetWhenActivated.GlobalPosition.Y + this._latestMouseUpWidgetWhenActivated.Size.Y / 2f - base.Size.Y / 2f;
				}
				base.ScaledPositionXOffset = MathF.Clamp(num, 0f, base.EventManager.PageSize.X - base.Size.X);
				base.ScaledPositionYOffset = MathF.Clamp(num2, 0f, base.EventManager.PageSize.Y - base.Size.Y);
			}
		}

		// Token: 0x06001431 RID: 5169 RVA: 0x000374C9 File Offset: 0x000356C9
		private void Activate()
		{
			if (this._latestMouseUpWidgetWhenActivated == null)
			{
				this._latestMouseUpWidgetWhenActivated = base.EventManager.LatestMouseDownWidget;
			}
			base.IsVisible = true;
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x000374EB File Offset: 0x000356EB
		private void Deactivate()
		{
			base.EventFired("Deactivate", Array.Empty<object>());
			base.IsVisible = false;
			this._latestMouseUpWidgetWhenActivated = null;
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06001433 RID: 5171 RVA: 0x0003750B File Offset: 0x0003570B
		// (set) Token: 0x06001434 RID: 5172 RVA: 0x00037513 File Offset: 0x00035713
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (this._isActive != value)
				{
					this._isActive = value;
					base.OnPropertyChanged(value, "IsActive");
					if (this._isActive)
					{
						this.Activate();
						return;
					}
					this.Deactivate();
				}
			}
		}

		// Token: 0x04000931 RID: 2353
		private float _distBetweenPerkItemsMultiplier = 16f;

		// Token: 0x04000932 RID: 2354
		private Widget _latestMouseUpWidgetWhenActivated;

		// Token: 0x04000933 RID: 2355
		private bool _isActive;
	}
}
