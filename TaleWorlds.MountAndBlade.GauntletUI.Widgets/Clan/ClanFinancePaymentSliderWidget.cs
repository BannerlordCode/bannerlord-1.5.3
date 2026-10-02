using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Clan
{
	// Token: 0x02000177 RID: 375
	public class ClanFinancePaymentSliderWidget : SliderWidget
	{
		// Token: 0x060013B6 RID: 5046 RVA: 0x00035BA2 File Offset: 0x00033DA2
		public ClanFinancePaymentSliderWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x00035BAC File Offset: 0x00033DAC
		protected override void OnLateUpdate(float dt)
		{
			this.CurrentRatioIndicatorWidget.ScaledPositionXOffset = Mathf.Clamp(base.Size.X * ((float)this.CurrentSize / (float)this.SizeLimit) - this.CurrentRatioIndicatorWidget.Size.X / 2f, 0f, base.Size.X);
			this.InitialFillWidget.ScaledPositionXOffset = this.CurrentRatioIndicatorWidget.PositionXOffset * base._scaleToUse + this.CurrentRatioIndicatorWidget.Size.X / 2f;
			this.InitialFillWidget.ScaledSuggestedWidth = base.Size.X - this.CurrentRatioIndicatorWidget.PositionXOffset * base._scaleToUse - this.CurrentRatioIndicatorWidget.Size.X / 2f;
			if (base.Handle.PositionXOffset > this.CurrentRatioIndicatorWidget.PositionXOffset)
			{
				this.NewIncreaseFillWidget.ScaledPositionXOffset = this.CurrentRatioIndicatorWidget.PositionXOffset * base._scaleToUse + this.CurrentRatioIndicatorWidget.Size.X / 2f;
				this.NewIncreaseFillWidget.ScaledSuggestedWidth = Mathf.Clamp((base.Handle.PositionXOffset - this.CurrentRatioIndicatorWidget.PositionXOffset) * base._scaleToUse, 0f, base.Size.X);
				this.NewDecreaseFillWidget.ScaledSuggestedWidth = 0f;
			}
			else if (base.Handle.PositionXOffset < this.CurrentRatioIndicatorWidget.PositionXOffset)
			{
				this.NewDecreaseFillWidget.ScaledPositionXOffset = base.Handle.PositionXOffset * base._scaleToUse + base.Handle.Size.X / 2f;
				this.NewDecreaseFillWidget.ScaledSuggestedWidth = Mathf.Clamp(this.CurrentRatioIndicatorWidget.PositionXOffset * base._scaleToUse + this.CurrentRatioIndicatorWidget.Size.X / 2f - (base.Handle.PositionXOffset * base._scaleToUse + base.Handle.Size.X / 2f), 0f, base.Size.X);
				this.NewIncreaseFillWidget.ScaledSuggestedWidth = 0f;
			}
			else
			{
				this.NewIncreaseFillWidget.ScaledSuggestedWidth = 0f;
				this.NewDecreaseFillWidget.ScaledSuggestedWidth = 0f;
			}
			base.OnLateUpdate(dt);
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x060013B8 RID: 5048 RVA: 0x00035E1C File Offset: 0x0003401C
		// (set) Token: 0x060013B9 RID: 5049 RVA: 0x00035E24 File Offset: 0x00034024
		[Editor(false)]
		public Widget InitialFillWidget
		{
			get
			{
				return this._initialFillWidget;
			}
			set
			{
				if (this._initialFillWidget != value)
				{
					this._initialFillWidget = value;
				}
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x060013BA RID: 5050 RVA: 0x00035E36 File Offset: 0x00034036
		// (set) Token: 0x060013BB RID: 5051 RVA: 0x00035E3E File Offset: 0x0003403E
		[Editor(false)]
		public Widget NewIncreaseFillWidget
		{
			get
			{
				return this._newIncreaseFillWidget;
			}
			set
			{
				if (this._newIncreaseFillWidget != value)
				{
					this._newIncreaseFillWidget = value;
				}
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x060013BC RID: 5052 RVA: 0x00035E50 File Offset: 0x00034050
		// (set) Token: 0x060013BD RID: 5053 RVA: 0x00035E58 File Offset: 0x00034058
		[Editor(false)]
		public Widget NewDecreaseFillWidget
		{
			get
			{
				return this._newDecreaseFillWidget;
			}
			set
			{
				if (this._newDecreaseFillWidget != value)
				{
					this._newDecreaseFillWidget = value;
				}
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x060013BE RID: 5054 RVA: 0x00035E6A File Offset: 0x0003406A
		// (set) Token: 0x060013BF RID: 5055 RVA: 0x00035E72 File Offset: 0x00034072
		[Editor(false)]
		public Widget CurrentRatioIndicatorWidget
		{
			get
			{
				return this._currentRatioIndicatorWidget;
			}
			set
			{
				if (this._currentRatioIndicatorWidget != value)
				{
					this._currentRatioIndicatorWidget = value;
				}
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x060013C0 RID: 5056 RVA: 0x00035E84 File Offset: 0x00034084
		// (set) Token: 0x060013C1 RID: 5057 RVA: 0x00035E8C File Offset: 0x0003408C
		[Editor(false)]
		public int CurrentSize
		{
			get
			{
				return this._currentSize;
			}
			set
			{
				if (this._currentSize != value)
				{
					this._currentSize = value;
				}
			}
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x060013C2 RID: 5058 RVA: 0x00035E9E File Offset: 0x0003409E
		// (set) Token: 0x060013C3 RID: 5059 RVA: 0x00035EA6 File Offset: 0x000340A6
		[Editor(false)]
		public int TargetSize
		{
			get
			{
				return this._targetSize;
			}
			set
			{
				if (this._targetSize != value)
				{
					this._targetSize = value;
				}
			}
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x060013C4 RID: 5060 RVA: 0x00035EB8 File Offset: 0x000340B8
		// (set) Token: 0x060013C5 RID: 5061 RVA: 0x00035EC0 File Offset: 0x000340C0
		[Editor(false)]
		public int SizeLimit
		{
			get
			{
				return this._sizeLimit;
			}
			set
			{
				if (this._sizeLimit != value)
				{
					this._sizeLimit = value;
				}
			}
		}

		// Token: 0x040008F9 RID: 2297
		private Widget _initialFillWidget;

		// Token: 0x040008FA RID: 2298
		private Widget _newIncreaseFillWidget;

		// Token: 0x040008FB RID: 2299
		private Widget _newDecreaseFillWidget;

		// Token: 0x040008FC RID: 2300
		private Widget _currentRatioIndicatorWidget;

		// Token: 0x040008FD RID: 2301
		private int _currentSize;

		// Token: 0x040008FE RID: 2302
		private int _targetSize;

		// Token: 0x040008FF RID: 2303
		private int _sizeLimit;
	}
}
