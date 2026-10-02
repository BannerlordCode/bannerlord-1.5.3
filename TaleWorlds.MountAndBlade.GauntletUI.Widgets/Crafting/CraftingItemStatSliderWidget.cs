using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Crafting
{
	// Token: 0x02000169 RID: 361
	public class CraftingItemStatSliderWidget : SliderWidget
	{
		// Token: 0x06001326 RID: 4902 RVA: 0x00034AA6 File Offset: 0x00032CA6
		public CraftingItemStatSliderWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x00034AB0 File Offset: 0x00032CB0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			float num = 1f;
			float x = base.SliderArea.Size.X;
			if (MathF.Abs(base.MaxValueFloat - base.MinValueFloat) > 1E-45f)
			{
				num = (base.ValueFloat - base.MinValueFloat) / (base.MaxValueFloat - base.MinValueFloat) * x;
				if (base.ReverseDirection)
				{
					num = 1f - num;
				}
			}
			if (this.HasValidTarget && this.TargetFill != null && base.Handle != null && this.ValueText != null)
			{
				float num2 = base.SliderArea.Size.X / base.MaxValueFloat * this.TargetValue;
				int num3 = MathF.Ceiling(MathF.Min(num, num2));
				int num4 = MathF.Floor(MathF.Max(num, num2));
				base.Filler.ScaledSuggestedWidth = (float)num3;
				this.TargetFill.ScaledPositionXOffset = (float)num3;
				this.TargetFill.ScaledSuggestedWidth = (float)(num4 - num3);
				base.Handle.ScaledPositionXOffset = num2 - base.Handle.Size.X / 2f;
				string text = ((this.IsExceedingBeneficial ? (base.ValueFloat >= this.TargetValue) : (base.ValueFloat <= this.TargetValue)) ? "Bonus" : "Penalty");
				this.TargetFill.SetState(text);
				this.ValueText.SetState(text);
				if (!this.HasValidValue)
				{
					this.LabelTextWidget.SetState(text);
					return;
				}
			}
			else
			{
				base.Filler.ScaledSuggestedWidth = num;
			}
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06001328 RID: 4904 RVA: 0x00034C4D File Offset: 0x00032E4D
		// (set) Token: 0x06001329 RID: 4905 RVA: 0x00034C55 File Offset: 0x00032E55
		[Editor(false)]
		public TextWidget ValueText
		{
			get
			{
				return this._valueText;
			}
			set
			{
				if (value != this._valueText)
				{
					this._valueText = value;
				}
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x0600132A RID: 4906 RVA: 0x00034C67 File Offset: 0x00032E67
		// (set) Token: 0x0600132B RID: 4907 RVA: 0x00034C6F File Offset: 0x00032E6F
		[Editor(false)]
		public TextWidget LabelTextWidget
		{
			get
			{
				return this._labelTextWidget;
			}
			set
			{
				if (value != this._labelTextWidget)
				{
					this._labelTextWidget = value;
				}
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x0600132C RID: 4908 RVA: 0x00034C81 File Offset: 0x00032E81
		// (set) Token: 0x0600132D RID: 4909 RVA: 0x00034C89 File Offset: 0x00032E89
		[Editor(false)]
		public bool HasValidTarget
		{
			get
			{
				return this._hasValidTarget;
			}
			set
			{
				if (value != this._hasValidTarget)
				{
					this._hasValidTarget = value;
				}
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x0600132E RID: 4910 RVA: 0x00034C9B File Offset: 0x00032E9B
		// (set) Token: 0x0600132F RID: 4911 RVA: 0x00034CA3 File Offset: 0x00032EA3
		[Editor(false)]
		public bool HasValidValue
		{
			get
			{
				return this._hasValidValue;
			}
			set
			{
				if (value != this._hasValidValue)
				{
					this._hasValidValue = value;
				}
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06001330 RID: 4912 RVA: 0x00034CB5 File Offset: 0x00032EB5
		// (set) Token: 0x06001331 RID: 4913 RVA: 0x00034CBD File Offset: 0x00032EBD
		[Editor(false)]
		public bool IsExceedingBeneficial
		{
			get
			{
				return this._isExceedingBeneficial;
			}
			set
			{
				if (value != this._isExceedingBeneficial)
				{
					this._isExceedingBeneficial = value;
				}
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06001332 RID: 4914 RVA: 0x00034CCF File Offset: 0x00032ECF
		// (set) Token: 0x06001333 RID: 4915 RVA: 0x00034CD7 File Offset: 0x00032ED7
		[Editor(false)]
		public float TargetValue
		{
			get
			{
				return this._targetValue;
			}
			set
			{
				if (value != this._targetValue)
				{
					this._targetValue = value;
				}
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06001334 RID: 4916 RVA: 0x00034CE9 File Offset: 0x00032EE9
		// (set) Token: 0x06001335 RID: 4917 RVA: 0x00034CF1 File Offset: 0x00032EF1
		[Editor(false)]
		public BrushWidget TargetFill
		{
			get
			{
				return this._targetFill;
			}
			set
			{
				if (value != this._targetFill)
				{
					this._targetFill = value;
				}
			}
		}

		// Token: 0x040008BB RID: 2235
		private bool _hasValidTarget;

		// Token: 0x040008BC RID: 2236
		private bool _hasValidValue;

		// Token: 0x040008BD RID: 2237
		private bool _isExceedingBeneficial;

		// Token: 0x040008BE RID: 2238
		private float _targetValue;

		// Token: 0x040008BF RID: 2239
		private BrushWidget _targetFill;

		// Token: 0x040008C0 RID: 2240
		private TextWidget _valueText;

		// Token: 0x040008C1 RID: 2241
		private TextWidget _labelTextWidget;
	}
}
