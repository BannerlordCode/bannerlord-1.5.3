using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000033 RID: 51
	public class NavigationTargetSwitcher : Widget
	{
		// Token: 0x06000301 RID: 769 RVA: 0x00009893 File Offset: 0x00007A93
		public NavigationTargetSwitcher(UIContext context)
			: base(context)
		{
			base.WidthSizePolicy = SizePolicy.Fixed;
			base.HeightSizePolicy = SizePolicy.Fixed;
			base.SuggestedHeight = 0f;
			base.SuggestedWidth = 0f;
			base.IsVisible = false;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x000098C7 File Offset: 0x00007AC7
		private void OnFromTargetNavigationIndexUpdated(PropertyOwnerObject propertyOwner, string propertyName, int value)
		{
			if (propertyName == "GamepadNavigationIndex" && this.ToTarget != null)
			{
				this.TransferGamepadNavigation();
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x000098E4 File Offset: 0x00007AE4
		private void TransferGamepadNavigation()
		{
			if (!this._isTransferingNavigationIndices)
			{
				this._isTransferingNavigationIndices = true;
				int gamepadNavigationIndex = this.FromTarget.GamepadNavigationIndex;
				this.ToTarget.GamepadNavigationIndex = gamepadNavigationIndex;
				this.FromTarget.GamepadNavigationIndex = -1;
				if (this.FromTarget.OnGamepadNavigationFocusGained != null)
				{
					Widget toTarget = this.ToTarget;
					toTarget.OnGamepadNavigationFocusGained = (Action<Widget>)Delegate.Combine(toTarget.OnGamepadNavigationFocusGained, this.FromTarget.OnGamepadNavigationFocusGained);
					this.FromTarget.OnGamepadNavigationFocusGained = null;
				}
				this._isTransferingNavigationIndices = false;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000304 RID: 772 RVA: 0x0000996A File Offset: 0x00007B6A
		// (set) Token: 0x06000305 RID: 773 RVA: 0x00009972 File Offset: 0x00007B72
		public Widget ToTarget
		{
			get
			{
				return this._toTarget;
			}
			set
			{
				if (value != this._toTarget)
				{
					this._toTarget = value;
					if (this._toTarget != null && this.FromTarget != null && this.FromTarget.GamepadNavigationIndex != -1)
					{
						this.TransferGamepadNavigation();
					}
				}
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000306 RID: 774 RVA: 0x000099A8 File Offset: 0x00007BA8
		// (set) Token: 0x06000307 RID: 775 RVA: 0x000099B0 File Offset: 0x00007BB0
		public Widget FromTarget
		{
			get
			{
				return this._fromTarget;
			}
			set
			{
				if (value != this._fromTarget)
				{
					if (this._fromTarget != null)
					{
						this._fromTarget.intPropertyChanged -= this.OnFromTargetNavigationIndexUpdated;
					}
					this._fromTarget = value;
					this._fromTarget.intPropertyChanged += this.OnFromTargetNavigationIndexUpdated;
				}
			}
		}

		// Token: 0x04000136 RID: 310
		private bool _isTransferingNavigationIndices;

		// Token: 0x04000137 RID: 311
		private Widget _toTarget;

		// Token: 0x04000138 RID: 312
		private Widget _fromTarget;
	}
}
