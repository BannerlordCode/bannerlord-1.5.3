using System;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.ExtraWidgets
{
	// Token: 0x0200000B RID: 11
	public class FillBarVerticalClipWidget : Widget
	{
		// Token: 0x06000081 RID: 129 RVA: 0x00003431 File Offset: 0x00001631
		public FillBarVerticalClipWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000343C File Offset: 0x0000163C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.FillWidget != null && this.ClipWidget != null)
			{
				float y = base.Size.Y;
				float num = Mathf.Clamp(Mathf.Clamp(this._initialAmount, 0f, (float)this.MaxAmount) / (float)this.MaxAmount, 0f, 1f);
				float num2 = (this._isCurrentValueSet ? Mathf.Clamp((float)(this.CurrentAmount - this.InitialAmount), (float)(-(float)this.MaxAmount), (float)this.MaxAmount) : 0f);
				float num3 = (this._isCurrentValueSet ? Mathf.Clamp(num2 / (float)this.MaxAmount, -1f, 1f) : 0f);
				this.ClipWidget.VerticalAlignment = VerticalAlignment.Bottom;
				this.ClipWidget.ClipContents = true;
				this.FillWidget.VerticalAlignment = VerticalAlignment.Bottom;
				if (this.IsDirectionUpward)
				{
					this.ClipWidget.VerticalAlignment = VerticalAlignment.Bottom;
				}
				else
				{
					this.ClipWidget.VerticalAlignment = VerticalAlignment.Top;
				}
				this.ClipWidget.ScaledSuggestedHeight = y * num;
				if (this.ChangeWidget != null && this.DividerWidget != null)
				{
					this.DividerWidget.IsVisible = this.ChangeWidget != null && num3 != 0f;
				}
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003580 File Offset: 0x00001780
		protected override void OnRender(TwoDimensionContext twoDimensionContext, TwoDimensionDrawContext drawContext)
		{
			base.OnRender(twoDimensionContext, drawContext);
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000084 RID: 132 RVA: 0x0000358A File Offset: 0x0000178A
		// (set) Token: 0x06000085 RID: 133 RVA: 0x00003592 File Offset: 0x00001792
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

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000086 RID: 134 RVA: 0x000035B0 File Offset: 0x000017B0
		// (set) Token: 0x06000087 RID: 135 RVA: 0x000035B9 File Offset: 0x000017B9
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

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000088 RID: 136 RVA: 0x000035E0 File Offset: 0x000017E0
		// (set) Token: 0x06000089 RID: 137 RVA: 0x000035E9 File Offset: 0x000017E9
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

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003609 File Offset: 0x00001809
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00003612 File Offset: 0x00001812
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

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00003632 File Offset: 0x00001832
		// (set) Token: 0x0600008D RID: 141 RVA: 0x0000363A File Offset: 0x0000183A
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

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00003658 File Offset: 0x00001858
		// (set) Token: 0x0600008F RID: 143 RVA: 0x00003660 File Offset: 0x00001860
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

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000090 RID: 144 RVA: 0x0000367E File Offset: 0x0000187E
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00003686 File Offset: 0x00001886
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

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000092 RID: 146 RVA: 0x000036A4 File Offset: 0x000018A4
		// (set) Token: 0x06000093 RID: 147 RVA: 0x000036AC File Offset: 0x000018AC
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

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000094 RID: 148 RVA: 0x000036CA File Offset: 0x000018CA
		// (set) Token: 0x06000095 RID: 149 RVA: 0x000036D2 File Offset: 0x000018D2
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

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000096 RID: 150 RVA: 0x000036F0 File Offset: 0x000018F0
		// (set) Token: 0x06000097 RID: 151 RVA: 0x000036F8 File Offset: 0x000018F8
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

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00003716 File Offset: 0x00001916
		// (set) Token: 0x06000099 RID: 153 RVA: 0x0000371E File Offset: 0x0000191E
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

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600009A RID: 154 RVA: 0x0000373C File Offset: 0x0000193C
		// (set) Token: 0x0600009B RID: 155 RVA: 0x00003744 File Offset: 0x00001944
		public Widget ClipWidget
		{
			get
			{
				return this._clipWidget;
			}
			set
			{
				if (this._clipWidget != value)
				{
					this._clipWidget = value;
					base.OnPropertyChanged<Widget>(value, "ClipWidget");
				}
			}
		}

		// Token: 0x0400003E RID: 62
		private bool _isCurrentValueSet;

		// Token: 0x0400003F RID: 63
		private Widget _fillWidget;

		// Token: 0x04000040 RID: 64
		private Widget _changeWidget;

		// Token: 0x04000041 RID: 65
		private Widget _containerWidget;

		// Token: 0x04000042 RID: 66
		private Widget _dividerWidget;

		// Token: 0x04000043 RID: 67
		private Widget _clipWidget;

		// Token: 0x04000044 RID: 68
		private float _maxAmount;

		// Token: 0x04000045 RID: 69
		private float _currentAmount;

		// Token: 0x04000046 RID: 70
		private float _initialAmount;

		// Token: 0x04000047 RID: 71
		private bool _isDirectionUpward;
	}
}
