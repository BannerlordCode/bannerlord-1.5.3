using System;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.ExtraWidgets
{
	// Token: 0x02000008 RID: 8
	public class FillBar : BrushWidget
	{
		// Token: 0x0600004C RID: 76 RVA: 0x000029CB File Offset: 0x00000BCB
		public FillBar(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000029D4 File Offset: 0x00000BD4
		protected override void OnRender(TwoDimensionContext twoDimensionContext, TwoDimensionDrawContext drawContext)
		{
			if (base.IsVisible)
			{
				StyleLayer layer = base.Brush.DefaultStyle.GetLayer("DefaultFill");
				StyleLayer layer2 = base.Brush.DefaultStyle.GetLayer("ChangeFill");
				layer.WidthPolicy = BrushLayerSizePolicy.Overriden;
				layer2.WidthPolicy = BrushLayerSizePolicy.Overriden;
				layer.HeightPolicy = BrushLayerSizePolicy.Overriden;
				layer2.HeightPolicy = BrushLayerSizePolicy.Overriden;
				float num = Mathf.Clamp(this._initialAmount / (float)this.MaxAmount, 0f, 1f);
				float num2 = Mathf.Clamp(Mathf.Clamp((float)(this.CurrentAmount - this.InitialAmount), 0f, (float)(this.MaxAmount - this.InitialAmount)) / (float)this.MaxAmount, 0f, 1f);
				if (this.IsVertical)
				{
					if (!this.IsSmoothFillEnabled)
					{
						this._localDt = 1f;
					}
					float num3 = base.Size.Y * num * base._inverseScaleToUse;
					layer.OverridenHeight = Mathf.Lerp(layer.OverridenHeight, num3, this._localDt);
					num3 = base.Size.Y - layer.OverridenHeight;
					layer.YOffset = Mathf.Lerp(layer.YOffset, num3, this._localDt);
					num3 = base.Size.Y * num2 * base._inverseScaleToUse;
					layer2.OverridenHeight = Mathf.Lerp(layer2.OverridenHeight, num3, this._localDt);
					num3 = base.Size.Y - (layer.OverridenHeight + layer2.OverridenHeight);
					layer2.YOffset = Mathf.Lerp(layer2.YOffset, num3, this._localDt);
					layer.OverridenWidth = base.Size.X * base._inverseScaleToUse;
					layer2.OverridenWidth = base.Size.X * base._inverseScaleToUse;
				}
				else
				{
					if (!this.IsSmoothFillEnabled)
					{
						this._localDt = 1f;
					}
					float num4 = base.Size.X * num * base._inverseScaleToUse;
					layer.OverridenWidth = Mathf.Lerp(layer.OverridenWidth, num4, this._localDt);
					num4 = layer.OverridenWidth;
					layer2.XOffset = Mathf.Lerp(layer2.XOffset, num4, this._localDt);
					num4 = base.Size.X * num2 * base._inverseScaleToUse;
					layer2.OverridenWidth = Mathf.Lerp(layer2.OverridenWidth, num4, this._localDt);
					layer.OverridenHeight = base.Size.Y * base._inverseScaleToUse;
					layer2.OverridenHeight = base.ScaledSuggestedHeight * base._inverseScaleToUse;
				}
				base.OnRender(twoDimensionContext, drawContext);
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002C68 File Offset: 0x00000E68
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			this._localDt = dt * 10f;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002C7E File Offset: 0x00000E7E
		// (set) Token: 0x06000050 RID: 80 RVA: 0x00002C87 File Offset: 0x00000E87
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

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002CA7 File Offset: 0x00000EA7
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00002CB0 File Offset: 0x00000EB0
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

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002CD0 File Offset: 0x00000ED0
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00002CD9 File Offset: 0x00000ED9
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

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00002CF9 File Offset: 0x00000EF9
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00002D01 File Offset: 0x00000F01
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

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00002D1F File Offset: 0x00000F1F
		// (set) Token: 0x06000058 RID: 88 RVA: 0x00002D27 File Offset: 0x00000F27
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

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00002D45 File Offset: 0x00000F45
		// (set) Token: 0x0600005A RID: 90 RVA: 0x00002D4D File Offset: 0x00000F4D
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

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00002D6B File Offset: 0x00000F6B
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00002D73 File Offset: 0x00000F73
		[Editor(false)]
		public bool IsVertical
		{
			get
			{
				return this._isVertical;
			}
			set
			{
				if (this._isVertical != value)
				{
					this._isVertical = value;
					base.OnPropertyChanged(value, "IsVertical");
				}
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00002D91 File Offset: 0x00000F91
		// (set) Token: 0x0600005E RID: 94 RVA: 0x00002D99 File Offset: 0x00000F99
		[Editor(false)]
		public bool IsSmoothFillEnabled
		{
			get
			{
				return this._isSmoothFillEnabled;
			}
			set
			{
				if (this._isSmoothFillEnabled != value)
				{
					this._isSmoothFillEnabled = value;
					base.OnPropertyChanged(value, "IsSmoothFillEnabled");
				}
			}
		}

		// Token: 0x04000027 RID: 39
		private float _localDt;

		// Token: 0x04000028 RID: 40
		private float _maxAmount;

		// Token: 0x04000029 RID: 41
		private float _currentAmount;

		// Token: 0x0400002A RID: 42
		private float _initialAmount;

		// Token: 0x0400002B RID: 43
		private bool _isVertical;

		// Token: 0x0400002C RID: 44
		private bool _isSmoothFillEnabled;
	}
}
