using System;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.ExtraWidgets
{
	// Token: 0x02000013 RID: 19
	public class SmoothDecreaseIndicatorFillBar : BrushWidget
	{
		// Token: 0x06000121 RID: 289 RVA: 0x000068BD File Offset: 0x00004ABD
		public SmoothDecreaseIndicatorFillBar(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000068D4 File Offset: 0x00004AD4
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
				float num = Mathf.Clamp(this.CurrentAmount / this.MaxAmount, 0f, 1f);
				float num2 = Mathf.Clamp(Mathf.Clamp(this._smoothedCurrentAmount - this.CurrentAmount, 0f, this.MaxAmount - this.CurrentAmount) / this.MaxAmount, 0f, 1f);
				if (this._smoothedCurrentAmount > this.CurrentAmount)
				{
					this._smoothedCurrentAmount = Mathf.Lerp(this._smoothedCurrentAmount * 0.99f, this.CurrentAmount, this._localDt);
				}
				else
				{
					this._smoothedCurrentAmount = this.CurrentAmount;
				}
				if (this.IsVertical)
				{
					layer.OverridenHeight = base.Size.Y * num * base._inverseScaleToUse;
					layer.YOffset = base.Size.Y - layer.OverridenHeight;
					layer2.OverridenHeight = base.Size.Y * num2 * base._inverseScaleToUse;
					layer2.YOffset = base.Size.Y - (layer.OverridenHeight + layer2.OverridenHeight);
					layer.OverridenWidth = base.Size.X * base._inverseScaleToUse;
					layer2.OverridenWidth = base.Size.X * base._inverseScaleToUse;
				}
				else
				{
					layer.OverridenWidth = base.Size.X * num * base._inverseScaleToUse;
					layer2.XOffset = layer.OverridenWidth;
					layer2.OverridenWidth = base.Size.X * num2 * base._inverseScaleToUse;
					layer.OverridenHeight = base.Size.Y * base._inverseScaleToUse;
					layer2.OverridenHeight = base.ScaledSuggestedHeight * base._inverseScaleToUse;
				}
				base.OnRender(twoDimensionContext, drawContext);
				return;
			}
			this._smoothedCurrentAmount = this.CurrentAmount;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00006AF3 File Offset: 0x00004CF3
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			this._localDt = dt * 3f;
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00006B09 File Offset: 0x00004D09
		// (set) Token: 0x06000125 RID: 293 RVA: 0x00006B13 File Offset: 0x00004D13
		[Editor(false)]
		public float MaxAmount
		{
			get
			{
				return (float)((int)this._maxAmount);
			}
			set
			{
				if (this._maxAmount != value)
				{
					this._maxAmount = value;
					base.OnPropertyChanged(value, "MaxAmount");
				}
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00006B31 File Offset: 0x00004D31
		// (set) Token: 0x06000127 RID: 295 RVA: 0x00006B3B File Offset: 0x00004D3B
		[Editor(false)]
		public float CurrentAmount
		{
			get
			{
				return (float)((int)this._currentAmount);
			}
			set
			{
				if (this._currentAmount != value)
				{
					this._currentAmount = value;
					base.OnPropertyChanged(value, "CurrentAmount");
					if (this._smoothedCurrentAmount == -1f)
					{
						this._smoothedCurrentAmount = this.CurrentAmount;
					}
				}
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00006B72 File Offset: 0x00004D72
		// (set) Token: 0x06000129 RID: 297 RVA: 0x00006B7A File Offset: 0x00004D7A
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

		// Token: 0x04000087 RID: 135
		private float _localDt;

		// Token: 0x04000088 RID: 136
		private float _smoothedCurrentAmount = -1f;

		// Token: 0x04000089 RID: 137
		private float _maxAmount;

		// Token: 0x0400008A RID: 138
		private float _currentAmount;

		// Token: 0x0400008B RID: 139
		private bool _isVertical;
	}
}
