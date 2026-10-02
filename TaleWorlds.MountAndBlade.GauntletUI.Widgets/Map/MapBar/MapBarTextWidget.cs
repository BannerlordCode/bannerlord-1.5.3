using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.MapBar
{
	// Token: 0x0200012E RID: 302
	public class MapBarTextWidget : TextWidget
	{
		// Token: 0x06000FEA RID: 4074 RVA: 0x0002C1A0 File Offset: 0x0002A3A0
		public MapBarTextWidget(UIContext context)
			: base(context)
		{
			base.intPropertyChanged += this.TextPropertyChanged;
			base.OverrideDefaultStateSwitchingEnabled = true;
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x0002C1CC File Offset: 0x0002A3CC
		private void TextPropertyChanged(PropertyOwnerObject widget, string propertyName, int propertyValue)
		{
			if (propertyName == "IntText")
			{
				if (this._prevValue != -99)
				{
					if (propertyValue - this._prevValue > 0)
					{
						if (base.CurrentState == "Positive")
						{
							base.BrushRenderer.RestartAnimation();
						}
						else
						{
							this.SetState("Positive");
						}
					}
					else if (propertyValue - this._prevValue < 0)
					{
						if (base.CurrentState == "Negative")
						{
							base.BrushRenderer.RestartAnimation();
						}
						else
						{
							this.SetState("Negative");
						}
					}
				}
				this._prevValue = propertyValue;
			}
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x0002C268 File Offset: 0x0002A468
		private void RefreshFontColor()
		{
			Color color;
			if (this.IsWarning)
			{
				color = this.WarningColor;
			}
			else
			{
				color = this.NormalColor;
			}
			foreach (Style style in base.Brush.Styles)
			{
				style.FontColor = color;
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06000FED RID: 4077 RVA: 0x0002C2D8 File Offset: 0x0002A4D8
		// (set) Token: 0x06000FEE RID: 4078 RVA: 0x0002C2E0 File Offset: 0x0002A4E0
		[Editor(false)]
		public bool IsWarning
		{
			get
			{
				return this._isWarning;
			}
			set
			{
				if (value != this._isWarning)
				{
					this._isWarning = value;
					base.OnPropertyChanged(value, "IsWarning");
					this.RefreshFontColor();
				}
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06000FEF RID: 4079 RVA: 0x0002C304 File Offset: 0x0002A504
		// (set) Token: 0x06000FF0 RID: 4080 RVA: 0x0002C30C File Offset: 0x0002A50C
		[Editor(false)]
		public Color NormalColor
		{
			get
			{
				return this._normalColor;
			}
			set
			{
				if (value != this._normalColor)
				{
					this._normalColor = value;
					base.OnPropertyChanged(value, "NormalColor");
					this.RefreshFontColor();
				}
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06000FF1 RID: 4081 RVA: 0x0002C335 File Offset: 0x0002A535
		// (set) Token: 0x06000FF2 RID: 4082 RVA: 0x0002C33D File Offset: 0x0002A53D
		[Editor(false)]
		public Color WarningColor
		{
			get
			{
				return this._warningColor;
			}
			set
			{
				if (value != this._warningColor)
				{
					this._warningColor = value;
					base.OnPropertyChanged(value, "WarningColor");
					this.RefreshFontColor();
				}
			}
		}

		// Token: 0x04000744 RID: 1860
		private int _prevValue = -99;

		// Token: 0x04000745 RID: 1861
		private bool _isWarning;

		// Token: 0x04000746 RID: 1862
		private Color _normalColor;

		// Token: 0x04000747 RID: 1863
		private Color _warningColor;
	}
}
