using System;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.ExtraWidgets
{
	// Token: 0x0200000C RID: 12
	public class FillBarVerticalWidget : Widget
	{
		// Token: 0x0600009C RID: 156 RVA: 0x00003762 File Offset: 0x00001962
		public FillBarVerticalWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600009D RID: 157 RVA: 0x0000376C File Offset: 0x0000196C
		protected override void OnRender(TwoDimensionContext twoDimensionContext, TwoDimensionDrawContext drawContext)
		{
			if (this.FillWidget != null)
			{
				float y = this.FillWidget.ParentWidget.Size.Y;
				float num = 0f;
				if (this._maxAmount != 0f)
				{
					num = Mathf.Clamp(Mathf.Clamp(this._initialAmount, 0f, this._maxAmount) / this._maxAmount, 0f, 1f);
				}
				float num2 = (this._isCurrentValueSet ? Mathf.Clamp(this._currentAmount - this._initialAmount, -this._maxAmount, this._maxAmount) : 0f);
				float num3 = 0f;
				if (this._maxAmount != 0f)
				{
					num3 = (this._isCurrentValueSet ? Mathf.Clamp(num2 / this._maxAmount, -1f, 1f) : 0f);
				}
				if (this.IsDirectionUpward)
				{
					this.FillWidget.VerticalAlignment = VerticalAlignment.Bottom;
					this.FillWidget.ScaledSuggestedHeight = num * (y - this.FillWidget.ScaledMarginTop - this.FillWidget.ScaledMarginBottom);
					if (this.ChangeWidget != null)
					{
						this.ChangeWidget.VerticalAlignment = VerticalAlignment.Bottom;
						this.ChangeWidget.ScaledSuggestedHeight = num3 * (y - this.ChangeWidget.ScaledMarginTop - this.ChangeWidget.ScaledMarginBottom);
						if (num3 >= 0f)
						{
							this.ChangeWidget.ScaledPositionYOffset = -this.FillWidget.ScaledSuggestedHeight;
							this.ChangeWidget.Color = new Color(1f, 1f, 1f, 1f);
						}
						else
						{
							this.ChangeWidget.ScaledPositionYOffset = -this.FillWidget.ScaledSuggestedHeight + this.ChangeWidget.ScaledSuggestedHeight;
							this.ChangeWidget.Color = new Color(1f, 0f, 0f, 1f);
						}
					}
				}
				else
				{
					this.FillWidget.VerticalAlignment = VerticalAlignment.Top;
					this.FillWidget.ScaledSuggestedHeight = num * (y - this.FillWidget.ScaledMarginTop - this.FillWidget.ScaledMarginBottom);
					if (this.ChangeWidget != null)
					{
						this.ChangeWidget.VerticalAlignment = VerticalAlignment.Top;
						this.ChangeWidget.ScaledSuggestedHeight = num3 * (y - this.ChangeWidget.ScaledMarginTop - this.ChangeWidget.ScaledMarginBottom);
						if (num3 >= 0f)
						{
							this.ChangeWidget.ScaledPositionYOffset = this.FillWidget.ScaledSuggestedHeight;
							this.ChangeWidget.Color = new Color(1f, 1f, 1f, 1f);
						}
						else
						{
							this.ChangeWidget.ScaledPositionYOffset = this.FillWidget.ScaledSuggestedHeight - this.ChangeWidget.ScaledSuggestedHeight;
							this.ChangeWidget.Color = new Color(1f, 0f, 0f, 1f);
						}
					}
				}
				if (this.ChangeWidget != null && this.DividerWidget != null)
				{
					this.DividerWidget.IsVisible = this.ChangeWidget != null && num3 != 0f;
				}
			}
			base.OnRender(twoDimensionContext, drawContext);
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00003A83 File Offset: 0x00001C83
		// (set) Token: 0x0600009F RID: 159 RVA: 0x00003A8B File Offset: 0x00001C8B
		[Editor(false)]
		public bool IsDirectionUpward
		{
			get
			{
				return this._isDirectionUpward;
			}
			set
			{
				if (this._isDirectionUpward != value)
				{
					this._isDirectionUpward = value;
					base.OnPropertyChanged(value, "IsDirectionUpward");
				}
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00003AA9 File Offset: 0x00001CA9
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x00003AB2 File Offset: 0x00001CB2
		[Editor(false)]
		public int CurrentAmount
		{
			get
			{
				return (int)this._currentAmount;
			}
			set
			{
				if (this._currentAmount != (float)value)
				{
					this._currentAmount = (float)value;
					base.OnPropertyChanged(value, "CurrentAmount");
					this._isCurrentValueSet = true;
				}
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00003AD9 File Offset: 0x00001CD9
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x00003AE2 File Offset: 0x00001CE2
		[Editor(false)]
		public int MaxAmount
		{
			get
			{
				return (int)this._maxAmount;
			}
			set
			{
				if (this._maxAmount != (float)value)
				{
					this._maxAmount = (float)value;
					base.OnPropertyChanged(value, "MaxAmount");
				}
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00003B02 File Offset: 0x00001D02
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x00003B0B File Offset: 0x00001D0B
		[Editor(false)]
		public int InitialAmount
		{
			get
			{
				return (int)this._initialAmount;
			}
			set
			{
				if (this._initialAmount != (float)value)
				{
					this._initialAmount = (float)value;
					base.OnPropertyChanged(value, "InitialAmount");
				}
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00003B2B File Offset: 0x00001D2B
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x00003B33 File Offset: 0x00001D33
		[Editor(false)]
		public float MaxAmountAsFloat
		{
			get
			{
				return this._maxAmount;
			}
			set
			{
				if (this._maxAmount != value)
				{
					this._maxAmount = value;
					base.OnPropertyChanged(value, "MaxAmountAsFloat");
				}
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00003B51 File Offset: 0x00001D51
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x00003B59 File Offset: 0x00001D59
		[Editor(false)]
		public float CurrentAmountAsFloat
		{
			get
			{
				return this._currentAmount;
			}
			set
			{
				if (this._currentAmount != value)
				{
					this._currentAmount = value;
					base.OnPropertyChanged(value, "CurrentAmountAsFloat");
					this._isCurrentValueSet = true;
				}
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00003B7E File Offset: 0x00001D7E
		// (set) Token: 0x060000AB RID: 171 RVA: 0x00003B86 File Offset: 0x00001D86
		[Editor(false)]
		public float InitialAmountAsFloat
		{
			get
			{
				return this._initialAmount;
			}
			set
			{
				if (this._initialAmount != value)
				{
					this._initialAmount = value;
					base.OnPropertyChanged(value, "InitialAmountAsFloat");
				}
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00003BA4 File Offset: 0x00001DA4
		// (set) Token: 0x060000AD RID: 173 RVA: 0x00003BAC File Offset: 0x00001DAC
		public Widget FillWidget
		{
			get
			{
				return this._fillWidget;
			}
			set
			{
				if (this._fillWidget != value)
				{
					this._fillWidget = value;
					base.OnPropertyChanged<Widget>(value, "FillWidget");
				}
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00003BCA File Offset: 0x00001DCA
		// (set) Token: 0x060000AF RID: 175 RVA: 0x00003BD2 File Offset: 0x00001DD2
		public Widget ChangeWidget
		{
			get
			{
				return this._changeWidget;
			}
			set
			{
				if (this._changeWidget != value)
				{
					this._changeWidget = value;
					base.OnPropertyChanged<Widget>(value, "ChangeWidget");
				}
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00003BF0 File Offset: 0x00001DF0
		// (set) Token: 0x060000B1 RID: 177 RVA: 0x00003BF8 File Offset: 0x00001DF8
		public Widget DividerWidget
		{
			get
			{
				return this._dividerWidget;
			}
			set
			{
				if (this._dividerWidget != value)
				{
					this._dividerWidget = value;
					base.OnPropertyChanged<Widget>(value, "DividerWidget");
				}
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00003C16 File Offset: 0x00001E16
		// (set) Token: 0x060000B3 RID: 179 RVA: 0x00003C1E File Offset: 0x00001E1E
		public Widget ContainerWidget
		{
			get
			{
				return this._containerWidget;
			}
			set
			{
				if (this._containerWidget != value)
				{
					this._containerWidget = value;
					base.OnPropertyChanged<Widget>(value, "ContainerWidget");
				}
			}
		}

		// Token: 0x04000048 RID: 72
		private bool _isCurrentValueSet;

		// Token: 0x04000049 RID: 73
		private Widget _fillWidget;

		// Token: 0x0400004A RID: 74
		private Widget _changeWidget;

		// Token: 0x0400004B RID: 75
		private Widget _containerWidget;

		// Token: 0x0400004C RID: 76
		private Widget _dividerWidget;

		// Token: 0x0400004D RID: 77
		private float _maxAmount;

		// Token: 0x0400004E RID: 78
		private float _currentAmount;

		// Token: 0x0400004F RID: 79
		private float _initialAmount;

		// Token: 0x04000050 RID: 80
		private bool _isDirectionUpward;
	}
}
