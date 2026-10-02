using System;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.ExtraWidgets
{
	// Token: 0x0200000D RID: 13
	public class FillBarWidget : Widget
	{
		// Token: 0x060000B4 RID: 180 RVA: 0x00003C3C File Offset: 0x00001E3C
		public FillBarWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003C48 File Offset: 0x00001E48
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.FillWidget != null)
			{
				float x = this.FillWidget.ParentWidget.Size.X;
				if (this._maxAmount == 0f)
				{
					this.FillWidget.ScaledSuggestedWidth = 0f;
				}
				else
				{
					float num = Mathf.Clamp(this._initialAmount / this._maxAmount, 0f, 1f);
					this.FillWidget.ScaledSuggestedWidth = num * (x - this.FillWidget.ScaledMarginLeft - this.FillWidget.ScaledMarginRight);
				}
				if (this.ChangeWidget != null)
				{
					float num2 = Mathf.Clamp(Mathf.Clamp(this._currentAmount - this._initialAmount, -this._maxAmount, this._maxAmount) / this._maxAmount, -1f, 1f);
					if (num2 > 0f)
					{
						float num3 = x - this.ChangeWidget.ScaledMarginLeft - this.ChangeWidget.ScaledMarginRight;
						if (this.CompletelyFillChange)
						{
							float num4 = Mathf.Clamp(Mathf.Clamp(this._currentAmount, 0f, this._maxAmount) / this._maxAmount, 0f, 1f);
							this.ChangeWidget.ScaledSuggestedWidth = num4 * num3;
						}
						else
						{
							this.ChangeWidget.ScaledSuggestedWidth = Mathf.Clamp(num2 * num3, 0f, num3 - this.FillWidget.ScaledSuggestedWidth);
							this.ChangeWidget.ScaledPositionXOffset = this.FillWidget.ScaledSuggestedWidth;
						}
						if (!this.CustomChangeColor)
						{
							this.ChangeWidget.Color = new Color(1f, 1f, 1f, 1f);
						}
					}
					else if (num2 < 0f && this.ShowNegativeChange)
					{
						this.ChangeWidget.ScaledSuggestedWidth = num2 * (x - this.ChangeWidget.ScaledMarginLeft - this.ChangeWidget.ScaledMarginRight) * -1f;
						this.ChangeWidget.ScaledPositionXOffset = this.FillWidget.ScaledSuggestedWidth - this.ChangeWidget.ScaledSuggestedWidth;
						if (!this.CustomChangeColor)
						{
							this.ChangeWidget.Color = new Color(1f, 0f, 0f, 1f);
						}
					}
					else
					{
						this.ChangeWidget.ScaledSuggestedWidth = 0f;
					}
					if (this.DividerWidget != null)
					{
						if (num2 > 0f)
						{
							this.DividerWidget.ScaledPositionXOffset = this.ChangeWidget.ScaledPositionXOffset - this.DividerWidget.Size.X;
						}
						else if (num2 < 0f)
						{
							this.DividerWidget.ScaledPositionXOffset = this.FillWidget.ScaledSuggestedWidth - this.DividerWidget.Size.X;
						}
						this.DividerWidget.IsVisible = this.ChangeWidget != null && num2 != 0f;
					}
				}
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00003F28 File Offset: 0x00002128
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x00003F31 File Offset: 0x00002131
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
				}
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00003F51 File Offset: 0x00002151
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x00003F5A File Offset: 0x0000215A
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

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00003F7A File Offset: 0x0000217A
		// (set) Token: 0x060000BB RID: 187 RVA: 0x00003F83 File Offset: 0x00002183
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

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00003FA3 File Offset: 0x000021A3
		// (set) Token: 0x060000BD RID: 189 RVA: 0x00003FAB File Offset: 0x000021AB
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

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00003FC9 File Offset: 0x000021C9
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00003FD1 File Offset: 0x000021D1
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

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00003FEF File Offset: 0x000021EF
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00003FF7 File Offset: 0x000021F7
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

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00004015 File Offset: 0x00002215
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x0000401D File Offset: 0x0000221D
		[Editor(false)]
		public bool CompletelyFillChange
		{
			get
			{
				return this._completelyFillChange;
			}
			set
			{
				if (this._completelyFillChange != value)
				{
					this._completelyFillChange = value;
					base.OnPropertyChanged(value, "CompletelyFillChange");
				}
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x0000403B File Offset: 0x0000223B
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x00004043 File Offset: 0x00002243
		[Editor(false)]
		public bool ShowNegativeChange
		{
			get
			{
				return this._showNegativeChange;
			}
			set
			{
				if (this._showNegativeChange != value)
				{
					this._showNegativeChange = value;
					base.OnPropertyChanged(value, "ShowNegativeChange");
				}
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00004061 File Offset: 0x00002261
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00004069 File Offset: 0x00002269
		[Editor(false)]
		public bool CustomChangeColor
		{
			get
			{
				return this._customChangeColor;
			}
			set
			{
				if (this._customChangeColor != value)
				{
					this._customChangeColor = value;
					base.OnPropertyChanged(value, "CustomChangeColor");
				}
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00004087 File Offset: 0x00002287
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x0000408F File Offset: 0x0000228F
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

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000CA RID: 202 RVA: 0x000040AD File Offset: 0x000022AD
		// (set) Token: 0x060000CB RID: 203 RVA: 0x000040B5 File Offset: 0x000022B5
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

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000CC RID: 204 RVA: 0x000040D3 File Offset: 0x000022D3
		// (set) Token: 0x060000CD RID: 205 RVA: 0x000040DB File Offset: 0x000022DB
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

		// Token: 0x04000051 RID: 81
		private Widget _fillWidget;

		// Token: 0x04000052 RID: 82
		private Widget _changeWidget;

		// Token: 0x04000053 RID: 83
		private Widget _dividerWidget;

		// Token: 0x04000054 RID: 84
		private float _maxAmount;

		// Token: 0x04000055 RID: 85
		private float _currentAmount;

		// Token: 0x04000056 RID: 86
		private float _initialAmount;

		// Token: 0x04000057 RID: 87
		private bool _completelyFillChange;

		// Token: 0x04000058 RID: 88
		private bool _showNegativeChange;

		// Token: 0x04000059 RID: 89
		private bool _customChangeColor;
	}
}
