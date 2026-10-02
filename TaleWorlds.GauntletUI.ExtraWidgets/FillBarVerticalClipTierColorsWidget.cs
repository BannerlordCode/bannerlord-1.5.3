using System;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.ExtraWidgets
{
	// Token: 0x0200000A RID: 10
	public class FillBarVerticalClipTierColorsWidget : FillBarVerticalWidget
	{
		// Token: 0x06000077 RID: 119 RVA: 0x00003200 File Offset: 0x00001400
		public FillBarVerticalClipTierColorsWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000322C File Offset: 0x0000142C
		protected override void OnRender(TwoDimensionContext twoDimensionContext, TwoDimensionDrawContext drawContext)
		{
			base.OnRender(twoDimensionContext, drawContext);
			float num = (float)base.InitialAmount / base.MaxAmountAsFloat;
			Color color = new Color(0f, 0f, 0f, 0f);
			if (num == 1f)
			{
				base.FillWidget.Color = Color.ConvertStringToColor(this.MaxedColor);
				return;
			}
			float num2 = this._maxThreshold;
			float num3 = this._maxThreshold;
			Color color2 = Color.ConvertStringToColor(this.MaxedColor);
			Color color3 = Color.ConvertStringToColor(this.MaxedColor);
			if (num >= this._highThreshold && num < this._maxThreshold)
			{
				num2 = this._highThreshold;
				num3 = this._maxThreshold;
				color2 = Color.ConvertStringToColor(this.HighColor);
				color3 = Color.ConvertStringToColor(this.MaxedColor);
			}
			else if (num >= this._mediumThreshold && num < this._highThreshold)
			{
				num2 = this._mediumThreshold;
				num3 = this._highThreshold;
				color2 = Color.ConvertStringToColor(this.MediumColor);
				color3 = Color.ConvertStringToColor(this.HighColor);
			}
			else if (num >= this._lowThreshold && num < this._mediumThreshold)
			{
				num2 = this._lowThreshold;
				num3 = this._mediumThreshold;
				color2 = Color.ConvertStringToColor(this.LowColor);
				color3 = Color.ConvertStringToColor(this.MediumColor);
			}
			float num4 = (num - num2) / (num3 - num2);
			color = Color.Lerp(color2, color3, num4);
			base.FillWidget.Color = color;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00003385 File Offset: 0x00001585
		// (set) Token: 0x0600007A RID: 122 RVA: 0x0000338D File Offset: 0x0000158D
		[Editor(false)]
		public string MaxedColor
		{
			get
			{
				return this._maxedColor;
			}
			set
			{
				if (value != this._maxedColor)
				{
					this._maxedColor = value;
					base.OnPropertyChanged<string>(value, "MaxedColor");
				}
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600007B RID: 123 RVA: 0x000033B0 File Offset: 0x000015B0
		// (set) Token: 0x0600007C RID: 124 RVA: 0x000033B8 File Offset: 0x000015B8
		[Editor(false)]
		public string HighColor
		{
			get
			{
				return this._highColor;
			}
			set
			{
				if (value != this._highColor)
				{
					this._highColor = value;
					base.OnPropertyChanged<string>(value, "HighColor");
				}
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600007D RID: 125 RVA: 0x000033DB File Offset: 0x000015DB
		// (set) Token: 0x0600007E RID: 126 RVA: 0x000033E3 File Offset: 0x000015E3
		[Editor(false)]
		public string MediumColor
		{
			get
			{
				return this._mediumColor;
			}
			set
			{
				if (value != this._mediumColor)
				{
					this._mediumColor = value;
					base.OnPropertyChanged<string>(value, "MediumColor");
				}
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00003406 File Offset: 0x00001606
		// (set) Token: 0x06000080 RID: 128 RVA: 0x0000340E File Offset: 0x0000160E
		[Editor(false)]
		public string LowColor
		{
			get
			{
				return this._lowColor;
			}
			set
			{
				if (value != this._lowColor)
				{
					this._lowColor = value;
					base.OnPropertyChanged<string>(value, "LowColor");
				}
			}
		}

		// Token: 0x04000036 RID: 54
		private readonly float _maxThreshold = 1f;

		// Token: 0x04000037 RID: 55
		private readonly float _highThreshold = 0.6f;

		// Token: 0x04000038 RID: 56
		private readonly float _mediumThreshold = 0.35f;

		// Token: 0x04000039 RID: 57
		private readonly float _lowThreshold;

		// Token: 0x0400003A RID: 58
		private string _maxedColor;

		// Token: 0x0400003B RID: 59
		private string _highColor;

		// Token: 0x0400003C RID: 60
		private string _mediumColor;

		// Token: 0x0400003D RID: 61
		private string _lowColor;
	}
}
