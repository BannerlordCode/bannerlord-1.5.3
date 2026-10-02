using System;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.GauntletUI.ExtraWidgets
{
	// Token: 0x02000018 RID: 24
	public class TwoWaySliderWidget : SliderWidget
	{
		// Token: 0x06000143 RID: 323 RVA: 0x00007370 File Offset: 0x00005570
		public TwoWaySliderWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000737C File Offset: 0x0000557C
		protected override void OnValueIntChanged(int value)
		{
			base.OnValueIntChanged(value);
			if (this.ChangeFillWidget == null || base.MaxValueInt == 0)
			{
				return;
			}
			float num = base.Size.X / base._scaleToUse;
			float num2 = (float)this.BaseValueInt / base.MaxValueFloat * num;
			if (value < this.BaseValueInt)
			{
				this.ChangeFillWidget.SetState("Positive");
				this.ChangeFillWidget.SuggestedWidth = (float)(this.BaseValueInt - value) / base.MaxValueFloat * num;
				this.ChangeFillWidget.PositionXOffset = num2 - this.ChangeFillWidget.SuggestedWidth;
			}
			else if (value > this.BaseValueInt)
			{
				this.ChangeFillWidget.SetState("Negative");
				this.ChangeFillWidget.SuggestedWidth = (float)(value - this.BaseValueInt) / base.MaxValueFloat * num;
				this.ChangeFillWidget.PositionXOffset = num2;
			}
			else
			{
				this.ChangeFillWidget.SetState("Default");
				this.ChangeFillWidget.SuggestedWidth = 0f;
			}
			if (this._handleClicked || this._valueChangedByMouse || this._manuallyIncreased)
			{
				this._manuallyIncreased = false;
				base.OnPropertyChanged(base.ValueInt, "ValueInt");
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x000074A9 File Offset: 0x000056A9
		private void ChangeFillWidgetUpdated()
		{
			if (this.ChangeFillWidget != null)
			{
				this.ChangeFillWidget.AddState("Negative");
				this.ChangeFillWidget.AddState("Positive");
				this.ChangeFillWidget.HorizontalAlignment = HorizontalAlignment.Left;
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x000074DF File Offset: 0x000056DF
		private void BaseValueIntUpdated()
		{
			this.OnValueIntChanged(base.ValueInt);
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000147 RID: 327 RVA: 0x000074ED File Offset: 0x000056ED
		// (set) Token: 0x06000148 RID: 328 RVA: 0x000074F5 File Offset: 0x000056F5
		[Editor(false)]
		public BrushWidget ChangeFillWidget
		{
			get
			{
				return this._changeFillWidget;
			}
			set
			{
				if (this._changeFillWidget != value)
				{
					this._changeFillWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "ChangeFillWidget");
					this.ChangeFillWidgetUpdated();
				}
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00007519 File Offset: 0x00005719
		// (set) Token: 0x0600014A RID: 330 RVA: 0x00007521 File Offset: 0x00005721
		[Editor(false)]
		public int BaseValueInt
		{
			get
			{
				return this._baseValueInt;
			}
			set
			{
				if (this._baseValueInt != value)
				{
					this._baseValueInt = value;
					base.OnPropertyChanged(value, "BaseValueInt");
					this.BaseValueIntUpdated();
				}
			}
		}

		// Token: 0x0400009F RID: 159
		protected bool _manuallyIncreased;

		// Token: 0x040000A0 RID: 160
		private BrushWidget _changeFillWidget;

		// Token: 0x040000A1 RID: 161
		private int _baseValueInt;
	}
}
