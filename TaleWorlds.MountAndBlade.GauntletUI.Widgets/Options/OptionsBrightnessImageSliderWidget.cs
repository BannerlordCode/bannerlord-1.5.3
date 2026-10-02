using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Options
{
	// Token: 0x02000076 RID: 118
	public class OptionsBrightnessImageSliderWidget : SliderWidget
	{
		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x00012F5C File Offset: 0x0001115C
		// (set) Token: 0x06000662 RID: 1634 RVA: 0x00012F64 File Offset: 0x00011164
		public bool IsMax { get; set; }

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x00012F6D File Offset: 0x0001116D
		// (set) Token: 0x06000664 RID: 1636 RVA: 0x00012F75 File Offset: 0x00011175
		public Widget ImageWidget { get; set; }

		// Token: 0x06000665 RID: 1637 RVA: 0x00012F7E File Offset: 0x0001117E
		public OptionsBrightnessImageSliderWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00012F88 File Offset: 0x00011188
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._isInitialized)
			{
				float num;
				if (this.IsMax)
				{
					num = (float)(base.ValueInt - 1) * 0.003f + 1f;
				}
				else
				{
					num = (float)(base.ValueInt + 1) * 0.003f;
				}
				this.SetColorOfImage(MBMath.ClampFloat(num, 0f, 1f));
				this._isInitialized = true;
			}
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00012FF8 File Offset: 0x000111F8
		protected override void OnValueFloatChanged(float value)
		{
			base.OnValueFloatChanged(value);
			float num;
			if (this.IsMax)
			{
				num = (value - 1f) * 0.003f + 1f;
			}
			else
			{
				num = (value + 1f) * 0.003f;
			}
			this.SetColorOfImage(MBMath.ClampFloat(num, 0f, 1f));
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00013054 File Offset: 0x00011254
		private void SetColorOfImage(float value)
		{
			this.ImageWidget.Color = new Color(value, value, value, 1f);
		}

		// Token: 0x040002BA RID: 698
		private bool _isInitialized;
	}
}
