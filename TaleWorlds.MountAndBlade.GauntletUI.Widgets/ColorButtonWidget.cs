using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200000F RID: 15
	public class ColorButtonWidget : ButtonWidget
	{
		// Token: 0x060000B3 RID: 179 RVA: 0x00003C0C File Offset: 0x00001E0C
		public ColorButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003C18 File Offset: 0x00001E18
		private void ApplyStringColorToBrush(string color)
		{
			Color color2 = Color.ConvertStringToColor(color);
			foreach (Style style in base.Brush.Styles)
			{
				StyleLayer[] layers = style.GetLayers();
				for (int i = 0; i < layers.Length; i++)
				{
					layers[i].Color = color2;
				}
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00003C8C File Offset: 0x00001E8C
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x00003C94 File Offset: 0x00001E94
		[Editor(false)]
		public string ColorToApply
		{
			get
			{
				return this._colorToApply;
			}
			set
			{
				if (this._colorToApply != value)
				{
					this._colorToApply = value;
					base.OnPropertyChanged<string>(value, "ColorToApply");
					if (!string.IsNullOrEmpty(value))
					{
						this.ApplyStringColorToBrush(value);
					}
				}
			}
		}

		// Token: 0x04000057 RID: 87
		private string _colorToApply;
	}
}
