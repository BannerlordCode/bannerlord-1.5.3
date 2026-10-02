using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterDeveloper
{
	// Token: 0x0200017F RID: 383
	public class CharacterDeveloperAttributeInspectionPopupWidget : Widget
	{
		// Token: 0x06001412 RID: 5138 RVA: 0x00036C80 File Offset: 0x00034E80
		public CharacterDeveloperAttributeInspectionPopupWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x00036C8C File Offset: 0x00034E8C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (base.ParentWidget.IsVisible && this._latestMouseUpWidgetWhenActivated != base.EventManager.LatestMouseUpWidget && !base.CheckIsMyChildRecursive(base.EventManager.LatestMouseUpWidget))
			{
				this.Deactivate();
			}
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x00036CD9 File Offset: 0x00034ED9
		private void Activate()
		{
			this._latestMouseUpWidgetWhenActivated = base.EventManager.LatestMouseDownWidget;
			base.ParentWidget.IsVisible = true;
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x00036CF8 File Offset: 0x00034EF8
		private void Deactivate()
		{
			base.EventFired("Deactivate", Array.Empty<object>());
			base.ParentWidget.IsVisible = false;
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x06001416 RID: 5142 RVA: 0x00036D16 File Offset: 0x00034F16
		// (set) Token: 0x06001417 RID: 5143 RVA: 0x00036D1E File Offset: 0x00034F1E
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

		// Token: 0x04000925 RID: 2341
		private Widget _latestMouseUpWidgetWhenActivated;

		// Token: 0x04000926 RID: 2342
		private bool _isActive;
	}
}
