using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x0200013C RID: 316
	public class InventoryArmorAnimationTextWidget : TextWidget
	{
		// Token: 0x06001092 RID: 4242 RVA: 0x0002D8BA File Offset: 0x0002BABA
		public InventoryArmorAnimationTextWidget(UIContext context)
			: base(context)
		{
			base.FloatText = 0f;
			this._isSettingInitialValue = true;
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x0002D8D5 File Offset: 0x0002BAD5
		private void HandleAnimation(float oldValue, float newValue)
		{
			if (!this._isSettingInitialValue)
			{
				if (oldValue > newValue)
				{
					this.SetState("Decrease");
					return;
				}
				if (oldValue < newValue)
				{
					this.SetState("Increase");
					return;
				}
				this.SetState("Default");
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06001094 RID: 4244 RVA: 0x0002D90A File Offset: 0x0002BB0A
		// (set) Token: 0x06001095 RID: 4245 RVA: 0x0002D912 File Offset: 0x0002BB12
		[Editor(false)]
		public float FloatAmount
		{
			get
			{
				return this._floatAmount;
			}
			set
			{
				if (this._floatAmount != value)
				{
					this.HandleAnimation(this._floatAmount, value);
					this._floatAmount = value;
					base.FloatText = this._floatAmount;
					base.OnPropertyChanged(value, "FloatAmount");
				}
				this._isSettingInitialValue = false;
			}
		}

		// Token: 0x0400078B RID: 1931
		private bool _isSettingInitialValue;

		// Token: 0x0400078C RID: 1932
		private float _floatAmount;
	}
}
