using System;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.ExtraWidgets
{
	// Token: 0x02000009 RID: 9
	public class FillBarHorizontalWidget : Widget
	{
		// Token: 0x0600005F RID: 95 RVA: 0x00002DB7 File Offset: 0x00000FB7
		public FillBarHorizontalWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002DC0 File Offset: 0x00000FC0
		protected override void OnRender(TwoDimensionContext twoDimensionContext, TwoDimensionDrawContext drawContext)
		{
			if (this.FillWidget != null)
			{
				float x = this.FillWidget.ParentWidget.Size.X;
				float num = Mathf.Clamp(Mathf.Clamp((float)this.InitialAmount, 0f, (float)this.MaxAmount) / (float)this.MaxAmount, 0f, 1f);
				float num2 = (this._isCurrentValueSet ? Mathf.Clamp((float)(this.CurrentAmount - this.InitialAmount), (float)(-(float)this.MaxAmount), (float)this.MaxAmount) : 0f);
				float num3 = (this._isCurrentValueSet ? Mathf.Clamp(num2 / (float)this.MaxAmount, -1f, 1f) : 0f);
				if (this.IsDirectionRightward)
				{
					this.FillWidget.HorizontalAlignment = HorizontalAlignment.Left;
					this.FillWidget.ScaledSuggestedWidth = num * x;
					if (this.ChangeWidget != null)
					{
						this.ChangeWidget.ScaledSuggestedWidth = num3 * x;
						if (num3 >= 0f)
						{
							this.ChangeWidget.ScaledPositionXOffset = -this.FillWidget.ScaledSuggestedWidth;
							this.ChangeWidget.Color = new Color(1f, 1f, 1f, 1f);
						}
						else
						{
							this.ChangeWidget.ScaledPositionXOffset = -this.FillWidget.ScaledSuggestedWidth + this.ChangeWidget.ScaledSuggestedWidth;
							this.ChangeWidget.Color = new Color(1f, 0f, 0f, 1f);
						}
					}
				}
				else
				{
					this.FillWidget.HorizontalAlignment = HorizontalAlignment.Right;
					this.FillWidget.ScaledSuggestedWidth = num * x;
					if (this.ChangeWidget != null)
					{
						this.ChangeWidget.ScaledSuggestedWidth = num3 * x;
						this.ChangeWidget.HorizontalAlignment = HorizontalAlignment.Right;
						if (num3 >= 0f)
						{
							this.ChangeWidget.ScaledPositionXOffset = -this.FillWidget.ScaledSuggestedWidth;
							this.ChangeWidget.Color = new Color(1f, 1f, 1f, 1f);
						}
						else
						{
							this.ChangeWidget.ScaledPositionXOffset = -this.FillWidget.ScaledSuggestedWidth + this.ChangeWidget.ScaledSuggestedWidth;
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

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000061 RID: 97 RVA: 0x0000304E File Offset: 0x0000124E
		// (set) Token: 0x06000062 RID: 98 RVA: 0x00003056 File Offset: 0x00001256
		[Editor(false)]
		public bool IsDirectionRightward
		{
			get
			{
				return this._isDirectionRightward;
			}
			set
			{
				if (this._isDirectionRightward != value)
				{
					this._isDirectionRightward = value;
					base.OnPropertyChanged(value, "IsDirectionRightward");
				}
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00003074 File Offset: 0x00001274
		// (set) Token: 0x06000064 RID: 100 RVA: 0x0000307D File Offset: 0x0000127D
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

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000065 RID: 101 RVA: 0x000030A4 File Offset: 0x000012A4
		// (set) Token: 0x06000066 RID: 102 RVA: 0x000030AD File Offset: 0x000012AD
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

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000067 RID: 103 RVA: 0x000030CD File Offset: 0x000012CD
		// (set) Token: 0x06000068 RID: 104 RVA: 0x000030D6 File Offset: 0x000012D6
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

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000069 RID: 105 RVA: 0x000030F6 File Offset: 0x000012F6
		// (set) Token: 0x0600006A RID: 106 RVA: 0x000030FE File Offset: 0x000012FE
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

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600006B RID: 107 RVA: 0x0000311C File Offset: 0x0000131C
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00003124 File Offset: 0x00001324
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
				}
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00003142 File Offset: 0x00001342
		// (set) Token: 0x0600006E RID: 110 RVA: 0x0000314A File Offset: 0x0000134A
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

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00003168 File Offset: 0x00001368
		// (set) Token: 0x06000070 RID: 112 RVA: 0x00003170 File Offset: 0x00001370
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

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000071 RID: 113 RVA: 0x0000318E File Offset: 0x0000138E
		// (set) Token: 0x06000072 RID: 114 RVA: 0x00003196 File Offset: 0x00001396
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

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000073 RID: 115 RVA: 0x000031B4 File Offset: 0x000013B4
		// (set) Token: 0x06000074 RID: 116 RVA: 0x000031BC File Offset: 0x000013BC
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

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000075 RID: 117 RVA: 0x000031DA File Offset: 0x000013DA
		// (set) Token: 0x06000076 RID: 118 RVA: 0x000031E2 File Offset: 0x000013E2
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

		// Token: 0x0400002D RID: 45
		private bool _isCurrentValueSet;

		// Token: 0x0400002E RID: 46
		private Widget _fillWidget;

		// Token: 0x0400002F RID: 47
		private Widget _changeWidget;

		// Token: 0x04000030 RID: 48
		private Widget _containerWidget;

		// Token: 0x04000031 RID: 49
		private Widget _dividerWidget;

		// Token: 0x04000032 RID: 50
		private float _maxAmount;

		// Token: 0x04000033 RID: 51
		private float _currentAmount;

		// Token: 0x04000034 RID: 52
		private float _initialAmount;

		// Token: 0x04000035 RID: 53
		private bool _isDirectionRightward;
	}
}
