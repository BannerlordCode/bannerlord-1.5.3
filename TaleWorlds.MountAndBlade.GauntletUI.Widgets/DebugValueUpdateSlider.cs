using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000015 RID: 21
	public class DebugValueUpdateSlider : SliderWidget
	{
		// Token: 0x06000123 RID: 291 RVA: 0x000051CE File Offset: 0x000033CE
		public DebugValueUpdateSlider(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000051D7 File Offset: 0x000033D7
		protected override void OnValueIntChanged(int value)
		{
			base.OnValueIntChanged(value);
			this.OnValueChanged((float)value);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000051E8 File Offset: 0x000033E8
		protected override void OnValueFloatChanged(float value)
		{
			base.OnValueFloatChanged(value);
			this.OnValueChanged(value);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000051F8 File Offset: 0x000033F8
		private void OnValueChanged(float value)
		{
			if (this.WidgetToUpdate != null)
			{
				this.WidgetToUpdate.Text = this.WidgetToUpdate.GlobalPosition.Y.ToString("F0");
			}
			if (this.ValueToUpdate != null)
			{
				this.ValueToUpdate.InitialAmount = (int)value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000127 RID: 295 RVA: 0x0000524A File Offset: 0x0000344A
		// (set) Token: 0x06000128 RID: 296 RVA: 0x00005252 File Offset: 0x00003452
		public TextWidget WidgetToUpdate { get; set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000129 RID: 297 RVA: 0x0000525B File Offset: 0x0000345B
		// (set) Token: 0x0600012A RID: 298 RVA: 0x00005263 File Offset: 0x00003463
		public FillBarVerticalWidget ValueToUpdate { get; set; }
	}
}
